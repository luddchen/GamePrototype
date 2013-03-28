using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Connection : HUD_Item, IUpdatableItem {

        private Vector2 start;
        private Vector2 end;
        private Vector2 preStart;
        private Vector2 postEnd;
        private Vector2 center;
        private Vector2 centerTangent;
        private float dist;

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
                start = this.source.Position;
                end = this.target.Position;
                preStart = Vector2.UnitY;
                postEnd = Vector2.UnitY;
                center = ( end + start ) * 0.5f;
                centerTangent = end - start;
                dist = Vector2.Distance( start, end );

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

            float step = (float)(this.width) / this.dist;
            for ( float i = 0.1f; i <= 1.0f; i += step ) {
                Vector2 p = Vector2.Hermite( start, preStart, center, centerTangent, i );
                if ( Intersects( p, point ) ) {
                    return true;
                }
            }
            for ( float i = 0.0f; i <= 0.9f; i += step ) {
                Vector2 p = Vector2.Hermite( center, centerTangent, end, postEnd, i );
                if ( Intersects( p, point ) ) {
                    return true;
                }
            }

            return false;
        }

        private bool Intersects( Vector2 pos1, Vector2 pos2 ) {
            return ( Math.Abs( pos1.X - pos2.X ) < this.width && Math.Abs( pos1.Y - pos2.Y ) < this.width );
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
