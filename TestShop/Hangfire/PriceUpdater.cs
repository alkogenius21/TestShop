using TestShop.Database;

namespace TestShop.Hangfire
{
    public class PriceUpdater
    {
        private readonly ApplicationDbContext _dbContext;

        public PriceUpdater(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task UpdatePrices()
        {
            var materials = _dbContext.Materials.ToList();

            var random = new Random();

            foreach (var material in materials)
            {
                // Генерация случайной цены в пределах от 1 до 100 единиц
                material.Price = random.Next(1, 101);
            }

            await _dbContext.SaveChangesAsync(); // Используйте async версию для сохранения изменений
        }
    }

}
