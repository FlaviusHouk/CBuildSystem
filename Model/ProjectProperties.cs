using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;
using System.Collections.Generic;
using System.Linq;

namespace CBuildSystem.Model
{
    [Serializable]
    public class ProjectProperties : IXmlSerializable
    {
        [XmlArray("CompilerProperties"), XmlArrayItem(typeof(CompilerProperty))]
        internal List<CompilerProperty> Properties { get; } 
           = new List<CompilerProperty>();


        private void AddGCCProps()
        {
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.UseAnsiProperty, "-ansi"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.StandardProperty, "-std="));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.GNU89InlineProperty, "-fgnu89-inline"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.StandardFLTEvalProperty, "-fpermitted-flt-eval-methods="));//toDo: check params
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.AuxInfoProperty, "-aux-info ", true));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.ParameterlessVariadicFunctionsAllowedProperty, "-fallow-parameterless-variadic-functions"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.FNO_ASMProperty, "-fno-asm"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.BuiltFuncIgnoredProperty, "-fno-builtin"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.GimpleProperty, "-fgimple"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.RunEnvironmetProperty, ""));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.OpenACCProperty, "-fopenacc"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.OpenACCDimProperty, "-fopenacc-dim=", true));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.OpenMPProperty, "-fopenmp")); 
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.OpenMPSimdProperty, "-fopenmp-simd"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.GNU_TMProperty,"-fgnu-tm"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.MSExtensionsProperty, "-fms-extensions"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.Plan9ExtensionsProperty, "-fplan9-extensions"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.CondMismatchProperty, "-fcond-mismatch"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.FlaxVectorConversationProperty, "-flax-vector-conversions"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.CharAlwaysSignedProperty, "-fsigned-char"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.CharAlwaysUnSignedProperty, "-funsigned-char"));
            Properties.Add(new CompilerProperty(GCCPropertiesEnum.EndianProperty, ""));
        }
        public ProjectProperties()
        {
            AddGCCProps();
        }
        
        #region CDialectOptionsProperties
        public bool IsAnsi
        {
            get
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.UseAnsiProperty);
                return p.IsUsed;
            }
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.UseAnsiProperty);
                p.IsUsed = value;

                if(value)
                {
                    p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardProperty);
                    p.IsUsed = true;
                    p.Value = CStandardEnum.C89;
                }
            }
        }

        public bool IsStandardExplicitlySet
        {
            get
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardProperty);
                return p.IsUsed;
            }
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardProperty);
                p.IsUsed = value;

                if(!value)
                    p.StringParametr = "-std=";
            }
        }


        public CStandardEnum Standard 
        { 
            get 
            {
                if(IsStandardExplicitlySet)
                {
                    CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardProperty);
                    return (CStandardEnum)p.Value;
                }
                else
                {
                    return CStandardEnum.C11;
                }
            } 
            set 
            { 
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardProperty);
                p.Value = value;

                switch(value)
                {
                    case CStandardEnum.C89:
                    {
                        p.StringParametr = "-std=c89";
                        break;
                    }
                    case CStandardEnum.C90:
                    {
                        p.StringParametr = "-std=c90";
                        break;
                    }
                    case CStandardEnum.C99:
                    {
                        p.StringParametr = "-std=c99";
                        break;
                    }
                    case CStandardEnum.C11:
                    {
                        p.StringParametr = "-std=c11";
                        break;
                    }
                    case CStandardEnum.C17:
                    {
                        p.StringParametr = "-std=c17";
                        break;
                    }
                    case CStandardEnum.GNU_C90:
                    {
                        p.StringParametr = "-std=gnu90";
                        break;
                    }
                    case CStandardEnum.GNU_C99:
                    {
                        p.StringParametr = "-std=gnu99";
                        break;
                    }
                    case CStandardEnum.GNU_C11:
                    {
                        p.StringParametr = "-std=gnu11";
                        break;
                    }
                    case CStandardEnum.GNU_C17:
                    {
                        p.StringParametr = "-std=gnu17";
                        break;
                    }
                }
            }
        }

        public bool IsGNU89Inline
        {
            get
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GNU89InlineProperty);
                return p.IsUsed;
            }
            set
            {
                if(Standard == CStandardEnum.C99)
                {
                    CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GNU89InlineProperty);
                    p.Value = value;
                }
            }
        }

        public bool UseStandardFLTEval 
        { 
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardFLTEvalProperty);
                return p.IsUsed;
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.StandardFLTEvalProperty);
                p.IsUsed = value; 
            }
        }

        public string AuxInfo 
        { 
            get
            { 
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.AuxInfoProperty);
                return p.Value.ToString(); 
            }
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.AuxInfoProperty);
                
                p.IsUsed = !string.IsNullOrEmpty(value);

                if(p.IsUsed)
                    p.Value = value;     
            }
        }

        public bool IsParameterlessVariadicFunctionsAllowed 
        { 
            get
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.ParameterlessVariadicFunctionsAllowedProperty);
                return p.IsUsed;
            }
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.ParameterlessVariadicFunctionsAllowedProperty);
                p.IsUsed = value;                
            } 
        }
        public bool IsFNOASM 
        { 
            get
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.FNO_ASMProperty);
                return p.IsUsed;
            } 
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.ParameterlessVariadicFunctionsAllowedProperty);
                p.IsUsed = value;
            }
        }
        public bool IsBuiltFuncIgnored 
        {
             get
             {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.BuiltFuncIgnoredProperty);
                return p.IsUsed;                 
             }
             set 
             {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.BuiltFuncIgnoredProperty);
                p.IsUsed = value;
             }
        }
        public bool IsGimpleAllowed 
        { 
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GimpleProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GimpleProperty);
                p.IsUsed = value;
            }
        }
        
        public bool UseExplicitEnvironent 
        { 
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.RunEnvironmetProperty);
                return p.IsUsed;
            } 
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.ParameterlessVariadicFunctionsAllowedProperty);
                p.IsUsed = value;

                if(!value)
                    p.StringParametr = string.Empty;
            }
        }
        public EnvironmentEnum RunEnvironmet 
        { 
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.RunEnvironmetProperty);
                return (EnvironmentEnum)p.Value;
            } 
            set
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.RunEnvironmetProperty);
                p.Value = value;

                switch(value)
                {
                    case EnvironmentEnum.FreeStanding:
                    {
                        p.StringParametr = "-ffreestanding";
                        break;
                    }
                    case EnvironmentEnum.Hosting:
                    {
                        p.StringParametr = "-fhosted";
                        break;
                    }
                }
            }
        }

        public bool IsOpenACCUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenACCProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenACCProperty);
                p.IsUsed = value;
            }
        }
        public string OpenACCDim 
        { 
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GimpleProperty);
                return p.Value.ToString();                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GimpleProperty);
                
                p.IsUsed = !string.IsNullOrEmpty(value);

                if(p.IsUsed)
                    p.Value = value;
            } 
        }
        public bool IsOpenMPUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenMPProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenMPProperty);
                p.IsUsed = value;
            }
        }
        public bool IsOpenMPSimdUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenMPSimdProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.OpenMPSimdProperty);
                p.IsUsed = value;
            }
        }
        public bool IsGNUTMUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GNU_TMProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.GNU_TMProperty);
                p.IsUsed = value;
            }
        }
        public bool IsMSExtensionsUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.MSExtensionsProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.MSExtensionsProperty);
                p.IsUsed = value;
            }
        }
        public bool IsPlan9ExtensionsUsed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.Plan9ExtensionsProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.Plan9ExtensionsProperty);
                p.IsUsed = value;
            }
        }
        public bool IsCondMismatchAllowed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CondMismatchProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CondMismatchProperty);
                p.IsUsed = value;
            }
        }
        public bool IsFlaxVectorConversationAllowed 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.FlaxVectorConversationProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.FlaxVectorConversationProperty);
                p.IsUsed = value;
            }
        }
        public bool IsCharAlwaysSigned 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CharAlwaysSignedProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CharAlwaysSignedProperty);
                p.IsUsed = value;
            }
        }

        public bool IsCharAlwaysUnSigned 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CharAlwaysUnSignedProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.CharAlwaysUnSignedProperty);
                p.IsUsed = value;
            }
        }

        public bool IsEndianExplicitlySet 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.EndianProperty);
                return p.IsUsed;                
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.EndianProperty);
                p.IsUsed = value;
            }
        }
        public EndianEnum Endian 
        {
            get 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.EndianProperty);
                return (EndianEnum)p.Value;              
            }
            set 
            {
                CompilerProperty p = Properties.First(prop=>prop.Property == GCCPropertiesEnum.EndianProperty);
                p.Value = value;

                switch(value)
                {
                    case EndianEnum.BigEndian:
                    {
                        p.StringParametr = "-fsso-struct=big-endian";
                        break;
                    }
                    case EndianEnum.LittleEndian:
                    {
                        p.StringParametr = "-fsso-struct=little-endian";
                        break;
                    }
                    case EndianEnum.Native:
                    {
                        p.StringParametr = "-fsso-struct=native";
                        break;
                    }
                }
            }
        }
        #endregion

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            XmlSerializer serializer = new XmlSerializer(typeof(List<CompilerProperty>), new XmlRootAttribute("CompilerProperties"));
            Properties.AddRange(serializer.Deserialize(reader) as List<CompilerProperty>);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(List<CompilerProperty>), new XmlRootAttribute("CompilerProperties"));
            serializer.Serialize(writer, Properties, ns);
        }
    }
}