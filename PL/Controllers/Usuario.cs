using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Usuario : Controller
    {
        private readonly IConfiguration _configuration;

        private readonly Microsoft.AspNetCore.Hosting.IHostingEnvironment _hostingEnvironment;

        public Usuario(IConfiguration configuration, Microsoft.AspNetCore.Hosting.IHostingEnvironment hostingEnvironment)
        {
            _configuration = configuration;
            _hostingEnvironment = hostingEnvironment;
        }

        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Usuario usuario = new ML.Usuario();
            usuario.Rol = new ML.Rol();

            ML.Result result = BL.Usuario.GetAll(usuario);
            //ML.Result result = GetAllAPI(usuario);    API
            ML.Result resultRol = BL.Rol.GetAll();


            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                usuario.Rol.Roles = resultRol.Objects;

                return View(usuario);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetAll(ML.Usuario usuario)
        {
            //ML.Result result = BL.Usuario.GetAll(usuario);    API
            ML.Result result = GetAllAPIPost(usuario);
            ML.Result resultRol = BL.Rol.GetAll();

            if (result.Correct)
            {
                usuario.Usuarios = result.Objects;
                usuario.Rol.Roles = resultRol.Objects;

                return View(usuario);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int? idUsuario)
        {
            ML.Usuario usuario = new ML.Usuario();

            ML.Result resultRol = BL.Rol.GetAll();
            ML.Result resultPaises = BL.Pais.GetAll();

            if (idUsuario == null)
            {
                //Formulario vacio pal ADD
                usuario.Rol = new ML.Rol();

                usuario.Direccion = new ML.Direccion();
                usuario.Direccion.Colonia = new ML.Colonia();
                usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
            }
            else
            {
                //Formulario lleno por GetByID
                //ML.Result result = BL.Usuario.GetById(idUsuario.Value); <--------- API
                ML.Result result = GetByIdAPI(idUsuario.Value);

                if (result.Correct)
                {
                    usuario = (ML.Usuario)result.Object;

                    usuario.Direccion.Colonia.Colonias = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio).Objects;
                    usuario.Direccion.Colonia.Municipio.Municipios = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado).Objects;
                    usuario.Direccion.Colonia.Municipio.Estado.Estados = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais).Objects;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }

            usuario.Rol.Roles = resultRol.Objects;
            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;

            return View(usuario);
        }

        [HttpPost]
        public ActionResult Form(ML.Usuario usuario)
        {
            IFormFile img = Request.Form.Files["IFImagen"];
            if (img != null)
            {
                byte[] imgBytes = GetBytes(img);
                usuario.Imagen = Convert.ToBase64String(imgBytes);
            }

            if (ModelState.IsValid)
            {
                if (usuario.IdUsuario == 0)
                {
                    //ML.Result result = BL.Usuario.Add(usuario);   <-------- API
                    ML.Result result = AddAPI(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Message = "Usuario agregado con éxito.";
                    }
                    else
                    {
                        ViewBag.Message = $"Error: {result.ErrorMessage}";
                    }
                }
                else
                {
                    ML.Result result = UpdateAPI(usuario);
                    //ML.Result result = BL.Usuario.Update(usuario);
                    if (result.Correct)
                    {
                        ViewBag.Message = "Usuario modificado con éxito.";
                    }
                    else
                    {
                        ViewBag.Message = $"Error: {result.ErrorMessage}";
                    }
                }
                return PartialView("Modal");
            }
            else
            {
                ML.Result resultRol = BL.Rol.GetAll();
                ML.Result resultPaises = BL.Pais.GetAll();

                usuario.Direccion.Colonia.Colonias = BL.Colonia.GetByIdMunicipio(usuario.Direccion.Colonia.Municipio.IdMunicipio).Objects;
                usuario.Direccion.Colonia.Municipio.Municipios = BL.Municipio.GetByIdEstado(usuario.Direccion.Colonia.Municipio.Estado.IdEstado).Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Estados = BL.Estado.GetByIdPais(usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais).Objects;

                usuario.Rol.Roles = resultRol.Objects;
                usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = resultPaises.Objects;

                return View(usuario);
            }
        }

        [HttpGet]
        public ActionResult Delete(int? IdUsuario)
        {
            if (IdUsuario == null)
            {
                ViewBag.Message = "Error al intentar encontrar al usuario.";
            }
            else
            {
                //ML.Result result = BL.Usuario.Delete(IdUsuario.Value);
                ML.Result result = DeleteAPI(IdUsuario.Value);

                if (result.Correct)
                {
                    ViewBag.Message = "Usuario eliminado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }


        public JsonResult GetEstado(int IdPais)
        {
            var result = BL.Estado.GetByIdPais(IdPais);

            return Json(result.Objects);
        }

        public JsonResult GetMunicipio(int IdEstado)
        {
            var result = BL.Municipio.GetByIdEstado(IdEstado);

            return Json(result.Objects);
        }

        public JsonResult GetColonia(int IdMunicipio)
        {
            var result = BL.Colonia.GetByIdMunicipio(IdMunicipio);

            return Json(result.Objects);
        }

        public byte[] GetBytes(IFormFile imagen)
        {
            using var reader = imagen.OpenReadStream();
            byte[] arreglo = new byte[reader.Length];

            reader.Read(arreglo, 0, (int)reader.Length);

            return arreglo;
        }

        public JsonResult CambiarStatus(byte idUsuario, bool status)
        {
            ML.Result result = BL.Usuario.ChangeStatus(idUsuario, status);

            return Json(result);
        }




        public ML.Result GetAllAPI(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    var responseTask = client.GetAsync("Usuario/GetAll");

                    responseTask.Wait();

                    var resultServicio = responseTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        var readTask = resultServicio.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();

                        foreach (var resultItem in readTask.Result.Objects)
                        {
                            ML.Usuario resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(resultItem.ToString());
                            result.Objects.Add(resultItemList);
                        }

                        result.Correct = true;
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public ML.Result GetAllAPIPost(ML.Usuario usuario)
        {
            usuario = InicializarUsuario(usuario);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/GetAll", usuario);

                    postTask.Wait();

                    var resultServicio = postTask.Result;

                    if (resultServicio.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = resultServicio.StatusCode.ToString();
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
            }

            return result;
        }

        public ML.Result GetByIdAPI(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var responseTask = client.GetAsync("Usuario/GetbyId/" + idUsuario);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        var readTask = resultAPI.Content.ReadAsAsync<ML.Result>();
                        readTask.Wait();
                        ML.Usuario resultItemList = new ML.Usuario();
                        resultItemList = Newtonsoft.Json.JsonConvert.DeserializeObject<ML.Usuario>(readTask.Result.Object.ToString());
                        result.Object = resultItemList;

                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Departamento";
                    }

                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }

        public ML.Result UpdateAPI(ML.Usuario usuario)
        {
            usuario = InicializarUsuario(usuario);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PutAsJsonAsync<ML.Usuario>("Usuario/Update", usuario);
                    postTask.Wait();

                    var resultAPI = postTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Departamento";
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public ML.Result AddAPI(ML.Usuario usuario)
        {
            usuario = InicializarUsuario(usuario);
            ML.Result result = new ML.Result();
            result.Objects = new List<object>();

            try
            {
                using (var client = new HttpClient())
                {
                    string urlAPI = _configuration["UrlAPI"];
                    client.BaseAddress = new Uri(urlAPI);

                    //HTTP POST
                    var postTask = client.PostAsJsonAsync<ML.Usuario>("Usuario/Add", usuario);
                    postTask.Wait();

                    var resultAPI = postTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Departamento";
                    }

                }
            }
            catch (Exception ex)
            {

            }

            return result;
        }

        public ML.Result DeleteAPI(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                string urlAPI = _configuration["UrlAPI"];
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(urlAPI);
                    var responseTask = client.DeleteAsync("Usuario/Delete/" + idUsuario);
                    responseTask.Wait();
                    var resultAPI = responseTask.Result;
                    if (resultAPI.IsSuccessStatusCode)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No existen registros en la tabla Departamento";
                    }

                }
            }

            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;

            }

            return result;
        }


        public ML.Usuario InicializarUsuario(ML.Usuario usuario)
        {
            usuario.Nombre = (usuario.Nombre == null) ? "" : usuario.Nombre;
            usuario.ApellidoPaterno = (usuario.ApellidoPaterno == null) ? "" : usuario.ApellidoPaterno;
            usuario.ApellidoMaterno = (usuario.ApellidoMaterno == null) ? "" : usuario.ApellidoMaterno;
            usuario.FechaNacimiento = (usuario.FechaNacimiento == null) ? "" : usuario.FechaNacimiento;
            usuario.Sexo = (usuario.Sexo == 0) ? 'X' : usuario.Sexo;
            usuario.Telefono = (usuario.Telefono == null) ? "" : usuario.Telefono;
            usuario.Password = (usuario.Password == null) ? "" : usuario.Password;
            usuario.UserName = (usuario.UserName == null) ? "" : usuario.UserName;
            usuario.Email = (usuario.Email == null) ? "" : usuario.Email;
            usuario.Celular = (usuario.Celular == null) ? "" : usuario.Celular;
            usuario.CURP = (usuario.CURP == null) ? "" : usuario.CURP;
            usuario.Imagen = (usuario.Imagen == null) ? "" : usuario.Imagen;

            usuario.Usuarios = new List<object>();

            usuario.Rol = (usuario.Rol == null) ? new ML.Rol() : usuario.Rol;
            usuario.Rol.Nombre = (usuario.Rol.Nombre == null) ? "" : usuario.Rol.Nombre;
            usuario.Rol.Roles = new List<object>();

            usuario.Direccion = (usuario.Direccion == null) ? new ML.Direccion() : usuario.Direccion;
            usuario.Direccion.Calle = (usuario.Direccion.Calle == null) ? "" : usuario.Direccion.Calle;
            usuario.Direccion.NumeroExterior = (usuario.Direccion.NumeroExterior == null) ? "" : usuario.Direccion.NumeroExterior;
            usuario.Direccion.NumeroInterior = (usuario.Direccion.NumeroInterior == null) ? "" : usuario.Direccion.NumeroInterior;
            usuario.Direccion.Direcciones = new List<object>();

            usuario.Direccion.Colonia = (usuario.Direccion.Colonia == null) ? new ML.Colonia() : usuario.Direccion.Colonia;
            usuario.Direccion.Colonia.Nombre = (usuario.Direccion.Colonia.Nombre == null) ? "" : usuario.Direccion.Colonia.Nombre;
            usuario.Direccion.Colonia.Colonias = new List<object>();

            usuario.Direccion.Colonia.Municipio = (usuario.Direccion.Colonia.Municipio == null) ? new ML.Municipio() : usuario.Direccion.Colonia.Municipio;
            usuario.Direccion.Colonia.Municipio.Nombre = (usuario.Direccion.Colonia.Municipio.Nombre == null) ? "" : usuario.Direccion.Colonia.Municipio.Nombre;
            usuario.Direccion.Colonia.Municipio.Municipios = new List<object>();

            usuario.Direccion.Colonia.Municipio.Estado = (usuario.Direccion.Colonia.Municipio.Estado == null) ? new ML.Estado() : usuario.Direccion.Colonia.Municipio.Estado;
            usuario.Direccion.Colonia.Municipio.Estado.Nombre = (usuario.Direccion.Colonia.Municipio.Estado.Nombre == null) ? "" : usuario.Direccion.Colonia.Municipio.Estado.Nombre;
            usuario.Direccion.Colonia.Municipio.Estado.Estados = new List<object>();

            usuario.Direccion.Colonia.Municipio.Estado.Pais = (usuario.Direccion.Colonia.Municipio.Estado.Pais == null) ? new ML.Pais() : usuario.Direccion.Colonia.Municipio.Estado.Pais;
            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = (usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre == null) ? "" : usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre;
            usuario.Direccion.Colonia.Municipio.Estado.Pais.Paises = new List<object>();

            return usuario;
        }
    }
}
