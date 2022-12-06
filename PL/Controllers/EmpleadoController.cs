using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Empleado.GetAll();

            if (result.Correct)
            {
                ML.Empleado empleado = new ML.Empleado();
                empleado.Empleados = result.Objects;

                return View(empleado);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(string? NumeroEmpleado)
        {
            ML.Empleado empleado = new ML.Empleado();
            ML.Result resultEmpresas = BL.Empresa.GetAll(new ML.Empresa());

            if (NumeroEmpleado == null)
            {
                //Formulario vacio pal ADD
                empleado.Empresa = new ML.Empresa();
            }
            else
            {
                //Formulario lleno por GetByID
                ML.Result result = BL.Empleado.GetById(NumeroEmpleado);

                if (result.Correct)
                {
                    empleado = (ML.Empleado)result.Object;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }

            empleado.Empresa.Empresas = resultEmpresas.Objects;

            return View(empleado);
        }

        [HttpPost]
        public ActionResult Form(ML.Empleado empleado)
        {
            IFormFile img = Request.Form.Files["IFFoto"];
            if (img != null)
            {
                byte[] imgBytes = GetBytes(img);
                empleado.Foto = Convert.ToBase64String(imgBytes);
            }

            ML.Result buscarEmpleado = BL.Empleado.GetById(empleado.NumeroEmpleado);

            if (!buscarEmpleado.Correct)
            {
                ML.Result result = BL.Empleado.Add(empleado);
                if (result.Correct)
                {
                    ViewBag.Message = "Empleado agregado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            else
            {
                //Update
                ML.Result result = BL.Empleado.Update(empleado);
                if (result.Correct)
                {
                    ViewBag.Message = "Empleado modificado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(string? NumeroEmpleado)
        {
            if (NumeroEmpleado == null)
            {
                ViewBag.Message = "Error al intentar encontrar al empleado.";
            }
            else
            {
                ML.Result result = BL.Empleado.Delete(NumeroEmpleado);

                if (result.Correct)
                {
                    ViewBag.Message = "Empleado eliminado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }

        public byte[] GetBytes(IFormFile imagen)
        {
            using var reader = imagen.OpenReadStream();
            byte[] arreglo = new byte[reader.Length];

            reader.Read(arreglo, 0, (int)reader.Length);

            return arreglo;
        }
    }
}
