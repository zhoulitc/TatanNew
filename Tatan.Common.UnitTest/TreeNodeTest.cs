using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Tatan.Common.Compiler;

namespace Tatan.Common.UnitTest
{
    using Common;

    [TestClass]
    public class TreeNodeTest
    {
        [TestMethod]
        public void NewTest()
        {
            var root = new TreeNode<string>("root");
            var left = new TreeNode<string>("left", root);
            var right = new TreeNode<string>("right", root);
            var leftleft = new TreeNode<string>("left_left", left);
            var leftright = new TreeNode<string>("left_right", left);
            var rightleft = new TreeNode<string>("right_left", right);
            var rightright = new TreeNode<string>("right_right", right);
            var t = new Tree<string> {Root = root};

            var depth = t.Depth;
            var leaf = t.Leaf;

            var d = new Dictionary<string, string>();
            d.Add("1", null);
            d.Add("2", null);
            string s = string.Empty;
            TreeNode<string>.DeepVisit(root, node =>
            {
                s += node.Value + "->";
            });
            string ss = string.Empty;
            TreeNode<string>.LayerVisit(root, node =>
            {
                ss += node.Value + "->";
            });

            var sss = s;
        }

        [TestMethod]
        public void NewFormatTest()
        {
        }
    }
}
