﻿using System;
using System.Collections.Generic;
//using System.ComponentModel;
//using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ML
{
    public class Estado
    {
        public int IdEstado { get; set; }

        //[DisplayName("Estado:")]
        public string Nombre { get; set; }
        public List<object> Estados { get; set; }
        public ML.Pais Pais { get; set; }
    }
}
