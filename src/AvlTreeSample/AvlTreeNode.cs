using System;
using System.Collections.Generic;
using System.Text;

namespace AvlTreeSample
{
    public class AvlTreeNode
    {
        public AvlTreeNode(int value)
        {
            Value = value;
        }

        public AvlTreeNode Parent { get; private set; }
        public int Value { get; private set; }
        public short BalanceFator { get; private set; }
        public AvlTreeNode Left { get; private set; }
        public AvlTreeNode Right { get; private set; }
    }
}
