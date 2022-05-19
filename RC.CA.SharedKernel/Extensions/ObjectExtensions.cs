using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using RC.CA.SharedKernel.GuardClauses;

namespace RC.CA.SharedKernel.Extensions;
public static class ObjectExtensions
{
    /// <summary>
    /// Shape data for single object performs better than the collection version (IEnumerableExtensions.ShapeData)
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="source"></param>
    /// <param name="fields"></param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException"></exception>
    /// <exception cref="Exception"></exception>
    public static ExpandoObject ShapeDataExt<TSource>(this TSource source,
         string fields)
    {
        Guard.Against.Null(source, nameof(source));

        var dataShapedObject = new ExpandoObject();

        if (string.IsNullOrWhiteSpace(fields))
        {
            // all public properties should be in the ExpandoObject 
            var propertyInfos = typeof(TSource)
                    .GetProperties(BindingFlags.IgnoreCase |
                    BindingFlags.Public | BindingFlags.Instance);

            foreach (var propertyInfo in propertyInfos)
            {
                // get the value of the property on the source object
                var propertyValue = propertyInfo.GetValue(source);

                // add the field to the ExpandoObject
                ((IDictionary<string, object>)dataShapedObject)
                    .Add(propertyInfo.Name, propertyValue);
            }

            return dataShapedObject;
        }

        // the field are separated by ",", so we split it.
        var fieldsAfterSplit = fields.Split(',');

        foreach (var field in fieldsAfterSplit)
        {
            // trim each field, as it might contain leading 
            // or trailing spaces. Can't trim the var in foreach,
            // so use another var.
            var propertyName = field.Trim();

            // use reflection to get the property on the source object
            // we need to include public and instance, b/c specifying a 
            // binding flag overwrites the already-existing binding flags.
            var propertyInfo = typeof(TSource)
                .GetProperty(propertyName,
                BindingFlags.IgnoreCase | BindingFlags.Public | BindingFlags.Instance);

            if (propertyInfo == null)
            {
                throw new Exception($"Property {propertyName} wasn't found " +
                    $"on {typeof(TSource)}");
            }

            // get the value of the property on the source object
            var propertyValue = propertyInfo.GetValue(source);

            // add the field to the ExpandoObject
            ((IDictionary<string, object>)dataShapedObject)
                .Add(propertyInfo.Name, propertyValue);
        }

        // return the list
        return dataShapedObject;
    }
}
