ReadFile();

static void ReadFile()
{
    string file = @"C:\Users\digis\Documents\Juan Alberto Gonzalez Gutierrez\Lista de usuarios.txt";

    string logPath = @"C:\Users\digis\Documents\Juan Alberto Gonzalez Gutierrez\logErrores.txt";

    if (File.Exists(file))
    {
        using (StreamWriter sw = File.CreateText(logPath))
        {
            StreamReader lector = new StreamReader(file);
            string linea;
            linea = lector.ReadLine();

            int contador = 0;

            while ((linea = lector.ReadLine()) != null)
            {
                contador++;

                try
                {
                    string[] lineas = linea.Split('|');

                    ML.Usuario usuario = new ML.Usuario();

                    usuario.Nombre = lineas[0];
                    usuario.ApellidoPaterno = lineas[1];
                    usuario.ApellidoMaterno = lineas[2];
                    usuario.FechaNacimiento = lineas[3];
                    usuario.Sexo = char.Parse(lineas[4]);
                    usuario.Telefono = lineas[5];
                    usuario.UserName = lineas[6];
                    usuario.Password = lineas[7];
                    usuario.Email = lineas[8];
                    usuario.Celular = lineas[9];
                    usuario.CURP = lineas[10];

                    usuario.Rol = new ML.Rol();
                    usuario.Rol.IdRol = byte.Parse(lineas[11]);

                    usuario.Imagen = null;

                    usuario.Direccion = new ML.Direccion();
                    usuario.Direccion.Calle = lineas[12];
                    usuario.Direccion.NumeroExterior = lineas[13];
                    usuario.Direccion.NumeroInterior = lineas[14];

                    usuario.Direccion.Colonia = new ML.Colonia();
                    usuario.Direccion.Colonia.IdColonia = int.Parse(lineas[15]);

                    ML.Result result = BL.Usuario.Add(usuario);

                    if (result.Correct)
                    {
                        Console.WriteLine($"Registro {contador} correcto.");
                    }
                    else
                    {
                        sw.WriteLine($"Error Registro {contador}: {result.ErrorMessage}");
                    }
                }
                catch (Exception ex)
                {
                    sw.WriteLine($"Error Registro {contador}: {ex.Message}");
                }

            }
        }

    }
}