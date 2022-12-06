using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class DependienteController : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Dependiente.GetAll();

            if (result.Correct)
            {
                ML.Dependiente dependiente = new ML.Dependiente();
                dependiente.Dependientes = result.Objects;

                return View(dependiente);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int? IdDependiente)
        {
            ML.Dependiente dependiente = new ML.Dependiente();
            ML.Result resultEmpleados = BL.Empleado.GetAll();
            ML.Result resultEstadosCiviles = BL.EstadoCivil.GetAll();
            ML.Result resultDependienteTipos = BL.DependienteTipo.GetAll();

            if (IdDependiente == null)
            {
                //Formulario vacio pal ADD
                dependiente.Empleado = new ML.Empleado();
                dependiente.EstadoCivil = new ML.EstadoCivil();
                dependiente.DependienteTipo = new ML.DependienteTipo();
            }
            else
            {
                //Formulario lleno por GetByID
                ML.Result result = BL.Dependiente.GetById(IdDependiente.Value);

                if (result.Correct)
                {
                    dependiente = (ML.Dependiente)result.Object;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }

            foreach (ML.Empleado empleado in resultEmpleados.Objects)
            {
                empleado.Nombre = $"{empleado.Nombre} {empleado.ApellidoPaterno} {empleado.ApellidoMaterno}";
            }
            dependiente.Empleado.Empleados = resultEmpleados.Objects;
            dependiente.EstadoCivil.EstadosCiviles = resultEstadosCiviles.Objects;
            dependiente.DependienteTipo.DependienteTipos = resultDependienteTipos.Objects;

            return View(dependiente);
        }

        [HttpPost]
        public ActionResult Form(ML.Dependiente dependiente)
        {
            if (dependiente.IdDependiente == 0)
            {
                ML.Result result = BL.Dependiente.Add(dependiente);
                if (result.Correct)
                {
                    ViewBag.Message = "Dependiente agregado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            else
            {
                //Update
                ML.Result result = BL.Dependiente.Update(dependiente);
                if (result.Correct)
                {
                    ViewBag.Message = "Dependiente modificado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int? IdDependiente)
        {
            if (IdDependiente == null)
            {
                ViewBag.Message = "Error al intentar encontrar el dependiente.";
            }
            else
            {
                ML.Result result = BL.Dependiente.Delete(IdDependiente.Value);

                if (result.Correct)
                {
                    ViewBag.Message = "Dependiente eliminado con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }
    }
}
