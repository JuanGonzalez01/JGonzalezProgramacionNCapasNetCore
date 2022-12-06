using System;
using System.Collections.Generic;

namespace DL;

public partial class Aseguradora
{
    public int IdAseguradora { get; set; }

    public string Nombre { get; set; } = null!;

    public DateTime? FechaCreacion { get; set; }

    public DateTime? FechaModificacion { get; set; }

    public byte? IdUsuario { get; set; }

    public virtual Usuario? IdUsuarioNavigation { get; set; }

    //Asignadas
    public string UsuarioNombre { get; set; }
    public string ApellidoPaterno { get; set; }
    public string ApellidoMaterno { get; set; }
}
