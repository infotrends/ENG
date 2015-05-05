using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Converters;

namespace InfoTrendsAPI
{
    public class JsonEnumTypeConverter : JsonConverter
    {
        public override bool CanConvert(System.Type objectType)
        {
            return objectType.IsEnum;
        }
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            writer.WriteValue(value.ToString());
        }

        public override object ReadJson(JsonReader reader, System.Type objectType, object value, JsonSerializer serializer)
        {
            return Enum.Parse(objectType, reader.Value.ToString());
        }

    }
}
