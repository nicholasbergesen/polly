using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;

namespace Polly.Domain
{
    public interface ITakealotProcessor
    {
        Task HandleResultStringAsync(string downloadResult);
    }
}
