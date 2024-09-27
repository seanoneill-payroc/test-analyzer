using System.Text.Json;
using System.Text.Json.Serialization;

namespace Testiny.Client.Serialization;

public class TestinyIdConverter<T> : JsonConverter<T>
    where T: class
{
    public override T? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var input = reader.GetString();
        var output = TestinyId.Create(input) as T;
        return output;
    }

    public override void Write(Utf8JsonWriter writer, T value, JsonSerializerOptions options) => writer.WriteStringValue(value.ToString());
}