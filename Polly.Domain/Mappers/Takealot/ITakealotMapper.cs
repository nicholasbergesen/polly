using Polly.Data;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface IMapper
    {
        Task<Data.Product> MapAndSaveStringAsync(string dataString);

        Task<Data.Product> MapAndSaveJsonAsync(TakealotJson json);
    }

    public interface ITakealotMapper : IMapper
    {
    }

}