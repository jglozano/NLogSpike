using System;
using System.Collections;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using Elmah;

namespace ElmahR.Client
{
    public class ElmahRErrorLog : ErrorLog
    {
        public ElmahRErrorLog(IDictionary config)
        {
            if (config == null)
            {
                return;
            }

            Config = new ErrorLogConfig(config);
        }

        private ErrorLogConfig Config { get; set; }

        public override string Log(Error error)
        {
            if (Config == null)
            {
                return string.Empty;
            }


            var targetUrl = Config.GetTargerUrl();
            var request = (HttpWebRequest)WebRequest.Create(targetUrl);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";

            // See http://blogs.msdn.com/shitals/archive/2008/12/27/9254245.aspx
            request.ServicePoint.Expect100Continue = false;

            // The idea is to post to an url the json representation
            // of the intercepted error. We do a base 64 encoding
            // to fool the other side just in case some sort of
            // automated post validation is performed (do we have a 
            // better way to do this?). We post also the application
            // name and the handshaking token.

            var token = Config.GetSourceId();
            var payload = ErrorJson.EncodeString(error);

            var form = new NameValueCollection
            {
                {"error", Base64Encode(payload)},
                {"errorId", Guid.NewGuid().ToString()},
                {"sourceId", token},
                {"infoUrl", null},
            };

            // Get the bytes to determine
            // and set the content length.
            var data = Encoding.ASCII.GetBytes(W3.ToW3FormEncoded(form));
            request.ContentLength = data.Length;

            // Post it! (asynchronously)
            request.BeginGetRequestStream(ErrorReportingAsyncCallback(ar => PostRequest(request, ar, data)), null);

            return string.Empty;
        }

        private static void PostRequest(WebRequest request, IAsyncResult ar, byte[] data)
        {
            using (var output = request.EndGetRequestStream(ar))
            {
                output.Write(data, 0, data.Length);
            }

            request.BeginGetResponse(ErrorReportingAsyncCallback(rar => request.EndGetResponse(rar).Close()), null);
        }

        private static AsyncCallback ErrorReportingAsyncCallback(AsyncCallback callback)
        {
            return ar =>
            {
                if (ar == null)
                {
                    throw new ArgumentNullException("ar");
                }

                try
                {
                    callback(ar);
                }
                catch { }
            };
        }

        public override ErrorLogEntry GetError(string id)
        {
            throw new NotImplementedException();
        }

        public override int GetErrors(int pageIndex, int pageSize, IList errorEntryList)
        {
            throw new NotImplementedException();
        }


        public static string Base64Encode(string str)
        {
            var encbuff = Encoding.UTF8.GetBytes(str);
            return Convert.ToBase64String(encbuff);
        }

    }
}
