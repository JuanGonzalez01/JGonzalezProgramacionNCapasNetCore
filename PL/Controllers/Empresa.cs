using Microsoft.AspNetCore.Mvc;

namespace PL.Controllers
{
    public class Empresa : Controller
    {
        [HttpGet]
        public ActionResult GetAll()
        {
            ML.Empresa empresa = new ML.Empresa();
            ML.Result result = BL.Empresa.GetAll(empresa);

            if (result.Correct)
            {
                empresa.Empresas = result.Objects;

                return View(empresa);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpPost]
        public ActionResult GetAll(ML.Empresa empresa)
        {
            ML.Result result = BL.Empresa.GetAll(empresa);

            if (result.Correct)
            {
                empresa.Empresas = result.Objects;

                return View(empresa);
            }
            else
            {
                ViewBag.Message = $"Error: {result.ErrorMessage}";
            }
            return View();
        }

        [HttpGet]
        public ActionResult Form(int? IdEmpresa)
        {
            ML.Empresa empresa = new ML.Empresa();

            if (IdEmpresa == null)
            {
                //Formulario vacio pal ADD
            }
            else
            {
                //Formulario lleno por GetByID
                ML.Result result = BL.Empresa.GetById(IdEmpresa.Value);

                if (result.Correct)
                {
                    empresa = (ML.Empresa)result.Object;
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return View(empresa);
        }

        [HttpPost]
        public ActionResult Form(ML.Empresa empresa)
        {
            IFormFile img = Request.Form.Files["IFLogo"];
            if (img != null)
            {
                byte[] imgBytes = GetBytes(img);
                empresa.Logo = Convert.ToBase64String(imgBytes);
            }

            if (empresa.IdEmpresa == 0)
            {
                ML.Result result = BL.Empresa.Add(empresa);
                if (result.Correct)
                {
                    ViewBag.Message = "Empresa agregada con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            else
            {
                //Update
                ML.Result result = BL.Empresa.Update(empresa);
                if (result.Correct)
                {
                    ViewBag.Message = "Empresa modificada con éxito.";
                }
                else
                {
                    ViewBag.Message = $"Error: {result.ErrorMessage}";
                }
            }
            return PartialView("Modal");
        }

        [HttpGet]
        public ActionResult Delete(int? IdEmpresa)
        {
            if (IdEmpresa == null)
            {
                ViewBag.Message = "Error al intentar encontrar la empresa.";
            }
            else
            {
                ML.Result result = BL.Empresa.Delete(IdEmpresa.Value);

                if (result.Correct)
                {
                    ViewBag.Message = "Empresa eliminada con éxito.";
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
