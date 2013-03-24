using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;

namespace Battlestation_Antares.View.HUD {

    public class MiniMapIcon : HUDTexture {

        public SpatialObject spatialObject;

        public bool updateRotation;


        public MiniMapIcon( Texture2D texture, SpatialObject spatialObject )
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
                return Antares.world.miniMap;
            }
        }


        public void Update( SpatialObject centerObject ) {
            Vector3 center = Vector3.Zero;
            if ( centerObject != null ) {
                center.X = centerObject.globalPosition.X;
                center.Y = centerObject.globalPosition.Y;
                center.Z = centerObject.globalPosition.Z;
            }

            this.AbstractPosition =
                new Vector2( this.spatialObject.globalPosition.X - center.X, this.spatialObject.globalPosition.Z - center.Z ) * miniMap.iconPositionScale;

            if ( this.updateRotation ) {
                this.rotation = Tools.Tools.GetUpAxisRotation( this.spatialObject.rotation.Forward, Matrix.Identity );
            }
        }

    }

}
