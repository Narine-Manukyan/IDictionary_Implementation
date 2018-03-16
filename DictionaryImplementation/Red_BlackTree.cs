using System;
using System.Collections;
using System.Collections.Generic;

namespace DictionaryImplementation
{
    /// <summary>
    /// This Class Implements the Node Construction of Tree.
    /// </summary>
    public sealed class Node<TKey, TValue> where TKey : IComparable<TKey>
    {
        internal TKey Key;
        internal TValue Value;
        // Color of Node.
        internal byte Color;
        // Left Node of the Current node. 
        internal Node<TKey, TValue> LeftNode { set; get; }
        // Right Node of the Current node. 
        internal Node<TKey, TValue> RightNode { set; get; }
        // Parent Node of the Current node.
        internal Node<TKey, TValue> Parent { set; get; }

        // The Parameterfull Constructor.
        public Node(TKey key, TValue val)
        {
            this.Key = key;
            this.Value = val;
            this.Color = Red_BlackTree<TKey, TValue>.RED;
            this.LeftNode = null;
            this.RightNode = null;
        }

        /// <summary>
        /// Overriding method ToString().
        /// </summary>
        public override string ToString()
        {
            return "Key: " + this.Key.ToString() + " Value: " + this.Value.ToString() + " Color: " + this.Color;
        }
    }

    /// <summary>
    /// This Class Implements The Red-Black Tree Construction With Dictionary.
    /// </summary>
    /// <typeparam name="TKey">Key of Key-Value Pair</typeparam>
    /// <typeparam name="TValue">Value of Key-Value Pair</typeparam>
    public class Red_BlackTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// This is a color of Node.
        /// </summary>
        internal const byte RED = 0;

        /// <summary>
        /// This is a color of Node.
        /// </summary>
        internal const byte BLACK = 1;

        /// <summary>
        /// Returns the Root of the tree.
        /// </summary>
        public Node<TKey, TValue> RootNode { private set; get; }

        /// <summary>
        /// Returns the number of items currently in the tree
        /// </summary>
        public int Count { private set; get; }

        /// <summary>
        /// The parameterfull constructor.
        /// </summary>
        /// <param name="count"></param>
        public Red_BlackTree(int count)
        {
            this.Count = count;
            keys = new List<TKey>(this.Count);
            values = new List<TValue>(this.Count);
        }

        /// <summary>
        /// The Parameterless constructor.
        /// </summary>
        public Red_BlackTree()
        {
            this.RootNode = null;
            this.Count = 0;
        }

        /// <summary>
        /// Rotating Left.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Node</returns>
        private Node<TKey, TValue> RotateLeft(Node<TKey, TValue> node)
        {
            // Set temp.
            Node<TKey, TValue> temp = node.RightNode;
            // Turn temp’s left subtree into node’s right subtree.
            node.RightNode = temp.LeftNode;
            if (temp.LeftNode != null)
                temp.LeftNode.Parent = node;
            if (temp != null)
                // Link node's parent to temp
                temp.Parent = node.Parent;
            if (node.Parent == null)
                this.RootNode = temp;
            else if (node == node.Parent.LeftNode)
                node.Parent.LeftNode = temp;
            else
                node.Parent.RightNode = temp;
            // Put node on temp's left
            temp.LeftNode = node;
            if (node != null)
                node.Parent = temp;
            temp.Color = node.Color;
            node.Color = RED;
            return temp;
        }

        /// <summary>
        /// Rotating Right.
        /// right rotate is simply mirror code from left rotate.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Node</returns>
        private Node<TKey, TValue> RotateRight(Node<TKey, TValue> node)
        {
            // Set temp.
            Node<TKey, TValue> temp = node.LeftNode;
            // Turn temp’s right subtree into node’s left subtree.
            node.LeftNode = temp.RightNode;
            if (temp.RightNode != null)
                temp.RightNode.Parent = node;
            if (temp != null)
                // Link node's parent to temp
                temp.Parent = node.Parent;
            if (node.Parent == null)
                this.RootNode = temp;
            else if (node == node.Parent.RightNode)
                node.Parent.RightNode = temp;
            if (node.Parent != null && node == node.Parent.LeftNode)
                node.Parent.LeftNode = temp;
            // Put node on temp's right.
            temp.RightNode = node;
            if (node != null)
                node.Parent = temp;
            temp.Color = node.Color;
            node.Color = RED;
            return temp;
        }

        /// <summary>
        /// Displaying Tree.
        /// </summary>
        public void DisplayTree()
        {
            if (this.RootNode == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (this.RootNode != null)
            {
                InOrderDisplay(this.RootNode);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// In Order Displaying Tree.
        /// </summary>
        private void InOrderDisplay(Node<TKey, TValue> current)
        {
            if (current != null)
            {
                InOrderDisplay(current.LeftNode);
                Console.Write("({0} {1}) ", current.Key, current.Value);
                InOrderDisplay(current.RightNode);
            }
        }

        /// <summary>
        /// Min finds the smallest node key including the given node .      
        /// </summary>
        private Node<TKey, TValue> Min(Node<TKey, TValue> node)
        {
            if (node != null)
            {
                while (node.LeftNode.LeftNode != null)
                {
                    node = node.LeftNode;
                }
                if (node.LeftNode.RightNode != null)
                    node = node.LeftNode.RightNode;
            }
            return node;
        }

        /// <summary>
        /// Find Tree Successor.
        /// </summary>
        /// <param name="node">Node</param>
        /// <returns>Node</returns>
        private Node<TKey, TValue> TreeSuccessor(Node<TKey, TValue> node)
        {
            if (node.LeftNode != null)
            {
                return Min(node);
            }
            else
            {
                Node<TKey, TValue> temp = node.Parent;
                while (temp != null && node == temp.Parent)
                {
                    node = temp;
                    temp = temp.Parent;
                }
                return temp;
            }
        }

        /// <summary>
        /// Get a value stored in the tree using a key.
        /// </summary>
        public TValue this[TKey key]
        {
            get
            {
                if (key == null)
                    throw new Exception("Key is null");
                Node<TKey, TValue> temp = this.Retrieve(key);
                if (temp == null)
                    throw new Exception("Key not found.");
                return temp.Value;
            }
            set
            {
                if (key == null)
                    throw new Exception("Key is null");
                Node<TKey, TValue> temp = this.Retrieve(key);
                if (temp == null)
                    throw new Exception("Key not found.");
                temp.Value = value;
            }
        }

        // Collectrion for Keys.
        private List<TKey> keys;
        // Collection for Values.
        private List<TValue> values;

        /// <summary>
        /// Gets an ICollection<TKey> containing the keys of the IDictionary[TKey,Tvalue>.
        /// </summary>
        public ICollection<TKey> Keys
        {
            get
            {
                if (this.keys == null)
                    this.keys = new List<TKey>();
                return this.keys;
            }
        }

        /// <summary>
        /// Gets an ICollection<T> containing the values in the IDictionary[TKey, TValue>.
        /// </summary>
        public ICollection<TValue> Values
        {
            get
            {
                if (this.values == null)
                    this.values = new List<TValue>();
                return this.values;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the collection is read-only.
        /// </summary>
        public bool IsReadOnly
        {
            get { return false; }
        }

        /// <summary>
        /// Add a specified Key and Value to the Dictionary.
        /// </summary>
        public void Add(TKey key, TValue value)
        {
            KeyValuePair<TKey, TValue> newNode = new KeyValuePair<TKey, TValue>(key, value);
            Add(newNode);
        }

        /// <summary>
        /// Add a specified Key-Value Pair to the Dictionary.
        /// </summary>
        /// <param name="item">Key-ValuePair node</param>
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            //Only add a new node if the key does not already exist
            if (!ContainsKey(item.Key))
            {
                this.Count++;
                this.Keys.Add(item.Key);
                this.Values.Add(item.Value);
                // Creating new item.
                Node<TKey, TValue> newItem = new Node<TKey, TValue>(item.Key, item.Value);
                if (this.RootNode == null)
                {
                    this.RootNode = newItem;
                    this.RootNode.Color = BLACK;
                    return;
                }
                Node<TKey, TValue> node1 = null;
                Node<TKey, TValue> node2 = this.RootNode;
                while (node2 != null)
                {
                    node1 = node2;
                    if (newItem.Key.CompareTo(node2.Key) < 0)
                        node2 = node2.LeftNode;
                    else
                        node2 = node2.RightNode;
                }
                newItem.Parent = node1;
                if (node1 == null)
                    this.RootNode = newItem;
                else if (newItem.Key.CompareTo(node1.Key) < 0)
                    node1.LeftNode = newItem;
                else
                    node1.RightNode = newItem;
                newItem.LeftNode = null;
                newItem.RightNode = null;
                // Color the new node red.
                newItem.Color = RED;
                // Calling the method to check for violations and fix.
                AddBalancing(newItem);
            }
            else
                Console.WriteLine("This Key-Value Pair is already Exists.");
        }

        /// <summary>
        /// Function for Balancing Tree. Checks Red-Black Tree properties.
        /// </summary>
        /// <param name="item">Node</param>
        private void AddBalancing(Node<TKey, TValue> item)
        {
            while (item != RootNode && item.Parent.Color == RED)
            {
                // We have a violation.
                if (item.Parent == item.Parent.Parent.LeftNode)
                {
                    Node<TKey, TValue> temp = item.Parent.Parent.RightNode;
                    //Case 1: Uncle is red.
                    if (temp != null && temp.Color == RED)
                    {
                        item.Parent.Color = BLACK;
                        temp.Color = BLACK;
                        item.Parent.Parent.Color = RED;
                        item = item.Parent.Parent;
                    }
                    // Case 2: Uncle is black.
                    else
                    {
                        if (item == item.Parent.RightNode)
                        {
                            item = item.Parent;
                            RotateLeft(item);
                        }
                        //Case 3: recolour & rotate
                        item.Parent.Color = BLACK;
                        item.Parent.Parent.Color = RED;
                        RotateRight(item.Parent.Parent);
                    }
                }
                else
                {
                    // Mirror image of code above.
                    Node<TKey, TValue> temp = item.Parent.Parent.LeftNode;
                    // Case 1.
                    if (temp != null && temp.Color == RED)
                    {
                        item.Parent.Color = RED;
                        temp.Color = RED;
                        item.Parent.Parent.Color = BLACK;
                        item = item.Parent.Parent;
                    }
                    // Case 2.
                    else
                    {
                        if (item == item.Parent.LeftNode)
                        {
                            item = item.Parent;
                            RotateRight(item);
                        }
                        // Case 3: recolor and rotate.
                        item.Parent.Color = BLACK;
                        item.Parent.Parent.Color = RED;
                        RotateLeft(item.Parent.Parent);

                    }

                }
                // Recolor the root black as necessary.
                RootNode.Color = BLACK;
            }
        }

        /// <summary>
        /// Removes all items from the Tree.
        /// </summary>
        public void Clear()
        {
            this.RootNode = null;
            this.Count = 0;
            this.Keys.Clear();
        }

        /// <summary>
        /// Determines whether the tree contains a specific item.
        /// </summary>
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ContainsKey(item.Key);
        }

        /// <summary>
        /// Determines whether the tree contains a specific key.
        /// </summary>
        public bool ContainsKey(TKey key)
        { 
            // Calling Retrieve search function.
            return (Retrieve(key) != null);
        }

        /// <summary>
        /// Copies the elements of the tree to an array, starting at a particular index.
        /// </summary>
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (arrayIndex < 0)
                throw new Exception("ArrayIndex cannot be less than 0");
            if (array == null)
                throw new Exception("Array cannot be null");
            if ((array.Length - arrayIndex) < this.Count)
                throw new Exception("Index is out of range.");
            int currentIndex = arrayIndex;
            foreach (KeyValuePair<TKey, TValue> item in this)
                array[arrayIndex++] = item;
        }
        
        /// <summary>
        /// Default GetEnumerator Method.
        /// </summary>
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            // Creating KeyValuePair array.
            KeyValuePair<TKey, TValue>[] items = new KeyValuePair<TKey, TValue>[this.Count];
            int index = 0;
            InOrderTraversalIterator(this.RootNode, items, ref index);
            if (this.RootNode != null)
                for (int i = 0; i < items.Length; i++)
                    yield return items[i];
            else
                yield break;
        }

        /// <summary>
        /// In-order Traversal method, which is used for default iteration.
        /// </summary>
        private void InOrderTraversalIterator(Node<TKey, TValue> node,
            KeyValuePair<TKey, TValue>[] items, ref int index)
        {
            if (node != null)
            {
                InOrderTraversalIterator(node.LeftNode, items, ref index);
                items[index++] = new KeyValuePair<TKey, TValue>(node.Key, node.Value);
                InOrderTraversalIterator(node.RightNode, items, ref index);
            }
        }

        /// <summary>
        /// Deletes a specified value from the tree with key.
        /// </summary>
        /// <param name="key">Key</param>
        public bool Remove(TKey key)
        {
            // At First find the node in the tree to delete and assign to item reference
            Node<TKey, TValue> item = Retrieve(key);
            if (item == null)
            {
                Console.WriteLine("Nothing to delete!");
                return false;
            }
            // Temp nodes for deleting.
            Node<TKey, TValue> node1 = null;
            Node<TKey, TValue> node2 = null;
            if (item.LeftNode == null || item.RightNode == null)
                node2 = item;
            else
                node2 = TreeSuccessor(item);
            if (node2.LeftNode != null)
                node1 = node2.LeftNode;
            else
                node1 = node2.RightNode;
            if (node1 != null)
                node1.Parent = node2;
            if (node2.Parent == null)
                this.RootNode = node1;
            else if (node2 == node2.Parent.LeftNode)
                node2.Parent.LeftNode = node1;
            else
                node2.Parent.RightNode = node1;
            if (node2 != item)
                item.Key = node2.Key;
            if (node2.Color == BLACK)
                RemoveBalancing(node1);
            this.Count--;
            this.Keys.Remove(item.Key);
            this.Values.Remove(item.Value);
            return true;
        }

        /// <summary>
        /// Removes the first occurrence of a specific object
        /// </summary>
        /// <remarks>
        /// The Red Black Tree implementation actually ignores the Value portion in the case of the delete, it removes the node with the matching Key.
        /// </remarks>
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return Remove(item.Key);
        }

        /// <summary>
        /// Checks the tree for any violations after deletion and performs a fix
        /// </summary>
        /// <param name="X"></param>
        private void RemoveBalancing(Node<TKey, TValue> node)
        {
            // Start with node and move up the tree until we reach a red node or the root.
            while (node != null && node != RootNode && node.Color == BLACK)
            {
                // Check to see if node is the left child.
                if (node == node.Parent.LeftNode)
                {
                    Node<TKey, TValue> temp = node.Parent.RightNode;
                    // Case 1.
                    if (temp.Color == RED)
                    {
                        temp.Color = BLACK;
                        node.Parent.Color = RED;
                        RotateLeft(node.Parent);
                        temp = node.Parent.RightNode;
                    }
                    // Case 2.
                    if (temp.LeftNode.Color == BLACK && temp.RightNode.Color == BLACK)
                    {
                        temp.Color = RED;
                        node = node.Parent;
                    }
                    // Case 3.
                    else if (temp.RightNode.Color == BLACK)
                    {
                        temp.LeftNode.Color = BLACK;
                        temp.Color = RED;
                        RotateRight(temp);
                        temp = node.Parent.RightNode;
                    }
                    // Case 4.
                    temp.Color = node.Parent.Color;
                    node.Parent.Color = BLACK;
                    temp.RightNode.Color = BLACK;
                    RotateLeft(node.Parent);
                    node = RootNode;
                }
                // Mirror code from above with "right" & "left" exchanged.
                else
                {
                    Node<TKey, TValue> temp = node.Parent.LeftNode;
                    // Case 1.
                    if (temp.Color == RED)
                    {
                        temp.Color = BLACK;
                        node.Parent.Color = RED;
                        RotateRight(node.Parent);
                        temp = node.Parent.LeftNode;
                    }
                    // Case 2.
                    if (temp.RightNode.Color == BLACK && temp.LeftNode.Color == BLACK)
                    {
                        temp.Color = RED;
                        node = node.Parent;
                    }
                    // Case 3.
                    else if (temp.LeftNode.Color == BLACK)
                    {
                        temp.RightNode.Color = BLACK;
                        temp.Color = RED;
                        RotateLeft(temp);
                        temp = node.Parent.LeftNode;
                    }
                    // Case 4.
                    temp.Color = node.Parent.Color;
                    node.Parent.Color = BLACK;
                    temp.LeftNode.Color = BLACK;
                    RotateRight(node.Parent);
                    node = RootNode;
                }
            }
            if (node != null)
                node.Color = BLACK;
        }

        /// <summary>
        /// Search and Find the item in the tree.
        /// </summary>
        /// <param name="key">Key</param>
        public Node<TKey, TValue> Retrieve(TKey key)
        {
            // For searching element.
            bool isFound = false;
            Node<TKey, TValue> temp = RootNode;
            while (!isFound)
            {
                if (temp == null)
                    break;
                // When key < temp.key.
                if (key.CompareTo(temp.Key) < 0)
                    temp = temp.LeftNode;
                // When key > temp.key.
                else if (key.CompareTo(temp.Key) > 0)
                    temp = temp.RightNode;
                // When key == temp.key.
                else
                {
                    isFound = true;
                    return temp;
                }
            }
            return null;
        }

        /// <summary>
        /// Gets the value associated with the specified key.
        /// </summary>
        public bool TryGetValue(TKey key, out TValue value)
        {
            // Search and find the element with key.
            if (Retrieve(key) == null)
                throw new Exception("Item doesn't exist.");
            else
                value = Retrieve(key).Value;
            return value != null;
        }

        /// <summary>
        /// IEnumerable.GetEnumerator()
        /// </summary>
        /// <returns>IEnumerator</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Overriding ToString Mmethod.
        /// </summary>
        public override string ToString()
        {
            string str = "";
            foreach (KeyValuePair<TKey, TValue> item in this)
            {
                str += "[" + item.Key.ToString() + " " + item.Value.ToString() + "]\t";
            }
            return str;
        }
    }
}
