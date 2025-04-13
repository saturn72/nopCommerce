namespace KedemMarket.Fairs.Models;

public record FairListApiModel
{
    public IEnumerable<FairApiModel> Fairs { get; set; }
}
