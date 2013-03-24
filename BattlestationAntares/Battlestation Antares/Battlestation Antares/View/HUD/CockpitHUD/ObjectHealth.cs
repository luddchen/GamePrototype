using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {
    public class ObjectHealth : HUDContainer {

        private HUDValueCircle shield;

        private HUDTexture hullImage;

        public delegate Color ColorProvider(SpatialObject o);

        private ColorProvider GetColor;

        private SpatialObject obj;


        public ObjectHealth( Vector2 abstractPosition, HUDType positionType ) : base( abstractPosition, positionType ) {
            this.shield = new HUDValueCircle( Vector2.Zero, HUDType.ABSOLUT, new Vector2( 0.09f, 0.16f ), HUDType.RELATIV );
            this.shield.SetMaxColor( Color.Blue );
            this.shield.SetMinColor( Color.DarkRed );
            this.Add( this.shield );

            this.hullImage = new HUDTexture();
            this.hullImage.AbstractSize = new Vector2( 0.05f, 0.09f );
            this.hullImage.SizeType = HUDType.RELATIV;
            this.hullImage.IsVisible = false;
            this.Add( this.hullImage );
        }


        public void setObject( SpatialObject obj, Texture2D hullTexture, float hullScale) {
            this.obj = obj;
            this.shield.GetValue =
                delegate() {
                    return obj.attributes.Shield.CurrentHealthPoints / obj.attributes.Shield.MaxHealthPoints;
                };

            this.hullImage.Texture = hullTexture;
            this.hullImage.scale = hullScale;
            this.hullImage.IsVisible = true;
            this.GetColor = 
                delegate(SpatialObject o) {
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
