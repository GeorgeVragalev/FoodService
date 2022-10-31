namespace FoodService.Models;

public class OrderRating
{
    public int Rating { get; set; }
    public Order Order { get; set; }
}