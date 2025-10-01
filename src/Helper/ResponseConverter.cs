using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Nepal.Payments.Gateways.Helper
{
    public static class ResponseConverter
    {
        public static T ConvertTo<T>(object response)
        {
            if (response == null)
                return default!;
            if (response is T tValue)
                return tValue;
            if (response is string str)
            {
                return JsonConvert.DeserializeObject<T>(str)!;
            }

            if (response is JObject jObj)
            {
                return jObj.ToObject<T>()!;
            }

            if (typeof(IConvertible).IsAssignableFrom(typeof(T)))
                return (T)Convert.ChangeType(response, typeof(T));
            return (T)response;
        }
    }
}