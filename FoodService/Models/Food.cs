namespace FoodService.Models;

public class Food : BaseEntity
{
    public string Name { get; set; }
    public int PreparationTime { get; set; }
    public int Complexity { get; set; }
    public int Priority { get; set; }
    public int OrderId { get; set; }
    public int RestaurantId { get; set; }
}