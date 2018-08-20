using System;
using System.Linq;
using System.Collections.Generic;
using CBuildSystem.Model;

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
            new CommandInfo("--build", 4, 4, ProcessBuildCommand)
        };
        private List<CommandInfo> _commands = new List<CommandInfo>();

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

            Project p = null;
            bool isNew = Project.LoadOrCreateProject(path, out p); 

            if(!isNew)
            {
                throw new ArgumentException("Path is wrong. Project cannot be created");
            }
        }

        private static void ProcessAddCommand(CommandInfo info)
        {}

        private static void ProcessDeleteCommand(CommandInfo info)
        {}

        private static void ProcessBuildCommand(CommandInfo info)
        {}

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
