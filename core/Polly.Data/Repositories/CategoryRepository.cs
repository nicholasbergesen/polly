using Microsoft.EntityFrameworkCore;

namespace Polly.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        private readonly IDbContextFactory<PollyDbContext> _contextFactory;
        public CategoryRepository(IDbContextFactory<PollyDbContext> contextFactory)
        {
            _contextFactory = contextFactory ?? throw new ArgumentNullException($"{nameof(contextFactory)} is null");
        }
        public async Task<Category> Create(string description)
        {
            using PollyDbContext context = await _contextFactory.CreateDbContextAsync();
            var category = context.Category.Add(new Category() { Description = description });
            await context.SaveChangesAsync();
            return category.Entity;
        }

        public bool TryGet(string description, out Category category)
        {
            using PollyDbContext context = _contextFactory.CreateDbContext();
            category = context.Category.FirstOrDefault(x => x.Description.Equals(description));
            return category != default(Category);
        }
    }
}
