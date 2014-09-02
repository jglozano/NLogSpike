using System;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace ElmahR.Client
{
    public static class W3
    {
        public static string ToW3FormEncoded(NameValueCollection collection)
        {
            return W3FormEncode(collection, null);
        }

        private static string W3FormEncode(NameValueCollection collection, string prefix)
        {
            if (collection == null)
                throw new ArgumentNullException("collection");

            if (collection.Count == 0)
                return String.Empty;

            const int size = 32766;
            var sb = new StringBuilder();

            var names = collection.AllKeys;
            for (var i = 0; i < names.Length; i++)
            {
                var name = names[i];
                var values = collection.GetValues(i);

                if (values == null)
                    continue;

                foreach (var value in values)
                {
                    var current = value;

                    if (sb.Length > 0)
                        sb.Append('&');

                    if (!String.IsNullOrEmpty(name))
                        sb.Append(name).Append('=');

                    //  This portion of the original code has been modified 
                    //  to avoid System.UriFormatException if input string size 
                    //  exceeds a limit defined by the underlying .NET fw version

                    var chunks = from index in Enumerable.Range(0, current.Length / size + 1)
                        let chunk = (index + 1) * size <= current.Length
                            ? current.Substring(index * size, size)
                            : current.Substring(index * size)
                        where chunk.Length > 0
                        select Uri.EscapeDataString(chunk);
                    chunks.Aggregate(sb, (acc, x) => acc.Append(x));

                    //  End of modified portion
                }
            }

            if (sb.Length > 0 && !String.IsNullOrEmpty(prefix))
                sb.Insert(0, prefix);

            return sb.ToString();
        }
    }
}
