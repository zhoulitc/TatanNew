using System.Collections;
using System.Collections.Generic;

namespace Tatan.Common.Compiler
{
    /// <summary>
    /// 树集合
    /// </summary>
    public class TreeNodeCollection<T> : IEnumerable<TreeNode<T>>
    {
        private readonly TreeNode<T> _parent;
        private readonly IList<TreeNode<T>> _nodes = new List<TreeNode<T>>();

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parent"></param>
        public TreeNodeCollection(TreeNode<T> parent)
        {
            _parent = parent;
        }

        /// <summary>
        /// 根据索引获取树集合中的树
        /// </summary>
        /// <param name="index"></param>
        public TreeNode<T> this[int index]
        {
            get { return _nodes[index]; }
            set { _nodes[index] = value; }
        }

        /// <summary>
        /// 是否包含孩子，或者后裔
        /// </summary>
        /// <param name="node"></param>
        /// <param name="deep">是否包含后裔</param>
        /// <returns></returns>
        public bool Contains(TreeNode<T> node, bool deep = false)
        {
            TreeNode<T> parent = null;
            FindPosterity(node, deep, ref parent);
            return (parent != null);
        }

        internal void FindPosterity(TreeNode<T> node, bool deep, ref TreeNode<T> parent)
        {
            if (node == null || parent != null) 
                return;

            if (node.Children._nodes.Contains(node))
            {
                parent = node.Parent;
                return;
            }
            if (!deep)
                return;
            foreach (var subNode in _nodes)
            {
                FindPosterity(subNode, true, ref parent);
            }
        }

        /// <summary>
        /// 获取树集合的个数
        /// </summary>
        public int Count 
        {
            get { return _nodes.Count; }
        }

        public IEnumerator<TreeNode<T>> GetEnumerator()
        {
            return _nodes.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// 添加一个孩子节点
        /// </summary>
        /// <param name="node"></param>
        public void Add(TreeNode<T> node)
        {
            _nodes.Add(node);
        }

        /// <summary>
        /// 移除一个孩子节点
        /// </summary>
        /// <param name="node"></param>
        public bool Remove(TreeNode<T> node)
        {
            return _nodes.Remove(node);
        }
    }
}