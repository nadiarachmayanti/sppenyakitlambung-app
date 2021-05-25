using System;
using System.IO;
using System.Reflection;
using Newtonsoft.Json.Linq;

namespace sppenyakitlambung.Settings
{
    public class AppSettingsManager
    {
        private static AppSettingsManager _instance;
        private readonly JObject _secrets;

        private const string Namespace = "sppenyakitlambung";
        private const string FileName = "appsettings.json";

        public static AppSettingsManager Settings => _instance ??= new AppSettingsManager();

        private AppSettingsManager()
        {
            try
            {
                var stream = Assembly.GetAssembly(typeof(AppSettingsManager)).GetManifestResourceStream($"{Namespace}.{FileName}");

                using var reader = new StreamReader(stream ?? throw new InvalidOperationException("Cannot read settings file."));
                var json = reader.ReadToEnd();
                _secrets = JObject.Parse(json);
            }
            catch (Exception exception)
            {
                throw new InvalidOperationException($"Cannot read settings file : {exception.Message}");
            }
        }

        public string this[string name]
        {
            get
            {
                try
                {
                    var path = name.Split(':');
                    var node = _secrets[path[0]];
                    for (var index = 1; index < path.Length; index++)
                    {
                        node = node[path[index]];
                    }

                    return node.ToString();
                }
                catch (Exception)
                {
                    throw new InvalidOperationException($"Unable to retrieve setting for '{name}'");
                }
            }
        }
    }
}