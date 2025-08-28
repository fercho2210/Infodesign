// Models/DimFecha.cs
using System.ComponentModel.DataAnnotations;

public class DimFecha
{
    [Key]
    public int FechaId { get; set; } // Esta es la clave primaria
    public DateTime Fecha { get; set; }
    public int Anio { get; set; }
    public int Mes { get; set; }
    public int Trimestre { get; set; }
    public string DiaSemana { get; set; } = string.Empty;
}