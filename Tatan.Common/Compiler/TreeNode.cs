using System;
using System.Collections.Generic;

namespace Tatan.Common.Compiler
{
    /// <summary>
    /// 树节点
    /// </summary>
    public class TreeNode<T> : IObject
    {
        /// <summary>
        /// 构造一个节点
        /// </summary>
        /// <param name="value"></param>
        /// <param name="parent"></param>
        public TreeNode(T value, TreeNode<T> parent = null)
        {
            Value = value;
            if (parent != null)
            {
                Parent = parent;
            }
        }

        private TreeNode<T> _parent;

        /// <summary>
        /// 节点的值
        /// </summary>
        public T Value { get; set; }

        /// <summary>
        /// 父节点
        /// </summary>
        public TreeNode<T> Parent
        {
            get { return _parent; }
            set
            {
                TreeNode<T> parent = null;
                Children.FindPosterity(value, true, ref parent);
                if (parent != null)
                {
                    parent.Children.Remove(value);
                    value.Children.Add(this);
                }
                else
                {
                    if (!value.Children.Contains(this))
                        value.Children.Add(this);
                }
                _parent = value;
            }
        }
       
        /// <summary>
        /// 前一个兄弟节点
        /// </summary>
        public TreeNode<T> PreviousSibling
        {
            get
            {
                var parent = Parent;
                if (parent == null)
                    return null;

                var i = parent.Children.GetEnumerator();
                if (i.MoveNext())
                {
                    var previous = i.Current;
                    while (i.MoveNext())
                    {
                        if (Equals(i.Current))
                            return previous;
                        previous = i.Current;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 后一个兄弟节点
        /// </summary>
        public TreeNode<T> NextSibling
        {
            get
            {
                var parent = Parent;
                if (parent == null)
                    return null;

                var i = parent.Children.GetEnumerator();
                if (i.MoveNext())
                {
                    var previous = i.Current;
                    while (i.MoveNext())
                    {
                        if (Equals(previous))
                            return i.Current;
                        previous = i.Current;
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// 第一个子节点
        /// </summary>
        public TreeNode<T> FirstChild
        {
            get { return Children.Count > 0 ? Children[0] : null; }
        }

        /// <summary>
        /// 最后一个子节点
        /// </summary>
        public TreeNode<T> LastChild
        {
            get { return Children.Count > 0 ? Children[Children.Count - 1] : null; }
        }

        private TreeNodeCollection<T> _children;
        /// <summary>
        /// 所有子节点
        /// </summary>
        public TreeNodeCollection<T> Children
        {
            get { return _children ?? (_children = new TreeNodeCollection<T>(this)); }
        }

        /// <summary>
        /// 判断节点是否为叶子节点
        /// </summary>
        public bool IsLeaf
        {
            get { return Children.Count == 0; }
        }

        /// <summary>
        /// 深序遍历
        /// </summary>
        /// <param name="start"></param>
        /// <param name="action"></param>
        public static void DeepVisit(TreeNode<T> start, Action<TreeNode<T>> action)
        {
            if (start == null || action == null) return;
            
            action(start);
            foreach (var child in start.Children)
            {
                DeepVisit(child, action);
            }
        }

        /// <summary>
        /// 层序遍历
        /// </summary>
        /// <param name="start"></param>
        /// <param name="action"></param>
        public static void LayerVisit(TreeNode<T> start, Action<TreeNode<T>> action)
        {
            if (start == null || action == null) return;
            var queue = new Queue<TreeNode<T>>();
            queue.Enqueue(start);
            while (queue.Count > 0)
            {
                var node = queue.Dequeue();
                action(node);

                foreach (var child in node.Children)
                {
                    queue.Enqueue(child);
                }
            }
        }

        /// <summary>
        /// 计算树节点的深度
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static int GetDeep(TreeNode<T> node)
        {
            if (node == null) return 0;

            var max = 0;
            var deep = 0;
            DeepVisit(node, n =>
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
            return max;
        }

        /// <summary>
        /// 计算树节点的深度
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        internal static int GetLeaf(TreeNode<T> node)
        {
            if (node == null) return 0;

            var max = 0;
            foreach (var child in node.Children)
            {
                var deep = GetDeep(child);
                if (deep > max)
                    max = deep;
            }
            return max + 1;
        }

        public override string ToString()
        {
            return string.Format("value:{0}", Value);
        }
    }
}