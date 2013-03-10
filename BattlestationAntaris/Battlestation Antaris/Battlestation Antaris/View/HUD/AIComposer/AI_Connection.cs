using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.AIComposer
{

    public class AI_Connection : HUD2D
    {

        AI_ItemPort source;

        AI_ItemPort target;

        public int width = 3;

        public AI_Connection(Game1 game)
            : base(game)
        {
        }


        public void setSource(AI_ItemPort source)
        {
            if (this.source != null)
            {
                this.source.connections.Remove(this);
            }
            this.source = source;
            this.source.connections.Add(this);
        }

        public void setTarget(AI_ItemPort target)
        {
            if (this.target != null)
            {
                this.target.connections.Remove(this);
            }
            this.target = target;
            this.target.connections.Add(this);
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
                        primitiveBatch.AddVertex(this.source.position + pos, Color.Yellow);
                        primitiveBatch.AddVertex(this.target.position + pos, Color.Yellow);
                    }
                }
                else
                {
                    primitiveBatch.AddVertex(this.source.position, Color.Yellow);
                    primitiveBatch.AddVertex(this.target.position, Color.Yellow);
                }
            }
        }


        public override bool Intersects(Vector2 point)
        {
            throw new NotImplementedException();
        }

    }

}
