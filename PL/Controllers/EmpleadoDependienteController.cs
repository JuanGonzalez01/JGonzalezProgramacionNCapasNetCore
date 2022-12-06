using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class EmpleadoDependienteController : Controller
    {
        [HttpGet]
        public ActionResult EmpleadosGetAll()
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
    }
}
