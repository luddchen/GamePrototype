﻿using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD {

    /// <summary>
    /// a Head Up Display String
    /// </summary>
    public class HUDString : HUD_Item {

        /// <summary>
        /// the Font
        /// </summary>
        protected SpriteFont font;


        private String text;

        /// <summary>
        /// the displayed text
        /// </summary>
        public String Text {
            get {
                return this.text;
            }
            set {
                this.text = value;
                this.measureString = this.font.MeasureString( value );
            }
        }


        /// <summary>
        /// the width and height of the string (depends on font)
        /// </summary>
        protected Vector2 measureString;


        /// <summary>
        /// size of scaled string
        /// </summary>
        public override Vector2 Size {
            get {
                return this.measureString * this.scale;
            }
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        public HUDString() {
            this.font = Antares.content.Load<SpriteFont>( "Fonts\\Font" );
            this.Text = "HUD2DString";
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="text">text to display</param>
        public HUDString( String text) {
            this.font = Antares.content.Load<SpriteFont>( "Fonts\\Font" );
            this.Text = text;
        }


        /// <summary>
        /// creates a new HUD string
        /// </summary>
        /// <param name="text">text to display</param>
        /// <param name="font">font</param>
        /// <param name="position">position</param>
        /// <param name="color">color</param>
        /// <param name="scale">scale</param>
        /// <param name="rotation">rotation</param>
        public HUDString( String text, SpriteFont font, Vector2? position, Color? color, float? scale, float? rotation) {
            if ( font == null ) {
                this.font = Antares.content.Load<SpriteFont>( "Fonts\\Font" );
            }
            if ( font != null ) {
                this.font = font;
            }

            if ( text == null ) {
                this.Text = " ";
            }
            if ( text != null ) {
                this.Text = text;
            }

            this.AbstractPosition = position ?? Vector2.Zero;
            this.color = color ?? Color.Beige;
            this.scale = scale ?? 1.0f;
            this.rotation = rotation ?? 0.0f;
        }


        /// <summary>
        /// draw this element
        /// </summary>
        /// <param name="spriteBatch">the spritebatch</param>
        public override void Draw( SpriteBatch spriteBatch ) {
            if ( isVisible ) {
                spriteBatch.DrawString( this.font, this.Text, this.Position,
                                        this.color, -this.rotation, this.measureString / 2,
                                        this.scale, this.effect, this.LayerDepth );
            }
        }


        /// <summary>
        /// testing intersection with point
        /// </summary>
        /// <param name="point">the test point</param>
        /// <returns>true if there is an intersetion</returns>
        public override bool Intersects( Vector2 point ) {
            return ( Math.Abs( Position.X - point.X ) < Size.X / 2 && Math.Abs( Position.Y - point.Y ) < Size.Y / 2 );
        }


    }
}