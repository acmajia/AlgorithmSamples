using System;
using System.Collections.Generic;
using System.Linq;

namespace AvlTreeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var avlTree = new AvlTree();

            var arr = GenerateRamdomSequenceIntegers();

            foreach (var item in arr)
            {
                avlTree.Insert(item);
            }

            Console.WriteLine($"Tree printed as below: ");
            Console.WriteLine();

            Console.WriteLine($"Vertical view:");
            avlTree.PrintVertical();
            Console.WriteLine();

            // works just with a small amount nodes (<=10)
            Console.WriteLine($"Horizontal view:");
            avlTree.PrintHorizontal();
            Console.WriteLine();

            Console.WriteLine($"Traverse by preorder");
            foreach (var i in avlTree.Traverse(AvlTreeTraverseType.Preorder))
            {
                Console.Write($"{i.Value} ");
            }

            Console.WriteLine();
            Console.WriteLine($"Traverse by inorder: ");
            foreach (var i in avlTree.Traverse(AvlTreeTraverseType.Inorder))
            {
                Console.Write($"{i.Value} ");
            }

            Console.WriteLine();
            Console.WriteLine($"Traverse by postorder: ");
            foreach (var i in avlTree.Traverse(AvlTreeTraverseType.Postorder))
            {
                Console.Write($"{i.Value} ");
            }

            Console.Read();
        }

        static int[] GenerateRamdomSequenceIntegers()
        {
            int n = 1_000_000;
            var random = new Random();
            Console.WriteLine("Generating {0} random elements...", n);
            var numbers = Enumerable.Range(0, n).Select(x => random.Next());

            return numbers.ToArray();
        }
    }
}
