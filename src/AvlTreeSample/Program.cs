using System;
using System.Collections.Generic;

namespace AvlTreeSample
{
    class Program
    {
        static void Main(string[] args)
        {
            var avlTree = new AvlTree();

            var arr = new int[] { 2, 4, 6, 1, 9, 8, 7, 3, 5, 10 };

            arr = GenerateRamdomSequenceIntegers();

            foreach (var item in arr)
            {
                avlTree.Insert(item);
            }

            Console.WriteLine($"Tree printed as below: ");
            Console.WriteLine();

            Console.WriteLine($"Vertical view:");
            avlTree.PrintVertical();
            Console.WriteLine();

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
            var list = new List<int>();

            var random = new Random();
            
            while(list.Count < 10)
            {
                var value = random.Next(short.MinValue, short.MaxValue);
                if (list.Contains(value))
                {
                    continue;
                }
                list.Add(value);
            }

            return list.ToArray();
        }
    }
}
