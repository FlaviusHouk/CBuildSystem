using System;
using System.Linq;
using System.Collections.Generic;
using CBuildSystem.Model;
using System.IO;

namespace CBuildSystem
{
    class CommandInfo
    {
        public string Command { get; set; }
        public int ArgsCount { get; set; }
        public int Order { get; set; }

        public List<string> Arguments {get;} = new List<string>();
        public Action<CommandInfo> ExecutionMethod { get; private set; }

        public CommandInfo(string command, int argsCount, int order, Action<CommandInfo> exec)
        {
            Command = command;
            ArgsCount = argsCount;
            Order = order;
            ExecutionMethod = exec;
        }
    }
    class CommandParser
    {
        private static readonly List<CommandInfo> _availableCommands = new List<CommandInfo>
        {
            new CommandInfo("--create", 1, 1, ProcessCreateCommand),
            new CommandInfo("--addFile", -1, 2, ProcessAddCommand),
            new CommandInfo("--deleteFile", -1, 3, ProcessDeleteCommand),
            new CommandInfo("--addDependency", 1, 4, ProcessAddDepCommand),
            new CommandInfo("--build", -1, 5, ProcessBuildCommand)
        };
        private List<CommandInfo> _commands = new List<CommandInfo>();
        private static Project p = null;
        private void ParseCommands(string[] args)
        {
            for(int i = 0; i<args.Length; i++)
            {
                if(!args[i].StartsWith("--"))
                    break;

                IEnumerable<string> pars = args.Skip(i + 1)
                                               .TakeWhile(str=>!str.StartsWith("--"));

                CommandInfo command = _availableCommands.FirstOrDefault(info => string.Compare(info.Command, args[i]) == 0);

                if(command != null)
                {
                    command.Arguments.AddRange(pars);
                    _commands.Add(command);
                }
                else
                {
                    PrintErrorMessage();
                    return;
                }

                i+=pars.Count();
            }

            _commands.Sort((obj1, obj2) => { return obj1.Order - obj2.Order; });
        }
        public CommandParser(string[] args)
        {
            ParseCommands(args);
        }

        public bool IsValid
        {
            get
            {
                return true; // toDo                
            }
        }

        public void PerformActions()
        {
            if(!IsValid)
                PrintErrorMessage();

            foreach(CommandInfo command in _commands)
            {
                command.ExecutionMethod.Invoke(command);
            }
        }        

        private static void ProcessCreateCommand(CommandInfo info)
        {
            string path = info.Arguments.First();

            bool isNew = Project.LoadOrCreateProject(path, out p); 

            if(!isNew)
            {
                throw new ArgumentException("Path is wrong. Project cannot be created");
            }

            string projLoc = Path.GetDirectoryName(path);

            if(Directory.Exists($"{projLoc}/src"))
            {
                var sources = Directory.GetFiles($"{projLoc}/src")
                                       .Where(file => string.Compare(Path.GetExtension(file), ".c") == 0);

                foreach(string sourceFile in sources)
                {
                    p.AddFile(sourceFile);
                }
            }
            else
            {
                Directory.CreateDirectory($"{projLoc}/src");
            }


            if(!Directory.Exists($"{projLoc}/headers"))
            {
                Directory.CreateDirectory($"{projLoc}/headers");
            }

            p.IncludeFolders.Add($"{projLoc}/headers");

            p.SaveProject();
        }

        private static void ProcessAddCommand(CommandInfo info)
        {
            if(p == null)
            {
                string projPath = info.Arguments.First();
                info.Arguments.Remove(projPath);

                Project.LoadOrCreateProject(projPath, out p);
            }

            foreach(string file in info.Arguments)
            {
                p.AddFile(file);
            }

            p.SaveProject();
        }

        private static void ProcessDeleteCommand(CommandInfo info)
        {
            if(p == null)
            {
                string projPath = info.Arguments.First();
                info.Arguments.Remove(projPath);

                Project.LoadOrCreateProject(projPath, out p);
            }

            foreach(string file in info.Arguments)
            {
                p.RemoveFile(file);
            }

            p.SaveProject();
        }

        private static void ProcessAddDepCommand(CommandInfo info)
        {
            if(p == null)
            {
                string projPath = info.Arguments.First();
                info.Arguments.Remove(projPath);

                Project.LoadOrCreateProject(projPath, out p);
            }

            foreach(string dep in info.Arguments)
            {
                p.SystemDeps.Add(dep);
            }

            p.SaveProject();
        }
        private static void ProcessBuildCommand(CommandInfo info)
        {
            if(p == null)
            {
                string projPath = info.Arguments.First();
                Project.LoadOrCreateProject(projPath, out p);
            }

            p.Build();
        }

        private void PrintErrorMessage()
        {}
    }

    class Program
    {
        static void Main(string[] args)
        {
            CommandParser pars = new CommandParser(args);
            
            pars.PerformActions();
        }
    }
}
