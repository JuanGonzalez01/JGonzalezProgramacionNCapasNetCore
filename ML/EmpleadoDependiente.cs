using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class EmpleadoDependiente
    {
        public int IdEmpleadoDependiente { get; set; }
        public ML.Empleado empleado { get; set; }
        public int Cantidad { get; set; }
        public ML.Dependiente Dependiente { get; set; }
        public List<object> EmpleadoDependientes { get; set; }
        public int Total { get; set; }
    }
}
