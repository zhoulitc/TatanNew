using System;

namespace Tatan.Common.Compiler
{
    /// <summary>
    /// 树
    /// </summary>
    public class Tree<T> : IObject
    {
        /// <summary>
        /// 树的根节点
        /// </summary>
        public TreeNode<T> Root { get; set; }

        /// <summary>
        /// 层序访问树的所有节点
        /// </summary>
        public void Visit(Action<TreeNode<T>> action)
        {
            TreeNode<T>.LayerVisit(Root, action);
        }

        /// <summary>
        /// 获取树的节点数
        /// </summary>
        public int Count
        {
            get
            {
                if (Root == null) return 0;
                if (Root.IsLeaf) return 1;

                var count = 0;
                TreeNode<T>.LayerVisit(Root, n =>
                {
                    count++;
                });
                return count;
            }
        }

        /// <summary>
        /// 获取树的深度
        /// </summary>
        public int Depth
        {
            get
            {
                if (Root == null) return 0;
                if (Root.IsLeaf) return 1;

                var max = 0;
                var deep = 0;
                TreeNode<T>.DeepVisit(Root, n =>
                {
                    if (!n.IsLeaf)
                        deep++;
                    else
                    {
                        if (deep > max)
                            max = deep;
                        deep = 0;
                    }
                });
                return max + 1;
            }
        }

        /// <summary>
        /// 获取树的叶子数
        /// </summary>
        public int Leaf
        {
            get
            {
                if (Root == null) return 0;
                if (Root.IsLeaf) return 1;

                var leaf = 0;
                TreeNode<T>.DeepVisit(Root, n =>
                {
                    if (n.IsLeaf)
                        leaf++;
                });
                return leaf;
            }
        }
    }
}