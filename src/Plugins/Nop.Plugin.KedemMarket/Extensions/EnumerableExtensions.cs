using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic;

public static class EnumerableExtensions
{
    public static bool IsNullOrEmpty<T>([NotNullWhen(false)] this IEnumerable<T>? value)
    {
        return value == null || !value.Any();
    }

    public static bool NotNullAndNotNotEmpty<T>([NotNullWhen(true)] this IEnumerable<T> value) =>
        value != null && value.Any();


    public static void ThrowIfNullOrEmpty<T>(
    [NotNull]this IEnumerable<T> value, 
    string? paramName, 
    string? message = null)
    {
        if (value == null)
            throw new ArgumentNullException(paramName);
        if (!value.Any())
            throw new ArgumentException(paramName, message);
    }
}