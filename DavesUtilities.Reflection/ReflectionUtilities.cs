using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace DavesUtilities.Reflection
{
    public static class ReflectionUtilities
    {
        public static void CopyProperties<TSource, TTarget>(TSource source, TTarget target)
        {
            CopyProperties(source, target, new CopySettings());
        }

        public static void CopyProperties<TSource, TTarget>(
                                                     TSource source,
                                                     TTarget target,
                                                     CopySettings settings)
        {
            if (source == null)
            {
                throw new ArgumentNullException(nameof(source));
            }

            if (target == null)
            {
                throw new ArgumentNullException(nameof(target));
            }

            if (settings == null)
            {
                // set default settings;
                settings = new CopySettings();
            }

            var propertyFlags = BindingFlags.Instance;
            if (settings.CopyPulic)
                propertyFlags |= BindingFlags.Public;
            if (settings.CopyPrivateAndProtected)
                propertyFlags |= BindingFlags.NonPublic;


            var sourceType = source.GetType();
            var sourceProperties = sourceType
                .GetProperties(propertyFlags)
                .Where(x => x.CanRead && !settings.IgnoredPropertyNames.Contains(x.Name))
                .ToDictionary(x => GetTargetPropertyName(settings, x.Name));

            var targetType = target.GetType();
            var targetProperties = targetType.GetProperties(propertyFlags)
                .Where(x => x.CanWrite)
                .ToDictionary(x => x.Name);

            var missingTargetProperties = targetProperties.Keys.Where(x => !sourceProperties.ContainsKey(x));
            if (settings.ThrowOnMissingTargetPropeties && missingTargetProperties.Any())
            {
                throw new TragetPropertiesAreMissingException("TODO: exception type");
            }

            if (settings.ThrowOnMissingSourcePropeties && sourceProperties.Keys.All(x => targetProperties.ContainsKey(x)))
            {
                throw new Exception("TODO: exception type");
            }

            foreach (var sourceProperty in sourceProperties)
            {
                if (targetProperties.TryGetValue(sourceProperty.Key, out var targetProperty))
                {
                    Type sourcePropertyType = sourceProperty.Value.PropertyType;
                    Type targetPropertyType = targetProperty.PropertyType;

                    object sourceValue = sourceProperty.Value.GetValue(source);

                    if (!targetPropertyType.IsAssignableFrom(sourcePropertyType))
                    {
                        var converter = settings.TypeConverters.FirstOrDefault(x =>
                            x.CanConvertFrom(sourcePropertyType) &&
                            x.CanConvertTo(targetPropertyType));

                        if (converter == null)
                            throw new Exception("TODO: exception type. Converter is missing");

                        sourceValue = converter.ConvertTo(sourceValue, targetPropertyType);
                    }

                    targetProperty.SetValue(target, sourceValue);
                }
            }
        }

        private static string GetTargetPropertyName(CopySettings settings, string sourcePropertyName)
        {
            if (settings.PropertyMapping.TryGetValue(sourcePropertyName, out var targetPropertyName))
            {
                return targetPropertyName;
            }
            return sourcePropertyName;
        }
    }
}