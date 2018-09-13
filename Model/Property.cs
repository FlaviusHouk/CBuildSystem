using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CBuildSystem.Model
{
    public class CompilerProperty : IXmlSerializable
    {
        public GCCPropertiesEnum Property { get; private set; }
        public string StringParametr { get; internal set;}
        public bool IsUsed { get; set; } = false;
        public bool IsSpecialFormat { get; private set; } = false;
        public object Value { get; set; } = null;

        public CompilerProperty()
        {}

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

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();
            
            XmlSerializer serializer = new XmlSerializer(typeof(GCCPropertiesEnum));
            Property = (GCCPropertiesEnum)serializer.Deserialize(reader);

            serializer = new XmlSerializer(typeof(string), new XmlRootAttribute("StringParametr"));
            StringParametr = serializer.Deserialize(reader).ToString();

            serializer = new XmlSerializer(typeof(bool), new XmlRootAttribute("IsUsed"));
            IsUsed = Convert.ToBoolean(serializer.Deserialize(reader));

            serializer = new XmlSerializer(typeof(bool), new XmlRootAttribute("IsSpecialRepresentation"));
            IsSpecialFormat = Convert.ToBoolean(serializer.Deserialize(reader));

            serializer = new XmlSerializer(typeof(object), new XmlRootAttribute("Value"));
            Value = serializer.Deserialize(reader);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(GCCPropertiesEnum));
            serializer.Serialize(writer, Property);

            serializer = new XmlSerializer(typeof(string), new XmlRootAttribute("StringParametr"));
            serializer.Serialize(writer, StringParametr);

            serializer = new XmlSerializer(typeof(bool), new XmlRootAttribute("IsUsed"));
            serializer.Serialize(writer, IsUsed);

            serializer = new XmlSerializer(typeof(bool), new XmlRootAttribute("IsSpecialRepresentation"));
            serializer.Serialize(writer, IsSpecialFormat);

            serializer = new XmlSerializer(typeof(object), new XmlRootAttribute("Value"));
            serializer.Serialize(writer, Value);
        }
    }
}