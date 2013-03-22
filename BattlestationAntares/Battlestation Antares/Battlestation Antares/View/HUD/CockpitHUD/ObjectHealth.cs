using System;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using Battlestation_Antares;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {
    public class ObjectHealth : HUDContainer {

        private HUDValueCircle shield;

        private HUDTexture hullImage;

        public delegate Color ColorProvider(SpatialObject o);

        private ColorProvider GetColor;

        private SpatialObject obj;


        public ObjectHealth( Vector2 abstractPosition, HUDType positionType ) : base( abstractPosition, positionType ) {
            this.shield = new HUDValueCircle( Vector2.Zero, HUDType.ABSOLUT, new Vector2( 150, 150 ), HUDType.ABSOLUT );
            this.shield.SetMaxColor( Color.Blue );
            this.shield.SetMinColor( Color.DarkRed );
            this.Add( this.shield );

            this.hullImage = new HUDTexture();
            this.hullImage.abstractSize = new Vector2( 75, 75 );
            this.hullImage.sizeType = HUDType.ABSOLUT;
            this.hullImage.isVisible = false;
            this.Add( this.hullImage );

            setLayerDepth( this.layerDepth );
        }


        public void setObject( SpatialObject obj, Texture2D hullTexture, float hullScale) {
            this.obj = obj;
            this.shield.GetValue =
                delegate() {
                    return obj.attributes.Shield.CurrentHealthPoints / obj.attributes.Shield.MaxHealthPoints;
                };

            this.hullImage.Texture = hullTexture;
            this.hullImage.scale = hullScale;
            this.hullImage.isVisible = true;
            this.GetColor = 
                delegate(SpatialObject o) {
                    float v = o.attributes.Hull.CurrentHealthPoints / o.attributes.Hull.MaxHealthPoints;
                    return Color.Lerp( Color.Red, Color.Green, v );
                };
        }


        public override void setLayerDepth( float layerDepth ) {
            base.setLayerDepth( layerDepth );
            this.hullImage.setLayerDepth( this.layerDepth - 0.05f );
        }


        public override void Draw( SpriteBatch spritBatch ) {
            if ( this.GetColor != null ) {
                this.hullImage.color = this.GetColor(this.obj);
            }
            base.Draw( spritBatch );
        }

    }
}
