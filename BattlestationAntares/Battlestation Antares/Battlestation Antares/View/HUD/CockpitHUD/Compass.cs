using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using Battlestation_Antares;
using Battlestation_Antares.Tools;
using HUD.HUD;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {
    public class Compass : HUDRenderedTexture {

        /// <summary>
        /// the compass 3d model
        /// </summary>
        private Microsoft.Xna.Framework.Graphics.Model model3d;


        /// <summary>
        /// the transformation matrices of the 3d model parts
        /// </summary>
        private Matrix[] boneTransforms;


        /// <summary>
        /// the spatial object (e.g. spaceship) that contains the compass
        /// </summary>
        private SpatialObject source;


        /// <summary>
        /// the targeted 3d point
        /// </summary>
        public Vector3 target;


        private Matrix view;
        private Matrix projection;
        private Vector3 compassPos;


        /// <summary>
        /// creates a new compass instance
        /// </summary>
        public Compass() : base( new Vector2( 300, 300 ), null ) {
            this.model3d = Antares.content.Load<Microsoft.Xna.Framework.Graphics.Model>( "Models/compass3" );
            this.boneTransforms = new Matrix[model3d.Bones.Count];

            this.target = new Vector3();

            this.view = Matrix.CreateLookAt( Vector3.Zero, Vector3.Forward, Vector3.Up );
            this.projection = Matrix.CreatePerspectiveFieldOfView( MathHelper.PiOver4 / 2, 16f / 9f, 1, 5000 );
            this.compassPos = Vector3.Forward * 1.8f;
        }


        /// <summary>
        /// initialize the compass on the specified spatial object that contains this compass
        /// </summary>
        /// <param name="source"></param>
        public void Initialize( SpatialObject source ) {
            this.source = source;
        }


        protected override void _RenderContent() {
            Antares.InitDepthBuffer();

            // if source is set
            if ( this.source != null ) {
                // get distance vector
                Vector3 pointer = Vector3.Subtract( this.target, this.source.globalPosition );

                // get local rotation
                Vector3 rot = Tools.GetRotation( pointer, this.source.rotation );

                Draw3D.Draw( model3d, boneTransforms, this.view, this.projection,
                            this.compassPos,
                            Matrix.CreateFromAxisAngle( Vector3.Right, rot.X ) * Matrix.CreateFromAxisAngle( Vector3.Up, rot.Z ),
                            new Vector3( 0.35f ) );
            }
        }
    }
}
