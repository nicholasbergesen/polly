using Newtonsoft.Json;

namespace Polly.Domain
{
    public abstract class MapperBase
    {

        public MapperBase()
        {
        }

        public bool TryDeserialize<T>(string jsonString, out T jsonObject)
        {
            try
            {
                jsonObject = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException jsonException)
            {
                jsonObject = default;
                return false;
            }

            return true;
        }
    }
}
