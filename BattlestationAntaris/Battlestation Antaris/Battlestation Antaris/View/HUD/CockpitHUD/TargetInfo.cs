using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View.HUD.CockpitHUD
{
    public class TargetInfo : HUD2DArray
    {

        private HUD2DString targetObject;

        private HUD2DString targetDistance;

        private WorldModel world;

        public SpatialObject target;


        public TargetInfo(Vector2 abstractPosition, HUDType positionType, Vector2 abstractSize, HUDType sizeType , Game1 game)
            : base(abstractPosition, positionType, abstractSize, sizeType , game)
        {
            this.world = this.game.world;

            targetObject = new HUD2DString("", game);
            targetObject.scale = 0.5f;
            targetDistance = new HUD2DString("", game);
            targetDistance.scale = 0.5f;

            Add(targetObject);
            Add(targetDistance);

            CreateBackground(true);

            this.isVisible = false;
        }

        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {

            float testDist = float.MaxValue;

            target = this.world.treeTest.CastRay(new Ray(this.world.spaceShip.globalPosition, this.world.spaceShip.rotation.Forward), 1, ref testDist);

            if (target != null)
            {
                this.targetObject.String = target.ToString();
                this.targetDistance.String = String.Format("{0:F0} m", testDist);
                this.isVisible = true;
            }
            else
            {
                this.targetObject.String = "";
                this.targetDistance.String = "";
                this.isVisible = false;
            }

            base.Draw(spritBatch);
        }

    }
}
