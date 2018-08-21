using System;
using System.Xml;
using System.Xml.Schema;
using System.Xml.Serialization;

namespace CBuildSystem.Model
{
    [Serializable]
    public class SourceFile : IXmlSerializable
    {
        private string _path;

        public string Path { get => _path; set => _path = value; }

        public SourceFile(string path)
        {
            _path = path;            
        }

        public SourceFile()
        {}

        public override bool Equals(object obj)
        {
            if(!(obj is SourceFile))
                return false;

            return string.Compare((obj as SourceFile).Path, this.Path) == 0;
        }

        public override int GetHashCode()
        {
            return _path.GetHashCode();
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            XmlSerializer serializer = new XmlSerializer(typeof(string), new XmlRootAttribute(nameof(Path)));
            _path = serializer.Deserialize(reader) as string;
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializer serializer = new XmlSerializer(typeof(string), new XmlRootAttribute(nameof(Path)));
            serializer.Serialize(writer, _path);
        }
    }
}