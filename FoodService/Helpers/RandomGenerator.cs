namespace FoodService.Helpers;

public static class RandomGenerator
{
    public static int NumberGenerator(int max)
    {
        return Random.Shared.Next(1, max);
    }
}