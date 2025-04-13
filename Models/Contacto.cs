namespace AgendaContactos.Models;

public class Contacto
{
    public int Id { get; set; }
    public required string Nombre { get; set; }  // "required" es de .NET 7/8
    public string? Telefono { get; set; }        // "?" indica que puede ser null
    public string? Email { get; set; }
}