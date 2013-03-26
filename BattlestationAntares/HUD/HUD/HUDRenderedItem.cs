using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HUD.HUD {

    public class HUDRenderedItem : HUDRenderedTexture {

        SpriteBatch batch;

        HUD_Item item;


        public HUDRenderedItem( HUD_Item item, Vector2? renderSize, Color? backgroundColor ) : base(renderSize, backgroundColor) {
            this.item = item;
            batch = new SpriteBatch( HUD_Item.game.GraphicsDevice );
        }


        protected override void  _RenderContent()
        {
            item.RenderSizeChanged();

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            item.Draw( batch );
            batch.End();
        }

    }
}
