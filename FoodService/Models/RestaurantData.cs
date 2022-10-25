namespace FoodService.Models;

public class RestaurantData : BaseEntity
{
    public string RestaurantName { get; set; }
    public double Rating { get; set; }
    public string Url { get; set; }
    public IList<Food> Menu { get; set; }

    public RestaurantData()
    {
    }
}