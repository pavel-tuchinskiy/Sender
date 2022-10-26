using Newtonsoft.Json;

namespace Service.Helpers
{
    public static class JsonParser
    {
        public static T DeserializeFile<T>(string filePath)
        {
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
