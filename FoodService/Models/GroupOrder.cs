namespace FoodService.Models;

public class GroupOrder : BaseEntity
{
    public int ClientId { get; set; }
    public IList<Order> Orders { get; set; }
}