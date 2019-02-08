using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Polly.Data
{
    public interface ICategoryRepository
    {
        bool TryGet(string description, out Category category);
        Task<Category> Create(string description);
    }
}
