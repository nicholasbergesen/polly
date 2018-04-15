using Polly.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PollyAdmin
{
    public static class DataAccess
    {
        public static IEnumerable<Website> GetWebsites()
        {
            using (PollyDbContext context = new PollyDbContext())
            {
                return context.Website.ToList();
            }
        }
    }
}
