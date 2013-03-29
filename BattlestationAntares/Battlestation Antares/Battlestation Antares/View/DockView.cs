using Battlestation_Antares.View;
using HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares;
using Battlestation_Antares.Tools;

namespace Battlestation_Antaris.View {

    class DockView : HUDView{

        /// <summary>
        /// the dock view camera
        /// </summary>
        protected Camera camera;

        public float Rotation {
            set;
            private get;
        }

        public float Distance {
            set;
            private get;
        }


        public DockView( Color? backgroundColor ) : base( backgroundColor ) {

            this.camera = new Camera();
        }

    
        public override void  Initialize() { }


        protected override void DrawPreContent() {
            Antares.InitDepthBuffer();

            Vector3 pos = Vector3.Transform( Vector3.Forward * Distance, Matrix.CreateRotationY( Rotation ) );
            this.camera.Update( pos - Vector3.Up * 115.0f, -pos, Vector3.Up );

            Draw3D.Draw( Antares.world.allDrawable, this.camera );
        }

}

}
