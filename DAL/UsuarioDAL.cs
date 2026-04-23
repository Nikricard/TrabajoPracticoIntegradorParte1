using BE;
using SE;
using System.Data.SqlClient;
using System.Data;

namespace DAL
{
    public class UsuarioDAL
    {
        public Usuario Add(Usuario usuario)
        {
            var connectionString = "Server=.;Database=Usuarios;Integrated Security=True";
            //string conexion
            using (var con = new SqlConnection(connectionString)) //establecemos conexion
            {
                con.Open();
                //abrimos
                using (var cmd = new SqlCommand("SELECT COUNT(1) FROM Usuarios WHERE Nombre = @Nombre", con))
                {        //ingresamos la query en la variable cmd junto con la conexion    
                    cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = usuario.Nombre;
                    int count = Convert.ToInt32(cmd.ExecuteScalar());
                    //nos aseguramos que el usuario no exista en la db 
                    if (count > 0)
                    {
                        // Lanzamos una excepción específica que controlaremos en la interfaz
                        throw new Exception("El usuario ya existe en la base de datos.");
                    }
                }

                using (var transaccion = con.BeginTransaction())
                { //comenzamos una transaccion y la almacenamos en una variable
                    try
                    {
                        using (var cmd = new SqlCommand("Insert into Usuarios (Nombre,Contrasena) Values (@Nombre,@Contraseña)", con, transaccion))
                        {
                            //command.CommandType = CommandType.StoredProcedure;
                            //al final opte por no usar stored procedure
                            cmd.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = usuario.Nombre;
                            cmd.Parameters.Add("@Contraseña", SqlDbType.VarChar).Value = usuario.Contraseña;
                            //pasamos los parametros a la query 
                            var result = cmd.ExecuteScalar(); //ejecutamos 
                        }

                        transaccion.Commit(); //ejecutamos
                        return usuario;
                    }
                    catch (SqlException ex)
                    {
                        transaccion.Rollback(); //en caso de error
                        throw new Exception("Error de SQL.", ex);
                    }
                    catch
                    {
                        transaccion.Rollback();
                        throw;
                    }
                }
            }
        }

        public Usuario Modify(Usuario usuario)
        {
            var connectionString = "Server=.;Database=Usuarios;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = new SqlCommand("Update Usuarios set Nombre = @Nombre where Id = @Id", connection, transaction))
                    {
                        //command.CommandType = CommandType.StoredProcedure; 

                        command.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = usuario.Nombre;
                        command.Parameters.Add("@Id", SqlDbType.Int).Value = usuario.Id;

                        var result = command.ExecuteScalar();
                    }

                    transaction.Commit();
                    return usuario;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error de SQL.", ex);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Usuario Delete(Usuario usuario)
        {
            var connectionString = "Server=.;Database=Usuarios;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            connection.Open();
            using (var transaction = connection.BeginTransaction())
            {
                try
                {
                    using (var command = new SqlCommand("Delete from Usuarios where Id = @Id", connection, transaction))
                    {
                        //command.CommandType = CommandType.StoredProcedure; 

                        command.Parameters.Add("@Id", SqlDbType.Int).Value = usuario.Id;

                        var result = command.ExecuteScalar();
                    }

                    transaction.Commit();
                    return usuario;
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error de SQL.", ex);
                }
                catch
                {
                    transaction.Rollback();
                    throw;
                }
            }
        }

        public Usuario? Login(string nombre, string contrasenaHash)
        {
            var connectionString = "Server=.;Database=Usuarios;Integrated Security=True";

            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (var command = new SqlCommand(
                    "SELECT Id, Nombre FROM Usuarios WHERE Nombre = @Nombre AND Contrasena = @Contrasena",
                    connection))
                {
                    command.Parameters.Add("@Nombre", SqlDbType.VarChar).Value = nombre;
                    command.Parameters.Add("@Contrasena", SqlDbType.VarChar).Value = contrasenaHash;

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Usuario
                            {
                                Id = reader.GetInt32(0),
                                Nombre = reader.GetString(1)
                                // No devolvemos la contraseña, no es necesario
                            };
                        }
                        return null; // usuario no encontrado o contraseña incorrecta
                    }
                }
            }
        }

        public List<Usuario> GetAll()
        {
            var connectionString = "Server=.;Database=Usuarios;Integrated Security=True";
            SqlConnection connection = new SqlConnection(connectionString);
            List<Usuario> usuarios = new List<Usuario>();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("Select * from Usuarios ORDER BY Id ASC", connection);
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {//Ejecutamos un reader que almacene en una lista todos los usuarios
                    Usuario usuario = new Usuario();
                    usuario.Id = reader.GetInt32(0);
                    usuario.Nombre = reader.GetString(1);
                    usuario.Contraseña = reader.GetString(2);
                    usuarios.Add(usuario);
                }

                return usuarios; //retornamos la lista para mostrar
            }
            catch (SqlException)
            {
                throw new Exception("SQL Error.");
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
        }

    }
}
