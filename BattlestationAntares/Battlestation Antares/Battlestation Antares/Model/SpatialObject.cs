using System;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.Model {

    /// <summary>
    /// the basis class for spatial objects
    /// </summary>
    class SpatialObject {

        #region elements

        /// <summary>
        /// the position within the world model
        /// </summary>
        public Vector3 globalPosition = Vector3.Zero;

        /// <summary>
        /// the objects rotation matrix
        /// rotation.Forward is the moving direction
        /// </summary>
        public Matrix rotation = Matrix.Identity;

        /// <summary>
        /// the objects scale
        /// </summary>
        public Vector3 scale = Vector3.Zero;

        /// <summary>
        /// the visibility of the object
        /// </summary>
        public bool isVisible = true;

        /// <summary>
        /// the 3D model
        /// </summary>
        protected Microsoft.Xna.Framework.Graphics.Model model;

        /// <summary>
        /// the transform matrices for all model parts / bones
        /// </summary>
        protected Matrix[] boneTransforms;

        /// <summary>
        /// name of the loaded model
        /// </summary>
        private String modelName;

        #endregion

        public SpatialObject( String modelName, Vector3 position = new Vector3(), Matrix? rotation = null, Vector3? scale = null, bool isVisible = true ) {
            this.modelName = modelName;
            this.model = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Models//" + this.modelName );
            this.boneTransforms = new Matrix[this.model.Bones.Count];

            this.globalPosition = position;
            this.rotation = rotation ?? Matrix.Identity;
            this.scale = scale ?? Vector3.One;
            this.isVisible = isVisible;

            this.addDebugOutput();
        }

        #region methods

        public virtual void Draw( Camera camera ) {
            if ( this.isVisible ) {
                Draw3D.Draw( this.model, this.boneTransforms, camera.view, camera.projection, this.globalPosition, this.rotation, this.scale );
            }
        }

        public virtual void Update( GameTime gameTime ) {
        }

        public virtual void addDebugOutput() {
        }

        public override string ToString() {
            return this.modelName;
        }

        #endregion

    }

}
