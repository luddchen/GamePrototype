using HUD.HUD;
using Battlestation_Antares.View.HUD;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Model;

namespace Battlestation_Antaris.View.HUD {

    public class MiniMapRenderer : HUDRenderedItem {

        public class Config {
            public Vector2 abstractPosition;
            public Vector2 abstractSize;
            public MiniMap.Config mapConfig;

            public Config( Vector2 abstractPosition, Vector2 abstractSize, MiniMap.Config mapConfig ) {
                this.abstractPosition = abstractPosition;
                this.abstractSize = abstractSize;
                this.mapConfig = mapConfig;
            }
        }


        private MiniMapRenderer.Config oldConfig;

        public MiniMap miniMap {
            get;
            private set;
        }

        public MiniMapRenderer( MiniMap item ) : base( item, new Point( 512, 512 ), MiniMap.BACKGROUND_COLOR ) {
            this.miniMap = item;
        }


        public void changeConfig( MiniMapRenderer.Config config ) {
            if (oldConfig != null) {
                oldConfig.abstractPosition = this.AbstractPosition;
                oldConfig.abstractSize = this.AbstractSize;
            }

            oldConfig = config;
            this.AbstractPosition = config.abstractPosition;
            this.AbstractSize = config.abstractSize;

            this.miniMap.changeConfig( config.mapConfig );
        }

        

    }

}
