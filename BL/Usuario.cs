using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BL
{
    public class Usuario
    {
        public static ML.Result Add(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioAdd '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Sexo.ToString()}', '{usuario.Telefono}', '{usuario.UserName}', '{usuario.Password}', '{usuario.Email}', '{usuario.Celular}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroExterior}', '{usuario.Direccion.NumeroInterior}', {usuario.Direccion.Colonia.IdColonia}");

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

        public static ML.Result Delete(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioDelete {(byte)idUsuario}");

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

        public static ML.Result Update(ML.Usuario usuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioUpdate {usuario.IdUsuario}, '{usuario.Nombre}', '{usuario.ApellidoPaterno}', '{usuario.ApellidoMaterno}', '{usuario.FechaNacimiento}', '{usuario.Sexo.ToString()}', '{usuario.Telefono}', '{usuario.UserName}', '{usuario.Password}', '{usuario.Email}', '{usuario.Celular}', '{usuario.CURP}', {usuario.Rol.IdRol}, '{usuario.Imagen}', '{usuario.Status}', '{usuario.Direccion.Calle}', '{usuario.Direccion.NumeroExterior}', '{usuario.Direccion.NumeroInterior}', {usuario.Direccion.Colonia.IdColonia}");

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

        public static ML.Result GetById(int idUsuario)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetById {idUsuario}").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.IdUsuario = query.IdUsuario;
                        usuario.Nombre = query.Nombre;
                        usuario.ApellidoPaterno = query.ApellidoPaterno;
                        usuario.ApellidoMaterno = query.ApellidoMaterno;
                        usuario.FechaNacimiento = query.FechaNacimiento.ToString("dd-MM-yyyy");
                        usuario.Sexo = char.Parse(query.Sexo);
                        usuario.Telefono = query.Telefono;
                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;
                        usuario.Email = query.Email;
                        usuario.Celular = query.Celular;
                        usuario.CURP = query.Curp;

                        usuario.Rol = new ML.Rol();
                        usuario.Rol.IdRol = query.IdRol;

                        usuario.Imagen = query.Imagen;

                        usuario.Direccion = new ML.Direccion();
                        usuario.Direccion.IdDireccion = query.IdDireccion;
                        usuario.Direccion.Calle = query.Calle;
                        usuario.Direccion.NumeroExterior = query.NumeroExterior;
                        usuario.Direccion.NumeroInterior = query.NumeroInterior;

                        usuario.Direccion.Colonia = new ML.Colonia();
                        usuario.Direccion.Colonia.IdColonia = query.IdColonia;
                        usuario.Direccion.Colonia.Nombre = query.ColoniaNombre;

                        usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                        usuario.Direccion.Colonia.Municipio.IdMunicipio = query.IdMunicipio;
                        usuario.Direccion.Colonia.Municipio.Nombre = query.MunicipioNombre;

                        usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                        usuario.Direccion.Colonia.Municipio.Estado.IdEstado = query.IdEstado;
                        usuario.Direccion.Colonia.Municipio.Estado.Nombre = query.EstadoNombre;

                        usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = query.IdPais;
                        usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = query.PaisNombre;

                        result.Object = usuario;
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

        public static ML.Result GetByUserName(string userName)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetByUsername '{userName}'").AsEnumerable().FirstOrDefault();

                    if (query != null)
                    {
                        ML.Usuario usuario = new ML.Usuario();

                        usuario.UserName = query.UserName;
                        usuario.Password = query.Password;

                        result.Object = usuario;
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

        public static ML.Result GetAll(ML.Usuario usuarioBusqueda)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    if (usuarioBusqueda.Rol == null)
                    {
                        usuarioBusqueda.Rol = new ML.Rol();
                        usuarioBusqueda.Rol.IdRol = (byte)0;
                    }
                    
                    //usuarioBusqueda.Rol.IdRol = () ? (byte)0 : usuarioBusqueda.Rol.IdRol;

                    var query = context.Usuarios.FromSqlRaw($"UsuarioGetAll '{usuarioBusqueda.Nombre}', '{usuarioBusqueda.ApellidoPaterno}', {usuarioBusqueda.Rol.IdRol}").ToList();

                    result.Objects = new List<object>();

                    if (query != null)
                    {
                        foreach (var row in query)
                        {
                            ML.Usuario usuario = new ML.Usuario();

                            usuario.IdUsuario = row.IdUsuario;
                            usuario.Nombre = row.Nombre;
                            usuario.ApellidoPaterno = row.ApellidoPaterno;
                            usuario.ApellidoMaterno = row.ApellidoMaterno;
                            usuario.FechaNacimiento = row.FechaNacimiento.ToString("dd-MM-yyyy");
                            usuario.Sexo = char.Parse(row.Sexo);
                            usuario.Telefono = row.Telefono;
                            usuario.UserName = row.UserName;
                            usuario.Password = row.Password;
                            usuario.Email = row.Email;
                            usuario.Celular = row.Celular;
                            usuario.CURP = row.Curp;

                            usuario.Rol = new ML.Rol();
                            usuario.Rol.IdRol = row.IdRol;
                            usuario.Rol.Nombre = row.RolNombre;
                            usuario.Imagen = row.Imagen;
                            usuario.Status = row.Status;

                            usuario.Direccion = new ML.Direccion();
                            usuario.Direccion.IdDireccion = row.IdDireccion;
                            usuario.Direccion.Calle = row.Calle;
                            usuario.Direccion.NumeroExterior = row.NumeroExterior;
                            usuario.Direccion.NumeroInterior = row.NumeroInterior;

                            usuario.Direccion.Colonia = new ML.Colonia();
                            usuario.Direccion.Colonia.IdColonia = row.IdColonia;
                            usuario.Direccion.Colonia.Nombre = row.ColoniaNombre;

                            usuario.Direccion.Colonia.Municipio = new ML.Municipio();
                            usuario.Direccion.Colonia.Municipio.IdMunicipio = row.IdMunicipio;
                            usuario.Direccion.Colonia.Municipio.Nombre = row.MunicipioNombre;

                            usuario.Direccion.Colonia.Municipio.Estado = new ML.Estado();
                            usuario.Direccion.Colonia.Municipio.Estado.IdEstado = row.IdEstado;
                            usuario.Direccion.Colonia.Municipio.Estado.Nombre = row.EstadoNombre;

                            usuario.Direccion.Colonia.Municipio.Estado.Pais = new ML.Pais();
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.IdPais = row.IdPais;
                            usuario.Direccion.Colonia.Municipio.Estado.Pais.Nombre = row.PaisNombre;

                            result.Objects.Add(usuario);
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

        public static ML.Result CovertExcelToDatabase(string conectionString)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (OleDbConnection context = new OleDbConnection(conectionString))
                {
                    using (OleDbCommand cmd = new OleDbCommand())
                    {
                        cmd.CommandText = "SELECT * FROM [Sheet1$]";
                        cmd.Connection = context;

                        OleDbDataAdapter dataAdapter = new OleDbDataAdapter();
                        dataAdapter.SelectCommand = cmd;

                        DataTable tablaUsuario = new DataTable();
                        dataAdapter.Fill(tablaUsuario);

                        if (tablaUsuario.Rows.Count >0)
                        {
                            result.Objects = new List<object>();

                            foreach (DataRow row in tablaUsuario.Rows)
                            {
                                ML.Usuario usuario = new ML.Usuario();

                                usuario.Nombre = row[0].ToString();
                                usuario.ApellidoPaterno = row[1].ToString();
                                usuario.ApellidoMaterno = row[2].ToString();
                                usuario.FechaNacimiento = row[3].ToString();
                                usuario.Sexo = char.Parse(row[4].ToString());
                                usuario.Telefono = row[5].ToString();
                                usuario.UserName = row[6].ToString();
                                usuario.Password = row[7].ToString();
                                usuario.Email = row[8].ToString();
                                usuario.Celular = row[9].ToString();
                                usuario.CURP = row[10].ToString();

                                usuario.Rol = new ML.Rol();
                                usuario.Rol.IdRol = byte.Parse(row[11].ToString());

                                usuario.Direccion = new ML.Direccion();
                                usuario.Direccion.Calle = row[12].ToString();
                                usuario.Direccion.NumeroExterior = row[13].ToString();
                                usuario.Direccion.NumeroInterior = row[14].ToString();

                                usuario.Direccion.Colonia = new ML.Colonia();
                                usuario.Direccion.Colonia.IdColonia = int.Parse(row[15].ToString());

                                result.Objects.Add(usuario);
                            }
                            result.Correct = true;
                        }
                        else
                        {
                            result.Correct = false;
                            result.ErrorMessage = "No existen registros en el archivo";
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result ValidarExcel(List<object> Objects)
        {
            ML.Result result = new ML.Result();
            try
            {
                result.Objects = new List<object>();

                int contador = 1;
                foreach (ML.Usuario usuario in Objects)
                {
                    ML.ErrorMasivo error = new ML.ErrorMasivo();
                    error.IdRegistro = contador++;

                    //usuario.Nombre = (usuario.Nombre == "") ? error.Mensaje += "Ingresar el nombre " : usuario.Nombre;
                    if (usuario.Nombre == "" | usuario.Nombre == " ")
                    {
                        error.Mensaje += "Ingresar el nombre. ";
                    }

                    if (usuario.ApellidoPaterno == "" | usuario.ApellidoPaterno == " ")
                    {
                        error.Mensaje += "Ingresar el Apellido Paterno. ";
                    }

                    if (usuario.ApellidoMaterno == "" | usuario.ApellidoMaterno == " ")
                    {
                        error.Mensaje += "Ingresar el Apellido materno. ";
                    }

                    if (usuario.FechaNacimiento == "" | usuario.FechaNacimiento == " ")
                    {
                        error.Mensaje += "Ingresar la Fecha de nacimiento. ";
                    }

                    if (usuario.Sexo == null | usuario.Sexo == ' ')
                    {
                        error.Mensaje += "Ingresar el sexo. ";
                    }

                    if (usuario.Telefono == "" | usuario.Telefono == " ")
                    {
                        error.Mensaje += "Ingresar el telefono. ";
                    }

                    if (usuario.UserName == "" | usuario.UserName == " ")
                    {
                        error.Mensaje += "Ingresar el nombre de usuario. ";
                    }

                    if (usuario.Password == "" | usuario.Password == " ")
                    {
                        error.Mensaje += "Ingresar la contraseña. ";
                    }

                    if (usuario.Email == "" | usuario.Email == " ")
                    {
                        error.Mensaje += "Ingresar el Email. ";
                    }

                    if (usuario.Celular == "" | usuario.Celular == " ")
                    {
                        error.Mensaje += "Ingresar el Celular. ";
                    }
                    
                    if (usuario.CURP == "" | usuario.CURP == " ")
                    {
                        error.Mensaje += "Ingresar el CURP. ";
                    }

                    if (usuario.Direccion.Calle == "" | usuario.Direccion.Calle == " ")
                    {
                        error.Mensaje += "Ingresar la Calle. ";
                    }

                    if (usuario.Direccion.NumeroExterior == "" | usuario.Direccion.NumeroExterior == " ")
                    {
                        error.Mensaje += "Ingresar el Numero Exterior. ";
                    }

                    if (usuario.Direccion.NumeroInterior == "" | usuario.Direccion.NumeroInterior == " ")
                    {
                        error.Mensaje += "Ingresar el Numero Interior. ";
                    }

                    if (error.Mensaje != null)
                    {
                        result.Objects.Add(error);
                    }
                }
                result.Correct = true;
            }
            catch (Exception ex)
            {
                result.Correct = false;
                result.ErrorMessage = ex.Message;
                result.Ex = ex;
            }
            return result;
        }

        public static ML.Result ChangeStatus(byte idUsuario, bool status)
        {
            ML.Result result = new ML.Result();

            try
            {
                using (DL.JgonzalezProgramacionNcapasContext context = new DL.JgonzalezProgramacionNcapasContext())
                {
                    var query = context.Database.ExecuteSqlRaw($"UsuarioChangeStatus {idUsuario}, {status}");

                    if (query > 0)
                    {
                        result.Correct = true;
                    }
                    else
                    {
                        result.Correct = false;
                        result.ErrorMessage = "No se realizó la acción.";
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
