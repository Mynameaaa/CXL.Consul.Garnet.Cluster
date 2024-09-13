using System.Reflection;

namespace WWC.Consul.Check.API.Extensions;

public static class FieldValueExtensions
{

    public static object? GetFieldValue(this System.Net.EndPoint? endPoint, string fieldName, BindingFlags bindingFlags)
    {
        if (endPoint == null)
        {
            return null;
        }
        return endPoint.GetType().GetField(fieldName, bindingFlags)?.GetValue(endPoint);
    }

}
