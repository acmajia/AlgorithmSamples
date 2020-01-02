using System;
using System.Collections.Generic;
using System.Text;

namespace AvlTreeSample
{
    public static class AvlTreePrinter
    {
        public static void PrintVertical(this AvlTree tree)
        {
            PrintVertical(tree.Root, string.Empty, AvlTreeNodePosition.NoParent, true, false);
        }

        public static void PrintVertical(this AvlTreeNode node, string indent, AvlTreeNodePosition nodePosition, bool last, bool empty)
        {
            Console.Write(indent);
            if (last)
            {
                Console.Write("└─ ");
                indent += "  ";
            }
            else
            {
                Console.Write("├─ ");
                indent += "│ ";
            }

            var stringValue = empty ? "-" : node.Value.ToString();
            PrintValue(stringValue, nodePosition);

            if (!empty && (node.Left != null || node.Right != null))
            {
                if (node.Left != null)
                    node.Left.PrintVertical(indent, AvlTreeNodePosition.Left, false, false);
                else
                    node.PrintVertical(indent, AvlTreeNodePosition.Left, false, true);

                if (node.Right != null)
                    node.Right.PrintVertical(indent, AvlTreeNodePosition.Right, true, false);
                else
                    node.PrintVertical(indent, AvlTreeNodePosition.Right, true, true);
            }
        }

        private static void PrintValue(string value, AvlTreeNodePosition nodePostion)
        {
            switch (nodePostion)
            {
                case AvlTreeNodePosition.Left:
                    PrintLeftValue(value);
                    break;
                case AvlTreeNodePosition.Right:
                    PrintRightValue(value);
                    break;
                case AvlTreeNodePosition.NoParent:
                    Console.WriteLine(value);
                    break;
                default:
                    throw new NotImplementedException();
            }
        }

        private static void PrintLeftValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.Write("L:");
            Console.ForegroundColor = (value == "-") ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        private static void PrintRightValue(string value)
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.Write("R:");
            Console.ForegroundColor = (value == "-") ? ConsoleColor.Red : ConsoleColor.Gray;
            Console.WriteLine(value);
            Console.ForegroundColor = ConsoleColor.Gray;
        }

        public static void PrintHorizontal(this AvlTree tree, int topMargin = 2, int leftMargin = 2)
        {
            var root = tree.Root;

            if (root == null) return;
            int rootTop = Console.CursorTop + topMargin;
            var last = new List<NodeInfo>();
            var next = root;
            for (int level = 0; next != null; level++)
            {
                var item = new NodeInfo { Node = next, Text = $" {next.Value} ({(next.BalanceFator > 0 ? "+" : string.Empty)}{next.BalanceFator})" };
                if (level < last.Count)
                {
                    item.StartPos = last[level].EndPos + 1;
                    last[level] = item;
                }
                else
                {
                    item.StartPos = leftMargin;
                    last.Add(item);
                }
                if (level > 0)
                {
                    item.Parent = last[level - 1];
                    if (next == item.Parent.Node.Left)
                    {
                        item.Parent.Left = item;
                        item.EndPos = Math.Max(item.EndPos, item.Parent.StartPos);
                    }
                    else
                    {
                        item.Parent.Right = item;
                        item.StartPos = Math.Max(item.StartPos, item.Parent.EndPos);
                    }
                }
                next = next.Left ?? next.Right;
                for (; next == null; item = item.Parent)
                {
                    PrintHorizontal(item, rootTop + 2 * level);
                    if (--level < 0) break;
                    if (item == item.Parent.Left)
                    {
                        item.Parent.StartPos = item.EndPos;
                        next = item.Parent.Node.Right;
                    }
                    else
                    {
                        if (item.Parent.Left == null)
                            item.Parent.EndPos = item.StartPos;
                        else
                            item.Parent.StartPos += (item.StartPos - item.Parent.EndPos) / 2;
                    }
                }
            }
            Console.SetCursorPosition(0, rootTop + 2 * last.Count - 1);
        }

        private static void PrintHorizontal(NodeInfo item, int top)
        {
            SwapColors();
            PrintHorizontal(item.Text, top, item.StartPos);
            SwapColors();
            if (item.Left != null)
                PrintHorizontalLink(top + 1, "┌", "┘", item.Left.StartPos + item.Left.Size / 2, item.StartPos);
            if (item.Right != null)
                PrintHorizontalLink(top + 1, "└", "┐", item.EndPos - 1, item.Right.StartPos + item.Right.Size / 2);
        }

        private static void PrintHorizontalLink(int top, string start, string end, int startPos, int endPos)
        {
            PrintHorizontal(start, top, startPos);
            PrintHorizontal("─", top, startPos + 1, endPos);
            PrintHorizontal(end, top, endPos);
        }

        private static void PrintHorizontal(string s, int top, int left, int right = -1)
        {
            Console.SetCursorPosition(left, top);
            if (right < 0) right = left + s.Length;
            while (Console.CursorLeft < right) Console.Write(s);
        }

        private static void SwapColors()
        {
            var color = Console.ForegroundColor;
            Console.ForegroundColor = Console.BackgroundColor;
            Console.BackgroundColor = color;
        }

        private class NodeInfo
        {
            public AvlTreeNode Node;
            public string Text;
            public int StartPos;
            public int Size { get { return Text.Length; } }
            public int EndPos { get { return StartPos + Size; } set { StartPos = value - Size; } }
            public NodeInfo Parent, Left, Right;
        }
    }
}
