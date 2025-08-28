using EnergyDataUploader.Models;

public interface IReportRepository
{    
    Task<List<FactoConsumoCostosPerdidas>> GetConsumoHistorico(DateTime fechaInicial, DateTime fechaFinal);

    Task<ClienteHistoricoDto> GetConsumoPorCliente(DateTime fechaInicial, DateTime fechaFinal);

    Task<TopPerdidasDto> GetTop20PerdidasPorCliente(DateTime fechaInicial, DateTime fechaFinal);

}