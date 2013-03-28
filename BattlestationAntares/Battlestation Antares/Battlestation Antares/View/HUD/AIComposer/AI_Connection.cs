using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Connection : HUD_Item, IUpdatableItem {

        AI_ItemPort source;

        AI_ItemPort target;

        public Action action;

        public int width = 3;

        public Color colorNormal = Color.Yellow;
        public Color color2Normal = Color.Blue;

        public Color colorHighlight = Color.Red;
        public Color color2Highlight = Color.HotPink;

        public Color color2;

        public AI_Connection() {
            this.color = this.colorNormal;
            this.color2 = this.color2Normal;
        }


        public void setSource( AI_ItemPort source ) {
            if ( this.source != null ) {
                this.source.connections.Remove( this );
            }
            this.source = source;
            if ( source != null ) {
                this.source.connections.Add( this );
            }
        }

        public void setTarget( AI_ItemPort target ) {
            if ( this.target != null ) {
                this.target.connections.Remove( this );
            }
            this.target = target;
            if ( target != null ) {
                this.target.connections.Add( this );
            }
        }

        public AI_ItemPort getSource() {
            return this.source;
        }

        public AI_ItemPort getTarget() {
            return this.target;
        }

        public void Delete() {
            if ( this.source != null ) {
                this.source.connections.Remove( this );
                this.source = null;
            }

            if ( this.target != null ) {
                this.target.connections.Remove( this );
                this.target = null;
            }
        }


        public override void Draw( SpriteBatch spritBatch ) {
        }


        public void Draw( PrimitiveBatch primitiveBatch ) {
            if ( this.source != null && this.target != null ) {
                Vector2 start = this.source.Position;
                Vector2 end = this.target.Position;

                Vector2 preStart = Vector2.UnitY;
                Vector2 postEnd = Vector2.UnitY;

                float dist = Vector2.Distance( start, end );
                Vector2 center = ( end + start ) * 0.5f;
                Vector2 centerTangent = end - start;

                if ( start.X == end.X ) {
                    center.X += dist * 0.5f;
                    centerTangent.X = 0;
                    preStart *= dist * 0.4f;
                    postEnd *= dist * 0.4f;
                } else {
                    centerTangent.Y = 0;
                    float f = Math.Max( Math.Abs( start.X - end.X ) / dist, 0.1f);
                    preStart *= dist * 0.33f / f;
                    postEnd *= dist * 0.33f / f;
                }
                if ( this.width > 1 ) {
                    for ( Vector2 pos = new Vector2( -this.width / 2, 0 ); pos.X <= ( this.width / 2 ); pos.X += 1.0f ) {
                        DrawLinePart( primitiveBatch, preStart, start + pos, center + pos, centerTangent, dist, 0.0f, 1.0f, 0.5f, 0.0f );
                        DrawLinePart( primitiveBatch, centerTangent, center + pos, end + pos, postEnd, dist, 0.0f, 1.0f, 0.5f, 0.5f );
                    }
                } else {
                    DrawLinePart( primitiveBatch, preStart, start, center, centerTangent, dist, 0.0f, 1.0f, 0.5f, 0.0f );
                    DrawLinePart( primitiveBatch, centerTangent, center, end, postEnd, dist, 0.0f, 1.0f, 0.5f, 0.5f );
                }
            }
        }

        private void DrawLinePart( PrimitiveBatch primitiveBatch, Vector2 p0, Vector2 p1, Vector2 p2, Vector2 p3, float dist, float min, float max, float lerpMul, float lerpOffset ) {
            if ( dist > 10 ) {
                DrawLinePart( primitiveBatch, p0, p1, p2, p3, dist * 0.5f, min, ( min + max ) * 0.5f, lerpMul, lerpOffset );
                DrawLinePart( primitiveBatch, p0, p1, p2, p3, dist * 0.5f, ( min + max ) * 0.5f, max, lerpMul, lerpOffset );
            } else {
                primitiveBatch.AddVertex( Vector2.Hermite( p1, p0, p2, p3, min ), Color.Lerp( this.color, this.color2, min * lerpMul + lerpOffset ) );
                primitiveBatch.AddVertex( Vector2.Hermite( p1, p0, p2, p3, max ), Color.Lerp( this.color, this.color2, max * lerpMul + lerpOffset) );
            }
        }


        public override bool Intersects( Vector2 point ) {
            if ( this.source == null || this.target == null ) {
                return false;
            }
            Vector2 lineVec = this.target.Position - this.source.Position; // Vector source -> target
            Vector2 pointVec = point - this.source.Position; // Vector source -> point

            float xOff = pointVec.X / lineVec.X;
            float yOff = pointVec.Y / lineVec.Y;

            if ( Math.Abs( lineVec.X ) < 0.01f ) {
                if ( Math.Abs( pointVec.X ) < 1f && yOff > 0.05f && yOff < 0.95f ) {
                    return true;
                }
            }

            if ( xOff > 0.05f && xOff < 0.95f && yOff > 0.05f && yOff < 0.95f ) {
                if ( Math.Abs( xOff - yOff ) < 0.2f ) {
                    return true;
                }
            }

            return false;
        }


        public void Update( GameTime gameTime ) {
            if ( this.action != null ) {
                this.action();
            }
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
