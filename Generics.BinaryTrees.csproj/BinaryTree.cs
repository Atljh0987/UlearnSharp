using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Generics.BinaryTrees
{
    public class BinaryTree<T> : IEnumerable<T> where T : IComparable<T>
    {

        public T Value { get; set; }
        public BinaryTree<T> Right { get; set; }
        public BinaryTree<T> Left { get; set; }
        public bool Used = false;


        public BinaryTree<T> LeafCheckAndCreate(BinaryTree<T> checkingLeaf)
        {
            if(checkingLeaf != null)
            {
                return checkingLeaf;
            }
            else
            {
                return new BinaryTree<T>();
            }
        }

        public void Add(T value)
        {
            if(!Used)
            {
                Value = value;
                Used = true;
                return;
            }
            else
            {                
                if(value.CompareTo(Value) <= 0)
                {
                    Left = LeafCheckAndCreate(Left);
                    Add(value, Left);        
                }
                else
                {
                    Right = LeafCheckAndCreate(Right);
                    Add(value, Right);
                }
            }
        }

        public void Add(T value, BinaryTree<T> current)
        {
            if (!current.Used)
            {
                current.Value = value;
                current.Used = true;
                return;
            }
            else
            {
                if (value.CompareTo(current.Value) <= 0)
                {
                    current.Left = LeafCheckAndCreate(current.Left);
                    Add(value, current.Left);
                }
                else
                {
                    current.Right = LeafCheckAndCreate(current.Right);
                    Add(value, current.Right);
                }
            }
        }

        public IEnumerator<T> GetEnumerator() => EnumeratorGetElements(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        IEnumerator<T> EnumeratorGetElements(BinaryTree<T> leaf)
        {
            if (!Used || leaf == null)
            {
                yield break;
            }
            else
            {
                var enumeratorLeafLeft = EnumeratorGetElements(leaf.Left);
                while(enumeratorLeafLeft.MoveNext())
                    yield return enumeratorLeafLeft.Current;

                yield return leaf.Value;

                var enumeratorLeafRight = EnumeratorGetElements(leaf.Right);
                while (enumeratorLeafRight.MoveNext())
                    yield return enumeratorLeafRight.Current;

            }
        }
    }

    public class BinaryTree
    {
        
        public static BinaryTree<int> Create(params int[] values)
        {
            BinaryTree<int> binaryTree = new BinaryTree<int>();

            foreach(var value in values)
                binaryTree.Add(value);

            return binaryTree;
        }       
    }
}
