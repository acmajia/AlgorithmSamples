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
        public int BalanceFator
        {
            get
            {
                return LeftHeight - RightHeight;
            }
        }
        public AvlTreeNode Left { get; set; }
        public int LeftHeight { get; set; }
        public AvlTreeNode Right { get; set; }
        public int RightHeight { get; set; }
        public int Height
        {
            get
            {
                return LeftHeight > RightHeight ? LeftHeight : RightHeight;
            }
        }
    }
}
