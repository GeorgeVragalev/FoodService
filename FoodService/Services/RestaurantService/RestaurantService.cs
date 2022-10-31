using System.Text;
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
    
    public async Task SubmitRating(OrderRating orderRating, string? url)
    {
        try
        {
            var json = JsonConvert.SerializeObject(orderRating);
            var data = new StringContent(json, Encoding.UTF8, "application/json");

            using var client = new HttpClient();

            Console.WriteLine($"Order rating for order id {orderRating.Order.Id} submitted to restaurant {orderRating.Order.RestaurantId}");
            await client.PostAsync(url, data);
        }
        catch (Exception e)
        {
            PrintConsole.Write(" Failed to submit rating", ConsoleColor.DarkRed);
        }
    }
}