using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Battlestation_Antares.View.HUD;
using HUD.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares;
using Battlestation_Antares.Control;

namespace Battlestation_Antaris.View.HUD.CockpitHUD {

    class EngineControllVisualizer : HUDContainer {

        private HUDValueBar posYaw;
        private HUDValueBar negYaw;
        private HUDValueBar posPitch;
        private HUDValueBar negPitch;

        private HUDButton activateButton;
        private bool active = false;

        private Vector2 mouseCenter = new Vector2();

        private HUDTexture mouseTex;

        public EngineControllVisualizer( Vector2 position, Vector2 size, SituationController controller ) : base(position, size) {

            this.posYaw = new HUDValueBar( new Vector2( -size.X / 4, 0 ),
                                new Vector2( size.Y * 0.03f, size.X * 0.8f ), false );
            this.posYaw.AbstractRotation = (float)( Math.PI / 2 );
            this.posYaw.GetValue =
                delegate() {
                    return Math.Max( 0, Antares.world.spaceShip.attributes.EngineYaw.CurrentVelocity / Antares.world.spaceShip.attributes.EngineYaw.MaxVelocity );
                };
            this.posYaw.SetDiscreteBig();
            this.posYaw.SetMaxColor( Color.Yellow );

            this.negYaw = new HUDValueBar( new Vector2( size.X / 4, 0 ),
                    new Vector2( size.Y * 0.03f, size.X * 0.8f ), false );
            this.negYaw.AbstractRotation = -(float)( Math.PI / 2 );
            this.negYaw.GetValue =
                delegate() {
                    return -Math.Min( 0, Antares.world.spaceShip.attributes.EngineYaw.CurrentVelocity / Antares.world.spaceShip.attributes.EngineYaw.MaxVelocity );
                };
            this.negYaw.SetDiscreteBig();
            this.negYaw.SetMaxColor( Color.Yellow );

            this.posPitch = new HUDValueBar( new Vector2( 0, -size.Y / 4 ),
                    new Vector2( size.X * 0.05f, size.Y * 0.4f ), false );
            this.posPitch.GetValue =
                delegate() {
                    return Math.Max( 0, Antares.world.spaceShip.attributes.EnginePitch.CurrentVelocity / Antares.world.spaceShip.attributes.EnginePitch.MaxVelocity );
                };
            this.posPitch.SetDiscreteBig();
            this.posPitch.SetMaxColor( Color.Yellow );

            this.negPitch = new HUDValueBar( new Vector2( 0, size.Y / 4 ),
                    new Vector2( size.X * 0.05f, size.Y * 0.4f ), true );
            this.negPitch.GetValue =
                delegate() {
                    return -Math.Min( 0, Antares.world.spaceShip.attributes.EnginePitch.CurrentVelocity / Antares.world.spaceShip.attributes.EnginePitch.MaxVelocity );
                };
            this.negPitch.SetDiscreteBig();
            this.negPitch.SetMaxColor( Color.Yellow );

            this.Add( this.posYaw );
            this.Add( this.negYaw );
            this.Add( this.posPitch );
            this.Add( this.negPitch );

            this.activateButton = new HUDButton( " ", size: size, controller: controller );
            this.activateButton.style = AntaresButtonStyles.EngineControllerStyle();

            this.activateButton.SetPressedAction(
                delegate() {
                    if ( this.active ) {
                        this.activateButton.style = AntaresButtonStyles.EngineControllerStyle();
                    } else {
                        this.mouseCenter = this.Position;
                        Antares.inputProvider.setMousePos( this.mouseCenter );
                        this.activateButton.style = AntaresButtonStyles.EngineControllerStyle2();
                    }
                    this.activateButton.Toggle();
                    this.active = !this.active;
                    this.mouseTex.IsVisible = this.active;
                    controller.SetMouseVisibility( !this.active );
                });

            this.Add( this.activateButton );
            this.activateButton.LayerDepth = this.LayerDepth;

            this.mouseTex = new HUDTexture( "Sprites//HUD//LampLight", Color.Red, size: size * 0.1f );
            this.mouseTex.PositionType = HUDType.ABSOLUT;
            this.mouseTex.IsVisible = false;
            this.Add( this.mouseTex );
            this.mouseTex.LayerDepth = 0.1f;
        }

        public bool isActive() {
            return this.active;
        }

        public void use() {
            Vector2 currentMousePos = Antares.inputProvider.getMousePos() - mouseCenter;
            float length = currentMousePos.Length();
            if ( length > 150 ) {
                currentMousePos = currentMousePos * 150.0f / length;
                Antares.inputProvider.setMousePos( mouseCenter + currentMousePos );
            }
            this.mouseTex.AbstractPosition = currentMousePos;
            currentMousePos /= 150.0f;
            Antares.world.spaceShip.attributes.EngineYaw.TargetVelocity( -currentMousePos.X * Math.Abs( currentMousePos.X ) );
            Antares.world.spaceShip.attributes.EnginePitch.TargetVelocity( -currentMousePos.Y * Math.Abs( currentMousePos.Y ) );
        }

    }
}
