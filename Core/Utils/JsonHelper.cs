using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

/// <summary>
/// Provides basic JSON serialization and parsing utilities.
/// </summary>
public static class JsonHelper
{
    /// <summary>
    /// Deserializes a JSON string into an object of type T.
    /// </summary>
    public static T Deserialize<T>(string json)
    {
        if (json == null)
            throw new ArgumentNullException(nameof(json));

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string cannot be empty or whitespace", nameof(json));

        return JsonConvert.DeserializeObject<T>(json) ??
               throw new InvalidOperationException($"Deserialization to {typeof(T).Name} returned null.");
    }

    /// <summary>
    /// Serializes an object to a JSON string.
    /// </summary>
    public static string Serialize<T>(T obj, bool indented = true)
    {
        return JsonConvert.SerializeObject(obj, indented ? Formatting.Indented : Formatting.None);
    }

    /// <summary>
    /// Validates whether a string contains well-formed JSON.
    /// </summary>
    public static bool IsValidJson(string json)
    {
        if (string.IsNullOrWhiteSpace(json))
            return false;

        try
        {
            JToken.Parse(json);
            return true;
        }
        catch
        {
            return false;
        }
    }

    /// <summary>
    /// Parses a JSON string into a JObject.
    /// </summary>
    public static JObject ParseObject(string json)
    {
        if (json == null)
            throw new ArgumentNullException(nameof(json));

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string cannot be empty or whitespace", nameof(json));

        return JObject.Parse(json);
    }

    /// <summary>
    /// Parses a JSON string into a JArray.
    /// </summary>
    public static JArray ParseArray(string json)
    {
        if (json == null)
            throw new ArgumentNullException(nameof(json));

        if (string.IsNullOrWhiteSpace(json))
            throw new ArgumentException("JSON string cannot be empty or whitespace", nameof(json));

        return JArray.Parse(json);
    }

    /// <summary>
    /// Extracts a value from a JObject using a JSONPath expression.
    /// </summary>
    public static T GetValue<T>(JObject obj, string path)
    {
        if (obj == null)
            throw new ArgumentNullException(nameof(obj));

        if (string.IsNullOrWhiteSpace(path))
            throw new ArgumentException("Path cannot be empty or whitespace", nameof(path));

        var token = obj.SelectToken(path);
        return token != null ? token.Value<T>() : default!;
    }
}
