using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Newtonsoft.Json;

namespace DSharpPlus.Net.Serialization
{
    internal class SnowflakeConverter : JsonConverter
    {
        public override bool CanRead => false;
        public override bool CanWrite => true;

        public override bool CanConvert(Type objectType) => objectType == typeof(ulong) || objectType == typeof(ulong?);

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
            => throw new InvalidOperationException("nop");

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            if (value is ulong?)
            {
                var nu = (ulong?)value;
                if (nu.HasValue)
                {
                    writer.WriteValue(nu.Value.ToString(CultureInfo.InvariantCulture));
                    return;
                }
            }

            writer.WriteNull();
        }
    }
}
