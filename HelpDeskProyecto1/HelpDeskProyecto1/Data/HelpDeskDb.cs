using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using HelpDeskProyecto1.Models;

namespace HelpDeskProyecto1.Data
{
    public class HelpDeskDb
    {
        private readonly string connectionString;

        public HelpDeskDb()
        {
            connectionString =
                ConfigurationManager
                .ConnectionStrings["HelpDeskConnection"]
                .ConnectionString;
        }

        public List<Usuario> ObtenerUsuarios()
        {
            List<Usuario> usuarios = new List<Usuario>();

            string sql = @"
                SELECT
                    Id,
                    Nombre,
                    Email,
                    PasswordHash,
                    Rol,
                    FechaRegistro,
                    Estado
                FROM Usuarios
                ORDER BY Id";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                conexion.Open();

                using (SqlDataReader lector =
                       comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Usuario usuario = new Usuario
                        {
                            Id = (int)lector["Id"],
                            Nombre = lector["Nombre"].ToString(),
                            Email = lector["Email"].ToString(),
                            PasswordHash = lector["PasswordHash"].ToString(),
                            Rol = lector["Rol"].ToString(),
                            FechaRegistro =
                                (System.DateTime)lector["FechaRegistro"],
                            Estado = (bool)lector["Estado"]
                        };

                        usuarios.Add(usuario);
                    }
                }
            }

            return usuarios;
        }

        public Usuario ValidarUsuario(string email, string passwordHash)
        {
            string sql = @"
        SELECT
            Id,
            Nombre,
            Email,
            PasswordHash,
            Rol,
            FechaRegistro,
            Estado
        FROM Usuarios
        WHERE Email = @Email
          AND PasswordHash = @PasswordHash
          AND Estado = 1";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue("@Email", email);
                comando.Parameters.AddWithValue("@PasswordHash", passwordHash);

                conexion.Open();

                using (SqlDataReader lector =
                       comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Usuario
                        {
                            Id = (int)lector["Id"],
                            Nombre = lector["Nombre"].ToString(),
                            Email = lector["Email"].ToString(),
                            PasswordHash = lector["PasswordHash"].ToString(),
                            Rol = lector["Rol"].ToString(),
                            FechaRegistro =
                                (System.DateTime)lector["FechaRegistro"],
                            Estado = (bool)lector["Estado"]
                        };
                    }
                }
            }

            return null;
        }
    }
}