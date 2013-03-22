using Microsoft.Xna.Framework;

namespace Battlestation_Antaris {

    public interface IUpdatableItem {

        void Update( GameTime gameTime );

        bool Enabled {
            get;
        }

    }

}
