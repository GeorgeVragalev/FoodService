﻿using FoodService.Models;

namespace FoodService.Services.RestaurantService;

public interface IRestaurantService
{
    public Task<IList<RestaurantData>?> GetRestaurantsData();
    public Task RegisterRestaurant(RestaurantData restaurantData);
    public Task<string?> GetRestaurantUrlById(int? orderRestaurantId);
}