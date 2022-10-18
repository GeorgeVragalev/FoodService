using FoodService.Models;

namespace FoodService.FoodService;

public interface IGlovo
{
    public Task ServeOrder(GroupOrder groupOrder);
    public Task DistributeOrderToRestaurants(GroupOrder groupOrder);
}