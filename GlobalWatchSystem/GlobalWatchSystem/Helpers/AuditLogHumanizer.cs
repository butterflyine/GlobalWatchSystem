using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using GlobalWatchSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using SharpRepository.Repository.Helpers;

namespace GlobalWatchSystem.Helpers
{
    public static class AuditLogHumanizer
    {
        private const string DEFAULT_NAMESPACE = "GlobalWatchSystem.Models";

        private static readonly Dictionary<String, Type> TYPES = new Dictionary<string, Type>();
        private static readonly Dictionary<String, DisplayAttribute> DISPLAY_ATTRIBUTESES_CACHE = new Dictionary<string, DisplayAttribute>();

        public static string Humanize(AuditLog log)
        {
            if (String.IsNullOrWhiteSpace(log.Changes))
            {
                return "";
            }

            var sb = new StringBuilder();
            var changes = ((JObject) JsonConvert.DeserializeObject(log.Changes.Trim()));
            Type type = GetLogType(log.Type);
            foreach (JToken change in changes.Children())
            {
                string fieldName = change.Path;
                if (!change.Values().Any())
                {
                    continue;
                }

                string value = ((JValue) change.Values().FirstOrDefault()).Value.ToString();
                String localizedFieldName = GetLocalizedFieldName(type, fieldName);
                if (!String.IsNullOrEmpty(localizedFieldName))
                {
                    if (sb.Length > 0)
                    {
                        sb.Append(", ");
                    }
                    sb.Append(String.Format("{0} => '{1}'", localizedFieldName, value));
                }
            }
            return sb.ToString().Trim();
        }

        private static string GetLocalizedFieldName(Type type, string fieldName)
        {
            DisplayAttribute displayAttribute = GetFieldDisplayAttribute(type, fieldName);
            if (displayAttribute == null)
            {
                return String.Empty;
            }

            return Resources.Resources.ResourceManager.GetString(displayAttribute.Name);
        }

        private static DisplayAttribute GetFieldDisplayAttribute(Type type, string fieldName)
        {
            DisplayAttribute displayAttribute;

            string key = String.Format("{0}_{1}", type, fieldName);
            if (!DISPLAY_ATTRIBUTESES_CACHE.ContainsKey(key))
            {
                DisplayAttribute[] displayAttributes = type.GetProperty(fieldName).GetAllAttributes<DisplayAttribute>();
                if (displayAttributes != null && displayAttributes.Length > 0)
                {
                    DISPLAY_ATTRIBUTESES_CACHE.Add(key, displayAttributes[0]);
                }
            }
            DISPLAY_ATTRIBUTESES_CACHE.TryGetValue(key, out displayAttribute);
            return displayAttribute;
        }

        private static Type GetLogType(string type)
        {
            if (!TYPES.ContainsKey(type))
            {
                TYPES.Add(type, Type.GetType(string.Format("{0}.{1}", DEFAULT_NAMESPACE, type)));
            }
            return TYPES[type];
        }
    }
}