using System;

namespace CBuildSystem.Model
{
    public class SourceFile
    {
        private string _path;
        
        public SourceFile(string path)
        {
            _path = path;            
        }

        public SourceFile()
        {}
    }
}