using System;

namespace AvlTreeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var avlTree = new AvlTree();

            for (var i = 10; i > 0; i--)
            {
                avlTree.Insert(i);
            }

            foreach (var i in avlTree.Traverse(AvlTreeTraverseType.Postorder))
            {

            }
        }
    }
}
