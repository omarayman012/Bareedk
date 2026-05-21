using System.Reflection;
using BaridikExpress.Domain.Entities.Nationality;
using BaridikExpress.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace BaridikExpress.Infrastructure.Data.Seeder.NationalitySeeder;

public static class NationalitySeeder
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        try
        {
            if (await context.Nationalities.AnyAsync())
                return;

            var assembly = typeof(NationalitySeeder).Assembly;

            var resourceNames = assembly.GetManifestResourceNames();

            foreach (var resource in resourceNames)
            {
                Console.WriteLine(resource);
            }

            var resourceName = resourceNames.FirstOrDefault(x =>
                x.EndsWith("nationalities.json",
                    StringComparison.OrdinalIgnoreCase));

            if (resourceName is null)
            {
                Console.WriteLine("nationalities.json not found");
                return;
            }

            await using var stream =
                assembly.GetManifestResourceStream(resourceName)!;

            using var reader = new StreamReader(stream);

            var json = await reader.ReadToEndAsync();

            var parsedData =
                JsonConvert.DeserializeObject<List<NationalityJsonDto>>(json);

            if (parsedData is null || parsedData.Count == 0)
                return;

            var nationalities = parsedData
                .Select(x => new Nationality
                {
                    Id = Guid.Parse(x.Id),
                    Name = x.Name,
                    CreatedAt = DateTime.UtcNow
                })
                .ToList();

            await context.Nationalities.AddRangeAsync(nationalities);

            await context.SaveChangesAsync();

            Console.WriteLine("Nationalities seeded successfully");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error seeding nationalities: {ex.Message}");

            context.ChangeTracker.Clear();
        }
    }

    private sealed class NationalityJsonDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }
}