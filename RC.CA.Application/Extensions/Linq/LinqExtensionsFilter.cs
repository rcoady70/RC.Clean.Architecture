﻿using System.Linq.Expressions;
using System;

namespace RC.CA.Application.Extensions.Linq;
public static partial class LinqExtensionsFilter
{
    /// <summary>
    /// Build dynamic predicate
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="propertyName"></param>
    /// <param name="comparison"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    public static Expression<Func<T, bool>> BuildPredicate<T>(string propertyName, string comparison, string value)
    {
        try
        {
            var parameter = Expression.Parameter(typeof(T), "x");
            var left = propertyName.Split('.').Aggregate((Expression)parameter, Expression.Property);
            var body = MakeComparison(left, comparison, value);
            var c0 = Expression.Lambda<Func<T, bool>>(body, parameter);
            return c0;
        }
        catch (Exception ex)
        {
            throw new ArgumentException($"BuildPredicate failed with column name {propertyName} comparison {comparison} value {value}", propertyName);
        }
    }
    /// <summary>
    /// Get comparison operator
    /// </summary>
    /// <param name="left"></param>
    /// <param name="comparison"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    private static Expression MakeComparison(Expression left, string comparison, string value)
    {
        switch (comparison)
        {
            case "==":
                return MakeBinary(ExpressionType.Equal, left, value);
            case "!=":
                return MakeBinary(ExpressionType.NotEqual, left, value);
            case ">":
                return MakeBinary(ExpressionType.GreaterThan, left, value);
            case ">=":
                return MakeBinary(ExpressionType.GreaterThanOrEqual, left, value);
            case "<":
                return MakeBinary(ExpressionType.LessThan, left, value);
            case "<=":
                return MakeBinary(ExpressionType.LessThanOrEqual, left, value);
            case "contains":
            case "startswith":
            case "endswith":
                return Expression.Call(MakeString(left), comparison, Type.EmptyTypes, Expression.Constant(value, typeof(string)));
            default:
                throw new NotSupportedException($"Invalid comparison operator '{comparison}'.");
        }
    }
    /// <summary>
    /// Expression as string
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    private static Expression MakeString(Expression source)
    {
        return source.Type == typeof(string) ? source : Expression.Call(source, "ToString", Type.EmptyTypes);
    }
    /// <summary>
    /// Build binary operator
    /// </summary>
    /// <param name="type"></param>
    /// <param name="left"></param>
    /// <param name="value"></param>
    /// <returns></returns>
    private static Expression MakeBinary(ExpressionType type, Expression left, string value)
    {
        object typedValue = value;
        if (left.Type != typeof(string))
        {
            if (string.IsNullOrEmpty(value))
            {
                typedValue = null;
                if (Nullable.GetUnderlyingType(left.Type) == null)
                    left = Expression.Convert(left, typeof(Nullable<>).MakeGenericType(left.Type));
            }
            else
            {
                var valueType = Nullable.GetUnderlyingType(left.Type) ?? left.Type;
                typedValue = valueType.IsEnum ? Enum.Parse(valueType, value) :
                    valueType == typeof(Guid) ? Guid.Parse(value) :
                    Convert.ChangeType(value, valueType);
            }
        }
        var right = Expression.Constant(typedValue, left.Type);
        return Expression.MakeBinary(type, left, right);
    }


}
