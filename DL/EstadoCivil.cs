using System;
using System.Collections.Generic;

namespace DL;

public partial class EstadoCivil
{
    public int IdEstadoCivil { get; set; }

    public string? Nombre { get; set; }

    public virtual ICollection<Dependiente> Dependientes { get; } = new List<Dependiente>();
}
