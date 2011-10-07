// HtmlAgilityPack V1.0 - Simon Mourier <simon underscore mourier at hotmail dot com>
using System;
using System.Collections;

namespace HtmlAgilityPack
{
    /// <summary>
    /// Represents a combined list and collection of HTML nodes.
    /// </summary>
    public class HtmlNodeCollection : IEnumerable
    {
        private ArrayList _items = new ArrayList();
        private HtmlNode _parentnode;

        internal HtmlNodeCollection(HtmlNode parentnode)
        {
            _parentnode = parentnode; // may be null
        }

        /// <summary>
        /// Gets the number of elements actually contained in the list.
        /// </summary>
        public int Count
        {
            get
            {
                return _items.Count;
            }
        }

        internal void Clear()
        {
            foreach (HtmlNode node in _items)
            {
                node._parentnode = null;
                node._nextnode = null;
                node._prevnode = null;
            }
            _items.Clear();
        }

        internal void Remove(int index)
        {
            HtmlNode next = null;
            HtmlNode prev = null;
            HtmlNode oldnode = (HtmlNode)_items[index];

            if (index > 0)
            {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < (_items.Count - 1))
            {
                next = (HtmlNode)_items[index + 1];
            }

            _items.RemoveAt(index);

            if (prev != null)
            {
                if (next == prev)
                {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = next;
            }

            if (next != null)
            {
                next._prevnode = prev;
            }

            oldnode._prevnode = null;
            oldnode._nextnode = null;
            oldnode._parentnode = null;
        }

        internal void Replace(int index, HtmlNode node)
        {
            HtmlNode next = null;
            HtmlNode prev = null;
            HtmlNode oldnode = (HtmlNode)_items[index];

            if (index > 0)
            {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < (_items.Count - 1))
            {
                next = (HtmlNode)_items[index + 1];
            }

            _items[index] = node;

            if (prev != null)
            {
                if (node == prev)
                {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = node;
            }

            if (next != null)
            {
                next._prevnode = node;
            }

            node._prevnode = prev;
            if (next == node)
            {
                throw new InvalidProgramException("Unexpected error.");
            }
            node._nextnode = next;
            node._parentnode = _parentnode;

            oldnode._prevnode = null;
            oldnode._nextnode = null;
            oldnode._parentnode = null;
        }

        internal void Insert(int index, HtmlNode node)
        {
            HtmlNode next = null;
            HtmlNode prev = null;

            if (index > 0)
            {
                prev = (HtmlNode)_items[index - 1];
            }

            if (index < _items.Count)
            {
                next = (HtmlNode)_items[index];
            }

            _items.Insert(index, node);

            if (prev != null)
            {
                if (node == prev)
                {
                    throw new InvalidProgramException("Unexpected error.");
                }
                prev._nextnode = node;
            }

            if (next != null)
            {
                next._prevnode = node;
            }

            node._prevnode = prev;

            if (next == node)
            {
                throw new InvalidProgramException("Unexpected error.");
            }

            node._nextnode = next;
            node._parentnode = _parentnode;
        }

        internal void Append(HtmlNode node)
        {
            HtmlNode last = null;
            if (_items.Count > 0)
            {
                last = (HtmlNode)_items[_items.Count - 1];
            }

            _items.Add(node);
            node._prevnode = last;
            node._nextnode = null;
            node._parentnode = _parentnode;
            if (last != null)
            {
                if (last == node)
                {
                    throw new InvalidProgramException("Unexpected error.");
                }
                last._nextnode = node;
            }
        }

        internal void Prepend(HtmlNode node)
        {
            HtmlNode first = null;
            if (_items.Count > 0)
            {
                first = (HtmlNode)_items[0];
            }

            _items.Insert(0, node);

            if (node == first)
            {
                throw new InvalidProgramException("Unexpected error.");
            }
            node._nextnode = first;
            node._prevnode = null;
            node._parentnode = _parentnode;
            if (first != null)
            {
                first._prevnode = node;
            }
        }

        internal void Add(HtmlNode node)
        {
            _items.Add(node);
        }

        /// <summary>
        /// Gets the node at the specified index.
        /// </summary>
        public HtmlNode this[int index]
        {
            get
            {
                return _items[index] as HtmlNode;
            }
        }

        internal int GetNodeIndex(HtmlNode node)
        {
            // TODO: should we rewrite this? what would be the key of a node?
            for (int i = 0; i < _items.Count; i++)
            {
                if (node == ((HtmlNode)_items[i]))
                {
                    return i;
                }
            }
            return -1;
        }

        /// <summary>
        /// Gets a given node from the list.
        /// </summary>
        public int this[HtmlNode node]
        {
            get
            {
                int index = GetNodeIndex(node);
                if (index == -1)
                {
                    throw new ArgumentOutOfRangeException("node", "Node \"" + node.CloneNode(false).OuterHtml + "\" was not found in the collection");
                }
                return index;
            }
        }

        /// <summary>
        /// Returns an enumerator that can iterate through the list.
        /// </summary>
        /// <returns>An IEnumerator for the entire list.</returns>
        public HtmlNodeEnumerator GetEnumerator()
        {
            return new HtmlNodeEnumerator(_items);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <summary>
        /// Represents an enumerator that can iterate through the list.
        /// </summary>
        public class HtmlNodeEnumerator : IEnumerator
        {
            int _index;
            ArrayList _items;

            internal HtmlNodeEnumerator(ArrayList items)
            {
                _items = items;
                _index = -1;
            }

            /// <summary>
            /// Sets the enumerator to its initial position, which is before the first element in the collection.
            /// </summary>
            public void Reset()
            {
                _index = -1;
            }

            /// <summary>
            /// Advances the enumerator to the next element of the collection.
            /// </summary>
            /// <returns>true if the enumerator was successfully advanced to the next element, false if the enumerator has passed the end of the collection.</returns>
            public bool MoveNext()
            {
                _index++;
                return (_index < _items.Count);
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            public HtmlNode Current
            {
                get
                {
                    return (HtmlNode)(_items[_index]);
                }
            }

            /// <summary>
            /// Gets the current element in the collection.
            /// </summary>
            object IEnumerator.Current
            {
                get
                {
                    return (Current);
                }
            }
        }
    }

}
