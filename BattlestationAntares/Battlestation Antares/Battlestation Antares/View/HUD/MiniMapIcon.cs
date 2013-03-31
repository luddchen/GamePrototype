using Battlestation_Antaris.Model;
using HUD.HUD;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD {

    class MiniMapIcon : HUDTexture {

        public SpatialObject spatialObject;

        public bool updateRotation;


        public MiniMapIcon( Texture2D texture, SpatialObject spatialObject )
            : base( texture ) {
            this.spatialObject = spatialObject;
            this.updateRotation = true;

            if ( miniMap != null ) {
                miniMap.Add( this );
                this.AbstractSize = miniMap.iconSize;
            }
        }


        public void RemoveFromWorld() {
            if ( miniMap != null ) {
                miniMap.Remove( this );
            }
        }


        public MiniMap miniMap {
            get {
                return Antares.world.miniMapRenderer.miniMap;
            }
        }


        public void Update( TactileSpatialObject centerObject ) {
            Vector3 center = Vector3.Zero;
            if ( centerObject != null ) {
                center.X = centerObject.globalPosition.X;
                center.Y = centerObject.globalPosition.Y;
                center.Z = centerObject.globalPosition.Z;
            }

            this.AbstractPosition =
                new Vector2( this.spatialObject.globalPosition.X - center.X, this.spatialObject.globalPosition.Z - center.Z ) * miniMap.iconPositionScale / 1000;

            if ( this.updateRotation ) {
                this.AbstractRotation = Tools.Tools.GetUpAxisRotation( this.spatialObject.rotation.Forward, Matrix.Identity );
            }
        }

    }

}
