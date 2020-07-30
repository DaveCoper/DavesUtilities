using System.Collections.Generic;
using System.ComponentModel;

namespace DavesUtilities.Reflection
{
    public class CopySettings
    {
        private ISet<string> ignoredPropertyNames = new HashSet<string>();

        private ICollection<TypeConverter> typeConverters = new List<TypeConverter>();

        private IDictionary<string, string> propertyMapping = new Dictionary<string, string>();

        public ISet<string> IgnoredPropertyNames
        {
            get => ignoredPropertyNames;
            set => ignoredPropertyNames = value ?? new HashSet<string>();
        }

        public ICollection<TypeConverter> TypeConverters
        {
            get => typeConverters;
            set => typeConverters = value ?? new List<TypeConverter>();
        }

        public IDictionary<string, string> PropertyMapping
        {
            get => propertyMapping;
            set => propertyMapping = value ?? new Dictionary<string, string>();
        }

        public bool CopyPulic { get; set; } = true;

        public bool CopyPrivateAndProtected { get; set; } = false;

        public bool ThrowOnMissingTargetPropeties { get; set; } = false;

        public bool ThrowOnMissingSourcePropeties { get; set; } = false;
    }
}