using FoodService.Models;

namespace FoodService.Repositories.RestaurantRepository;

public interface IRestaurantRepository
{
    public Task<IList<RestaurantData>?> GetRestaurantsData();
    public Task RegisterRestaurant(RestaurantData restaurantData);
    public Task<string?> GetRestaurantUrlById(int? orderRestaurantId);
}