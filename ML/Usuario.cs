using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Usuario
    {
        public byte IdUsuario { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Nombre:")]
        //[StringLength(50)]
        public string Nombre { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Apellido paterno:")]
        //[StringLength(50)]
        public string ApellidoPaterno { get; set; }

        //[DisplayName("Apellido materno:")]
        //[StringLength(50)]
        public string ApellidoMaterno { get; set; }

        //[DisplayName("Fecha de nacimiento:")]
        public string FechaNacimiento { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Sexo:")]
        public char Sexo { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Teléfono:")]
        //[StringLength(10)]
        //[RegularExpression(@"^[0-9]*$")]
        public string Telefono { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Contraseña:")]
        //[StringLength(13)]
        public string Password { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("Nombre de usuario:")]
        //[StringLength(13)]
        public string UserName { get; set; }

        //[Required(ErrorMessage = "[Campo obligatorio].")]
        //[DisplayName("E-mail:")]
        //[StringLength(254)]
        //[RegularExpression(@"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$")]
        public string Email { get; set; }

        //[DisplayName("Celular:")]
        //[StringLength(10)]
        //[RegularExpression(@"^[0-9]*$")]
        public string Celular { get; set; }

        //[DisplayName("CURP:")]
        //[StringLength(18)]
        //[RegularExpression(@"^[A-ZÑ0-9]*$")]
        public string CURP { get; set; }

        //[DisplayName("Imagen:")]
        public string Imagen { get; set; }
        public bool Status { get; set; }


        public List<object> Usuarios { get; set; }
        public ML.Rol Rol { get; set; }
        public ML.Direccion Direccion { get; set; }
    }
}