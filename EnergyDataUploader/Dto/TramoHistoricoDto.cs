// DTOs/TramoHistoricoDto.cs
public class TramoHistoricoDto
{
    public string Linea { get; set; }

    public int FechaId { get; set; }
    public int TramoId { get; set; }
    public DateTime Fecha { get; set; }
    public decimal ConsumoResidencial { get; set; }
    public decimal ConsumoComercial { get; set; }
    public decimal ConsumoIndustrial { get; set; }
    public decimal CostoResidencial { get; set; }
    public decimal CostoComercial { get; set; }
    public decimal CostoIndustrial { get; set; }
    public decimal PerdidasResidenciales { get; set; }
    public decimal PerdidasComerciales { get; set; }
    public decimal PerdidasIndustriales { get; set; }
}