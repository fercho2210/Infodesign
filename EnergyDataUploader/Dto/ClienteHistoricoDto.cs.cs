// DTOs/ClienteHistoricoDto.cs
public class ClienteHistoricoDto
{
    public List<TramoClienteDto> Residencial { get; set; }
    public List<TramoClienteDto> Comercial { get; set; }
    public List<TramoClienteDto> Industrial { get; set; }
}