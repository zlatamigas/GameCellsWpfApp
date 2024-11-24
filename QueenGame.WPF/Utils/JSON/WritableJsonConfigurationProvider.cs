using Microsoft.Extensions.Configuration.Json;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.IO;

namespace QueenGame.WPF.Utils.JSON
{
    public class WritableJsonConfigurationProvider : JsonConfigurationProvider
    {
        public WritableJsonConfigurationProvider(JsonConfigurationSource source) : base(source) { }

        public override void Set(string key, string value)
        {
            base.Set(key, value);

            //Get Whole json file and change only passed key with passed value. It requires modification if you need to support change multi level json structure
            var fileFullPath = base.Source.FileProvider.GetFileInfo(base.Source.Path).PhysicalPath;
            string json = "{}";
            if (File.Exists(fileFullPath))
            {
                json = File.ReadAllText(fileFullPath);
            }
            dynamic jsonObj = JsonConvert.DeserializeObject(json);
            //-------------------
            var keys = key.Split(':');
            var splitedObject = jsonObj;
            {
                for(int i = 0; i< keys.Length-1;i++)
                {
                    if (splitedObject != null)
                    {
                        if (splitedObject[keys[i]] != null)
                        {
                            splitedObject = splitedObject[keys[i]];
                        }
                        else
                        {
                            splitedObject.Add(new JProperty(keys[i],new JObject()));
                            splitedObject = splitedObject[keys[i]];
                        }
                    }
                }
            }
            if (splitedObject != null)
            {
                splitedObject.Remove(keys[keys.Length - 1]);
                splitedObject.Add(new JProperty(keys[keys.Length - 1], value));
            }
            //-------------------
            string output = JsonConvert.SerializeObject(jsonObj, Formatting.Indented);
            File.WriteAllText(fileFullPath, output);
        }
    }
}
