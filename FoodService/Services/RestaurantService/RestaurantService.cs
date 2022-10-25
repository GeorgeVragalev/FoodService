using FoodService.Helpers;
using FoodService.Models;
using FoodService.Repositories.RestaurantRepository;
using Newtonsoft.Json;

namespace FoodService.Services.RestaurantService;

public class RestaurantService : IRestaurantService
{
    private readonly IRestaurantRepository _restaurantRepository;
    
    public RestaurantService(IRestaurantRepository restaurantRepository)
    {
        _restaurantRepository = restaurantRepository;
    }

    public async Task<IList<RestaurantData>?> GetRestaurantsData()
    {
        var restaurantsData = await _restaurantRepository.GetRestaurantsData();
        return await Task.FromResult(restaurantsData);
    }

    public async Task RegisterRestaurant(RestaurantData restaurantData)
    {
        await _restaurantRepository.RegisterRestaurant(restaurantData);
    }

    public async Task<string?> GetRestaurantUrlById(int? orderRestaurantId)
    {
        return await _restaurantRepository.GetRestaurantUrlById(orderRestaurantId);
    }
}