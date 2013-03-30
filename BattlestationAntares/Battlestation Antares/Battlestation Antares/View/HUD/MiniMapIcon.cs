using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD {

    public class MiniMapIcon : HUDTexture {

        public SpatialObjectOld spatialObject;

        public bool updateRotation;


        public MiniMapIcon( Texture2D texture, SpatialObjectOld spatialObject )
            : base( texture, null, null, null, null, null ) {
            this.spatialObject = spatialObject;
            this.updateRotation = true;
        }


        public void AddToWorld() {
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


        public void Update( SpatialObjectOld centerObject ) {
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
