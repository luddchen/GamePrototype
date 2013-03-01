using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Tools
{

    public class Octree<T>
    {

        public List<T> items;

        public List<Octree<T>> subTrees;

        public BoundingBox bounding;

        public bool isLeaf;


        int maxDepth;

        float minSize;

        Vector3 center;



        /// <summary>
        /// creates a new OcTree
        /// </summary>
        /// <param name="maxDepth">maximal depth of this Tree</param>
        /// <param name="minSize">minimal size of the bounding box of its sub-tree-nodes</param>
        /// <param name="bounding">the boundingbox of this tree node</param>
        public Octree(int maxDepth, float minSize, BoundingBox bounding) 
        {
            this.items = new List<T>();
            this.bounding = bounding;
            this.isLeaf = true;
            this.maxDepth = maxDepth;
            this.minSize = minSize;
            this.center = (bounding.Min + bounding.Max) / 2;

            Vector3 size = bounding.Max - bounding.Min;
            Vector3 size2 = size / 2;

            if (this.maxDepth > 0 && (size.X > this.minSize) && (size.Y > this.minSize) && (size.Z > this.minSize))
            {
                this.subTrees = new List<Octree<T>>();
                this.isLeaf = false;
                Vector3 min = new Vector3();

                for (min.X = bounding.Min.X; min.X < bounding.Max.X; min.X += size2.X)
                {
                    for (min.Y = bounding.Min.Y; min.Y < bounding.Max.Y; min.Y += size2.Y)
                    {
                        for (min.Z = bounding.Min.Z; min.Z < bounding.Max.Z; min.Z += size2.Z)
                        {
                            this.subTrees.Add(new Octree<T>(maxDepth - 1, minSize, new BoundingBox(min, min + size2)));
                        }
                    }
                }
            }

        }



        /// <summary>
        /// add the item to this node or an subnode, depending on its bounding
        /// </summary>
        /// <param name="item">the item</param>
        /// <param name="boundingSphere">the items bounding</param>
        /// <returns>true if successful (item bounding within node bounding)</returns>
        public bool Add(T item, BoundingSphere boundingSphere)
        {
            bool successful = false;

            if (boundingSphere.Contains(this.bounding) != ContainmentType.Disjoint)    // object in boundingbox
            {
                if (this.isLeaf)  // tree-node is leaf -> insert
                {
                    this.items.Add(item);
                    successful = true;
                }
                else    // tree-node is no leaf -> more testing
                {
                    if (boundingSphere.Contains(this.center) != ContainmentType.Disjoint)  // object intersects more than one child -> insert here
                    {
                        this.items.Add(item);
                        successful = true;
                    }
                    else    // test subtrees
                    {
                        foreach (Octree<T> tree in this.subTrees)
                        {
                            if (tree.Add(item, boundingSphere))
                            {
                                successful = true;
                                break;    // first fit
                            }
                        }
                    }
                }
            }

            return successful;
        }



        /// <summary>
        /// add the item to this node, independent of boundings
        /// </summary>
        /// <param name="item">the item</param>
        public void Add(T item)
        {
            this.items.Add(item);
        }



        /// <summary>
        /// remove all items from this node and its subnodes
        /// </summary>
        public void clear()
        {
            this.items.Clear();

            if (!this.isLeaf)
            {
                foreach (Octree<T> tree in this.subTrees)
                {
                    tree.clear();
                }
            }
        }



        /// <summary>
        /// the number of items in this tree
        /// </summary>
        public int Count
        {
            get
            {
                int count = this.items.Count;
                if (!this.isLeaf)
                {
                    foreach (Octree<T> tree in this.subTrees)
                    {
                        count += tree.Count;
                    }
                }
                return count;
            }
        }



        /// <summary>
        /// for debuging
        /// </summary>
        /// <returns>a string</returns>
        public String getCountString()
        {
            String output = "" + this.items.Count + "(";
            if (!this.isLeaf)
            {
                foreach (Octree<T> tree in this.subTrees)
                {
                    output += tree.getCountString();
                }
            }
            output += ")";
            return output;
        }


        /// <summary>
        /// to String
        /// </summary>
        /// <returns>a string with information about tree-depth and number of containing items</returns>
        public override string ToString()
        {
            String output = "octree level = " + this.maxDepth + " " + this.bounding + " objects = " + this.items.Count + "\n";
            if (!this.isLeaf)
            {
                foreach (Octree<T> tree in this.subTrees)
                {
                    output += tree.ToString() + "\n";
                }
            }
            return output;
        }

    }

}
