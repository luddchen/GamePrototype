using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD
{

    public abstract class HUD2DConcreteElement : HUD2D
    {

        public HUDType sizeType;

        public Vector2 abstractSize;

        public Vector2 size;

        public float scale;

        public float rotation;

        public SpriteEffects effect;

        public Color color;


        public HUD2DConcreteElement(Game1 game)
            : base(game)
        {
            this.sizeType = HUDType.ABSOLUT;
            this.abstractSize = Vector2.Zero;
            this.size = Vector2.Zero;
            this.scale = 1.0f;
            this.rotation = 0.0f;
            this.effect = SpriteEffects.None;
            this.color = Color.White;
        }

        public abstract bool Intersects(Vector2 point);

    }

}
