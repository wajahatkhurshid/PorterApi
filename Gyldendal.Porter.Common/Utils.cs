using System;
using Newtonsoft.Json;

namespace Gyldendal.Porter.Common
{
    public static class Utils
    {
        public static T Deserialize<T>(string payload)
        {
            try
            {
                var obj = JsonConvert.DeserializeObject<T>(payload);
                return obj;
            }
            catch (Exception)
            {
                return default;
            }
        }
    }
}
