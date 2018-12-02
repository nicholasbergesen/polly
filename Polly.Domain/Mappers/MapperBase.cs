using Newtonsoft.Json;

namespace Polly.Domain
{
    public abstract class MapperBase
    {
        protected Ilogger _logger;

        public MapperBase(Ilogger logger)
        {
            _logger = logger;
        }

        public bool TryDeserialize<T>(string jsonString, out T jsonObject)
        {
            try
            {
                jsonObject = JsonConvert.DeserializeObject<T>(jsonString);
            }
            catch (JsonException jsonException)
            {
                _logger.Log(jsonException.Message);
                jsonObject = default(T);
                return false;
            }

            return true;
        }
    }
}
