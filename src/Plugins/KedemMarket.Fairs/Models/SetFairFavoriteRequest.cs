namespace KedemMarket.Fairs.Models;
public record SetFairFavoriteRequest
{
    public int FairId { get; set; }
    public bool IsFavorite { get; set; }    
}
