using System.Text.RegularExpressions;

namespace Slang123.FriendlyIds;

public static class TypeFriendlyIdExtensions
{
    private static readonly Regex InvalidChars = new(
        pattern: @"[^a-zA-Z0-9\.\-_]+",
        options: RegexOptions.Compiled);

    /// <summary>
    /// Turns a System.Type into a Swagger/OpenAPI-safe ID (or readable name).
    /// </summary>
    public static string FriendlyId(this Type type, bool fullyQualified = false)
    {
        if (type is null) throw new ArgumentNullException(nameof(type));

        // FullName can be null for generic args / open generics
        string typeName = fullyQualified ? (type.FullName ?? type.Name) : type.Name;

        // Normalize nested types: "Outer+Inner" -> "Outer.Inner"
        typeName = typeName.Replace('+', '.');

        if (type.IsGenericType)
        {
            // Strip arity suffix (e.g., "Dictionary`2")
            var tickIndex = typeName.IndexOf('`');
            if (tickIndex > 0)
                typeName = typeName[..tickIndex];

            var args = type.GetGenericArguments()
                           .Select(t => t.FriendlyId(fullyQualified));
            typeName = $"{typeName}<{string.Join(", ", args)}>";
        }

        // Replace anything not allowed by OpenAPI schema IDs
        return InvalidChars.Replace(typeName, "_");
    }
}
