using System.ComponentModel.DataAnnotations;

namespace EnergyDataUploader.Models
{
    public class DimTramo
    {
        [Key]
        public int TramoId { get; set; }
        public string Linea { get; set; }
        public ICollection<FactoConsumoCostosPerdidas> Factos { get; set; }
    }
}
