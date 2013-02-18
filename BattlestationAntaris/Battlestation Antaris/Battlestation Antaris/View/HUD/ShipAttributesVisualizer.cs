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

    class ShipAttributesVisualizer : HUDElement2D
    {

        HUDString velocity;
        HUDString yawVelocity;
        HUDString pitchVelocity;
        HUDString rollVelocity;

        HUDString distance;

        SpatialObject ship;

        public ShipAttributesVisualizer(SpatialObject ship, Game1 game)
        {
            this.ship = ship;

            this.velocity = new HUDString("Speed : ", null, null, null, new Color(10, 10, 10, 200), 0.35f, 0.0f, game.Content);
            this.velocity.Position = new Vector2(game.GraphicsDevice.Viewport.Width * 0.1f, game.GraphicsDevice.Viewport.Height * 0.75f);

            this.yawVelocity = new HUDString("Yaw : ", null, null, null, new Color(10, 10, 10, 200), 0.35f, 0.0f, game.Content);
            this.yawVelocity.Position = new Vector2(game.GraphicsDevice.Viewport.Width * 0.1f, game.GraphicsDevice.Viewport.Height * 0.8f);

            this.pitchVelocity = new HUDString("Pitch : ", null, null, null, new Color(10, 10, 10, 200), 0.35f, 0.0f, game.Content);
            this.pitchVelocity.Position = new Vector2(game.GraphicsDevice.Viewport.Width * 0.1f, game.GraphicsDevice.Viewport.Height * 0.85f);

            this.rollVelocity = new HUDString("Roll : ", null, null, null, new Color(10, 10, 10, 200), 0.35f, 0.0f, game.Content);
            this.rollVelocity.Position = new Vector2(game.GraphicsDevice.Viewport.Width * 0.1f, game.GraphicsDevice.Viewport.Height * 0.9f);

            this.distance = new HUDString("Roll : ", null, null, null, new Color(10, 10, 10, 200), 0.35f, 0.0f, game.Content);
            this.distance.Position = new Vector2(game.GraphicsDevice.Viewport.Width * 0.1f, game.GraphicsDevice.Viewport.Height * 0.95f);
        }

        public override void Draw(SpriteBatch spritBatch)
        {
            this.velocity.String = String.Format("Speed : {0:F2}", this.ship.attributes.Engine.CurrentVelocity);
            this.yawVelocity.String = String.Format("Yaw : {0:F2}", this.ship.attributes.EngineYaw.CurrentVelocity * 100);
            this.pitchVelocity.String = String.Format("Pitch : {0:F2}", this.ship.attributes.EnginePitch.CurrentVelocity * 100);
            this.rollVelocity.String = String.Format("Roll : {0:F2}", this.ship.attributes.EngineRoll.CurrentVelocity * 100);
            this.distance.String = String.Format("Distance : {0:F2}", this.ship.globalPosition.Length());

            this.velocity.Draw(spritBatch);
            this.yawVelocity.Draw(spritBatch);
            this.pitchVelocity.Draw(spritBatch);
            this.rollVelocity.Draw(spritBatch);
            this.distance.Draw(spritBatch);
        }

    }

}
