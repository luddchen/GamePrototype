using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares;

namespace Battlestation_Antaris.View.HUD {
    class HUD2DRenderedItem : HUD2DTexture {

        Vector2 renderSize = new Vector2( 480, 480 );

        RenderTarget2D target;
        SpriteBatch batch;

        HUD2D item;

        bool inited = false;


        public HUD2DRenderedItem( HUD2D item) {
            this.item = item;
            target = new RenderTarget2D( Antares.graphics.GraphicsDevice, (int)renderSize.X, (int)renderSize.Y );
            batch = new SpriteBatch( Antares.graphics.GraphicsDevice );
        }


        public void Render() {
            RenderTarget2D oldTarget = null;
            if ( Antares.graphics.GraphicsDevice.GetRenderTargets().Length > 0 ) {
                oldTarget = (RenderTarget2D)Antares.graphics.GraphicsDevice.GetRenderTargets()[0].RenderTarget;
            }
            Antares.graphics.GraphicsDevice.SetRenderTarget( target );
            Antares.graphics.GraphicsDevice.Clear( Color.Transparent );

            if ( !inited ) {
                item.ClientSizeChanged();
                inited = true;
            }

            batch.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend);
            item.Draw( batch );
            batch.End();

            Antares.graphics.GraphicsDevice.SetRenderTarget( oldTarget );

            this.Texture = (Texture2D)target;
        }

    }
}
