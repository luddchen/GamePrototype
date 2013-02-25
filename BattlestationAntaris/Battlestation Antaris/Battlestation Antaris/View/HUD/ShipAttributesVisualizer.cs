using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antaris.Model;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antaris.View.HUD
{

    /// <summary>
    /// a test class for debuging spatial object movement
    /// </summary>
    class ShipAttributesVisualizer : HUDRelativeContainer
    {

        HUDString velocity;
        HUDString yawVelocity;
        HUDString pitchVelocity;
        HUDString rollVelocity;

        HUDString distance;

        HUDTexture bgTexture;

        SpatialObject ship;


        public ShipAttributesVisualizer(float relativeX, float relativeY, Viewport viewport, SpatialObject ship, Game1 game)
            : base(relativeX, relativeY, viewport)
        {
            this.ship = ship;

            this.velocity = new HUDString("Speed : ", null, null, null, null, 0.35f, 0.0f, game.Content);
            this.velocity.Position = new Vector2(0,-50);
            this.velocity.layerDepth = 0.4f;
            Add(this.velocity);

            this.yawVelocity = new HUDString("Yaw : ", null, null, null, null, 0.35f, 0.0f, game.Content);
            this.yawVelocity.Position = new Vector2(0,-25);
            this.yawVelocity.layerDepth = 0.4f;
            Add(this.yawVelocity);

            this.pitchVelocity = new HUDString("Pitch : ", null, null, null, null, 0.35f, 0.0f, game.Content);
            this.pitchVelocity.Position = new Vector2(0,0);
            this.pitchVelocity.layerDepth = 0.4f;
            Add(this.pitchVelocity);

            this.rollVelocity = new HUDString("Roll : ", null, null, null, null, 0.35f, 0.0f, game.Content);
            this.rollVelocity.Position = new Vector2(0,25);
            this.rollVelocity.layerDepth = 0.4f;
            Add(this.rollVelocity);

            this.distance = new HUDString("Distance : ", null, null, null, null, 0.35f, 0.0f, game.Content);
            this.distance.Position = new Vector2(0,50);
            this.distance.layerDepth = 0.4f;
            Add(this.distance);

            this.bgTexture = new HUDTexture(game.Content);
            this.bgTexture.Position = new Vector2(0,0);
            this.bgTexture.Width = 150;
            this.bgTexture.Height = 150;
            this.bgTexture.Color = new Color(10, 10, 10, 160);
            Add(this.bgTexture);
        }


        /// <summary>
        /// draw the spatial object attributes
        /// </summary>
        /// <param name="spritBatch"></param>
        public override void Draw(SpriteBatch spritBatch)
        {
            this.velocity.String = String.Format("Speed : {0:F2}", this.ship.attributes.Engine.CurrentVelocity);
            this.yawVelocity.String = String.Format("Yaw : {0:F2}", this.ship.attributes.EngineYaw.CurrentVelocity * 100);
            this.pitchVelocity.String = String.Format("Pitch : {0:F2}", this.ship.attributes.EnginePitch.CurrentVelocity * 100);
            this.rollVelocity.String = String.Format("Roll : {0:F2}", this.ship.attributes.EngineRoll.CurrentVelocity * 100);
            this.distance.String = String.Format("Distance : {0:F0}", this.ship.globalPosition.Length());

            base.Draw(spritBatch);
        }


        //public override void Window_ClientSizeChanged(Viewport viewport)
        //{
        //}

    }

}
