using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;
using HUD.HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_ItemPort : HUDActionTexture {
        public enum PortType {
            INPUT,
            OUTPUT
        }

        public static PortType Inverse( PortType type ) {
            return ( type == PortType.INPUT ) ? PortType.OUTPUT : PortType.INPUT;
        }

        public Color normalColor = Color.White;

        public Color hoverColor = Color.Green;

        public PortType portType;

        public AI_Item item;

        public List<AI_Connection> connections;

        public AI_ItemPort( Vector2 abstractPosition, HUDType positionType, PortType portType, SituationController controller) : base(null, controller) {
            this.portType = portType;
            this.connections = new List<AI_Connection>();
        }

        public AI_ItemPort( Vector2 abstractPosition, HUDType positionType, PortType portType, AI_Item item, SituationController controller ) : base(null, controller) {
            this.portType = portType;
            this.item = item;
            this.connections = new List<AI_Connection>();
            this.Action =
                delegate() {
                    if ( this.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        if ( Antares.inputProvider.isLeftMouseButtonPressed() 
                            && this.parent != null 
                            && this.parent is AI_Item 
                            && ((AI_Item)this.parent).container != null) 
                        {
                            ( (AI_Item)this.parent ).container.PortPressed( this );
                        }
                        this.color = this.hoverColor;
                    } else {
                        this.color = this.normalColor;
                    }
                };
        }

        public void Add( AI_Connection c ) {
            if ( !( this.portType == PortType.INPUT && this.connections.Count > 0 ) ) {
                switch ( this.portType ) {
                    case PortType.INPUT:
                        c.setTarget( this );
                        break;
                    case PortType.OUTPUT:
                        c.setSource( this );
                        break;
                }
            }
        }


        public void Remove( AI_Connection c ) {
            if ( this.connections.Contains( c ) ) {
                switch ( this.portType ) {
                    case PortType.INPUT:
                        c.setTarget( null );
                        break;
                    case PortType.OUTPUT:
                        c.setSource( null );
                        break;
                }
            }
        }

    }

}
