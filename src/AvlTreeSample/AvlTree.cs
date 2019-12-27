using System;
using System.Collections.Generic;
using System.Text;

namespace AvlTreeSample
{
    public class AvlTree
    {
        public AvlTree()
        {
            InitRoot();
        }

        public AvlTreeNode Root { get; private set; }

        public void Insert(int value)
        {
            if (value < Root.Value)
            {

            }
            else
            {

            }
        }

        private void InitRoot()
        {
            Root = new AvlTreeNode(0);
        }
    }
}
