using Polly.Data;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface IMapper
    {
        Task<Data.Product> MapAndSaveAsync(string dataString);
    }

    public interface ITakealotMapper : IMapper
    {
    }

}