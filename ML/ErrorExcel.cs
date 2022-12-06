using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class ErrorMasivo
    {
        public int IdRegistro { get; set; }
        public string Mensaje { get; set; }
        public List<object> Errores { get; set; }
        public bool Correct { get; set; }
    }
}
