using System;

namespace CBuildSystem.Model
{
    public class CompilerProperty
    {
        public GCCPropertiesEnum Property { get; private set; }
        public string StringParametr { get; internal set;}
        public bool IsUsed { get; set; } = false;
        public bool IsSpecialFormat { get; private set; } = false;
        public object Value { get; set; } = null;

        public CompilerProperty(GCCPropertiesEnum prop, string param, bool specFormat = false)
        {
            Property = prop;
            StringParametr = param;
            IsSpecialFormat = specFormat;
        }

        public CompilerProperty(GCCPropertiesEnum prop, string param, object value) : this(prop, param)
        {
            Value = value;
        }
    }
}