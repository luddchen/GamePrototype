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


        public T CastRay(Ray ray, float minDist, ref float targetDistance)
        {
            T targetObject = default(T);

            float distance;

            foreach (Tuple<T, BoundingSphere> item in this.items)
            {
                distance = ray.Intersects(item.Item2) ?? targetDistance;

                if (distance > minDist && distance < targetDistance)
                {
                    targetDistance = distance;
                    targetObject = item.Item1;
                }
            }


            foreach (DynamicOctree<T> tree in this.subTrees)
            {
                float? treeDist = ray.Intersects(tree.bounding);

                if (treeDist != null && treeDist < targetDistance)
                {
                    distance = targetDistance;
                    T subTreeTargetObject = tree.CastRay(ray, minDist, ref targetDistance);

                    if (targetDistance < distance)
                    {
                        targetObject = subTreeTargetObject;
                    }
                }
            }

            return targetObject;
        }


        public List<Tuple<T, T>> CheckCollisions(Tuple<T, BoundingSphere> toCheck)
        {
            List<Tuple<T, T>> list = new List<Tuple<T, T>>();

            foreach (Tuple<T, BoundingSphere> item in this.items)
            {
                if (toCheck.Item2.Intersects(item.Item2))
                {
                    list.Add(new Tuple<T, T>(toCheck.Item1, item.Item1));
                }
            }

            foreach (DynamicOctree<T> tree in this.subTrees)
            {
                if (toCheck.Item2.Intersects(tree.bounding))
                {
                    list.InsertRange(0, tree.CheckCollisions(toCheck));
                }
            }

            return list;
        }


        /// <summary>
        /// seems to be bugged
        /// </summary>
        /// <returns></returns>
        public List<Tuple<T, T>> CheckCollisions()
        {
            List<Tuple<T, T>> list = new List<Tuple<T, T>>();

            // check within this node
            for (int nr1 = 0; nr1 < this.items.Count - 1; nr1++)
            {
                for (int nr2 = nr1 + 1; nr2 < this.items.Count; nr2++)
                {
                    if (this.items[nr1].Item2.Intersects(this.items[nr2].Item2)) 
                    {
                        list.Add( new Tuple<T,T>(this.items[nr1].Item1,this.items[nr2].Item1)); 
                    }
                }
            }

            foreach (Tuple<T, BoundingSphere> item in this.items)
            {
                foreach (DynamicOctree<T> tree in this.subTrees)
                {
                    if (item.Item2.Intersects(tree.bounding)) 
                    {
                        list.InsertRange(0, tree.CheckCollisions(item));
                    }
                }
            }

            return list;
        }

    }

}
