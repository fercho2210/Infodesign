using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EnergyDataUploader.Models
{
    public class FactoConsumoCostosPerdidas
    {
        [Key]
        public int FechaId { get; set; }
        public int TramoId { get; set; }

        // Propiedades de Consumo (Wh)
        public decimal Residencial_Consumo { get; set; }
        public decimal Comercial_Consumo { get; set; }
        public decimal Industrial_Consumo { get; set; }

        // Propiedades de Costo (Costo/Wh)
        public decimal Residencial_Costo { get; set; }
        public decimal Comercial_Costo { get; set; }
        public decimal Industrial_Costo { get; set; }

        // Propiedades de Pérdidas (%)
        public decimal Residencial_Perdidas { get; set; }
        public decimal Comercial_Perdidas { get; set; }
        public decimal Industrial_Perdidas { get; set; }

        // Relaciones
    
        public DimFecha? Fecha { get; set; }
        public DimTramo? Tramo { get; set; }





    }
}
