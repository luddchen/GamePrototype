using System;
using System.Collections.Generic;
using Battlestation_Antares.View;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares;
using Battlestation_Antares.View.HUD;

namespace Battlestation_Antaris.View.HUD {
    public class HUDRenderedItem : HUDRenderedTexture {

        SpriteBatch batch;

        HUD_Item item;


        public HUDRenderedItem( HUD_Item item, Vector2? renderSize, Color? backgroundColor ) : base(renderSize, backgroundColor) {
            this.item = item;
            batch = new SpriteBatch( Antares.graphics.GraphicsDevice );
        }


        protected override void  _RenderContent()
        {
            item.ClientSizeChanged();

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            item.Draw( batch );
            batch.End();
        }

    }
}
