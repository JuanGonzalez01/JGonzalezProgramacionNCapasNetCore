using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Aseguradora : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Result result = BL.Aseguradora.GetAll();

            if (result.Correct)
            {
                ML.Aseguradora aseguradora = new ML.Aseguradora();
                aseguradora.Aseguradoras = result.Objects;

                return View(aseguradora);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int? IdAseguradora)
        {
            ML.Aseguradora aseguradora = new ML.Aseguradora();
            ML.Result resultUsuarios = BL.Usuario.GetAll(new ML.Usuario());

            if (IdAseguradora == null)
            {
                //Formulario vacio pal ADD
                aseguradora.Usuario = new ML.Usuario();
            }
            else
            {
                //Formulario lleno por GetByID
                ML.Result result = BL.Aseguradora.GetById(IdAseguradora.Value);

                if (result.Correct)
                {
                    aseguradora = (ML.Aseguradora)result.Object;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }

            foreach (ML.Usuario usuario in resultUsuarios.Objects)
            {
                usuario.Nombre = $"{usuario.Nombre} {usuario.ApellidoPaterno} {usuario.ApellidoMaterno}";
                //usuario.Nombre = usuario.Nombre + usuario.ApellidoPaterno + usuario.ApellidoMaterno;
            }
            aseguradora.Usuario.Usuarios = resultUsuarios.Objects;

            return View(aseguradora);
        }

        [HttpPost]
        public ActionResult Form(ML.Aseguradora aseguradora)
        {
            if (aseguradora.IdAseguradora == 0)
            {
                ML.Result result = BL.Aseguradora.Add(aseguradora);
                if (result.Correct)
                {
                    ViewBag.Message = "Aseguradora agregada con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            else
            {
                //Update
                ML.Result result = BL.Aseguradora.Update(aseguradora);
                if (result.Correct)
                {
                    ViewBag.Message = "Aseguradora modificada con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int? IdAseguradora)
        {
            if (IdAseguradora == null)
            {
                ViewBag.Message = "Error al intentar encontrar la aseguradora.";
            }
            else
            {
                ML.Result result = BL.Aseguradora.Delete(IdAseguradora.Value);

                if (result.Correct)
                {
                    ViewBag.Message = "Aseguradora eliminada con éxito.";
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
