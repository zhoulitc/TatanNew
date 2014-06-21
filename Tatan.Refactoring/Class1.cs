using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tatan.Refactoring.Collections;

namespace Tatan.Refactoring
{
    /// <summary>
    /// 解析器
    /// </summary>
    public class Parser
    {
        public Parser(CodeRoot root)
        {

        }
        public CodeRoot Parse(string path)
        {
            return null;
        }
    }

    public class Rule
    {
        public void Match(CodeRoot root)
        {
            if (root.Modules.Count > 10)
                throw new Exception("");
            foreach (var module in root.Modules)
            {
                MatchDirectory(module.Directories);
                MatchFile(module.Files);
            }
        }

        private void MatchDirectory(CodeDirectoryCollection directories)
        {
            if (directories.Count > 15)
                throw new Exception("");
            foreach (var directory in directories)
            {
                MatchDirectory(directory.Directories);
                MatchFile(directory.Files);
            }
        }

        private void MatchFile(CodeFileCollection files)
        {
            if (files.Count > 20)
                throw new Exception("");
            foreach (var file in files)
            {
                //文件中public类的个数大于1个
                if (file.Classes.Count > 1 && file.Classes[1].Accessibility == CodeAccessibility.Public)
                    throw new Exception("");
                foreach (var klass in file.Classes)
                {
                    if (klass.Lines > 500)
                        throw new Exception("");
                    if (klass.Fields.Count > 15)
                        throw new Exception("");
                    foreach (var function in klass.Functions)
                    {
                        if (function.Lines > 50)
                            throw new Exception("");
                        if (function.Parameters.Count > 5)
                            throw new Exception("");
                        if (function.Complex > 10)
                            throw new Exception("");
                        if (function.Depth > 5)
                            throw new Exception("");
                        if (function.IfElses.Count > 3) //每个if判断中的组合表达式不超过3
                            throw new Exception("");
                    }
                }
            }
        }
    }


    /// <summary>
    /// 代码目录，一个目录包含多个文件和多个子目录
    /// </summary>
    public class CodeDirectory
    {
        public string Name { get; }
        public string Path { get; }
        public CodeFileCollection Files { get; }
        /// <summary>
        /// 文件夹下面的子文件夹集合
        /// </summary>
        public CodeDirectoryCollection Directories { get; }
    }

    /// <summary>
    /// 代码文件，一个文件包含多个类，但通常是一个
    /// </summary>
    public class CodeFile
    {
        public string Name { get; }
        public string Path { get; }

        /// <summary>
        /// 文件扩展名
        /// </summary>
        public string Extension { get; }

        /// <summary>
        /// 文件中的类个数
        /// </summary>
        public CodeClassCollection Classes { get; }
    }

    /// <summary>
    /// 代码类，一个类包含多个属性
    /// </summary>
    public class CodeClass
    {
        public CodeAccessibility Accessibility { get; }

        public string Name { get; }

        /// <summary>
        /// 类占用的行数
        /// </summary>
        public int Lines { get; }

        /// <summary>
        /// 类继承父类和实现的接口，只向上一层
        /// </summary>
        public CodeClassCollection Extends { get; }

        /// <summary>
        /// 类中的成员字段
        /// </summary>
        public CodeVariableCollection Fields { get; }

        /// <summary>
        /// 类中的静态成员字段
        /// </summary>
        public CodeVariableCollection StaticFields { get; }

        /// <summary>
        /// 类中的成员函数、包括属性和方法
        /// </summary>
        public CodeFunctionCollection Functions { get; }

        /// <summary>
        /// 类中的静态成员函数、包括属性和方法
        /// </summary>
        public CodeFunctionCollection StaticFunctions { get; }
    }

    public class CodeVariable
    {
        public CodeAccessibility Accessibility { get; }
        public string Name { get; }
        public Type Type { get; }
        public int Index { get; }
    }

    public class CodeFunction
    {
        public CodeAccessibility Accessibility { get; }
        public string Name { get; }

        /// <summary>
        /// 函数占用的行数
        /// </summary>
        public int Lines { get; }

        /// <summary>
        /// 函数的Block深度
        /// </summary>
        public int Depth { get; }

        /// <summary>
        /// 函数的圈复杂度
        /// </summary>
        public int Complex { get; }

        /// <summary>
        /// 函数的参数
        /// </summary>
        public CodeVariableCollection Parameters { get; }

        public CodeIfElseCollection IfElses { get; }

        public CodeSwitchCollection Switches { get; }
    }

    public class CodeIfElse
    {
    }

    public class CodeSwitch
    {
    }
}
