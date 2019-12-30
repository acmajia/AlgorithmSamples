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

        private AvlTreeNode InsertNewNode(int value)
        {
            AvlTreeNode newNode = null;
            short balanceFoctorChangeValue = 0;
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
                        balanceFoctorChangeValue = -1;
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
                        balanceFoctorChangeValue = +1;
                        break;
                    }
                    else
                    {
                        currentNode = currentNode.Left;
                        continue;
                    }
                }
            }

            OnBalanceFactorChanging(newNode, balanceFoctorChangeValue);

            return newNode;
        }

        public IEnumerable<AvlTreeNode> Traverse(AvlTreeTraverseType traverseType)
        {
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

        private IEnumerable<AvlTreeNode> TraverseByPostorder()
        {
            var stack = new Stack<AvlTreeNode>();

            stack.Push(Root);

            AvlTreeNode prev = null;
            while (stack.Count > 0)
            {
                var current = stack.Peek();

                if ((current.Left != null || current.Right != null) && current.Left != prev && current.Right != prev)
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
            throw new NotImplementedException();
        }

        private IEnumerable<AvlTreeNode> TraverseByPreorder()
        {
            throw new NotImplementedException();
        }

        private void OnBalanceFactorChanging(AvlTreeNode childNode, short balanceFoctorChangeValue)
        {
            AvlTreeNodePosition lastPos = default;
            AvlTreeNodePosition currentPos = default;

            var index = 0;

            var currentNode = childNode.Parent;
            currentPos = currentNode.Position;

            while (currentNode != null)
            {
                index++;

                currentNode.BalanceFator += balanceFoctorChangeValue;
                if (currentNode.BalanceFator == 2 || currentNode.BalanceFator == -2)
                {
                    ResolveRotation(childNode, currentNode, currentPos, lastPos);
                    return;
                }
                else
                {
                    lastPos = currentPos;
                    currentPos = currentNode.Position;
                    currentNode = currentNode.Parent;
                    continue;
                }
            }
        }

        private void ResolveRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot, AvlTreeNodePosition firstLevelPos, AvlTreeNodePosition secondLevelPos)
        {
            if (firstLevelPos == AvlTreeNodePosition.Left)
            {
                if (secondLevelPos == AvlTreeNodePosition.Left)
                {
                    DoLeftLeftRotation(insertion, rotationRoot);
                    return;
                }
                else if (secondLevelPos == AvlTreeNodePosition.Right)
                {
                    DoLeftRightRotation(insertion, rotationRoot);
                    return;
                }
            }
            else if (firstLevelPos == AvlTreeNodePosition.Right)
            {
                if (secondLevelPos == AvlTreeNodePosition.Left)
                {
                    DoRightLeftRotation(insertion, rotationRoot);
                    return;
                }
                else if (secondLevelPos == AvlTreeNodePosition.Right)
                {
                    DoRightRightRotation(insertion, rotationRoot);
                    return;
                }
            }

            throw new NotSupportedException();
        }

        private void DoRightRightRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的左子树的右子树上时，执行RR旋转。

            // RR旋转与LL旋转是对称的关系。
            // 设A的右子结点为Right。要执行RR旋转，将A的右指针指向right的左子结点，right的左指针指向A，原来指向A的指针修改为指向right。
            // 完成旋转以后，将A和right的平衡因子都修改为0。所有其他结点的平衡因子都没有改变。

            var right = rotationRoot.Right;

            rotationRoot.Right = right.Left;
            if (rotationRoot.Right != null)
            {
                rotationRoot.Right.Parent = rotationRoot;
                rotationRoot.Right.Position = AvlTreeNodePosition.Right;
            }

            right.Left = rotationRoot;
            right.Left.Parent = right;
            right.Left.Position = AvlTreeNodePosition.Left;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                rotationRoot.Parent.Left = right;
                rotationRoot.Parent.Left.Parent = rotationRoot.Parent.Left;
                rotationRoot.Parent.Left.Position = AvlTreeNodePosition.Left;
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                rotationRoot.Parent.Right = right;
                rotationRoot.Parent.Right.Parent = rotationRoot.Parent.Right;
                rotationRoot.Parent.Right.Position = AvlTreeNodePosition.Right;
            }
            else
            {
                right.Position = AvlTreeNodePosition.NoParent;
            }

            rotationRoot.BalanceFator = 0;
            right.BalanceFator = 0;
        }

        private void DoRightLeftRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
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

            right.Left = grandChild.Right;
            right.Left.Parent = right;
            right.Left.Position = AvlTreeNodePosition.Left;

            grandChild.Right = right;
            grandChild.Right.Parent = grandChild;
            grandChild.Right.Position = AvlTreeNodePosition.Right;

            rotationRoot.Right = grandChild.Left;
            rotationRoot.Right.Parent = rotationRoot;
            rotationRoot.Right.Position = AvlTreeNodePosition.Right;

            grandChild.Left = rotationRoot;
            grandChild.Left.Parent = grandChild;
            grandChild.Left.Position = AvlTreeNodePosition.Left;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                rotationRoot.Parent.Left = grandChild;
                rotationRoot.Parent.Left.Parent = rotationRoot.Parent;
                rotationRoot.Parent.Left.Position = AvlTreeNodePosition.Left;
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                rotationRoot.Parent.Right = grandChild;
                rotationRoot.Parent.Right.Parent = rotationRoot.Parent;
                rotationRoot.Parent.Right.Position = AvlTreeNodePosition.Right;
            }
            else
            {
                grandChild.Position = AvlTreeNodePosition.NoParent;
            }

            if (grandChild.BalanceFator == 1)
            {
                rotationRoot.BalanceFator = -1;
                right.BalanceFator = 0;
            }
            else if (grandChild.BalanceFator == 0)
            {
                rotationRoot.BalanceFator = 0;
                right.BalanceFator = 0;
            }
            else if (grandChild.BalanceFator == -1)
            {
                rotationRoot.BalanceFator = 0;
                right.BalanceFator = 1;
            }
            else
            {
                throw new NotSupportedException();
            }

            grandChild.BalanceFator = 0;
        }

        private void DoLeftRightRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
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

            left.Right = grandChild.Left;
            left.Right.Parent = left;
            left.Right.Position = AvlTreeNodePosition.Right;

            grandChild.Left = left;
            grandChild.Left.Parent = grandChild;
            grandChild.Left.Position = AvlTreeNodePosition.Left;

            rotationRoot.Left = grandChild.Right;
            rotationRoot.Left.Parent = rotationRoot;
            rotationRoot.Left.Position = AvlTreeNodePosition.Left;

            grandChild.Right = rotationRoot;
            grandChild.Right.Parent = grandChild;
            grandChild.Right.Position = AvlTreeNodePosition.Right;

            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                rotationRoot.Parent.Left = grandChild;
                rotationRoot.Parent.Left.Parent = rotationRoot.Parent;
                rotationRoot.Parent.Left.Position = AvlTreeNodePosition.Left;
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                rotationRoot.Parent.Right = grandChild;
                rotationRoot.Parent.Right.Parent = rotationRoot.Parent;
                rotationRoot.Parent.Right.Position = AvlTreeNodePosition.Right;
            }
            else
            {
                grandChild.Position = AvlTreeNodePosition.NoParent;
            }

            if (grandChild.BalanceFator == 1)
            {
                rotationRoot.BalanceFator = -1;
                left.BalanceFator = 0;
            }
            else if (grandChild.BalanceFator == 0)
            {
                rotationRoot.BalanceFator = 0;
                left.BalanceFator = 0;
            }
            else if (grandChild.BalanceFator == -1)
            {
                rotationRoot.BalanceFator = 0;
                left.BalanceFator = 1;
            }
            else
            {
                throw new NotSupportedException();
            }

            grandChild.BalanceFator = 0;
        }

        private void DoLeftLeftRotation(AvlTreeNode insertion, AvlTreeNode rotationRoot)
        {
            // 当x位于A的左子树的左子树上时，执行LL旋转。

            // 设left为A的左子树，要执行LL旋转，将A的左指针指向left的右子结点，left的右指针指向A，将原来指向A的指针指向left。
            // 旋转过后，将A和left的平衡因子都改为0。所有其他结点的平衡因子没有发生变化。

            var left = rotationRoot.Left;
            rotationRoot.Left = left.Right;
            if (rotationRoot.Left != null)
            {
                rotationRoot.Left.Parent = rotationRoot;
                rotationRoot.Left.Position = AvlTreeNodePosition.Left;
            }

            left.Right = rotationRoot;
            left.Right.Parent = left;
            left.Right.Position = AvlTreeNodePosition.Right;
            
            if (rotationRoot.Position == AvlTreeNodePosition.Left)
            {
                rotationRoot.Parent.Left = left;
                rotationRoot.Parent.Left.Parent = rotationRoot.Parent.Left;
                rotationRoot.Parent.Left.Position = AvlTreeNodePosition.Left;
            }
            else if (rotationRoot.Position == AvlTreeNodePosition.Right)
            {
                rotationRoot.Parent.Right = left;
                rotationRoot.Parent.Right.Parent = rotationRoot.Parent.Right;
                rotationRoot.Parent.Right.Position = AvlTreeNodePosition.Right;
            }
            else
            {
                left.Position = AvlTreeNodePosition.NoParent;
            }

            rotationRoot.BalanceFator = 0;
            left.BalanceFator = 0;
        }

        private void InitRoot(int value)
        {
            Root = CreateNewNode(null, AvlTreeNodePosition.NoParent, value);
        }

        private AvlTreeNode CreateNewNode(AvlTreeNode parent, AvlTreeNodePosition position, int value)
        {
            var newNode = new AvlTreeNode
            {
                BalanceFator = 0,
                Left = null,
                Parent = parent,
                Right = null,
                Value = value,
                Position = position
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
