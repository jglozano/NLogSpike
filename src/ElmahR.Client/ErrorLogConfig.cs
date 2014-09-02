using System;
using System.Collections;
using System.Configuration;

namespace ElmahR.Client
{
    public class ErrorLogConfig
    {
        private readonly IDictionary _config;
        internal const string GroupName = "elmah";
        internal const string GroupSlash = GroupName + "/";

        public ErrorLogConfig(IDictionary config)
        {
            _config = config;
        }

        public string GetSourceId()
        {
            return _config == null ? string.Empty : GetSetting(_config, "sourceId");
        }

        public Uri GetTargerUrl()
        {
            return new Uri(GetSetting(_config, "targetUrl"), UriKind.Absolute);
        }

        public object GetSubsection(string name)
        {
            return GetSection(GroupSlash + name);
        }

        public object GetSection(string name)
        {
            return ConfigurationManager.GetSection(name);
        }

        public string GetSetting(string name)
        {
            return GetSetting(_config, name);
        }

        private static string GetSetting(IDictionary config, string name)
        {
            var value = ((string)config[name]) ?? string.Empty;

            if (value.Length == 0)
            {
                throw new Elmah.ApplicationException(string.Format(
                    "The required configuration setting '{0}' is missing for the error posting module.", name));
            }

            return value;
        }
    }
}