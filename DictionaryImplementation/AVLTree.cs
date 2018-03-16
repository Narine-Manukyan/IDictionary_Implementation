using System;
using System.Collections;
using System.Collections.Generic;

namespace DictionaryImplementation
{
    /// <summary>
    /// This Class Implements the Node Construction of Tree.
    /// </summary>
    public sealed class AVLNode<TKey, TValue> where TKey : IComparable<TKey>
    {
        internal TKey Key;
        internal TValue Value;
        // The height of the tree.
        private int height;
        // The property for height.
        public int Height
        {
            set
            {
                this.height = value;
            }
            get
            {
                if (this == null)
                    return -1;
                else
                    return this.height;
            }
        }
        // Left Node of the Current node. 
        internal AVLNode<TKey, TValue> LeftNode { set; get; }
        // Right Node of the Current node. 
        internal AVLNode<TKey, TValue> RightNode { set; get; }
        // Parent Node of the Current node.
        internal AVLNode<TKey, TValue> Parent { set; get; }

        // The Parameterfull Constructor.
        public AVLNode(TKey key, TValue val)
        {
            this.Key = key;
            this.Value = val;
            this.Height = 0;
            this.LeftNode = null;
            this.RightNode = null;
        }

        /// <summary>
        /// Overriding method ToString().
        /// </summary>
        public override string ToString()
        {
            return "Key: " + this.Key.ToString() + " Value: " + this.Value.ToString();
        }
    }

    /// <summary>
    /// This Class Implements The AVL Tree Tree Construction with Dictionary
    /// </summary>
    /// <typeparam name="TKey">Key of Key-Value Pair</typeparam>
    /// <typeparam name="TValue">Value of Key-Value Pair</typeparam>
    public class AVLTree<TKey, TValue> : IDictionary<TKey, TValue>
        where TKey : IComparable<TKey>
    {
        /// <summary>
        /// Returns the Root of the tree.
        /// </summary>
        public AVLNode<TKey, TValue> RootNode { private set; get; }

        /// <summary>
        /// Returns the number of items currently in the tree
        /// </summary>
        public int Count { private set; get; }

        /// <summary>
        /// The Parameterless constructor.
        /// </summary>
        public AVLTree()
        {
            this.RootNode = null;
            this.Count = 0;
        }

        /// <summary>
        /// The parameterfull constructor.
        /// </summary>
        /// <param name="count"></param>
        public AVLTree(int count)
        {
            this.Count = count;
            keys = new List<TKey>(this.Count);
            values = new List<TValue>(this.Count);
        }

        /// <summary>
        /// Rotating Right.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Node</returns>
        private AVLNode<TKey, TValue> RotateRight(AVLNode<TKey, TValue> node)
        {
            // Set temp.
            AVLNode<TKey, TValue> temp = node.RightNode;
            // Turn temp’s left subtree into node’s right subtree.
            node.RightNode = temp.LeftNode;
            if (temp.LeftNode != null)
                temp.LeftNode.Parent = node;
            if (temp != null)
                // Link node's parent to temp
                temp.Parent = node.Parent;
            if (node.Parent == null)
                RootNode = temp;
            else if (node == node.Parent.LeftNode)
                node.Parent.LeftNode = temp;
            else
                node.Parent.RightNode = temp;
            // Put node on temp's left
            temp.LeftNode = node;
            if (node != null)
                node.Parent = temp;
            //node.Height = Math.Max(node.LeftNode.Height, node.RightNode.Height) + 1;
            //temp.Height = Math.Max(temp.LeftNode.Height, node.Height) + 1;
            return temp;
        }

        /// <summary>
        /// Rotating Left.
        /// Left rotate is simply mirror code from Right rotate.
        /// </summary>
        /// <param name="node"></param>
        /// <returns>Node</returns>
        private AVLNode<TKey, TValue> RotateLeft(AVLNode<TKey, TValue> node)
        {
            // Set temp.
            AVLNode<TKey, TValue> temp = node.LeftNode;
            // Turn temp’s right subtree into node’s left subtree.
            node.LeftNode = temp.RightNode;
            if (temp.RightNode != null)
                temp.RightNode.Parent = node;
            if (temp != null)
                // Link node's parent to temp
                temp.Parent = node.Parent;
            if (node.Parent == null)
                RootNode = temp;
            else if (node == node.Parent.RightNode)
                node.Parent.RightNode = temp;
            if (node.Parent != null && node == node.Parent.LeftNode)
                node.Parent.LeftNode = temp;
            // Put node on temp's right.
            temp.RightNode = node;
            if (node != null)
                node.Parent = temp;
            return temp;
        }

        /// <summary>
        /// Displaying Tree.
        /// </summary>
        public void DisplayTree()
        {
            if (RootNode == null)
            {
                Console.WriteLine("Nothing in the tree!");
                return;
            }
            if (RootNode != null)
            {
                InOrderDisplay(RootNode);
            }
            Console.WriteLine();
        }

        /// <summary>
        /// In Order Displaying Tree.
        /// </summary>
        private void InOrderDisplay(AVLNode<TKey, TValue> current)
        {
            if (current != null)
            {
                InOrderDisplay(current.LeftNode);
                Console.Write("({0} {1}) ", current.Key, current.Value);
                InOrderDisplay(current.RightNode);
            }
        }

        /// <summary>
        /// Double Rotating Left Right.
        /// </summary>
        private AVLNode<TKey, TValue> DoubleLeft(AVLNode<TKey, TValue> node)
        {
            if(node.LeftNode != null)
                node.LeftNode = RotateRight(node.LeftNode);
            return RotateLeft(node);
        }

        /// <summary>
        /// Double Rotating Right Left.
        /// </summary>
        private AVLNode<TKey, TValue> DoubleRight(AVLNode<TKey, TValue> node)
        {
            if(node.RightNode != null)
                node.RightNode = RotateLeft(node.RightNode);
            return RotateRight(node);
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
                AVLNode<TKey, TValue> temp = this.Retrieve(key);
                if (temp == null)
                    throw new Exception("Key not found.");
                return temp.Value;
            }
            set
            {
                if (key == null)
                    throw new Exception("Key is null");
                AVLNode<TKey, TValue> temp = this.Retrieve(key);
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
                AVLNode<TKey, TValue> newItem = new AVLNode<TKey, TValue>(item.Key, item.Value);
                if (this.RootNode == null)
                    // Set The Root Node. 
                    this.RootNode = newItem;
                else
                    // Claling Recursive Addition.
                    this.RootNode = RecursiveAdd(this.RootNode, newItem);
            }
            else
                Console.WriteLine("This Key-Value Pair is already Exists.");
        }

        /// <summary>
        /// This function is for Recursive Addition.
        /// </summary>
        /// <param name="current">Current node</param>
        /// <param name="node">node</param>
        /// <returns>Current node</returns>
        private AVLNode<TKey, TValue> RecursiveAdd
            (AVLNode<TKey, TValue> current, AVLNode<TKey, TValue> node)
        {
            if (current == null)
            {
                current = node;
                return current;
            }
            // When node's Key Is less than current's.
            else if (node.Key.CompareTo(current.Key) < 0)
            {
                // Recursiv call.
                current.LeftNode = RecursiveAdd(current.LeftNode, node);
                // Recursiv call.
                current = AddBalancing(current);
            }
            // When node's Key Is big than current's.
            else if (node.Key.CompareTo(current.Key) > 0)
            {
                // Recursiv call.
                current.RightNode = RecursiveAdd(current.RightNode, node);
                // Recursiv call.
                current = AddBalancing(current);
            }
            return current;
        }

        /// <summary>
        /// Function for Balancing Tree. Checks Red-Black Tree properties.
        /// </summary>
        /// <param name="current"></param>
        /// <returns></returns>
        private AVLNode<TKey, TValue> AddBalancing(AVLNode<TKey, TValue> node)
        {
            // The balancing factor.
            int factor = BalanceFactor(node);
            if (factor > 1)
            {
                if (BalanceFactor(node.LeftNode) > 0)
                    // Rotating Left. 
                    node = RotateLeft(node);
                else
                    // Double Rotating .
                    node = DoubleLeft(node);
            }
            else if (factor < -1)
            {
                if (BalanceFactor(node.RightNode) > 0)
                    // Double Rotating .
                    node = DoubleRight(node);
                else
                    // Rotating Right.
                    node = RotateRight(node);
            }
            return node;
        }

        /// <summary>
        /// For Getting Balance factor.
        /// </summary>
        /// <param name="current">Current Node</param>
        /// <returns>Balance</returns>
        private int BalanceFactor(AVLNode<TKey, TValue> current)
        {
            int height1 = 0;
            int height2 = 0;
            if (current != null && current.LeftNode != null && current.RightNode != null
                && current.LeftNode.LeftNode != null && current.RightNode.LeftNode != null
                && current.LeftNode.RightNode != null && current.RightNode.RightNode != null)
            {
                height1 = Math.Max(current.LeftNode.LeftNode.Height, current.LeftNode.RightNode.Height) + 1;
                height2 = Math.Max(current.RightNode.LeftNode.Height, current.RightNode.RightNode.Height) + 1;
            }
            return height1 - height2; 
        }

        /// <summary>
        /// Removes all items from the Tree.
        /// </summary>
        public void Clear()
        {
            this.RootNode = null;
            this.Count = 0;
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
        private void InOrderTraversalIterator(AVLNode<TKey, TValue> node,
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
            if (Retrieve(key) != null)
            {
                this.Values.Remove(this[key]);
                this.Keys.Remove(key);
                this.Count--;
                this.RootNode = RecursiveRemove(this.RootNode, key);
                return true;
            }
            else
            {
                Console.WriteLine("Nothing to remove!");
                return false;
            }
        }

        /// <summary>
        /// This Function is for Recursive Deletion.
        /// </summary>
        /// <param name="current">Current node</param>
        /// <param name="key">Key</param>
        /// <returns>Avl Node</returns>
        private AVLNode<TKey, TValue> RecursiveRemove(AVLNode<TKey, TValue> current, TKey key)
        {
            AVLNode<TKey, TValue> parent;
            if (current == null)
                return null; 
            else
            {
                // Left subtree.
                if (key.CompareTo(current.Key) < 0)
                {
                    current.LeftNode = RecursiveRemove(current.LeftNode, key);
                    if (BalanceFactor(current) == -2)//here
                    {
                        if (BalanceFactor(current.RightNode) <= 0)
                            current = RotateRight(current);
                        else
                            current = DoubleRight(current);
                    }
                }
                // Right subtree
                else if (key.CompareTo(current.Key) > 0)
                {
                    current.RightNode = RecursiveRemove(current.RightNode, key);
                    if (BalanceFactor(current) == 2)
                    {
                        if (BalanceFactor(current.LeftNode) >= 0)
                            current = RotateLeft(current);
                        else
                            current = DoubleLeft(current);
                    }
                }
                // When target is found.
                else
                {
                    if (current.RightNode != null)
                    {
                        // Deleting its inorder successor.
                        parent = current.RightNode;
                        while (parent.LeftNode != null)
                        {
                            parent = parent.LeftNode;
                        }
                        current.Key = parent.Key;
                        current.RightNode = RecursiveRemove(current.RightNode, parent.Key);
                        // Rebalancing.
                        if (BalanceFactor(current) == 2)
                        {
                            if (BalanceFactor(current.LeftNode) >= 0)
                                current = RotateLeft(current);
                            else
                                current = DoubleLeft(current); 
                        }
                    }
                    // When current.left != null.
                    else
                        return current.LeftNode;
                }
            }
            return current;
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
        /// Search and Find the item in the tree.
        /// </summary>
        /// <param name="key">Key</param>
        public AVLNode<TKey, TValue> Retrieve(TKey key)
        {
            // For searching element.
            bool isFound = false;
            AVLNode<TKey, TValue> temp = RootNode;
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
