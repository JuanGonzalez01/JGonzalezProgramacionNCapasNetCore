using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Direccion
    {
        public int IdDireccion { get; set; }

        //[Required]
        //[DisplayName("Calle:")]
        //[StringLength(50)]
        public string Calle { get; set; }

        //[DisplayName("Número interior:")]
        //[StringLength(3)]
        //[RegularExpression(@"^[0-9]*$")]
        public string NumeroInterior { get; set; }

        //[Required]
        //[DisplayName("Número exterior:")]
        //[StringLength(3)]
        //[RegularExpression(@"^[0-9]*$")]
        public string NumeroExterior { get; set; }

        public List<object> Direcciones { get; set; }
        public ML.Colonia Colonia { get; set; }

    }
}
