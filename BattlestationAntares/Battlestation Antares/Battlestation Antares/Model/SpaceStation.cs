using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.View.HUD;
using SpatialObjectAttributesLibrary;

namespace Battlestation_Antares.Model {

    /// <summary>
    /// the Antares space station
    /// </summary>
    public class SpaceStation : SpatialObject {

        /// <summary>
        /// model bone of the rotating part of the station
        /// </summary>
        private ModelBone Barrier1;
        private ModelBone Barrier2;
        private ModelBone Barrier3;
        private ModelBone Airlock;


        /// <summary>
        /// the transformation matrix of the rotating part of the station
        /// </summary>
        Matrix Barrier1Transform;
        Matrix Barrier2Transform;
        Matrix Barrier3Transform;
        Matrix AirlockTransform;


        /// <summary>
        /// the current rotation value of the rotating part of the station
        /// </summary>
        float AxisRot = 0.0f;

        float dir = 0.1f;
        float airlockMove = 0;
        int airlockDelay = 0;

        /// <summary>
        /// create a new space station within the world
        /// </summary>
        /// <param name="position">world position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public SpaceStation( Vector3 position, String modelName, ContentManager content, WorldModel world )
            : base( position, modelName, content, world ) {
            init();
            this.attributes = new SpatialObjectAttributes( content.Load<SpatialObjectAttributes>( "Attributes//SpaceStation" ) );
            this.miniMapIcon.Texture = content.Load<Texture2D>( "Models//SpaceStation//station_2d" );
            this.miniMapIcon.color = MiniMap.SPECIAL_COLOR;
            this.miniMapIcon.AbstractScale = 2.0f;
        }


        /// <summary>
        /// init the space station
        /// </summary>
        private void init() {
            // test output of bounding sphere
            // Console.Out.WriteLine("Station Bounding Sphere : " + this.bounding + " (" + this.model3d.Meshes.Count + " meshes)");

            Barrier1 = model3d.Bones["Barrier1"];
            Barrier2 = model3d.Bones["Barrier2"];
            Barrier3 = model3d.Bones["Barrier3"];
            Airlock = model3d.Bones["Airlock"];

            Barrier1Transform = Barrier1.Transform;
            Barrier2Transform = Barrier2.Transform;
            Barrier3Transform = Barrier3.Transform;
            AirlockTransform = Airlock.Transform;

            // initial rotation of full station, dont know why this is necessary
            //this.rotation = Tools.Tools.Pitch( this.rotation, (float)( -Math.PI / 2 ) );
        }


        /// <summary>
        /// update the space station
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update( Microsoft.Xna.Framework.GameTime gameTime ) {
            base.Update( gameTime );
            this.attributes.Shield.Regenerate();

            // update rotation of the rotating part
            AxisRot += (float)( Math.PI / 1440 );

            if ( airlockDelay <= 0 ) {
                airlockMove += dir;
                if ( airlockMove > 16 ) {
                    airlockMove = 16;
                    airlockDelay = 120;
                    dir *= -1.0f;
                }
                if ( airlockMove < -0.5f ) {
                    airlockMove = -0.5f;
                    airlockDelay = 120;
                    dir *= -1.0f;
                }
            } else {
                airlockDelay--;
            }

            Barrier1.Transform = Matrix.CreateRotationY( AxisRot * 7.2f ) * Barrier1Transform;
            Barrier2.Transform = Matrix.CreateRotationY( -AxisRot * 17.7f ) * Barrier2Transform;
            Barrier3.Transform = Matrix.CreateRotationY( AxisRot * 3.4f) * Barrier3Transform;
            Airlock.Transform = Matrix.CreateTranslation(new Vector3( 0, airlockMove, 0 ) ) * AirlockTransform;
        }


        public override void onHit( float damage ) {
            if ( this.attributes.Shield.ApplyDamage( damage ) ) {
                this.attributes.Hull.ApplyDamage( damage );
            }
        }


        public override string ToString() {
            return "SpaceStation";
        }

    }
}
