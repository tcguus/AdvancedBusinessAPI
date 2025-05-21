namespace AdvancedBusinessAPI.Models;

public class Moto
{
    public int Id { get; set; }
    public string Placa { get; set; }
    public string Modelo { get; set; }
    public string Status { get; set; } // disponível, manutenção, em uso
}
