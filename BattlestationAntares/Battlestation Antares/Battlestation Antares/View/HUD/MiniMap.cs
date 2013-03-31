using System;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;
using Battlestation_Antaris.View.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD {

    class MiniMap : HUDMaskedContainer, IUpdatableItem {
        public class Config {
            public SpatialObjectOld centeredObject;
            public float iconPositionScale;

            public Config( float iconPositionScale, SpatialObjectOld centeredObject ) {
                this.iconPositionScale = iconPositionScale;
                this.centeredObject = centeredObject;
            }
        }

        public static Color ENEMY_COLOR = Color.Red;

        public static Color FRIEND_COLOR = Color.Blue;

        public static Color SPECIAL_COLOR = Color.Green;

        public static Color WEAPON_COLOR = Color.Yellow;

        public static float MIN_SCALE = 0.005f;

        public static float MAX_SCALE = 0.500f;

        public static Color BACKGROUND_COLOR = new Color( 16, 24, 24, 16 );

        public static Color BORDER_COLOR = new Color( 16, 16, 16, 8 );

        public static Color BORDER_COLOR_HOVER = new Color( 32, 32, 32, 32 );


        public SpatialObjectOld centeredObject;

        public Vector2 iconSize = new Vector2( 0.025f, 0.025f );

        public float iconPositionScale = 0.1f;

        public MiniMapRenderer renderer;

        private MiniMap.Config oldConfig;

        public MiniMap() : base( new Vector2( 0.5f, 0.5f ), new Vector2( 1f, 1f ) ) {
            SetMask( "Sprites//Square_Cross", MiniMap.BORDER_COLOR );
        }


        public override void Draw( SpriteBatch spritBatch ) {

            foreach ( HUD_Item element in this.AllChilds ) {
                if ( element is MiniMapIcon ) {
                    MiniMapIcon icon = ( (MiniMapIcon)element );
                    icon.Update( centeredObject );

                    if ( Math.Abs( icon.AbstractPosition.X ) < this.AbstractSize.X / 2 &&
                        Math.Abs( icon.AbstractPosition.Y ) < this.AbstractSize.Y / 2 &&
                        ( icon.spatialObject.isVisible || icon.spatialObject is SpaceShip ) ) {
                        icon.IsVisible = true;
                    } else {
                        icon.IsVisible = false;
                    }
                }
            }

            base.Draw( spritBatch );
        }


        public void ZoomOnMouseWheelOver() {
            if ( this.renderer != null ) {
                if ( this.renderer.Intersects( HUDService.Input.getMousePos() ) ) {
                    SetMask( MiniMap.BORDER_COLOR_HOVER );

                    int wheelChange = HUDService.Input.getMouseWheelChange();

                    if ( wheelChange != 0 ) {
                        if ( wheelChange > 0 ) {
                            this.iconPositionScale *= 1.2f;
                            if ( this.iconPositionScale > MiniMap.MAX_SCALE ) {
                                this.iconPositionScale = MiniMap.MAX_SCALE;
                            }
                        } else {
                            this.iconPositionScale /= 1.2f;
                            if ( this.iconPositionScale < MiniMap.MIN_SCALE ) {
                                this.iconPositionScale = MiniMap.MIN_SCALE;
                            }
                        }
                    }
                } else {
                    SetMask( MiniMap.BORDER_COLOR );
                }
            }
        }

        public void changeConfig( MiniMap.Config config ) {
            if ( oldConfig != null ) {
                oldConfig.iconPositionScale = this.iconPositionScale;
            }
            oldConfig = config;
            this.iconPositionScale = config.iconPositionScale;
            this.centeredObject = config.centeredObject;

        }

        public Vector2 screenToMiniMapCoord( Vector2 screenCoord ) {
            return screenCoord - this.Position;
        }

        public Vector3 miniMapToWorldCoord( Vector2 miniMapCoord ) {
            Vector3 worldCoord = new Vector3();
            worldCoord.X = miniMapCoord.X / this.iconPositionScale;
            worldCoord.Y = 0f;
            worldCoord.Z = miniMapCoord.Y / this.iconPositionScale;
            return worldCoord;
        }


        public void Update( GameTime gameTime ) {
            ZoomOnMouseWheelOver();
        }

        public bool Enabled {
            get {
                return true;
            }
        }
    }

}