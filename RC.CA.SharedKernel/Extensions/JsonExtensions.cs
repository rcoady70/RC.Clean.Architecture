﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace RC.CA.SharedKernel.Extensions;
public static class JsonExtensions
{
    private static readonly JsonSerializerOptions _jsonOptions = new JsonSerializerOptions
    {
        PropertyNameCaseInsensitive = true
    };

    public static T FromJsonExt<T>(this string json) => JsonSerializer.Deserialize<T>(json, _jsonOptions);

    public static string ToJsonExt<T>(this T obj) => JsonSerializer.Serialize<T>(obj, _jsonOptions);
}
