using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TemplateDefinition
{
    class Node<T>
    {
        private T nodeValue;
        private Node<T> leftChild;
        private Node<T> rightChild;
        public T NodeValue
        {
            get { return nodeValue; }
            set { nodeValue = value; }
        }
        public Node (){   //构造函数不需要<>
            nodeValue = default(T);
            leftChild = null;//引用值  原理类似指针
            rightChild = null;
        }
        public Node(T v)
        {   //构造函数不需要<>
            nodeValue = v;
            leftChild = null;
            rightChild = null;
        }
        public Node(T v,Node<T> nl,Node<T> nr)//可传入 new Node <T> (nodeValue) 参数
        {   //构造函数不需要<>
            nodeValue = v;
            leftChild = nl;
            rightChild = nr;
        }
        public Node(T v, T lv, T rv)
        {
            nodeValue = v ;
            leftChild = new Node<T>(lv);
            rightChild = new Node<T>(rv);
        }
        public Node<T> LeftChild
        {
            get { return leftChild; }
            set { leftChild = value; }
        }
        public Node<T> RightChild
        {
            get { return rightChild; }
            set { rightChild = value; }//可传入 new Node <T> (nodeValue) 参数
        }
        //public T NodeValue
        //{
        //    get { return nodeValue; }
        //    set { nodeValue = value; }
        //}
        public bool isLeaf()
        {
            return leftChild == null && rightChild == null;
        }
    }
    class BinaryTree<T>//有了上面一个类  二叉树已经可以在理论上被建立  不需要TREE类了 哈哈
    {
        private Node<T> parent;
        private int depth;
       
        public Node<T> Parent
        {
            get { return parent; }
            set { parent = value; }
        }
        public BinaryTree()
        {
            parent = null;
        }
        public BinaryTree  (Node<T> p)
        {
            parent=p ;
        }
        public BinaryTree(Node<T> p, Node<T> leftChildTreeParentNode, Node<T> rightChildTreeParentNode)
        {
            parent = new Node<T>(p.NodeValue, leftChildTreeParentNode, rightChildTreeParentNode);
        }
        public BinaryTree(T parentNodeValue, Node<T> leftChildTreeParentNode, Node<T> rightChildTreeParentNode)
        {
            parent = new Node<T>(parentNodeValue, leftChildTreeParentNode, rightChildTreeParentNode);
        }
        public void clear()
        {

        }
        //public bool addL
    }
}
