using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Aseguradora
    {
        public static ML.Result Add(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AseguradoraAdd '{aseguradora.Nombre}', {aseguradora.Usuario.IdUsuario}");
                    
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se insertó el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Delete(int idAseguradora)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AseguradoraDelete {idAseguradora}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se eliminó el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result Update(ML.Aseguradora aseguradora)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"AseguradoraUpdate {aseguradora.IdAseguradora}, '{aseguradora.Nombre}', {aseguradora.Usuario.IdUsuario}");
                    
                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se actuazlizó el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetById(int idAseguradora)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Aseguradoras.FromSqlRaw($"AseguradoraGetById {idAseguradora}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Aseguradora aseguradora = new ML.Aseguradora();

                        aseguradora.IdAseguradora = query.IdAseguradora;
                        aseguradora.Nombre = query.Nombre;
                        aseguradora.FechaCreacion = query.FechaCreacion.Value.ToString("dd-MM-yyyy");
                        aseguradora.FechaModificacion = query.FechaModificacion.Value.ToString("dd-MM-yyyy");
                        aseguradora.Usuario = new ML.Usuario();
                        aseguradora.Usuario.IdUsuario = query.IdUsuario.Value;

                        aseguradora.Usuario.Nombre = query.UsuarioNombre;
                        aseguradora.Usuario.ApellidoPaterno = query.ApellidoPaterno;
                        aseguradora.Usuario.ApellidoMaterno = query.ApellidoMaterno;



                        result.Object = aseguradora;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontró el registro";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }

        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Aseguradoras.FromSqlRaw("AseguradoraGetAll").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Aseguradora aseguradora = new ML.Aseguradora();

                            aseguradora.IdAseguradora = row.IdAseguradora;
                            aseguradora.Nombre = row.Nombre;
                            aseguradora.FechaCreacion = row.FechaCreacion.Value.ToString("dd-MM-yyyy");
                            aseguradora.FechaModificacion = row.FechaModificacion.Value.ToString("dd-MM-yyyy");
                            aseguradora.Usuario = new ML.Usuario();
                            aseguradora.Usuario.IdUsuario = row.IdUsuario.Value;
                            aseguradora.Usuario.Nombre = row.UsuarioNombre;
                            aseguradora.Usuario.ApellidoPaterno = row.ApellidoPaterno;
                            aseguradora.Usuario.ApellidoMaterno = row.ApellidoMaterno;

                            result.Objects.Add(aseguradora);
                        }
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontraron registros.";
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.Ex = ex;
                result.ErrorMessage = ex.Message;
            }
            return result;
        }
    }
}
