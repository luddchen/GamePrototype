using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD
{

    public class MiniMap : HUD2DContainer
    {
        public static Color ENEMY_COLOR = Color.Red;

        public static Color FRIEND_COLOR = Color.Blue;

        public static Color SPECIAL_COLOR = Color.Green;

        public static Color WEAPON_COLOR = Color.Yellow;

        public static float MIN_SCALE = 0.005f;

        public static float MAX_SCALE = 0.500f;

        public static Color BACKGROUND_COLOR = new Color(16, 24, 24, 128);

        public static Color BORDER_COLOR = new Color(16, 16, 16, 8);

        public static Color BORDER_COLOR_HOVER = new Color(32, 32, 32, 32);


        private HUD2DTexture background;

        private HUD2DTexture foreground;

        public Vector2 iconSize = new Vector2(15, 15);

        public float iconPositionScale = 0.1f;


        public MiniMap(Vector2 abstractPosition, HUDType positionType, Game1 game)
            : base(abstractPosition, positionType, game)
        {
            this.background = new HUD2DTexture(game);
            this.background.color = MiniMap.BACKGROUND_COLOR;
            this.background.abstractSize = new Vector2(0.25f, 0.4f);
            this.background.sizeType = HUDType.RELATIV;
            this.background.layerDepth = 0.6f;

            this.foreground = new HUD2DTexture(game);
            this.foreground.color = MiniMap.BORDER_COLOR;
            this.foreground.abstractSize = new Vector2(0.25f, 0.4f);
            this.foreground.sizeType = HUDType.RELATIV;
            this.foreground.layerDepth = 0.4f;
            this.foreground.Texture = game.Content.Load<Texture2D>("Sprites//SquareBorder");

            Add(this.background);
            Add(this.foreground);
        }


        public override void Draw(SpriteBatch spritBatch)
        {

            Vector2 backgroundSize = (this.background.size - this.iconSize) / 2;

            foreach (HUD2D element in this.allChilds) 
            {
                if (element is MiniMapIcon)
                {
                    MiniMapIcon icon = ((MiniMapIcon)element);
                    icon.Update();

                    if (Math.Abs(icon.abstractPosition.X) < backgroundSize.X &&
                        Math.Abs(icon.abstractPosition.Y) < backgroundSize.Y &&
                        icon.spatialObject.isVisible)
                    {
                        icon.isVisible = true;
                    }
                    else
                    {
                        icon.isVisible = false;
                    }
                }
            }

            ClientSizeChanged(Vector2.Zero);

            base.Draw(spritBatch);
        }


        public void ZoomOnMouseWheelOver()
        {
            if (this.background.Intersects(this.game.inputProvider.getMousePos()))
            {
                this.foreground.color = MiniMap.BORDER_COLOR_HOVER;

                int wheelChange = this.game.inputProvider.getMouseWheelChange();

                if (wheelChange != 0)
                {
                    if (wheelChange > 0)
                    {
                        this.iconPositionScale *= 1.2f;
                        if (this.iconPositionScale > MiniMap.MAX_SCALE)
                        {
                            this.iconPositionScale = MiniMap.MAX_SCALE;
                        }
                    }
                    else
                    {
                        this.iconPositionScale /= 1.2f;
                        if (this.iconPositionScale < MiniMap.MIN_SCALE)
                        {
                            this.iconPositionScale = MiniMap.MIN_SCALE;
                        }
                    }
                }
            }
            else
            {
                this.foreground.color = MiniMap.BORDER_COLOR;
            }
        }

    }

}
