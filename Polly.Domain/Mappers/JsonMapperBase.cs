using Newtonsoft.Json;
using System.Text.Json;

namespace Polly.Domain
{
    public abstract class JsonMapperBase<T>
    {
        protected abstract Task<Data.Product> MapInternal(T jsonObject);
        protected abstract bool IsValid(T jsonObject);

        protected bool TryDeserialize(string jsonString, out T jsonObject)
        {
            try
            {
                jsonObject = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (Newtonsoft.Json.JsonException)
            {
                jsonObject = default;
                return false;
            }

            return true;
        }
    }
}
