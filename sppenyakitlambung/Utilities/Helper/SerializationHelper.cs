using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Xml.Serialization;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using sppenyakitlambung.Models;
using sppenyakitlambung.Services;

namespace sppenyakitlambung.Helper
{
    /// <summary>
    /// Class to provide various functions to facilitate serialization operations.
    /// </summary>
    public class SerializationHelper
    {
        public static bool DeserializeFromJsonString(string json, Type type, out object deserializedObject)
        {
            bool success = true;

            try
            {
                var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace, TypeNameHandling = TypeNameHandling.All };
                deserializedObject = JsonConvert.DeserializeObject(json, type, serializerSettings);
            }
            catch (Exception exception)
            {
                success = false;
                deserializedObject = "";
                LoggingService.LogErrorMessage(exception, "SerializationHelper.DeserializeFromJsonString");
            }

            return success;
        }

        public static bool DeserializeFromJsonString<T>(string json, out T deserializedObject)
        {
            try
            {
                var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace, TypeNameHandling = TypeNameHandling.All };
                deserializedObject = JsonConvert.DeserializeObject<T>(json, serializerSettings);
                return true;
            }
            catch (Exception exception)
            {
                deserializedObject = default;
                LoggingService.LogErrorMessage(exception, "SerializationHelper.DeserializeFromJsonString");
                return false;
            }
        }

        public static bool DeserializeFromXmlString(string xml, Type type, out object deserializedObject)
        {
            XmlSerializer xmlSerializer = new XmlSerializer(type);
            StringReader stringReader = null;

            try
            {
                stringReader = new StringReader(xml);
                deserializedObject = xmlSerializer.Deserialize(stringReader);
                return true;
            }
            catch (Exception exception)
            {
                deserializedObject = null;
                LoggingService.LogErrorMessage(exception, "SerializationHelper.DeserializeFromXmlString()");
                return false;
            }
            finally
            {
                if (stringReader != null)
                {
                    stringReader.Close();
                }
            }
        }

        public static bool Clone(object objectToClone, out object clonedObject, string[] ignoredProperties = null)
        {
            bool success = false;
            clonedObject = null;

            if (SerializeToJsonString(objectToClone, out string serializedJsonObject, ignoredProperties))
            {
                if (DeserializeFromJsonString(serializedJsonObject, objectToClone.GetType(), out clonedObject))
                    success = true;
            }

            return success;
        }

        public static bool Clone<T>(T objectToClone, out T clonedObject, string[] ignoredProperties = null)
        {
            bool success = false;
            clonedObject = default;

            if (SerializeToJsonString(objectToClone, out string serializedJsonObject, ignoredProperties))
            {
                if (DeserializeFromJsonString<T>(serializedJsonObject, out clonedObject))
                    success = true;
            }

            return success;
        }

        public static object CreateInstance(Type instantiatedType)
        {
            try
            {
                return Activator.CreateInstance(instantiatedType);
            }
            catch (Exception exception)
            {
                LoggingService.LogErrorMessage(exception, "SerializationHelper.CreateInstance");
                return null;
            }
        }

        public static bool SerializeToJsonString(object objectToSerialize, out string serializedJsonObject, string[] ignoredProperties = null)
        {
            bool success = true;

            try
            {
                if (ignoredProperties != null)
                {
                    var jsonResolver = new PropertyRenameAndIgnoreSerializerContractResolver();
                    jsonResolver.IgnoreProperty(typeof(IBaseModel), ignoredProperties);
                    var serializerSettings = new JsonSerializerSettings { ObjectCreationHandling = ObjectCreationHandling.Replace, TypeNameHandling = TypeNameHandling.All };
                    serializerSettings.ContractResolver = jsonResolver;
                    serializedJsonObject = JsonConvert.SerializeObject(objectToSerialize, serializerSettings);
                }
                else
                    serializedJsonObject = JsonConvert.SerializeObject(objectToSerialize);
            }
            catch (Exception exception)
            {
                success = false;
                serializedJsonObject = "";
                LoggingService.LogErrorMessage(exception, "SerializationHelper.SerializeToJsonString");
            }

            return success;
        }

        /// <summary>
        /// Gets an array of the assemblies referenced in the current application domain.
        /// </summary>
        /// <param name="containText">Filter assemblies by ensuring the name contains 'containText'.</param>
        public static Assembly[] GetAssemblies(string containText = "")
        {
            AppDomain appDomain = AppDomain.CurrentDomain;
            Assembly[] assemblies = string.IsNullOrEmpty(containText)
                ? appDomain.GetAssemblies()
                : appDomain.GetAssemblies().Where(a => a.FullName.Contains(containText)).ToArray();
#if DEBUG
            Debug.WriteLine($"Com.Rfidiom.WasteVu.Mobile.Helpers.SerializationHelper.GetAssemblies() containText: {containText}, appDomain: {appDomain.FriendlyName}");

            foreach (Assembly assembly in assemblies)
                Debug.WriteLine("Assembly: " + assembly.FullName);
#endif
            return assemblies;
        }

        /// <summary>
        /// Gets an assembly with the given 'name' from the current AppDomain using reflection.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="containText">Filter assemblies by ensuring each must contain 'containText'.</param>
        public static Assembly GetAssemblyByName(string assemblyName, string containText = "")
        {
            return GetAssemblies(containText).SingleOrDefault(assembly => assembly.GetName().Name == assemblyName);
        }

        /// <summary>
        /// Gets an array of resource names in the given 'assembly'.
        /// </summary>
        /// <param name="assemblyName">The name of the assembly.</param>
        /// <param name="providedAssembly">The assembly (if not provided a search will be performed in the current app domain using the 'assemblyName').</param>
        public static string[] GetAssemblyResourceNames(string assemblyName = "", string containText = "", Assembly providedAssembly = null)
        {
            string[] assemblyResourceNames = null;

            providedAssembly ??= GetAssemblyByName(assemblyName, containText);
            assemblyResourceNames = providedAssembly?.GetManifestResourceNames();
#if DEBUG
            Debug.WriteLine($"Com.Rfidiom.WasteVu.Mobile.Helpers.SerializationHelper.GetAssemblyResourceNames() assemblyName: {assemblyName}, providedAssembly: {providedAssembly}");

            if (assemblyResourceNames != null)
            {
                foreach (string resourceName in assemblyResourceNames)
                    Debug.WriteLine("Resource: " + resourceName);
            }
#endif
            return assemblyResourceNames;
        }

        public static bool GetPropertyValue(object theObject, string propertyPath, out object propertyValue)
        {
            bool success = true;
            propertyValue = theObject;

            if (theObject != null)
            {
                Type currentType = propertyValue.GetType();

                try
                {
                    foreach (string propertyName in propertyPath.Split('.'))
                    {
                        PropertyInfo propertyInfo = currentType.GetProperty(propertyName);

                        if (propertyInfo == null)
                        {
                            throw new Exception("Property not found:  " + propertyName);
                        }

                        propertyValue = propertyInfo.GetValue(propertyValue, null);

                        if (propertyValue == null)
                        {
                            break;
                        }

                        currentType = propertyInfo.PropertyType;
                    }
                }
                catch (Exception exception)
                {
                    success = false;
                    LoggingService.LogErrorMessage(exception, "SerializationHelper.GetPropertyValue()");
                }
            }
            else
            {
                LoggingService.LogToConsoleAction?.Invoke("WARNING! SerializationHelper.GetPropertyValue received null object.", null);
            }

            return success;
        }
    }

    internal class PropertyRenameAndIgnoreSerializerContractResolver : DefaultContractResolver
    {
        private readonly Dictionary<Type, HashSet<string>> _ignores;
        private readonly Dictionary<Type, Dictionary<string, string>> _renames;

        public PropertyRenameAndIgnoreSerializerContractResolver()
        {
            _ignores = new Dictionary<Type, HashSet<string>>();
            _renames = new Dictionary<Type, Dictionary<string, string>>();
        }

        public void IgnoreProperty(Type type, params string[] jsonPropertyNames)
        {
            if (!_ignores.ContainsKey(type))
                _ignores[type] = new HashSet<string>();

            foreach (var prop in jsonPropertyNames)
                _ignores[type].Add(prop);
        }

        public void RenameProperty(Type type, string propertyName, string newJsonPropertyName)
        {
            if (!_renames.ContainsKey(type))
                _renames[type] = new Dictionary<string, string>();

            _renames[type][propertyName] = newJsonPropertyName;
        }

        protected override JsonProperty CreateProperty(MemberInfo member, MemberSerialization memberSerialization)
        {
            var property = base.CreateProperty(member, memberSerialization);

            if (IsIgnored(property.DeclaringType, property.PropertyName))
            {
                property.ShouldSerialize = i => false;
                property.Ignored = true;
            }

            if (IsRenamed(property.DeclaringType, property.PropertyName, out var newJsonPropertyName))
                property.PropertyName = newJsonPropertyName;

            return property;
        }

        private bool IsIgnored(Type type, string jsonPropertyName)
        {
            var interfaces = type.GetInterfaces();

            if (interfaces != null)
            {
                foreach (Type iface in interfaces)
                {
                    if (_ignores.ContainsKey(iface) && _ignores[iface].Contains(jsonPropertyName))
                        return true;
                }
            }

            if (!_ignores.ContainsKey(type))
                return false;

            return _ignores[type].Contains(jsonPropertyName);
        }

        private bool IsRenamed(Type type, string jsonPropertyName, out string newJsonPropertyName)
        {
            if (!_renames.TryGetValue(type, out Dictionary<string, string> renames) || !renames.TryGetValue(jsonPropertyName, out newJsonPropertyName))
            {
                newJsonPropertyName = null;
                return false;
            }

            return true;
        }
    }
}
