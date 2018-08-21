using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Diagnostics;
using System.Text;

namespace CBuildSystem.Model
{
    [Serializable]
    public class Project : IXmlSerializable
    {
        public static bool LoadOrCreateProject(string path, out Project proj)
        {
            bool isNew = !File.Exists(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Project));

            if(isNew)
            {
                Stream file = File.Create(path);
                proj = new Project(path);
                serializer.Serialize(file, proj);
            }
            else
            {
                using(Stream file = File.Open(path, FileMode.Open, FileAccess.ReadWrite))
                {
                    proj = serializer.Deserialize(file) as Project;
                }
            }

            return isNew;
        }

        
        private string _loc;
        
        public Project(string fullName)
        {
            _loc = fullName;
        }

        public Project()
        {}

        public void SaveProject()
        {
            using(Stream file = File.Open(_loc, FileMode.Open, FileAccess.ReadWrite))
            {
                file.SetLength(0);
                XmlSerializer serializer = new XmlSerializer(typeof(Project));

                serializer.Serialize(file, this);
            }
        }

        public void AddFile(string path)
        {
            SourceFile file = new SourceFile(path);
            
            if(SourceFiles.Contains(file))
            {
                return;
            }

            SourceFiles.Add(file);
        }

        public void RemoveFile(string path)
        {
            SourceFile file = SourceFiles.FirstOrDefault(f=>string.Compare(f.Path, path) == 0);

            if(file != null)
                SourceFiles.Remove(file);
        }

        public void Build()
        {
            string projectLocation = Path.GetDirectoryName(_loc);
            string objFolder = Path.Combine(projectLocation, "obj");
            string binFolder = Path.Combine(projectLocation, "bin");

            foreach(SourceFile file in SourceFiles)
            {
                string fileName = Path.GetFileNameWithoutExtension(file.Path);
                
                if(!Directory.Exists(objFolder))
                {
                    Directory.CreateDirectory(objFolder);
                }

                RunGCC($"-c {file.Path} -o {objFolder}/{fileName}.o");
            }

            var objFiles = Directory.GetFiles(objFolder)
                                    .Where(f=>string.Compare(Path.GetExtension(f), ".o") == 0);

            if (objFiles.Any())
            {
                StringBuilder sb = new StringBuilder();

                foreach (string o in objFiles)
                    sb.Append($"{o} ");

                string outName = Path.GetFileNameWithoutExtension(_loc);

                if(!Directory.Exists(binFolder))
                    Directory.CreateDirectory(binFolder);

                sb.Append($"-o {binFolder}/{outName}.lef");

                RunGCC(sb.ToString());
            }
        }

        private void RunGCC(string args)
        {
                ProcessStartInfo info = new ProcessStartInfo("gcc");
                info.Arguments = args;
                info.RedirectStandardOutput = true;
                info.RedirectStandardError = true;

                Console.WriteLine(args);

                Process proc = Process.Start(info);

                proc.WaitForExit();

                Console.Write(proc.StandardError.ReadToEnd());
        }

        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            XmlSerializer serializer = new XmlSerializer(typeof(string), new XmlRootAttribute(typeof(string).Name.ToLower()));
            _loc = serializer.Deserialize(reader) as string;

            XmlSerializer listSerializer = new XmlSerializer(typeof(List<SourceFile>));
            var des = listSerializer.Deserialize(reader) as List<SourceFile>;
            SourceFiles.AddRange(des);
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(string));
            serializer.Serialize(writer, _loc);

            XmlSerializer listSerializer = new XmlSerializer(typeof(List<SourceFile>));
            listSerializer.Serialize(writer, SourceFiles, ns);
        }

        [XmlArray("SourceFiles"), XmlArrayItem(typeof(SourceFile))]
        public List<SourceFile> SourceFiles { get; } = new List<SourceFile>();
    }
}