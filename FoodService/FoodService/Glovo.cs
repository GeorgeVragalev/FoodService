using System.Text;
using FoodService.Helpers;
using FoodService.Models;
using Newtonsoft.Json;

namespace FoodService.FoodService;

public class Glovo : IGlovo
{
    public Task ServeOrder(GroupOrder groupOrder)
    {
        throw new NotImplementedException();
    }

    public async Task DistributeOrderToRestaurants(GroupOrder groupOrder)
    {
        foreach (var order in groupOrder.Orders)
        {
            try
            {
                var json = JsonConvert.SerializeObject(order);
                var data = new StringContent(json, Encoding.UTF8, "application/json");

                var url = Settings.Settings.DiningHallUrl+"/sendorder";
                using var client = new HttpClient();

                PrintConsole.Write($"Group order {order.Id} sent to dining hall restaurant", ConsoleColor.Green);
                await client.PostAsync(url, data);
            }
            catch (Exception e)
            {
                PrintConsole.Write(Thread.CurrentThread.Name + " Failed to send order id: " + order.Id,
                    ConsoleColor.DarkRed);
            }
        }
    }
}