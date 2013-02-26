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
    class ShipAttributesVisualizer : HUD2DContainer
    {

        HUD2DString velocity;
        HUD2DString yawVelocity;
        HUD2DString pitchVelocity;
        HUD2DString rollVelocity;

        HUD2DString distance;

        HUD2DTexture bgTexture;

        SpatialObject ship;


        public ShipAttributesVisualizer(float relativeX, float relativeY, SpatialObject ship, Game1 game)
            : base(new Vector2(relativeX, relativeY), HUDType.RELATIV, game)
        {
            this.ship = ship;

            this.velocity = new HUD2DString("Speed : ", null, null, null, null, 0.35f, 0.0f, this.game);
            this.velocity.abstractPosition = new Vector2(0,-50);
            this.velocity.layerDepth = 0.4f;
            Add(this.velocity);

            this.yawVelocity = new HUD2DString("Yaw : ", null, null, null, null, 0.35f, 0.0f, this.game);
            this.yawVelocity.abstractPosition = new Vector2(0, -25);
            this.yawVelocity.layerDepth = 0.4f;
            Add(this.yawVelocity);

            this.pitchVelocity = new HUD2DString("Pitch : ", null, null, null, null, 0.35f, 0.0f, this.game);
            this.pitchVelocity.abstractPosition = new Vector2(0,0);
            this.pitchVelocity.layerDepth = 0.4f;
            Add(this.pitchVelocity);

            this.rollVelocity = new HUD2DString("Roll : ", null, null, null, null, 0.35f, 0.0f, this.game);
            this.rollVelocity.abstractPosition = new Vector2(0, 25);
            this.rollVelocity.layerDepth = 0.4f;
            Add(this.rollVelocity);

            this.distance = new HUD2DString("Distance : ", null, null, null, null, 0.35f, 0.0f, this.game);
            this.distance.abstractPosition = new Vector2(0, 50);
            this.distance.layerDepth = 0.4f;
            Add(this.distance);

            this.bgTexture = new HUD2DTexture(this.game);
            this.bgTexture.abstractSize = new Vector2(150, 150);
            this.bgTexture.color = new Color(10, 10, 10, 160);
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


    }

}
