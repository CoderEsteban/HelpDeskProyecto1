using HelpDeskProyecto1.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;

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

        public int CrearTicket(Ticket ticket)
        {
            string sql = @"
        INSERT INTO Tickets
        (
            Titulo,
            Descripcion,
            IdUsuarioSolicitante,
            IdUsuarioAsignado,
            IdEstado,
            FechaCreacion,
            FechaActualizacion,
            Categoria
        )
        VALUES
        (
            @Titulo,
            @Descripcion,
            @IdUsuarioSolicitante,
            NULL,
            @IdEstado,
            GETDATE(),
            GETDATE(),
            @Categoria
        );

        SELECT CAST(SCOPE_IDENTITY() AS INT);";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@Titulo",
                    ticket.Titulo
                );

                comando.Parameters.AddWithValue(
                    "@Descripcion",
                    ticket.Descripcion
                );

                comando.Parameters.AddWithValue(
                    "@IdUsuarioSolicitante",
                    ticket.IdUsuarioSolicitante
                );

                comando.Parameters.AddWithValue(
                    "@IdEstado",
                    ticket.IdEstado
                );

                comando.Parameters.AddWithValue(
                    "@Categoria",
                    ticket.Categoria
                );

                conexion.Open();

                return (int)comando.ExecuteScalar();
            }
        }

        public List<Ticket> ObtenerTicketsUsuario(int usuarioId)
        {
            List<Ticket> tickets = new List<Ticket>();

            string sql = @"
        SELECT
            T.Id,
            T.Titulo,
            T.Descripcion,
            T.IdUsuarioSolicitante,
            T.IdUsuarioAsignado,
            T.IdEstado,
            T.FechaCreacion,
            T.FechaActualizacion,
            T.Categoria,
            E.Nombre AS EstadoNombre
        FROM Tickets T
        INNER JOIN EstadosTicket E
            ON T.IdEstado = E.Id
        WHERE T.IdUsuarioSolicitante = @UsuarioId
        ORDER BY T.FechaCreacion DESC";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@UsuarioId",
                    usuarioId
                );

                conexion.Open();

                using (SqlDataReader lector =
                       comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        Ticket ticket = new Ticket
                        {
                            Id = (int)lector["Id"],
                            Titulo = lector["Titulo"].ToString(),
                            Descripcion = lector["Descripcion"].ToString(),
                            IdUsuarioSolicitante =
                                (int)lector["IdUsuarioSolicitante"],

                            IdUsuarioAsignado =
                                lector["IdUsuarioAsignado"] == DBNull.Value
                                ? (int?)null
                                : (int)lector["IdUsuarioAsignado"],

                            IdEstado = (int)lector["IdEstado"],
                            FechaCreacion =
                                (DateTime)lector["FechaCreacion"],
                            FechaActualizacion =
                                (DateTime)lector["FechaActualizacion"],
                            Categoria =
                                lector["Categoria"].ToString(),
                            EstadoNombre =
                                lector["EstadoNombre"].ToString()
                        };

                        tickets.Add(ticket);
                    }
                }
            }

            return tickets;
        }

        public Ticket ObtenerTicketUsuario(int ticketId, int usuarioId)
        {
            string sql = @"
        SELECT
            T.Id,
            T.Titulo,
            T.Descripcion,
            T.IdUsuarioSolicitante,
            T.IdUsuarioAsignado,
            T.IdEstado,
            T.FechaCreacion,
            T.FechaActualizacion,
            T.Categoria,
            E.Nombre AS EstadoNombre
        FROM Tickets T
        INNER JOIN EstadosTicket E
            ON T.IdEstado = E.Id
        WHERE T.Id = @TicketId
          AND T.IdUsuarioSolicitante = @UsuarioId";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando = new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue("@TicketId", ticketId);
                comando.Parameters.AddWithValue("@UsuarioId", usuarioId);

                conexion.Open();

                using (SqlDataReader lector = comando.ExecuteReader())
                {
                    if (lector.Read())
                    {
                        return new Ticket
                        {
                            Id = (int)lector["Id"],
                            Titulo = lector["Titulo"].ToString(),
                            Descripcion = lector["Descripcion"].ToString(),
                            IdUsuarioSolicitante =
                                (int)lector["IdUsuarioSolicitante"],

                            IdUsuarioAsignado =
                                lector["IdUsuarioAsignado"] == DBNull.Value
                                ? (int?)null
                                : (int)lector["IdUsuarioAsignado"],

                            IdEstado = (int)lector["IdEstado"],
                            FechaCreacion =
                                (DateTime)lector["FechaCreacion"],
                            FechaActualizacion =
                                (DateTime)lector["FechaActualizacion"],
                            Categoria =
                                lector["Categoria"].ToString(),
                            EstadoNombre =
                                lector["EstadoNombre"].ToString()
                        };
                    }
                }
            }

            return null;
        }

        public List<Comentario> ObtenerComentariosTicket(int ticketId)
        {
            List<Comentario> comentarios =
                new List<Comentario>();

            string sql = @"
        SELECT
            C.Id,
            C.IdTicket,
            C.IdUsuario,
            C.Texto,
            C.Fecha,
            U.Nombre AS NombreUsuario
        FROM Comentarios C
        INNER JOIN Usuarios U
            ON C.IdUsuario = U.Id
        WHERE C.IdTicket = @TicketId
        ORDER BY C.Fecha ASC";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@TicketId",
                    ticketId
                );

                conexion.Open();

                using (SqlDataReader lector =
                       comando.ExecuteReader())
                {
                    while (lector.Read())
                    {
                        comentarios.Add(
                            new Comentario
                            {
                                Id = (int)lector["Id"],
                                IdTicket = (int)lector["IdTicket"],
                                IdUsuario = (int)lector["IdUsuario"],
                                Texto = lector["Texto"].ToString(),
                                Fecha = (DateTime)lector["Fecha"],
                                NombreUsuario =
                                    lector["NombreUsuario"].ToString()
                            }
                        );
                    }
                }
            }

            return comentarios;
        }

        public void AgregarComentario(Comentario comentario)
        {
            string sql = @"
        INSERT INTO Comentarios
        (
            IdTicket,
            IdUsuario,
            Texto,
            Fecha
        )
        VALUES
        (
            @IdTicket,
            @IdUsuario,
            @Texto,
            GETDATE()
        )";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@IdTicket",
                    comentario.IdTicket
                );

                comando.Parameters.AddWithValue(
                    "@IdUsuario",
                    comentario.IdUsuario
                );

                comando.Parameters.AddWithValue(
                    "@Texto",
                    comentario.Texto
                );

                conexion.Open();

                comando.ExecuteNonQuery();
            }
        }

        public List<Ticket> ObtenerTodosTickets()
        {
            List<Ticket> tickets = new List<Ticket>();

            string sql = @"
        SELECT
            T.Id,
            T.Titulo,
            T.Descripcion,
            T.IdUsuarioSolicitante,
            T.IdUsuarioAsignado,
            T.IdEstado,
            T.FechaCreacion,
            T.FechaActualizacion,
            T.Categoria,
            E.Nombre AS EstadoNombre,
            US.Nombre AS SolicitanteNombre,
            UA.Nombre AS AsignadoNombre
        FROM Tickets T
        INNER JOIN EstadosTicket E
            ON T.IdEstado = E.Id
        INNER JOIN Usuarios US
            ON T.IdUsuarioSolicitante = US.Id
        LEFT JOIN Usuarios UA
            ON T.IdUsuarioAsignado = UA.Id
        ORDER BY T.FechaCreacion DESC";

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
                        tickets.Add(new Ticket
                        {
                            Id = (int)lector["Id"],
                            Titulo = lector["Titulo"].ToString(),
                            Descripcion = lector["Descripcion"].ToString(),
                            IdUsuarioSolicitante =
                                (int)lector["IdUsuarioSolicitante"],

                            IdUsuarioAsignado =
                                lector["IdUsuarioAsignado"] == DBNull.Value
                                    ? (int?)null
                                    : (int)lector["IdUsuarioAsignado"],

                            IdEstado = (int)lector["IdEstado"],
                            FechaCreacion =
                                (DateTime)lector["FechaCreacion"],
                            FechaActualizacion =
                                (DateTime)lector["FechaActualizacion"],
                            Categoria =
                                lector["Categoria"].ToString(),
                            EstadoNombre =
                                lector["EstadoNombre"].ToString(),
                            SolicitanteNombre =
                                lector["SolicitanteNombre"].ToString(),
                            AsignadoNombre =
                                lector["AsignadoNombre"] == DBNull.Value
                                    ? ""
                                    : lector["AsignadoNombre"].ToString()
                        });
                    }
                }
            }

            return tickets;
        }

        public List<EstadoTicket> ObtenerEstados()
        {
            List<EstadoTicket> estados =
                new List<EstadoTicket>();

            string sql = @"
        SELECT Id, Nombre
        FROM EstadosTicket
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
                        estados.Add(new EstadoTicket
                        {
                            Id = (int)lector["Id"],
                            Nombre = lector["Nombre"].ToString()
                        });
                    }
                }
            }

            return estados;
        }

        public bool AsignarTicket(
            int ticketId,
            int usuarioId)
        {
            string sql = @"
        UPDATE Tickets
        SET
            IdUsuarioAsignado = @UsuarioId,
            FechaActualizacion = GETDATE()
        WHERE Id = @TicketId";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@TicketId",
                    ticketId
                );

                comando.Parameters.AddWithValue(
                    "@UsuarioId",
                    usuarioId
                );

                conexion.Open();

                return comando.ExecuteNonQuery() > 0;
            }
        }

        public bool CambiarEstado(
            int ticketId,
            int estadoId)
        {
            string sql = @"
        UPDATE Tickets
        SET
            IdEstado = @EstadoId,
            FechaActualizacion = GETDATE()
        WHERE Id = @TicketId";

            using (SqlConnection conexion =
                   new SqlConnection(connectionString))
            {
                SqlCommand comando =
                    new SqlCommand(sql, conexion);

                comando.Parameters.AddWithValue(
                    "@TicketId",
                    ticketId
                );

                comando.Parameters.AddWithValue(
                    "@EstadoId",
                    estadoId
                );

                conexion.Open();

                return comando.ExecuteNonQuery() > 0;
            }
        }
    }
}