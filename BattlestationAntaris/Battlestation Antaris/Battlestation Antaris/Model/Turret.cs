﻿using System;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;

using SpatialObjectAttributesLibrary;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.Model
{

    /// <summary>
    /// a dangerous turret
    /// </summary>
    public class Turret : SpatialObject
    {

        private Random random;

        private int timeout;

        private int beams;

        /// <summary>
        /// create a new turret and insert into the world
        /// </summary>
        /// <param name="position">position</param>
        /// <param name="modelName">3D model name</param>
        /// <param name="content">game content manager</param>
        /// <param name="world">the world model</param>
        public Turret(Vector3 position, ContentManager content, WorldModel world) : base(position, "Models//Turret//turret", content, world) 
        {
            this.random = new Random((int)position.X);
            this.timeout = this.random.Next(120) + 60;
            this.beams = 0;

            this.attributes = new SpatialObjectAttributes( content.Load<SpatialObjectAttributes>("Attributes//Turret") );

            this.miniMapIcon.Texture = content.Load<Texture2D>("Models//Turret//turret_2d");
            this.miniMapIcon.color = MiniMap.FRIEND_COLOR;
            //this.miniMapIcon.scale = 2.0f;
        }


        /// <summary>
        /// update the turret
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(Microsoft.Xna.Framework.GameTime gameTime)
        {
            ApplyRotation(gameTime);

            this.timeout--;

            if (this.timeout < 0)
            {
                this.timeout = this.random.Next(30) + 30;
                this.beams = 1;// this.random.Next(2) + 2;
            }

            if (this.beams > 0)
            {
                Laser laser = new Laser(this, 0.0f, this.world.game.Content, this.world);
                this.beams--;
            }


            switch (this.random.Next(6))
            {
                case 0:
                    InjectControl(Control.Control.PITCH_DOWN);
                    break;
                case 1:
                    InjectControl(Control.Control.PITCH_UP);
                    break;
                case 2:
                    InjectControl(Control.Control.YAW_LEFT);
                    break;
                case 3:
                    InjectControl(Control.Control.YAW_RIGHT);
                    break;
                case 4:
                    InjectControl(Control.Control.ROLL_ANTICLOCKWISE);
                    break;
                case 5:
                    InjectControl(Control.Control.ROLL_CLOCKWISE);
                    break;
            }

        }


        public override string ToString()
        {
            return "Turret";
        }

    }

}
