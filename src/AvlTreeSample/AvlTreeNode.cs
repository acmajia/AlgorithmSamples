using System;
using System.Collections.Generic;
using System.Text;

namespace AvlTreeSample
{
    public class AvlTreeNode
    {
        public AvlTreeNode Parent { get; set; }
        public AvlTreeNodePosition Position { get; set; }
        public int Value { get; set; }
        public short BalanceFator { get; set; }
        public AvlTreeNode Left { get; set; }
        public AvlTreeNode Right { get; set; }
    }
}
