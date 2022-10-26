using Domain.Models.Response;
using Newtonsoft.Json;

namespace Service.Helpers
{
    public static class JsonParser
    {
        public static T DeserializeFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new ResponseException("Can't find requested file");
            }

            using(var sr = File.OpenText(filePath))
            {
                using (JsonTextReader jsonReader = new JsonTextReader(sr))
                {
                    var serializer = new JsonSerializer();
                    var returnObject = serializer.Deserialize<T>(jsonReader);

                    return returnObject;
                }
            }
        }
    }
}
