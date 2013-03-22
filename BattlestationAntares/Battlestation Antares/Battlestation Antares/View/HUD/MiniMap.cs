using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;

namespace Battlestation_Antares.View.HUD {

    public class MiniMap : HUDContainer {
        public class Config {
            public SpatialObject centeredObject;
            public Vector2 abstractPosition;
            public Vector2 bgAbstractSize;
            public Vector2 fgAbstractSize;
            public float iconPositionScale;

            public Config( Vector2 abstractPosition, Vector2 bgAbstractSize, Vector2 fgAbstractSize, SpatialObject centeredObject ) {
                this.abstractPosition = abstractPosition;
                this.bgAbstractSize = bgAbstractSize;
                this.fgAbstractSize = fgAbstractSize;
                iconPositionScale = 0.08f;
                this.centeredObject = centeredObject;
            }
        }

        public static Color ENEMY_COLOR = Color.Red;

        public static Color FRIEND_COLOR = Color.Blue;

        public static Color SPECIAL_COLOR = Color.Green;

        public static Color WEAPON_COLOR = Color.Yellow;

        public static float MIN_SCALE = 0.005f;

        public static float MAX_SCALE = 0.500f;

        public static Color BACKGROUND_COLOR = new Color( 16, 24, 24, 255 );

        public static Color BORDER_COLOR = new Color( 16, 16, 16, 8 );

        public static Color BORDER_COLOR_HOVER = new Color( 32, 32, 32, 32 );

        private HUDTexture background;

        private HUDTexture foreground;

        private SpatialObject centeredObject;

        public Vector2 iconSize = new Vector2( 15, 15 );

        public float iconPositionScale = 0.1f;

        private MiniMap.Config oldConfig;

        public MiniMap( Vector2 abstractPosition, HUDType positionType )
            : base( abstractPosition, positionType ) {
            this.background = new HUDTexture();
            this.background.color = MiniMap.BACKGROUND_COLOR;
            this.background.abstractSize = new Vector2( 0.25f, 0.4f );
            this.background.sizeType = HUDType.RELATIV;
            this.background.Texture = Antares.content.Load<Texture2D>( "Sprites//Square_Cross" );

            this.foreground = new HUDTexture();
            this.foreground.color = MiniMap.BORDER_COLOR;
            this.foreground.abstractSize = new Vector2( 0.25f, 0.4f );
            this.foreground.sizeType = HUDType.RELATIV;
            this.foreground.Texture = Antares.content.Load<Texture2D>( "Sprites//SquareBorder" );

            Add( this.background );
            Add( this.foreground );

            this.background.layerDepth = this.layerDepth;
            this.foreground.layerDepth = this.layerDepth - 0.01f;
        }


        public override void setLayerDepth( float layerDepth ) {
            base.setLayerDepth( layerDepth );
            this.background.layerDepth = this.layerDepth;
        }


        public override void Draw( SpriteBatch spritBatch ) {
            Vector2 backgroundSize = ( this.background.size - this.iconSize ) / 2;

            foreach ( HUD_Item element in this.allChilds ) {
                if ( element is MiniMapIcon ) {
                    MiniMapIcon icon = ( (MiniMapIcon)element );
                    icon.Update( centeredObject );

                    if ( Math.Abs( icon.abstractPosition.X ) < backgroundSize.X &&
                        Math.Abs( icon.abstractPosition.Y ) < backgroundSize.Y &&
                        ( icon.spatialObject.isVisible || icon.spatialObject is SpaceShip ) ) {
                        icon.IsVisible = true;
                    } else {
                        icon.IsVisible = false;
                    }
                }
            }

            ClientSizeChanged();

            base.Draw( spritBatch );
        }


        public void ZoomOnMouseWheelOver() {
            if ( this.background.Intersects( Antares.inputProvider.getMousePos() ) ) {
                this.foreground.color = MiniMap.BORDER_COLOR_HOVER;

                int wheelChange = Antares.inputProvider.getMouseWheelChange();

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
                this.foreground.color = MiniMap.BORDER_COLOR;
            }
        }

        public void changeConfig( MiniMap.Config config ) {
            if ( oldConfig != null ) {
                oldConfig.iconPositionScale = this.iconPositionScale;
            }
            oldConfig = config;
            this.abstractPosition = config.abstractPosition;
            this.background.abstractSize = config.bgAbstractSize;
            this.foreground.abstractSize = config.fgAbstractSize;
            this.iconPositionScale = config.iconPositionScale;
            this.centeredObject = config.centeredObject;

        }

        public Vector2 screenToMiniMapCoord( Vector2 screenCoord ) {
            return screenCoord - this.position;
        }

        public Vector3 miniMapToWorldCoord( Vector2 miniMapCoord ) {
            Vector3 worldCoord = new Vector3();
            worldCoord.X = miniMapCoord.X / this.iconPositionScale;
            worldCoord.Y = 0f;
            worldCoord.Z = miniMapCoord.Y / this.iconPositionScale;
            return worldCoord;
        }

    }

}