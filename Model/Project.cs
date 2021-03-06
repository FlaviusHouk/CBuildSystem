using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using System.Xml.Schema;
using System.Linq;
using System.Diagnostics;
using System.Text;

using CBuildSystem.Helpers;

namespace CBuildSystem.Model
{
    [Serializable]
    public class Project : IXmlSerializable
    {
        #region Static
        public static bool LoadOrCreateProject(string path, out Project proj)
        {
            bool isNew = !File.Exists(path);
            XmlSerializer serializer = new XmlSerializer(typeof(Project));

            if(isNew)
            {
                using(Stream file = File.Create(path))
                {
                    proj = new Project(path);
                    serializer.Serialize(file, proj);
                }
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

        #endregion
        
        #region Fields
        private string _loc;
        
        #endregion

        #region Construction
        public Project(string fullName)
        {
            _loc = fullName;
        }

        public Project()
        {}
        
        #endregion

        #region Public Properties

        [XmlArray("SourceFiles"), XmlArrayItem(typeof(SourceFile))]
        public List<SourceFile> SourceFiles { get; } = new List<SourceFile>();

        [XmlArray("IncludeFolders"), XmlArrayItem(typeof(string))]
        public List<string> IncludeFolders { get; } = new List<string>();

        [XmlArray("SystemDependencies"), XmlArrayItem(typeof(string))]
        public List<string> SystemDeps { get; } = new List<string>();

        public ProjectProperties Properties { get; private set; } = new ProjectProperties();

        #endregion

        #region PublicProjectMethods
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
            string includes = BuildIncludeString().Trim('\n');
            string propStr = BuildPropertiesString().Trim('\n');

            if(!Directory.Exists(objFolder))
            {
                Directory.CreateDirectory(objFolder);
            }

            if(File.Exists($"{projectLocation}/scripts/prebuildScript.sh"))
            {
                Console.Write(ExternalPrograms.RunScript($"{projectLocation}/scripts/prebuildScript.sh"));
            }

            IEnumerable<SourceFile> viewCode = null;
            if(SourceFiles.Any(f=>f.FileType == SourceCodeType.View))
            {
                IEnumerable<string> viewFiles = SourceFiles
                                          .Where(f=>f.FileType == SourceCodeType.View)
                                          .Select(f=>f.Path);

                GMLInterface.ProcessFiles(viewFiles, IncludeFolders.First());

                viewCode = viewFiles.Select(f=>new SourceFile($"{Path.GetDirectoryName(f)}/{Path.GetFileNameWithoutExtension(f)}.g.c")).ToArray();
                SourceFiles.AddRange(viewCode);
            }

            System.Console.WriteLine("Builing...");

            foreach(SourceFile file in SourceFiles.Where(f=>f.FileType == SourceCodeType.Code))
            {
                string fileName = System.IO.Path.GetFileName(file.Path).Split('.',StringSplitOptions.RemoveEmptyEntries)[0];      

                ExternalPrograms.RunCompiler($"-c {file.Path} -o {objFolder}/{fileName}.o {includes} {propStr}");
            }

            if(viewCode != null)
            {
                foreach(SourceFile viewFile in viewCode)
                {
                    SourceFiles.Remove(viewFile);
                }
                viewCode = null;
            }

            var objFiles = Directory.GetFiles(objFolder)
                                    .Where(f=>string.Compare(Path.GetExtension(f), ".o") == 0);

            if (objFiles.Any())
            {
                System.Console.WriteLine("Linking...");
                string link = BuildLinkString().Trim('\n');
                StringBuilder sb = new StringBuilder(link);

                foreach (string o in objFiles)
                    sb.Append($"{o} ");

                string outName = Path.GetFileNameWithoutExtension(_loc);

                if(!Directory.Exists(binFolder))
                    Directory.CreateDirectory(binFolder);

                sb.Append($"-o {binFolder}/{outName}.lef");

                ExternalPrograms.RunCompiler(sb.ToString());

                if(File.Exists($"{projectLocation}/scripts/postbuildScript.sh"))
                {
                    Console.Write(ExternalPrograms.RunScript($"{projectLocation}/scripts/postbuildScript.sh"));
                }
            }
        }

        #endregion

        private string BuildIncludeString()
        {
            StringBuilder includes = new StringBuilder();

            foreach(string path in IncludeFolders)
            {
                includes.Append($" -I{path}");
            }

            foreach(string dep in SystemDeps)
            {
                includes.Append($" {ExternalPrograms.RunPkgConfig(dep, true)}");
            }          

            return includes.ToString();
        }

        private string BuildLinkString()
        {
            StringBuilder includes = new StringBuilder();

            //toDo other project binaries

            foreach(string dep in SystemDeps)
            {
                includes.Append($" {ExternalPrograms.RunPkgConfig(dep, false)}");
            }          

            return includes.ToString();
        }

        private string BuildPropertiesString()
        {
            StringBuilder sb = new StringBuilder();

            foreach(string append in Properties.Properties
                                               .Where(prop=>prop.IsUsed && !prop.IsSpecialFormat)
                                               .Select(prop=>prop.StringParametr))
            {
                sb.Append($" {append}");
            }

            foreach(CompilerProperty p in Properties.Properties
                                               .Where(prop=>prop.IsUsed && prop.IsSpecialFormat))
            {
                sb.Append($" {p.StringParametr}{p.Value.ToString()}");
            }

            return sb.ToString();
        }

        #region Serialization
        public XmlSchema GetSchema()
        {
            return null;
        }

        public void ReadXml(XmlReader reader)
        {
            reader.Read();

            XmlSerializer serializer = new XmlSerializer(typeof(string), new XmlRootAttribute(typeof(string).Name.ToLower()));
            _loc = serializer.Deserialize(reader) as string;

            serializer = new XmlSerializer(typeof(List<SourceFile>), new XmlRootAttribute("SourceFiles"));
            SourceFiles.AddRange(serializer.Deserialize(reader) as List<SourceFile>);

            serializer = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("IncludeFolders"));
            IncludeFolders.AddRange(serializer.Deserialize(reader) as List<string>);

            serializer = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("SystemDependencies"));
            SystemDeps.AddRange(serializer.Deserialize(reader) as List<string>);

            serializer = new XmlSerializer(typeof(ProjectProperties), new XmlRootAttribute("ProjectProperties"));
            Properties = serializer.Deserialize(reader) as ProjectProperties;
        }

        public void WriteXml(XmlWriter writer)
        {
            XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
            ns.Add("", "");

            XmlSerializer serializer = new XmlSerializer(typeof(string));
            serializer.Serialize(writer, _loc);

            serializer = new XmlSerializer(typeof(List<SourceFile>), new XmlRootAttribute("SourceFiles"));
            serializer.Serialize(writer, SourceFiles, ns);

            serializer = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("IncludeFolders"));
            serializer.Serialize(writer, IncludeFolders, ns);

            serializer = new XmlSerializer(typeof(List<string>), new XmlRootAttribute("SystemDependencies"));
            serializer.Serialize(writer, SystemDeps, ns);

            serializer = new XmlSerializer(typeof(ProjectProperties), new XmlRootAttribute("ProjectProperties"));
            serializer.Serialize(writer, Properties);
        }

        #endregion
    }
}