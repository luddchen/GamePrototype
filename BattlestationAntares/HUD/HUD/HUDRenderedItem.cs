using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace HUD.HUD {

    public class HUDRenderedItem : HUDRenderedTexture {

        SpriteBatch batch;

        HUD_Item item;


        public HUDRenderedItem( HUD_Item item, Point? renderSize, Color? backgroundColor ) : base(renderSize, backgroundColor) {
            this.item = item;
            batch = new SpriteBatch( HUDService.Device );
        }


        protected override void  DrawContent()
        {
            item.RenderSizeChanged();

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            item.Draw( batch );
            batch.End();
        }

    }
}
