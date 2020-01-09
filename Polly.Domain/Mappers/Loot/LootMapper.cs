using System.Threading.Tasks;

namespace Polly.Domain
{
    public class LootMapper : ILootMapper
    {
        public LootMapper()
        {
        }

        public Task<Data.Product> MapAndSaveJsonAsync(TakealotJson json)
        {
            throw new System.NotImplementedException();
        }

        public Task<Data.Product> MapAndSaveStringAsync(string dataString)
        {
            throw new System.NotImplementedException();
        }
    }
}
