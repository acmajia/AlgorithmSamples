using System;
using System.Collections.Generic;
using System.Text;

namespace AvlTreeSample
{
    public class AvlTree
    {
        public AvlTreeNode Root { get; private set; }

        public AvlTreeNode Insert(int value)
        {
            if (Root == null)
            {
                InitRoot(value);
                return Root;
            }

            var newNode = InsertNewNode(value);
            return newNode;
        }

        public IEnumerable<AvlTreeNode> Traverse(AvlTreeTraverseType traverseType)
        {
            if (Root == null)
            {
                return new List<AvlTreeNode>();
            }

            switch (traverseType)
            {
                case AvlTreeTraverseType.Preorder:
                    return TraverseByPreorder();
                case AvlTreeTraverseType.Inorder:
                    return TraverseByInorder();
                case AvlTreeTraverseType.Postorder:
                    return TraverseByPostorder();
                default:
                    throw new NotSupportedException();
            }
        }

        private AvlTreeNode InsertNewNode(int value)
        {
            AvlTreeNode newNode = null;
            var currentNode = Root;

            while (true)
            {
                if (value == currentNode.Value)
                {
                    throw new ArgumentException($"Value '{value}' already exists in the tree.");
                }

                if (value > currentNode.Value)
                {
                    if (currentNode.Right == null)
                    {
                        newNode = CreateNewNode(currentNode, AvlTreeNodePosition.Right, value);
                        break;
                    }
                    else
                    {
                        currentNode = currentNode.Right;
                        continue;
                    }
                }
                else
                {
                    if (currentNode.Left == null)
                    {
                        newNode = CreateNewNode(currentNode, AvlTreeNodePosition.Left, value);
                        break;
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                        continue;
                    }
                }
            }

            OnNewNodeInserted(newNode);

            return newNode;
        }

        private IEnumerable<AvlTreeNode> TraverseByPostorder()
        {
            var stack = new Stack<AvlTreeNode>();

            stack.Push(Root);

            AvlTreeNode prev = null;
            while (stack.Count > 0)
            {
                var current = stack.Peek();

                if ((current.Left != null || current.Right != null)
                    &&
                    (
                        prev == null ||
                        (current.Left != prev && current.Right != prev)
                    ))
                {
                    if (current.Right != null)
                    {
                        stack.Push(current.Right);
                    }

                    if (current.Left != null)
                    {
                        stack.Push(current.Left);
                    }
                }
                else
                {
                    stack.Pop();
                    prev = current;
                    yield return current;
                }
            }
        }

        private IEnumerable<AvlTreeNode> TraverseByInorder()
        {
            var stack = new Stack<AvlTreeNode>();

            stack.Push(Root);
            var current = Root;
            bool goFuther = true;

            while(stack.Count > 0)
            {
                current = stack.Peek();

                if (goFuther && current.Left != null)
                {
                    stack.Push(current.Left);
                    goFuther = true;
                    continue;
                }

                stack.Pop();

                yield return current;

                if (current.Right != null)
                {
                    stack.Push(current.Right);
                    goFuther = true;
                    continue;
                }

                goFuther = false;
            }
        }

        private IEnumerable<AvlTreeNode> TraverseByPreorder()
        {
            var stack = new Stack<AvlTreeNode>();

            stack.Push(Root);

            while (stack.Count > 0)
            {
                var current = stack.Pop();

                yield return current;

                if (current.Right != null)
                {
                    stack.Push(current.Right);
                }

                if (current.Left != null)
                {
                    stack.Push(current.Left);
                }
            }
        }

        private void OnNewNodeInserted(AvlTreeNode insertion)
        {
            AvlTreeNodePosition lastPos = default;
            AvlTreeNodePosition currentPos = default;

            var index = 0;

            var currentNode = insertion.Parent;
            var prevNode = insertion;
            currentPos = insertion.Position;

            while (currentNode != null)
            {
                if (prevNode.Position == AvlTreeNodePosition.Left)
                {
                    currentNode.LeftHeight = prevNode.Height + 1;
                }
                else if (prevNode.Position == AvlTreeNodePosition.Right)
                {
                    currentNode.RightHeight = prevNode.Height + 1;
                }

                index++;

                if (currentNode.BalanceFator == 2 || currentNode.BalanceFator == -2)
                {
                    currentNode = ResolveRotation(insertion, currentNode, currentPos, lastPos);
                }

                lastPos = currentPos;
                currentPos = currentNode.Position;
                prevNode = currentNode;
                currentNode = currentNode.Parent;
            }
        }

        private AvlTreeNode ResolveRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot, AvlTreeNodePosition firstLevelPos, AvlTreeNodePosition secondLevelPos)
        {
            if (firstLevelPos == AvlTreeNodePosition.Left)
            {
                if (secondLevelPos == AvlTreeNodePosition.Left)
                {
                    return DoLeftLeftRotation(insertion, rotationRoot);
                }
                else if (secondLevelPos == AvlTreeNodePosition.Right)
                {
                    return DoLeftRightRotation(insertion, rotationRoot);
                }
            }
            else if (firstLevelPos == AvlTreeNodePosition.Right)
            {
                if (secondLevelPos == AvlTreeNodePosition.Left)
                {
                    return DoRightLeftRotation(insertion, rotationRoot);
                }
                else if (secondLevelPos == AvlTreeNodePosition.Right)
                {
                    return DoRightRightRotation(insertion, rotationRoot);
                }
            }

            throw new NotSupportedException();
        }

        private AvlTreeNode DoRightRightRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的左子树的右子树上时，执行RR旋转。

            // RR旋转与LL旋转是对称的关系。
            // 设A的右子结点为Right。要执行RR旋转，将A的右指针指向right的左子结点，right的左指针指向A，原来指向A的指针修改为指向right。
            // 完成旋转以后，将A和right的平衡因子都修改为0。所有其他结点的平衡因子都没有改变。

            var right = rotationRoot.Right;
            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                SetLeft(rotationRoot.Parent, right);
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                SetRight(rotationRoot.Parent, right);
            }
            else
            {
                SetRootNode(right);
            }

            SetRight(rotationRoot, right.Left);
            SetLeft(right, rotationRoot);

            return right;
        }

        private AvlTreeNode DoLeftLeftRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的左子树的左子树上时，执行LL旋转。

            // 设left为A的左子树，要执行LL旋转：
            //   1.将原来指向A的指针指向left
            //   2.将A的左指针指向left的右子结点
            //   3.left的右指针指向A
            // 旋转过后，将A和left的平衡因子都改为0。所有其他结点的平衡因子没有发生变化。

            var left = rotationRoot.Left;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                SetLeft(rotationRoot.Parent, left);
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                SetRight(rotationRoot.Parent, left);
            }
            else
            {
                SetRootNode(left);
            }

            SetLeft(rotationRoot, left.Right);
            SetRight(left, rotationRoot);

            return left;
        }

        private AvlTreeNode DoRightLeftRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的右子树的左子树上时，执行RL旋转。

            // RL旋转与LR旋转是对称的关系。
            // 设A的右子结点为right，right的左子结点为grandchild。要执行RL旋转，将right结点的左子结点指向grandchild的右子结点，将grandchild的右子结点指向right，将A的右子结点指向grandchild的左子结点，将grandchild的左子结点指向A，最后将原来指向A的指针指向grandchild。
            // 执行RL旋转以后，调整结点的平衡因子取决于旋转前grandchild结点的原平衡因子。这里也有三种情况需要考虑：
            // 如果grandchild的原始平衡因子值为 + 1，将A的平衡因子更新为0，right的更新为 - 1；
            // 如果grandchild的原始平衡因子值为  0，将A和right的平衡因子都更新为0；
            // 如果grandchild的原始平衡因子值为 - 1，将A的平衡因子更新为 + 1，right的更新为0；
            // 在所有情况中，都将grandchild的新平衡因子设置为0。所有其他结点的平衡因子不发生改变。

            var right = rotationRoot.Right;
            var grandChild = right.Left;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                SetLeft(rotationRoot.Parent, grandChild);
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                SetRight(rotationRoot.Parent, grandChild);
            }
            else
            {
                SetRootNode(grandChild);
            }

            SetLeft(right, grandChild.Right);
            SetRight(grandChild, right);
            SetRight(rotationRoot, grandChild.Left);
            SetLeft(grandChild, rotationRoot);

            return grandChild;
        }

        private AvlTreeNode DoLeftRightRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的左子树的右子树上时，执行LR旋转。

            // 设left是A的左子结点，并设A的子孙结点grandchild为left的右子结点。
            // 要执行LR旋转，将left的右子结点指向grandchild的左子结点，grandchild的左子结点指向left，A的左子结点指向grandchild的右子结点，再将grandchild的右子结点指向A，最后将原来指向A的指针指向grandchild。
            // 执行LR旋转之后，调整结点的平衡因子取决于旋转前grandchild结点的原平衡因子值。
            // 如果grandchild结点的原始平衡因子为 + 1，就将A的平衡因子设为 - 1，将left的平衡因子设为0。
            // 如果grandchild结点的原始平衡因子为0，就将A和left的平衡因子都设置为0。
            // 如果grandchild结点的原始平衡因子为 - 1，就将A的平衡因子设置为0，将left的平衡因子设置为 + 1。
            // 在所有的情况下，grandchild的新平衡因子都是0。所有其他结点的平衡因子都没有改变。

            var left = rotationRoot.Left;
            var grandChild = left.Right;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                SetLeft(rotationRoot.Parent, grandChild);
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                SetRight(rotationRoot.Parent, grandChild);
            }
            else
            {
                SetRootNode(grandChild);
            }

            SetRight(left, grandChild.Left);
            SetLeft(grandChild, left);
            SetLeft(rotationRoot, grandChild.Right);
            SetRight(grandChild, rotationRoot);

            return grandChild;
        }

        private void SetRootNode(AvlTreeNode node)
        {
            if (node == null)
            {
                throw new ArgumentNullException();
            }

            node.Parent = null;
            node.Position = AvlTreeNodePosition.NoParent;
            Root = node;
        }

        private void SetLeft(AvlTreeNode parent, AvlTreeNode child)
        {
            SetChild(parent, child, AvlTreeNodePosition.Left);
        }

        private void SetRight(AvlTreeNode parent, AvlTreeNode child)
        {
            SetChild(parent, child, AvlTreeNodePosition.Right);
        }

        private void SetChild(AvlTreeNode parent, AvlTreeNode child, AvlTreeNodePosition position)
        {
            if (parent == null)
            {
                position = AvlTreeNodePosition.NoParent;
            }
            else if (position == AvlTreeNodePosition.Left)
            {
                parent.Left = child;
                parent.LeftHeight = (child?.Height ?? -1) + 1;
            }
            else if (position == AvlTreeNodePosition.Right)
            {
                parent.Right = child;
                parent.RightHeight = (child?.Height ?? -1) + 1;
            }

            if (child != null)
            {
                child.Position = position;
                child.Parent = parent;
            }
        }

        private void InitRoot(int value)
        {
            Root = CreateNewNode(null, AvlTreeNodePosition.NoParent, value);
        }

        private AvlTreeNode CreateNewNode(AvlTreeNode parent, AvlTreeNodePosition position, int value)
        {
            var newNode = new AvlTreeNode
            {
                Left = null,
                Parent = parent,
                Right = null,
                Value = value,
                Position = position,
                LeftHeight = 0,
                RightHeight = 0
            };

            if (position == AvlTreeNodePosition.Left)
            {
                parent.Left = newNode;
            }
            else if (position == AvlTreeNodePosition.Right)
            {
                parent.Right = newNode;
            }

            return newNode;
        }
    }
}
