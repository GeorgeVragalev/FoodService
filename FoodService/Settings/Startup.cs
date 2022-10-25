using FoodService.FoodService;
using FoodService.Repositories.OrderListRepository;
using FoodService.Repositories.RestaurantRepository;
using FoodService.Services.OrderService;
using FoodService.Services.RestaurantService;

namespace FoodService.Settings;
public class Startup
{
    private IConfiguration ConfigRoot { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Add services to the container.
        services.AddControllers();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen();
        services.AddLogging(config => config.ClearProviders());
        
        services.AddSingleton<IOrderListRepository, OrderListRepository>();
        services.AddSingleton<IRestaurantRepository, RestaurantRepository>();
        services.AddSingleton<IOrderService, OrderService>();
        services.AddSingleton<IRestaurantService, RestaurantService>();
        services.AddSingleton<IGlovo, Glovo>();
    }

    public Startup(IConfiguration configuration)
    {
        ConfigRoot = configuration;
    }

    public void Configure(WebApplication app, IWebHostEnvironment env)
    {
        // Configure the HTTP request pipeline.
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
            app.UseHsts();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseRouting();
        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllers();
        });

        app.Run();
    }
}