using Domain.Models.Response;
using Newtonsoft.Json;
using Serilog;

namespace Service.Helpers
{
    public class JsonParser
    {
        public JsonParser(){}

        public T DeserializeFile<T>(string filePath)
        {
            if (!File.Exists(filePath))
            {
                Log.Error("Can't find file: {filePath}", filePath);
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
