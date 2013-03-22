using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares;

namespace Battlestation_Antaris.View.HUD {
    public class HUD2DRenderedItem : HUDRenderedTexture {

        SpriteBatch batch;

        HUD2D item;


        public HUD2DRenderedItem( HUD2D item, Vector2? renderSize, Color? backgroundColor ) : base(renderSize, backgroundColor) {
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
