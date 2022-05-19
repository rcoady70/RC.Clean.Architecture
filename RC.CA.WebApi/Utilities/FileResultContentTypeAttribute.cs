using System;

namespace RC.CA.WebApi.Utilities;

    /// <summary>
    /// Support for csv files
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class FileResultContentTypeAttribute : Attribute
    {
        public FileResultContentTypeAttribute(string contentType)
        {
            ContentType = contentType;
        }

        public string ContentType { get; }
    }

