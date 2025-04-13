using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using AgendaContactos.Models;

namespace AgendaContactos.Data
{
    public class ContactoRepository
    {
        private readonly string _dbPath = "agenda.db";

        public ContactoRepository()
        {
            if (!File.Exists(_dbPath))
            {
                SQLiteConnection.CreateFile(_dbPath);
            }

            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();

            const string sql = @"CREATE TABLE IF NOT EXISTS Contactos (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Nombre TEXT NOT NULL,
                                Telefono TEXT,
                                Email TEXT)";

            new SQLiteCommand(sql, conn).ExecuteNonQuery();
        }

        public void AddContacto(Contacto contacto)
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();

            const string sql = @"INSERT INTO Contactos (Nombre, Telefono, Email)
                                VALUES (@nombre, @telefono, @email)";

            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@nombre", contacto.Nombre);
            cmd.Parameters.AddWithValue("@telefono", contacto.Telefono ?? (object)DBNull.Value);
            cmd.Parameters.AddWithValue("@email", contacto.Email ?? (object)DBNull.Value);
            cmd.ExecuteNonQuery();
        }

        public List<Contacto> GetContactos()
        {
            var contactos = new List<Contacto>();

            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();

            const string sql = "SELECT * FROM Contactos";
            using var cmd = new SQLiteCommand(sql, conn);
            using var reader = cmd.ExecuteReader();

            while (reader.Read())
            {
                contactos.Add(new Contacto
                {
                    Id = Convert.ToInt32(reader["Id"]),
                    Nombre = reader["Nombre"].ToString() ?? "",
                    Telefono = reader["Telefono"]?.ToString(),
                    Email = reader["Email"]?.ToString()
                });
            }

            return contactos;
        }
        public void DeleteContacto(int id)
        {
            using var conn = new SQLiteConnection($"Data Source={_dbPath};Version=3;");
            conn.Open();

            const string sql = "DELETE FROM Contactos WHERE Id = @id";
            using var cmd = new SQLiteCommand(sql, conn);
            cmd.Parameters.AddWithValue("@id", id);
            cmd.ExecuteNonQuery();
        }

    }
}