using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model
{

    class CollisionOctree
    {

        public bool isLeaf;

        int maxDepth;

        float minSize;

        public BoundingBox box;
        public Vector3 center;

        public List<CollisionObject> objects;

        public List<CollisionOctree> subTrees;

        public CollisionOctree(int maxDepth, float minSize, BoundingBox box)
        {
            this.objects = new List<CollisionObject>();
            this.isLeaf = true;
            this.maxDepth = maxDepth;
            this.minSize = minSize;

            this.box = box;
            this.center = Vector3.Divide(Vector3.Add(box.Min, box.Max), 2f);

            Vector3 size = Vector3.Subtract(box.Max, box.Min);
            Vector3 size2 = Vector3.Divide(size, 2f);

            if (this.maxDepth > 0 && (size.X > this.minSize) && (size.Y > this.minSize) && (size.Z > this.minSize))
            {
                this.subTrees = new List<CollisionOctree>();
                this.isLeaf = false;
                Vector3 min = new Vector3();

                for (min.X = box.Min.X; min.X < box.Max.X; min.X += size2.X)
                {
                    for (min.Y = box.Min.Y; min.Y < box.Max.Y; min.Y += size2.Y)
                    {
                        for (min.Z = box.Min.Z; min.Z < box.Max.Z; min.Z += size2.Z)
                        {
                            this.subTrees.Add(new CollisionOctree(maxDepth - 1, minSize, new BoundingBox(min, Vector3.Add(min, size2))));
                        }
                    }
                }
            }
        }


        public void insertFromWorld(WorldModel world)
        {
            int unsuccessful = 0;

            foreach (SpatialObject obj in world.allObjects)
            {
                if (!insert(new CollisionObject(obj)))
                {
                    unsuccessful++;
                }
            }

            if (unsuccessful > 0)
            {
                //Console.Out.WriteLine("unsuccessful inserts : " + unsuccessful);
            }
        }


        public bool insert(CollisionObject obj)
        {
            bool successful = false;

            if (obj.movingSphere.Contains(this.box) != ContainmentType.Disjoint)    // object in boundingbox
            {
                if (this.isLeaf)  // tree-node is leaf -> insert
                {
                    this.objects.Add(obj);
                    successful = true;
                }
                else    // tree-node is no leaf -> more testing
                {
                    if (obj.movingSphere.Contains(this.center) != ContainmentType.Disjoint)  // object intersects more than one child -> insert here
                    {
                        this.objects.Add(obj);
                        successful = true;
                    }
                    else    // test subtrees
                    {
                        foreach (CollisionOctree tree in this.subTrees)
                        {
                            if (tree.insert(obj))
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


        public void clear()
        {
            this.objects.Clear();

            if (!this.isLeaf)
            {
                foreach (CollisionOctree t in this.subTrees)
                {
                    t.clear();
                }
            }
        }


        public override string ToString()
        {
            String output = "octree level = " + this.maxDepth + " " + this.box + " objects = " + this.objects.Count + "\n";
            if (!this.isLeaf)
            {
                foreach (CollisionOctree tree in this.subTrees)
                {
                    output += tree.ToString() + "\n";
                }
            }
            return output;
        }


    }

}
