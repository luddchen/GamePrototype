using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{

    public class MiniMap2D : HUD2DContainer
    {

        private HUD2DTexture background;

        private HUD2DTexture foreground;

        private Vector2 iconSize = new Vector2(15, 15);

        private float iconPositionScale = 0.125f;


        public MiniMap2D(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.background = new HUD2DTexture(game);
            this.background.color = new Color(16, 24, 24, 128);
            this.background.abstractSize = new Vector2(0.25f, 0.4f);
            this.background.sizeType = HUDType.RELATIV;
            this.background.layerDepth = 0.6f;

            this.foreground = new HUD2DTexture(game);
            this.foreground.color = new Color(16, 16, 16, 8);
            this.foreground.abstractSize = new Vector2(0.25f, 0.4f);
            this.foreground.sizeType = HUDType.RELATIV;
            this.foreground.layerDepth = 0.4f;
            this.foreground.Texture = game.Content.Load<Texture2D>("Sprites//SquareBorder");
        }


        public override void Draw(Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch)
        {
            Clear();

            Add(this.background);
            Add(this.foreground);

            Vector2 backgroundSize = (this.background.size - this.iconSize ) / 2;

            foreach (SpatialObject obj in this.game.world.allObjects)
            {
                if (obj.minimapIcon != null)
                {
                    float objRot = 0.0f;
                    float objScale = 0.7f;

                    Color objColor = obj.isEnemy ? Color.Red : Color.Blue;

                    if (!(obj is Radar))
                    {
                        objRot = Tools.Tools.GetRotation(obj.rotation.Forward, Matrix.Identity).Z - (float)(Math.PI / 2);
                        objScale = 1.0f;

                        if (obj is SpaceStation || obj is SpaceShip)
                        {
                            if (obj is SpaceStation)
                            {
                                objScale = 3.0f;
                            }
                            objColor = Color.Green;
                        }
                    }

                    Vector2 iconPos = new Vector2(obj.globalPosition.X, obj.globalPosition.Z) * this.iconPositionScale;

                    if (Math.Abs(iconPos.X) < backgroundSize.X && Math.Abs(iconPos.Y) < backgroundSize.Y)
                    {
                        Add(new HUD2DTexture(
                            obj.minimapIcon,
                            iconPos,
                            this.iconSize, objColor, objScale, objRot, this.game));

                    }
                }
            }

            foreach (SpatialObject obj in this.game.world.allWeapons)
            {
                if (obj.minimapIcon != null)
                {
                    Color objColor = Color.Yellow;

                    Vector2 iconPos = new Vector2(obj.globalPosition.X, obj.globalPosition.Z) * this.iconPositionScale;

                    if (Math.Abs(iconPos.X) < backgroundSize.X && Math.Abs(iconPos.Y) < backgroundSize.Y)
                    {
                        Add(new HUD2DTexture(
                            obj.minimapIcon,
                            iconPos,
                            this.iconSize, objColor, 0.5f, 0.0f, this.game));

                    }
                }
            }


            ClientSizeChanged(Vector2.Zero);

            base.Draw(spritBatch);
        }

    }

}
