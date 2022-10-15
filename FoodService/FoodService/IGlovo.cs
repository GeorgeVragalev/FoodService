using FoodService.Models;

namespace FoodService.FoodService;

public interface IGlovo
{
    public void ServeOrder(GroupOrder groupOrder);
    public void DistributeOrderToRestaurants(GroupOrder groupOrder);
}