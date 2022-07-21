using System.Text;

namespace RC.CA.SharedKernel.Extensions;
public static class StringBuilderExtensions
{
    /// <summary>
    /// Append formatted line
    /// </summary>
    /// <param name="str"></param>
    /// <param name="format"></param>
    /// <param name="args"></param>
    /// <returns></returns>
    public static StringBuilder AppendFormattedLineExt(this StringBuilder str,
                                                       string format,
                                                       params object[] args) =>
                                    str.AppendFormat(format, args).AppendLine();
    /// <summary>
    /// Append line based on true or false predicate. Second parameter is string builder gives flexibility on which append method to use 
    /// Example .AppendWhenExt(    
    ///                         () = > BooleanVar, 
    ///                         sb => sb.AppendLine("My string value")
    ///                       )
    /// </summary>
    /// <param name="str"></param>
    /// <param name="predicate"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static StringBuilder AppendWhenExt(this StringBuilder str,
                                              Func<bool> predicate,
                                              Func<StringBuilder, StringBuilder> func) =>
                         predicate() ? func(str) : str;
    /// <summary>
    /// Append line based on true or false predicate. Second parameter is string builder gives flexibility on which append method to use 
    /// Example .AppendCollectionExt(    
    ///                         collection, 
    ///                         (sb,opt) => sb.AppendFormattedLine("<option value="{0}">{1}</option>",opt.Key,opt.Value)
    ///                       )
    ///                       Note: type parameter not passed in code c# type inference system is string enough
    /// <typeparam name="T"></typeparam>
    /// <param name="str"></param>
    /// <param name="seq"></param>
    /// <param name="func"></param>
    /// <returns></returns>
    public static StringBuilder AppendCollectionExt<T>(this StringBuilder str,
                                                       IEnumerable<T> seq,
                                                       Func<StringBuilder, T, StringBuilder> func) =>
        seq.Aggregate(str, func);
    private static void Samples()
    {
        System.Collections.Generic.Dictionary<string, String> dic = new System.Collections.Generic.Dictionary<string, String>();
        dic.Add("IE", "Ireland");
        dic.Add("UK", "United Kingdom");
        StringBuilder sbuild = new StringBuilder();
        string result = sbuild.AppendCollectionExt(dic, (sb, opt) => sb.AppendFormattedLineExt("<option value=\"{0}\">{1}</option>", opt.Key, opt.Value)).ToString();

        sbuild.Clear();
        bool append = true;
        result = sbuild.AppendWhenExt(() => append, sb => sb.Append("Value to append if true")).ToString();

    }
}
