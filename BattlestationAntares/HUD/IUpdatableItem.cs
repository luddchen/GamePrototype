using Microsoft.Xna.Framework;

namespace HUD {

    public interface IUpdatableItem {

        void Update( GameTime gameTime );

        bool Enabled {
            get;
        }

    }

}
