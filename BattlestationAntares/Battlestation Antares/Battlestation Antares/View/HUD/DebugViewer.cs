using Battlestation_Antares.Tools;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Battlestation_Antares.View.HUD {
    public class DebugViewer : HUDContainer {
        public Boolean activeDebugging;

        private List<DebugElement> debugElements;

        private List<HUDString> outputStrings;

        public DebugViewer() : base( new Vector2( 10f, 0.05f ), HUDType.ABSOLUT_RELATIV) {
            this.activeDebugging = false;
            this.debugElements = new List<DebugElement>();
            this.outputStrings = new List<HUDString>();
        }

        public void Add( DebugElement debugElement ) {
            HUDString newHUDStr = new HUDString( "", null, null, null, 0.9f, 0.0f);
            newHUDStr.PositionType = HUDType.ABSOLUT_RELATIV;
            newHUDStr.SizeType = HUDType.ABSOLUT_RELATIV;
            newHUDStr.AbstractSize = new Vector2( 1, 0.02f );
            this.debugElements.Add( debugElement );
            this.outputStrings.Add( newHUDStr );

            this.Add( newHUDStr );

        }

        public void Remove( DebugElement debugElement ) {
            this.debugElements.Remove( debugElement );
            this.outputStrings.RemoveAt( 0 );
        }

        public override void Draw( Microsoft.Xna.Framework.Graphics.SpriteBatch spritBatch ) {
            int nrActive = 0;
            DebugElement debugElement;
            HUDString outputStr;
            for ( int i = 0; i < debugElements.Count; i++ ) {
                debugElement = debugElements.ElementAt( i );
                outputStr = outputStrings.ElementAt( i );
                if ( debugElement.active ) {
                    nrActive++;
                }
                outputStr.Text = debugElement.getDebugString();
                outputStr.AbstractPosition = new Vector2( outputStr.Size.X / 2, 0.02f * nrActive );
            }
            base.Draw( spritBatch );
        }
    }
}
