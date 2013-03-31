using Battlestation_Antares;
using Battlestation_Antares.Tools;
using Battlestation_Antares.View;
using Battlestation_Antaris.Model;
using HUD.HUD;
using Microsoft.Xna.Framework;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {

    class Compass : HUDRenderedTexture {

        private SpatialObject compass;

        private Camera camera;

        /// <summary>
        /// the spatial object (e.g. spaceship) that contains the compass
        /// </summary>
        private SpatialObject source;

        /// <summary>
        /// the targeted 3d point
        /// </summary>
        public Vector3 target;


        /// <summary>
        /// creates a new compass instance
        /// </summary>
        public Compass() : base( new Point( 300, 300 ), null ) {
            this.compass = new SpatialObject( "compass3", position: Vector3.Forward * 2.0f, scale: new Vector3( 0.8f ) );
            this.camera = new Camera();
            this.camera.Update( Vector3.Zero, Vector3.Forward, Vector3.Up );

            this.target = new Vector3();
        }


        /// <summary>
        /// initialize the compass on the specified spatial object that contains this compass
        /// </summary>
        /// <param name="source"></param>
        public void Initialize( SpatialObject source ) {
            this.source = source;
        }


        protected override void DrawContent() {
            Antares.InitDepthBuffer();

            if ( this.source != null ) {
                // get distance vector and local rotation
                Vector3 pointer = Vector3.Subtract( this.target, this.source.globalPosition );
                Vector3 rot = Tools.GetRotation( pointer, this.source.rotation );

                this.compass.rotation = Matrix.CreateFromAxisAngle( Vector3.Right, rot.X ) * Matrix.CreateFromAxisAngle( Vector3.Up, rot.Z );
                this.compass.Draw( this.camera );
            }
        }
    }
}
