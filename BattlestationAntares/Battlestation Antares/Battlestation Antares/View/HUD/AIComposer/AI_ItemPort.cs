using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Battlestation_Antares.Control;
using HUD.HUD;
using HUD;
using Microsoft.Xna.Framework.Graphics;

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

        public AI_ItemPort( PortType portType, SituationController controller) : base(null, controller) {
            this.portType = portType;
            this.connections = new List<AI_Connection>();
            this.AbstractSize = Vector2.Multiply( AI_Item.AI_ITEM_SIZE, new Vector2( 0.075f, 0.133f ) ) * 3f;
            this.Texture = HUDService.Content.Load<Texture2D>( "Sprites//HUD//Lamp2" );
        }

        public AI_ItemPort( PortType portType, AI_Item item, SituationController controller ) : this(portType, controller) {
            this.item = item;
            this.Action =
                delegate() {
                    if ( this.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        if ( Antares.inputProvider.isLeftMouseButtonPressed() 
                            && this.Parent != null 
                            && this.Parent is AI_Item 
                            && ((AI_Item)this.Parent).container != null) 
                        {
                            ( (AI_Item)this.Parent ).container.PortPressed( this );
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
