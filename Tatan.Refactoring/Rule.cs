using System;
using System.Collections.Generic;
using Tatan.Refactoring.Codes;
using Tatan.Refactoring.Collections;

namespace Tatan.Refactoring
{
    /// <summary>
    /// 规则校验
    /// </summary>
    public static class Rule
    {
        static Rule()
        {
            _condition = new Condition
            {
                Modules = {Count = 10},
                Directories = {Count = 15},
                Files = {Count = 20},
                Classes =
                {
                    Count = 5,
                    Lines = 500,
                    Variables = {Count = 15},
                    Functions =
                    {
                        Count = 10,
                        Lines = 50,
                        Parameters = {Count = 5},
                        IfElses = {Count = 3},
                        Switches = {Count = 5}
                    }
                }
            };
        }

        private static readonly Condition _condition;

        internal class Condition
        {
            public class ModulesCondition
            {
                public int Count { get; set; }
            }

            public ModulesCondition Modules { get; set; }

            public class DirectoriesCondition
            {
                public int Count { get; set; }
            }

            public DirectoriesCondition Directories { get; set; }

            public class FilesCondition
            {
                public int Count { get; set; }
            }

            public FilesCondition Files { get; set; }

            public class ClassesCondition
            {
                public int Count { get; set; }

                public int Lines { get; set; }

                public VariablesCondition Variables { get; set; }

                public class VariablesCondition
                {
                    public int Count { get; set; }
                }

                public FunctionsCondition Functions { get; set; }

                public class FunctionsCondition
                {
                    public int Count { get; set; }

                    public int Lines { get; set; }

                    public int Complex { get; set; }

                    public int Depth { get; set; }

                    public VariablesCondition Parameters { get; set; }

                    public IfElseCondition IfElses { get; set; }

                    public class IfElseCondition
                    {
                        public int Count { get; set; }
                    }

                    public SwitchCondition Switches { get; set; }

                    public class SwitchCondition
                    {
                        public int Count { get; set; }

                        public int Lines { get; set; }
                    }
                }
            }

            public ClassesCondition Classes { get; set; }
        }

        private static IDictionary<string, Action> _rules = new Dictionary<string, Action>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="root"></param>
        /// <exception cref="Exception"></exception>
        public static void Match(CodeRoot root)
        {
            if (root.Modules.Count > _condition.Modules.Count)
                throw new Exception("");
            foreach (var name in root.Modules)
            {
                var module = root.Modules[name];
                MatchDirectory(module.Directories);
                MatchFile(module.Files);
            }
        }

        private static void MatchDirectory(CodeDirectoryCollection directories)
        {
            if (directories.Count > _condition.Directories.Count)
                throw new Exception("");
            foreach (var name in directories)
            {
                var directory = directories[name];
                MatchDirectory(directory.Directories);
                MatchFile(directory.Files);
            }
        }

        private static void MatchFile(CodeFileCollection files)
        {
            if (files.Count > _condition.Files.Count)
                throw new Exception("");
            foreach (var name in files)
            {
                var file = files[name];
                MatchClass(file.Classes);
            }
            
        }

        private static void MatchClass(CodeClassCollection classes)
        {
            if (classes.Count > _condition.Classes.Count)
                throw new Exception("");
            if (!classes.HasPublicClass)
                throw new Exception("");
            foreach (var name in classes)
            {
                var klass = classes[name];
                if (klass.Lines > _condition.Classes.Lines)
                    throw new Exception("");

                MatchVariable(klass.Fields);
                MatchVariable(klass.StaticFields);

                MatchFunction(klass.Functions);
                MatchFunction(klass.StaticFunctions);
            }
        }

        private static void MatchVariable(CodeVariableCollection variables)
        {
            if (variables.Count > _condition.Classes.Variables.Count)
                throw new Exception("");
        }

        private static void MatchFunction(CodeFunctionCollection functions)
        {
            if (functions.Count > _condition.Classes.Functions.Count)
                throw new Exception("");

            foreach (var name in functions)
            {
                var function = functions[name];
                if (function.Lines > _condition.Classes.Functions.Lines)
                    throw new Exception("");
                if (function.Parameters.Count > _condition.Classes.Functions.Parameters.Count)
                    throw new Exception("");
                if (function.Complex > _condition.Classes.Functions.Complex)
                    throw new Exception("");
                if (function.Depth > _condition.Classes.Functions.Depth)
                    throw new Exception("");

                MatchIfElse(function.IfElses);
                MatchSwitch(function.Switches);
            }
        }

        private static void MatchIfElse(CodeIfElseCollection ifelses)
        {
            if (ifelses.Count > _condition.Classes.Functions.IfElses.Count) 
                throw new Exception("");
        }

        private static void MatchSwitch(CodeSwitchCollection switches)
        {
            if (switches.Count > _condition.Classes.Functions.Switches.Count) 
                throw new Exception("");
        }
    }
}
