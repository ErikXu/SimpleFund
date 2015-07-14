using System;
using MongoDB.Bson;
using Newtonsoft.Json;

namespace SimpleFund.Web.Common.Converters
{
    public class ObjectIdConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value.ToString());
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            ObjectId result;
            ObjectId.TryParse(reader.Value as string, out result);
            return result;
        }

        public override bool CanConvert(Type objectType)
        {
            return typeof(ObjectId).IsAssignableFrom(objectType);
        }
    }
}