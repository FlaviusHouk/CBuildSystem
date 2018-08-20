using System;
using System.IO;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace CBuildSystem.Model
{
    public class Project
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

        public List<SourceFile> SourceFiles { get; } = new List<SourceFile>();
    }
}