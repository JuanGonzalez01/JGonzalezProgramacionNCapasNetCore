using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Dependiente
    {
        public static ML.Result GetAll()
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Dependientes.FromSqlRaw($"DependienteGetAll").ToList();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Dependiente dependiente = new ML.Dependiente();

                            dependiente.IdDependiente = row.IdDependiente;

                            dependiente.Empleado = new ML.Empleado();
                            dependiente.Empleado.NumeroEmpleado = row.NumeroEmpleado;
                            dependiente.Empleado.Nombre = row.EmpleadoNombre;
                            dependiente.Empleado.ApellidoPaterno = row.EmpleadoApellidoPaterno;
                            dependiente.Empleado.ApellidoMaterno = row.EmpleadoApellidoMaterno;

                            dependiente.Nombre = row.Nombre;
                            dependiente.ApellidoPaterno = row.ApellidoPaterno;
                            dependiente.ApellidoMaterno = row.ApellidoMaterno;
                            dependiente.FechaNacimiento = row.FechaNacimiento.Value.ToString("dd-MM-yyyy");

                            dependiente.EstadoCivil = new ML.EstadoCivil();
                            dependiente.EstadoCivil.IdEstadoCivil = row.IdEstadoCivil;
                            dependiente.EstadoCivil.Nombre = row.EstadoCivilNombre;

                            dependiente.Sexo = char.Parse(row.Sexo);
                            dependiente.Telefono = row.Telefono;
                            dependiente.RFC = row.Rfc;

                            dependiente.DependienteTipo = new ML.DependienteTipo();
                            dependiente.DependienteTipo.IdDependienteTipo = row.IdDependienteTipo;
                            dependiente.DependienteTipo.Nombre = row.DependienteTipoNombre;

                            result.Objects.Add(dependiente);
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

        public static ML.Result GetById(int IdDependiente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Dependientes.FromSqlRaw($"DependienteGetById {IdDependiente}").AsEnumerable().FirstOrDefault();
                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        ML.Dependiente dependiente = new ML.Dependiente();

                        dependiente.IdDependiente = query.IdDependiente;

                        dependiente.Empleado = new ML.Empleado();
                        dependiente.Empleado.NumeroEmpleado = query.NumeroEmpleado;
                        dependiente.Empleado.Nombre = query.EmpleadoNombre;
                        dependiente.Empleado.ApellidoPaterno = query.EmpleadoApellidoPaterno;
                        dependiente.Empleado.ApellidoMaterno = query.EmpleadoApellidoMaterno;

                        dependiente.Nombre = query.Nombre;
                        dependiente.ApellidoPaterno = query.ApellidoPaterno;
                        dependiente.ApellidoMaterno = query.ApellidoMaterno;
                        dependiente.FechaNacimiento = query.FechaNacimiento.Value.ToString("dd-MM-yyyy");

                        dependiente.EstadoCivil = new ML.EstadoCivil();
                        dependiente.EstadoCivil.IdEstadoCivil = query.IdEstadoCivil;
                        dependiente.EstadoCivil.Nombre = query.EstadoCivilNombre;

                        dependiente.Sexo = char.Parse(query.Sexo);
                        dependiente.Telefono = query.Telefono;
                        dependiente.RFC = query.Rfc;

                        dependiente.DependienteTipo = new ML.DependienteTipo();
                        dependiente.DependienteTipo.IdDependienteTipo = query.IdDependienteTipo;
                        dependiente.DependienteTipo.Nombre = query.DependienteTipoNombre;

                        result.Object = dependiente;
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se encontró el registro.";
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

        public static ML.Result Add(ML.Dependiente dependiente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DependienteAdd '{dependiente.Empleado.NumeroEmpleado}', '{dependiente.Nombre}', '{dependiente.ApellidoPaterno}', '{dependiente.ApellidoMaterno}', '{dependiente.FechaNacimiento}', {dependiente.EstadoCivil.IdEstadoCivil}, '{dependiente.Sexo}', '{dependiente.Telefono}', '{dependiente.RFC}', {dependiente.DependienteTipo.IdDependienteTipo}");

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

        public static ML.Result Update(ML.Dependiente dependiente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DependienteUpdate {dependiente.IdDependiente}, '{dependiente.Empleado.NumeroEmpleado}', '{dependiente.Nombre}', '{dependiente.ApellidoPaterno}', '{dependiente.ApellidoMaterno}', '{dependiente.FechaNacimiento}', {dependiente.EstadoCivil.IdEstadoCivil}, '{dependiente.Sexo}', '{dependiente.Telefono}', '{dependiente.RFC}', {dependiente.DependienteTipo.IdDependienteTipo}");

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

        public static ML.Result Delete(int idDependiente)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"DependienteDelete {idDependiente}");

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
    }
}
