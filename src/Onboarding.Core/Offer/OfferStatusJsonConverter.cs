using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Onboarding.Core.Offer;

public class OfferStatusJsonConverter : JsonConverter<OfferStatus>
{
    public override OfferStatus Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        // Read the JSON value as a string
        string statusString = reader.GetString();

        // Convert the string to an OfferStatus enum value
        OfferStatus status = (OfferStatus)Enum.Parse(typeof(OfferStatus), statusString, ignoreCase: true);

        return status;
    }

    public override void Write(Utf8JsonWriter writer, OfferStatus value, JsonSerializerOptions options)
    {
        // Convert the OfferStatus enum value to a string
        string statusString = value.ToString();

        // Write the string to the JSON output
        writer.WriteStringValue(statusString);
    }
}

