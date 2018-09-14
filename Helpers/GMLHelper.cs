using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;

using GMLParser.Model;

namespace CBuildSystem.Helpers
{
    static class GMLInterface
    {
        private static Parser _parser;

        public static void ProcessFiles(IEnumerable<string> files, string pathToHeaders)
        {
            _parser = new Parser();
            foreach (string file in files)
            {
                string CCode = string.Empty;
                string loc = Path.GetDirectoryName(Path.GetFullPath(file));
                string name = Path.GetFileNameWithoutExtension(file);
                string outFile = $"{loc}/{name}.g.c";
                string outHeader = $"{pathToHeaders}/{name}.g.h";

                using (FileStream fileStream = File.Open(file, FileMode.Open, FileAccess.Read))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        CCode = _parser.ParseGML(reader.ReadToEnd(), name);
                    }
                }

                using (FileStream fileStream = File.Open(outFile, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fileStream.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.Write(CCode);
                    }
                }

                using (FileStream fileStream = File.Open(outHeader, FileMode.OpenOrCreate, FileAccess.ReadWrite))
                {
                    fileStream.SetLength(0);
                    using (StreamWriter writer = new StreamWriter(fileStream))
                    {
                        writer.Write(_parser.GenerateFileHeader(name));
                    }
                }
            }
        }
    }
}