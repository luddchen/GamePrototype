using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View.HUD {

    public class MiniMapIcon : HUD2DTexture {

        public SpatialObject spatialObject;

        public bool updateRotation;


        public MiniMapIcon( Texture2D texture, SpatialObject spatialObject)
            : base( texture, null, null, null, null, null) {
            this.spatialObject = spatialObject;
            this.updateRotation = true;
        }


        public void AddToWorld() {
            if ( miniMap != null ) {
                miniMap.Add( this );
                this.abstractSize = miniMap.iconSize;
            }
        }


        public void RemoveFromWorld() {
            if ( miniMap != null ) {
                miniMap.Remove( this );
            }
        }


        public MiniMap miniMap {
            get {
                return Antares.world.miniMap;
            }
        }


        public void Update() {
            this.abstractPosition.X = this.spatialObject.globalPosition.X * miniMap.iconPositionScale;
            this.abstractPosition.Y = this.spatialObject.globalPosition.Z * miniMap.iconPositionScale;

            if ( this.updateRotation ) {
                this.rotation = Tools.Tools.GetUpAxisRotation( this.spatialObject.rotation.Forward, Matrix.Identity );
            }
        }

    }

}
