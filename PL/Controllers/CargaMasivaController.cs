using Microsoft.AspNetCore.Mvc;
using System.IO;

namespace PL.Controllers
{
    public class CargaMasivaController : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public CargaMasivaController(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult CargaMasiva()
        {
            ML.Result result = new ML.Result();
            return View(result);
        }

        [HttpPost]
        public ActionResult CargarTXT()
        {
            IFormFile archivo = Request.Form.Files["FileTXT"];

            string logPath = @"C:\Users\digis\Documents\Juan Alberto Gonzalez Gutierrez\JGonzalezProgramacionNCapasNetCore\PL\CargaMasiva\logErrores.txt";

            if (archivo != null)
            {
                StreamReader lector = new StreamReader(archivo.OpenReadStream());
                string linea;
                linea = lector.ReadLine();

                string errores = "";

                int contador = 0;

                while ((linea = lector.ReadLine()) != null)
                {
                    contador++;

                    try
                    {
                        string[] lineas = linea.Split('|');

                        ML.Usuario usuario = new ML.Usuario();

                        usuario.Nombre = lineas[0];
                        usuario.ApellidoPaterno = lineas[1];
                        usuario.ApellidoMaterno = lineas[2];
                        usuario.FechaNacimiento = lineas[3];
                        usuario.Sexo = char.Parse(lineas[4]);
                        usuario.Telefono = lineas[5];
                        usuario.UserName = lineas[6];
                        usuario.Password = lineas[7];
                        usuario.Email = lineas[8];
                        usuario.Celular = lineas[9];
                        usuario.CURP = lineas[10];

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = byte.Parse(lineas[11]);

                        usuario.Imagen = null;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.Calle = lineas[12];
                        usuario.Direccion.NumeroExterior = lineas[13];
                        usuario.Direccion.NumeroInterior = lineas[14];

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = int.Parse(lineas[15]);

                        ML.Result result = BL.Usuario.Add(usuario);

                        if (!result.Correct)
                        {
                            errores += $"\nError Registro {contador}: {result.ErrorMessage}";
                        }
                    }
                    catch (Exception ex)
                    {
                        errores += $"\nError Registro {contador}: {ex.Message}";
                    }
                }

                ViewBag.Message = errores;

                using (StreamWriter sw = System.IO.File.CreateText(logPath))
                {
                    sw.WriteLine(errores);
                }

                ViewBag.logErrores = logPath;
            }

            return PartialView("Modal");
        }

        [HttpPost]
        public ActionResult CargaMasiva(ML.Usuario usuario)
        {
            IFormFile archivoExcel = Request.Form.Files["FileExcel"];

            //Si la sesión no existe:
            if (HttpContext.Session.GetString("PathArchivo") == null)
            {
                //validar si el archivo se cargó
                if (archivoExcel.Length > 0)
                {
                    //nombre del archivo
                    string fileName = Path.GetFileName(archivoExcel.FileName);
                    //dirección de la carpeta
                    string folderPath = _configuration["PathFolder:value"];
                    //obtenemos la extension del archivo
                    string extensionArchivo = Path.GetExtension(archivoExcel.FileName).ToLower();
                    //obtenemos la extension de la variable global
                    string extensionModulo = _configuration["TipoExcel"];

                    //validar si la extensión del archivo es la misma de la variable
                    if (extensionArchivo == extensionModulo)
                    {
                        //creamos la copia
                        //creamos la direccion y el nombre de la copia
                        string copiaPath = Path.Combine(_hostingEnvironment.ContentRootPath, folderPath, Path.GetFileNameWithoutExtension(fileName)) + '-' + DateTime.Now.ToString("yyyyMMddHHmmss") + ".xlsx";

                        //validamos si no existe el archivo
                        if (!System.IO.File.Exists(copiaPath))
                        {
                            //inicializamos el stream para instanciar el objeto
                            using (FileStream stream = new FileStream(copiaPath, FileMode.Create))
                            {
                                //se hace la copia
                                archivoExcel.CopyTo(stream);
                            }

                            //leer copia recien creada y obtener info
                            string excelConnection = _configuration["ConnectionStringExcel:value"] + copiaPath;
                            ML.Result resultExcel = BL.Usuario.CovertExcelToDatabase(excelConnection);

                            //si la lectura fue correcta
                            if (resultExcel.Correct)
                            {
                                //validar si los datos son correctos, que no tengan vacios o espacios, guardando los errrores
                                ML.Result resultValidacionExcel = BL.Usuario.ValidarExcel(resultExcel.Objects);
                                //si NO tiene errores
                                if (resultValidacionExcel.Objects.Count==0)
                                {
                                    //guardamos la sesion con la copia
                                    resultValidacionExcel.Correct = true;
                                    HttpContext.Session.SetString("PathArchivo", copiaPath);
                                }
                                //retornamos el result a la misma vista
                                return View("CargaMasiva", resultValidacionExcel);
                            }
                            else
                            {
                                ViewBag.Message = "Ocurrió un error al leer el archivo";
                                return PartialView("Modal");
                            }
                        }
                    }
                }
            }
            else
            {
                //obtenemos la sesión antes guardada
                string rutaArchivoExcel = HttpContext.Session.GetString("PathArchivo");
                //obtenemos la cadena de conexion del excel
                string connectionString = _configuration["ConnectionStringExcel:value"] + rutaArchivoExcel;

                //convertimos los datos de excel a objetos para la DB
                ML.Result resultData = BL.Usuario.CovertExcelToDatabase(connectionString);

                if (resultData.Correct)
                {
                    //instanciamos el result que retornará los errores pero de DB
                    ML.Result resultErrores = new ML.Result();
                    resultErrores.Objects = new List<object>();

                    //recorremos los datos leidos para agregarlos a la DB
                    foreach (ML.Usuario usuarioItem in resultData.Objects)
                    {
                        ML.Result resultAdd = BL.Usuario.Add(usuarioItem);
                        //si hubo un error, lo añadimos a la lista de errorres
                        if (!resultAdd.Correct)
                        {
                            resultErrores.Objects.Add($"== No se insertó el Usuario con nombre: {usuarioItem.Nombre} {usuarioItem.ApellidoPaterno} {usuarioItem.ApellidoMaterno} | Error: {resultAdd.ErrorMessage} \n");
                        }
                    }
                    //si la lista tiene errores
                    if (resultErrores.Objects.Count > 0)
                    {
                        //string fileError = Path.Combine(_hostingEnvironment.WebRootPath, @"~\Files\logErrores.txt");
                        //obtenemos el path para guardar en un archivo txt
                        string fileError = _hostingEnvironment.WebRootPath + @"\Files\logErrores.txt";

                        using (StreamWriter writer = new StreamWriter(fileError))
                        {
                            foreach (string ln in resultErrores.Objects)
                            {
                                //escribimos linea x linea los errores en el archivo
                                writer.WriteLine(ln);
                            }
                        }
                        ViewBag.Message = "Los usuarios NO han sido registrados correctamente";
                    }
                    else
                    {
                        ViewBag.Message = "Los usuarios han sido registrados correctamente";
                    }
                }
            }

            return PartialView("Modal");
        }
    }
}
