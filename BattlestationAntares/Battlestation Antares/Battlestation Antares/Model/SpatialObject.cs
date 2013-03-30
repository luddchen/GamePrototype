using System;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View;

namespace Battlestation_Antaris.Model {

    /// <summary>
    /// the basis class for spatial objects
    /// </summary>
    class SpatialObject {

        #region elements

        /// <summary>
        /// the position within the world model
        /// </summary>
        public Vector3 globalPosition;

        /// <summary>
        /// the objects rotation matrix
        /// rotation.Forward is the moving direction
        /// </summary>
        public Matrix rotation;

        /// <summary>
        /// the objects scale
        /// </summary>
        public Vector3 scale;

        /// <summary>
        /// the visibility of the object
        /// </summary>
        public bool isVisible;

        /// <summary>
        /// the 3D model
        /// </summary>
        private Microsoft.Xna.Framework.Graphics.Model model;

        /// <summary>
        /// the transform matrices for all model parts / bones
        /// </summary>
        private Matrix[] boneTransforms;

        private String modelName;

        #endregion

        #region constructors

        public SpatialObject( String modelName )
            : this( modelName, null, null, null, null ) {
        }

        public SpatialObject( String modelName, Vector3? position )
            : this( modelName, position, null, null, null ) {
        }

        public SpatialObject( String modelName, Vector3? position, float yaw, float pitch, float roll )
            : this( modelName, position, Tools.YawPitchRoll( Matrix.Identity, yaw, pitch, roll ), null, null ) {
        }

        public SpatialObject( String modelName, Vector3? position, Matrix? rotation ) 
            : this(modelName, position, rotation, null, null) {
        }

        public SpatialObject( String modelName, Vector3? position, Matrix? rotation, Vector3? scale, bool? isVisible ) {
            if ( model != null) {
                this.modelName = modelName;
                this.model = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( this.modelName );
            } else {
                throw new ArgumentNullException( "SpatialObject.modelName" );
            }

            this.globalPosition = position ?? Vector3.Zero;

            this.rotation = rotation ?? Matrix.Identity;

            this.scale = scale ?? Vector3.One;

            this.isVisible = isVisible ?? true;
        }

        #endregion

        #region methods

        public void Draw( Camera camera ) {
            if ( this.isVisible ) {
                Draw3D.Draw( this.model, this.boneTransforms, camera.view, camera.projection, this.globalPosition, this.rotation, this.scale );
            }
        }

        public override string ToString() {
            return this.GetType().Name + " : " + this.modelName;
        }

        #endregion

    }

}
