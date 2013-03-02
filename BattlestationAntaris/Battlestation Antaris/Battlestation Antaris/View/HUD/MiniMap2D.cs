using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;

namespace Battlestation_Antaris.View.HUD
{

    public class MiniMap2D : HUD2DContainer
    {

        private HUD2DTexture background;

        private Vector2 iconSize = new Vector2(20, 20);

        private float iconPositionScale = 0.1f;


        public MiniMap2D(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.background = new HUD2DTexture(game);
            this.background.color = new Color(128, 128, 128, 128);
            this.background.abstractSize = new Vector2(300, 300);
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {
            Clear();

            Add(this.background);

            Add(new HUD2DTexture(
                this.game.world.spaceStation.minimapIcon,
                new Vector2(this.game.world.spaceStation.globalPosition.X, this.game.world.spaceStation.globalPosition.Z) * this.iconPositionScale,
                this.iconSize * 2, Color.LightBlue, 1.0f, 0, this.game));


            float rot = Tools.Tools.GetRotation(this.game.world.spaceShip.rotation.Forward, Matrix.Identity).Z - (float)(Math.PI/2);
            Add(new HUD2DTexture(
                this.game.world.spaceShip.minimapIcon,
                new Vector2(this.game.world.spaceShip.globalPosition.X, this.game.world.spaceShip.globalPosition.Z) * this.iconPositionScale,
                this.iconSize, Color.Green, 1.0f, rot, this.game));

            foreach (SpatialObject obj in this.game.world.allRadars)
            {
                if (obj.minimapIcon != null)
                {
                    Add(new HUD2DTexture(
                        obj.minimapIcon,
                        new Vector2(obj.globalPosition.X, obj.globalPosition.Z) * this.iconPositionScale,
                        this.iconSize / 2, Color.Blue, 1.0f, 0, this.game));
                }
            }

            foreach (SpatialObject obj in this.game.world.allTurrets)
            {
                if (obj.minimapIcon != null)
                {
                    Add(new HUD2DTexture(
                        obj.minimapIcon,
                        new Vector2(obj.globalPosition.X, obj.globalPosition.Z) * this.iconPositionScale,
                        this.iconSize, Color.LightBlue, 1.0f, 0, this.game));
                }
            }


            ClientSizeChanged(Vector2.Zero);

            base.Draw(spritBatch);
        }

    }

}
