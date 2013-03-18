using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD.AIComposer
{

    public class AI_Connection : HUD2D
    {

        AI_ItemPort source;

        AI_ItemPort target;

        public int width = 3;

        public Color colorNormal = Color.Yellow;

        public Color colorHighlight = Color.Red;

        public AI_Connection(Antares game)
            : base(game)
        {
            this.color = this.colorNormal;
        }


        public void setSource(AI_ItemPort source)
        {
            if (this.source != null)
            {
                this.source.connections.Remove(this);
            }
            this.source = source;
            if (source != null)
            {
                this.source.connections.Add(this);
            }
        }

        public void setTarget(AI_ItemPort target)
        {
            if (this.target != null)
            {
                this.target.connections.Remove(this);
            }
            this.target = target;
            if (target != null)
            {
                this.target.connections.Add(this);
            }
        }

        public AI_ItemPort getSource()
        {
            return this.source;
        }

        public AI_ItemPort getTarget()
        {
            return this.target;
        }

        public void Delete()
        {
            if (this.source != null)
            {
                this.source.connections.Remove(this);
                this.source = null;
            }

            if (this.target != null)
            {
                this.target.connections.Remove(this);
                this.target = null;
            }
        }


        public override void Draw(SpriteBatch spritBatch)
        { }


        public void  Draw(PrimitiveBatch primitiveBatch)
        {
            if (this.source != null && this.target != null)
            {
                if (this.width > 1)
                {
                    for (Vector2 pos = new Vector2(-this.width / 2, 0); pos.X <= (this.width / 2); pos.X += 1.0f)
                    {
                        primitiveBatch.AddVertex(this.source.position + pos, this.color);
                        primitiveBatch.AddVertex(this.target.position + pos, this.color);
                    }
                }
                else
                {
                    primitiveBatch.AddVertex(this.source.position, this.color);
                    primitiveBatch.AddVertex(this.target.position, this.color);
                }
            }
        }


        public override bool Intersects(Vector2 point)
        {
            if (this.source == null || this.target == null)
            {
                return false;
            }
            Vector2 lineVec = this.target.position - this.source.position; // Vector source -> target
            Vector2 pointVec = point - this.source.position; // Vector source -> point

            float xOff = pointVec.X / lineVec.X;
            float yOff = pointVec.Y / lineVec.Y;

            if (Math.Abs(lineVec.X) < 0.01f)
            {
                if (Math.Abs(pointVec.X) < 1f && yOff > 0.05f && yOff < 0.95f)
                {
                    return true;
                }
            }

            if (xOff > 0.05f && xOff < 0.95f && yOff > 0.05f && yOff < 0.95f)
            {
                if (Math.Abs(xOff - yOff) < 0.2f)
                {
                    return true;
                }
            }

            return false;
        }

    }

}
