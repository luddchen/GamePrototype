using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Tools
{

    public class DynamicOctree<T>
    {

        public List<Tuple<T, BoundingSphere>> items;

        public List<DynamicOctree<T>> subTrees;

        public BoundingBox bounding;


        int maxDepth;

        float minSize;

        int minItems;

        Vector3 center;


        public DynamicOctree(int maxDepth, float minSize, int minItems, BoundingBox bounding)
        {
            this.items = new List<Tuple<T, BoundingSphere>>();
            this.subTrees = new List<DynamicOctree<T>>();
            this.bounding = bounding;
            this.maxDepth = maxDepth;
            this.minSize = minSize;
            this.minItems = minItems;
            this.center = (bounding.Min + bounding.Max) / 2;
        }


        public bool Add(T item, BoundingSphere boundingSphere)
        {
            bool successful = false;

            if (boundingSphere.Contains(this.bounding) != ContainmentType.Disjoint)
            {
                AddItem(item, boundingSphere);
                successful = true;
            }

            return successful;
        }

        public void AddItem(T item, BoundingSphere boundingSphere)
        {
            this.items.Add(new Tuple<T, BoundingSphere>(item, boundingSphere));
        }


        public void BuildTree()
        {
            List<Tuple<T, BoundingSphere>> newItems = new List<Tuple<T,BoundingSphere>>();

            Vector3 size = bounding.Max - bounding.Min;
            Vector3 size2 = size / 2;
            Vector3 min = new Vector3();

            if (size2.X < this.minSize || this.maxDepth == 0 || this.minItems > this.items.Count)
            {
                return;
            }

            for (min.X = bounding.Min.X; min.X < bounding.Max.X; min.X += size2.X)
            {
                for (min.Y = bounding.Min.Y; min.Y < bounding.Max.Y; min.Y += size2.Y)
                {
                    for (min.Z = bounding.Min.Z; min.Z < bounding.Max.Z; min.Z += size2.Z)
                    {
                        this.subTrees.Add(new DynamicOctree<T>(this.maxDepth - 1, this.minSize, this.minItems , new BoundingBox(min, min + size2)));
                    }
                }
            }

            foreach (Tuple<T, BoundingSphere> tuple in this.items)
            {
                bool success = false;

                if (tuple.Item2.Contains(this.center) == ContainmentType.Disjoint)
                {
                    foreach (DynamicOctree<T> tree in this.subTrees)
                    {
                        if (tree.Add(tuple.Item1, tuple.Item2))
                        {
                            success = true;
                            break;
                        }
                    }
                }

                if (!success)
                {
                    newItems.Add(tuple);
                }
            }

            this.items.Clear();
            this.items = newItems;

            // todo : remove empty subtrees

            foreach (DynamicOctree<T> tree in this.subTrees)
            {
                tree.BuildTree();
            }
        }


        public bool isLeaf()
        {
            return (this.subTrees.Count == 0);
        }


        public void Clear()
        {
            foreach (DynamicOctree<T> tree in this.subTrees)
            {
                tree.Clear();
            }

            this.subTrees.Clear();

            this.items.Clear();
        }


        /// <summary>
        /// the number of items in this tree
        /// </summary>
        public int Count
        {
            get
            {
                int count = this.items.Count;
                if (!this.isLeaf())
                {
                    foreach (DynamicOctree<T> tree in this.subTrees)
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
            String output = this.items.Count + "(";
            if (!this.isLeaf())
            {
                output += "\n";
                foreach (DynamicOctree<T> tree in this.subTrees)
                {
                    output += tree.getCountString();
                }
                output += "\n";
            }
            output += ")";
            return output;
        }


        public Tuple<T, float?> CastRay(Ray ray, bool bothDirections)
        {
            Tuple<T, float?> tuple = new Tuple<T, float?>(default(T), null);

            foreach (Tuple<T, BoundingSphere> item in this.items)
            {
                float? distance = ray.Intersects(item.Item2);

                if (distance != null)
                {
                    if (tuple.Item2 != null)
                    {
                        if (distance < tuple.Item2)
                        {
                            if (!((!bothDirections) && (distance < 0)))
                            {
                                tuple = new Tuple<T, float?>(item.Item1, distance);
                            }
                        }
                    }
                    else
                    {
                        tuple = new Tuple<T, float?>(item.Item1, distance);
                    }
                }
            }


            foreach (DynamicOctree<T> tree in this.subTrees)
            {
                float? distance = ray.Intersects(tree.bounding);

                if (distance != null)
                {
                    Tuple<T, float?> treeCastTuple = tree.CastRay(ray, bothDirections);

                    if (treeCastTuple.Item2 != null)
                    {
                        if (tuple.Item2 != null)
                        {
                            if (treeCastTuple.Item2 < tuple.Item2)
                            {
                                tuple = treeCastTuple;
                            }
                        }
                        else
                        {
                            tuple = treeCastTuple;
                        }
                    }
                }

            }

            return tuple;
        }

    }

}
