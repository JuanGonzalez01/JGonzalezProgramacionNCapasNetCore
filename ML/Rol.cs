using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Rol
    {
        public byte IdRol { get; set; }

        //[DisplayName("Rol:")]
        public string Nombre { get; set; }

        public List<object> Roles { get; set; }
    }
}
