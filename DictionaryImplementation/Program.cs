using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace DictionaryImplementation
{
    class Program
    {
        /// <summary>
        /// Test for adding random elements in dictionaries.
        /// </summary>
        /// <param name="dictionary">Instance od Dictionary</param>
        /// <param name="count">Count of Iterations</param>
        public static void TestAdd(IDictionary<int,char> dictionary, int count)
        {
            // Random number generator.
            Random rd = new Random();
            string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
            // For measure the execution time of a method.
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                dictionary.Add(rd.Next(), letters[rd.Next(0, letters.Length)]);
            }
            sw.Stop();
            Console.WriteLine("Running Time For Add With Milliseconds: " + sw.ElapsedMilliseconds + "\n");
        }

        /// <summary>
        /// Test for deleting random elements in dictionaries.
        /// </summary>
        /// <param name="dictionary">Instance od Dictionary</param>
        /// <param name="count">Count of Iterations</param>
        public static void TestRemove(IDictionary<int, char> dictionary, int count)
        {
            // Random number generator.
            Random rd = new Random();
            // For measure the execution time of a method.
            Stopwatch sw = Stopwatch.StartNew();
            for (int i = 0; i < count; i++)
            {
                dictionary.Remove(rd.Next());
            }
            //foreach (KeyValuePair<int, char> item in dictionary)
            //    dictionary.Remove(item.Key);
            sw.Stop();
            Console.WriteLine("Running Time for Remove With Milliseconds: " + sw.ElapsedMilliseconds + "\n");
        }

        /// <summary>
        /// For Showing the dictionary elements.
        /// </summary>
        public static void ShowDict(Dictionary<int,char> dictionary)
        {
            foreach (KeyValuePair<int, char> item in dictionary)
                Console.Write("[" + item.Key + " " + item.Value + "]\t");
        }

        static void Main(string[] args)
        {
            // Testing the Dictionary.
            Dictionary<int, char> d = new Dictionary<int, char>();
            Console.WriteLine("Dictionary:\n");
            // Test 1(With 320 iterations)
            TestAdd(d, 320);
            Stopwatch sw1 = Stopwatch.StartNew();
            ShowDict(d);
            sw1.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw1.ElapsedMilliseconds + "\n");
            d.Clear();
            // Test 2(With 640 iterations)
            TestAdd(d, 640);
            sw1 = Stopwatch.StartNew();
            ShowDict(d);
            sw1.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw1.ElapsedMilliseconds + "\n");
            d.Clear();
            // Test 3(With 1280 iterations)
            TestAdd(d, 1280);
            sw1 = Stopwatch.StartNew();
            ShowDict(d);
            sw1.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw1.ElapsedMilliseconds + "\n");
            d.Clear();

            // Testing the Red - Black Tree.
            Red_BlackTree<int, char> rb = new Red_BlackTree<int, char>();
            Console.WriteLine("Red_BlackTree:\n");
            // Test 1(With 320 iterations)
            TestAdd(rb, 320);
            Stopwatch sw2 = Stopwatch.StartNew();
            Console.WriteLine(rb);
            sw2.Stop();
            Console.WriteLine("Running Time With Milliseconds: " + sw2.ElapsedMilliseconds + "\n");
            rb.Clear();
            // Test 2(With 640 iterations)
            TestAdd(rb, 640);
            sw2 = Stopwatch.StartNew();
            Console.WriteLine(rb);
            sw2.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw2.ElapsedMilliseconds + "\n");
            rb.Clear();
            // Test 3(With 1280 iterations)
            TestAdd(rb, 1280);
            sw2 = Stopwatch.StartNew();
            Console.WriteLine(rb);
            sw2.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw2.ElapsedMilliseconds + "\n");
            rb.Clear();

            // Testing the Avl Tree.
            AVLTree<int, char> avl = new AVLTree<int, char>();
            Console.WriteLine("AVLTree:\n");
            // Test 1(With 320 iterations)
            TestAdd(avl, 320);
            Stopwatch sw3 = Stopwatch.StartNew();
            Console.WriteLine(avl);
            sw3.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw3.ElapsedMilliseconds + "\n");
            avl.Clear();
            // Test 2(With 640 iterations)
            TestAdd(avl, 640);
            sw3 = Stopwatch.StartNew();
            Console.WriteLine(avl);
            sw3.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw3.ElapsedMilliseconds + "\n");
            avl.Clear();
            // Test 3(With 1280 iterations)
            TestAdd(avl, 1280);
            sw3 = Stopwatch.StartNew();
            Console.WriteLine(avl);
            sw3.Stop();
            Console.WriteLine("Running Time  With Milliseconds: " + sw3.ElapsedMilliseconds + "\n");
            avl.Clear();

            // Test removing random elements.
            TestRemove(d, 100);
            TestRemove(rb, 100);
            TestRemove(avl, 100);
        }
    }
}
