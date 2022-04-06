using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class Leaf<T>
    {
        private T Value { get; }
        private bool Root;

        public Leaf(T value, bool root)
        {
            Value = value;
            Root = root;
        }

        private Leaf<T> Right { get; set; }
        private Leaf<T> Left { get; set; }

        
    }

    public class BinaryTree<T> : IEnumerable<T>
    {
        private List<Leaf<T>> binaryTree;



        public void AddLeaf(T value)
        {
            if(binaryTree.Count == 0)
            {
                binaryTree.Add(new Leaf<T>(value, true));
            }
            else
            {
                binaryTree.Add(new Leaf<T>(value, false));
            }
        }

        public IEnumerator<T> GetEnumerator()
        {
            return new BinaryTree<T>();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
