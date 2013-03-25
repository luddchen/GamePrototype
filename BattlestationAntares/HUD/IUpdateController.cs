using Microsoft.Xna.Framework;

namespace HUD {

    public interface IUpdateController {

        void Register( IUpdatableItem item );

        void Unregister( IUpdatableItem item );

        void Update( GameTime gameTime );

    }

}
