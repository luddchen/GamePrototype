using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace HUD {

    public class MouseProvider : IMouseProvider {

        /// <summary>
        /// the old mouse state
        /// </summary>
        private MouseState oldMouseState = Mouse.GetState();

        /// <summary>
        /// the new mouse state
        /// </summary>
        private MouseState newMouseState = Mouse.GetState();

        private Matrix mouseTransform = Matrix.Identity;
        private Matrix mouseTransformInv = Matrix.Identity;


        /// <summary>
        /// update the input provider
        /// </summary>
        public virtual void Update() {
            this.oldMouseState = this.newMouseState;
            this.newMouseState = Mouse.GetState();
        }


        public bool isLeftMouseButtonPressed() {
            return ( newMouseState.LeftButton == ButtonState.Pressed && oldMouseState.LeftButton == ButtonState.Released );
        }

        public bool isRightMouseButtonPressed() {
            return ( newMouseState.RightButton == ButtonState.Pressed && oldMouseState.RightButton == ButtonState.Released );
        }

        public bool isLeftMouseButtonDown() {
            return newMouseState.LeftButton == ButtonState.Pressed;
        }

        public bool isRightMouseButtonDown() {
            return newMouseState.RightButton == ButtonState.Pressed;
        }

        public Vector2 getMousePos() {
            return Vector2.Transform( new Vector2( newMouseState.X, newMouseState.Y ), this.mouseTransform );
        }

        public void setMousePos( Vector2 position ) {
            Vector2 newPos = Vector2.Transform( position, this.mouseTransformInv );
            Mouse.SetPosition( (int)newPos.X, (int)newPos.Y );
        }

        public bool isMouseMoved() {
            return ( oldMouseState.X == newMouseState.X && oldMouseState.Y == newMouseState.Y ) ? false : true;
        }

        public int getMouseWheelChange() {
            return ( newMouseState.ScrollWheelValue - oldMouseState.ScrollWheelValue );
        }

        public void setMouseTransform( Vector2 screenSizeHalf, Vector2 renderSizeHalf, float renderScale ) {
            this.mouseTransform = 
                Matrix.CreateTranslation( new Vector3( -screenSizeHalf, 0 ) )
                * Matrix.CreateScale( 1.0f / renderScale )
                * Matrix.CreateTranslation( new Vector3( renderSizeHalf, 0 ) );
            this.mouseTransformInv = Matrix.Invert( this.mouseTransform );
        }

        public void setMouseTransform( Matrix transform ) {
            this.mouseTransform = transform;
            this.mouseTransformInv = Matrix.Invert( this.mouseTransform );
        }

    }

}
