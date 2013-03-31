using System;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.Model;
using HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {

    class ObjectHealth : HUDContainer {

        private HUDValueCircle shield;

        private HUDTexture hullImage;

        public delegate Color ColorProvider(TactileSpatialObject o);

        private ColorProvider GetColor;

        private TactileSpatialObject obj;


        public ObjectHealth( Vector2 abstractPosition, HUDType positionType ) : base( abstractPosition ) {
            this.shield = new HUDValueCircle( Vector2.Zero, HUDType.ABSOLUT, new Vector2( 0.09f, 0.16f ), HUDType.RELATIV );
            this.shield.SetMaxColor( Color.Blue );
            this.shield.SetMinColor( Color.DarkRed );
            this.Add( this.shield );

            this.hullImage = new HUDTexture( null, null, new Vector2( 0.05f, 0.09f ) );
            this.hullImage.IsVisible = false;
            this.Add( this.hullImage );
        }


        public void setObject( TactileSpatialObject obj, String hullTexture, float hullScale) {
            this.obj = obj;
            this.shield.GetValue =
                delegate() {
                    return obj.attributes.Shield.CurrentHealthPoints / obj.attributes.Shield.MaxHealthPoints;
                };

            this.hullImage.Texture = HUDService.Content.Load<Texture2D>( hullTexture );
            this.hullImage.AbstractScale = hullScale;
            this.hullImage.IsVisible = true;
            this.GetColor = 
                delegate(TactileSpatialObject o) {
                    float v = o.attributes.Hull.CurrentHealthPoints / o.attributes.Hull.MaxHealthPoints;
                    return Color.Lerp( Color.Red, Color.Green, v );
                };
        }


        public new float LayerDepth {
            get {
                return base.LayerDepth;
            }
            set {
                base.LayerDepth = value;
                this.hullImage.LayerDepth = value - 0.05f;
            }
        }


        public override void Draw( SpriteBatch spriteBatch ) {
            if ( this.GetColor != null ) {
                this.hullImage.color = this.GetColor(this.obj);
            }
            base.Draw( spriteBatch );
        }

    }
}
