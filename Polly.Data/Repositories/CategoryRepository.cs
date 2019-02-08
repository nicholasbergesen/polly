using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public class CategoryRepository : ICategoryRepository
    {
        public async Task<Category> Create(string description)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                var category = context.Category.Add(new Category() { Description = description });
                await context.SaveChangesAsync();
                return category;
            }
        }

        public bool TryGet(string description, out Category category)
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                category = context.Category.FirstOrDefault(x => x.Description == description);
            }

            return category != default(Category);
        }
    }
}
