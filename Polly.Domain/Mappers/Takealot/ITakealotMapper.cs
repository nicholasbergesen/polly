using Polly.Data;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface IMapper
    {
        Task<Data.Product> MapAndSaveAsync(string dataString);
        Task<Data.Product> MapAndSaveFullAsync(TakealotJson json);
    }

    public interface ITakealotMapper : IMapper
    {
    }

}