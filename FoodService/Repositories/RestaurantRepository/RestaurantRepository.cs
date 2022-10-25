using System.Collections.Concurrent;
using FoodService.Models;

namespace FoodService.Repositories.RestaurantRepository;

public class RestaurantRepository : IRestaurantRepository
{
    private readonly ConcurrentBag<RestaurantData> _restaurantsData = new ConcurrentBag<RestaurantData>();

    public async Task<IList<RestaurantData>?> GetRestaurantsData()
    {
        if (_restaurantsData.IsEmpty)
        {
            return await Task.FromResult<IList<RestaurantData>?>(null);
        }

        return await Task.FromResult<IList<RestaurantData>?>(_restaurantsData.ToList());
    }
    
    public async Task RegisterRestaurant(RestaurantData restaurantData)
    {
        _restaurantsData.Add(restaurantData);
        await Task.CompletedTask;
    }

    public Task<string?> GetRestaurantUrlById(int? orderRestaurantId)
    {
        var restaurantUrl = _restaurantsData.FirstOrDefault(r => r.Id == orderRestaurantId)?.Url;
        return Task.FromResult(restaurantUrl);
    }
}