using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antares.View.HUD.CommandHUD {
    class MouseTexture : HUD2DTexture {
        public MouseTexture( Texture2D texture )
            : base( texture, null, new Microsoft.Xna.Framework.Vector2( 15f, 15f ), Color.Blue, null, null ) {
            this.positionType = HUDType.ABSOLUT;
            this.isVisible = false;
        }

        public virtual void update() {
            this.abstractPosition = Antares.inputProvider.getMousePos();
            ClientSizeChanged();
        }
    }
}
