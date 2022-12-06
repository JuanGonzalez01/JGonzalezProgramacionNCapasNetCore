using System;
using System.Collections.Generic;

namespace DL;

public partial class Dependiente
{
    public int IdDependiente { get; set; }

    public string NumeroEmpleado { get; set; } = null!;

    public string Nombre { get; set; } = null!;

    public string ApellidoPaterno { get; set; } = null!;

    public string? ApellidoMaterno { get; set; }

    public DateTime? FechaNacimiento { get; set; }

    public int IdEstadoCivil { get; set; }

    public string Sexo { get; set; } = null!;

    public string Telefono { get; set; } = null!;

    public string? Rfc { get; set; }

    public int IdDependienteTipo { get; set; }

    public virtual DependienteTipo IdDependienteTipoNavigation { get; set; } = null!;

    public virtual EstadoCivil IdEstadoCivilNavigation { get; set; } = null!;

    public virtual Empleado NumeroEmpleadoNavigation { get; set; } = null!;

    //Asignadas
    public string EmpleadoNombre { get; set; }
    public string EmpleadoApellidoPaterno { get; set; }
    public string EmpleadoApellidoMaterno { get; set; }
    public string EstadoCivilNombre { get; set; }
    public string DependienteTipoNombre { get; set; }


}
