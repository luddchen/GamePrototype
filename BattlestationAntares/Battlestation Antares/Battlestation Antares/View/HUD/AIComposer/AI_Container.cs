using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Container : HUD2DContainer {

        public List<AI_Item> aiItems;

        public List<AI_Connection> aiConnections;

        private AI_Item moveItem;
        private Vector2 moveOffset;

        private AI_Connection moveConnection;
        private AI_ItemPort movePort;

        public List<HUD2D> removeList;


        public AI_Container()
            : base( new Vector2( 0, 0 ), HUDType.RELATIV ) {
            this.removeList = new List<HUD2D>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();

            AI_Input ai_Item1 = new AI_Input( new Vector2( 0.4f, 0.3f ), HUDType.RELATIV);
            Add( ai_Item1 );

            AI_Output ai_Item2 = new AI_Output( new Vector2( 0.4f, 0.7f ), HUDType.RELATIV);
            Add( ai_Item2 );

            AI_Connection con1 = new AI_Connection();
            con1.setSource( ai_Item1.outputs[0] );
            con1.setTarget( ai_Item2.inputs[0] );

            Add( con1 );

        }


        public override void Add( HUD2D element ) {
            if ( element is AI_Item ) {
                ( (AI_Item)element ).container = this;
                this.aiItems.Add( (AI_Item)element );
            }
            base.Add( element );
        }


        public void Add( AI_Connection connection ) {
            this.aiConnections.Add( connection );
        }


        public override void Remove( HUD2D element ) {
            if ( element is AI_Item ) {
                this.aiItems.Remove( (AI_Item)element );
            }
            base.Remove( element );
        }


        public void DrawConnections() {
            Antares.primitiveBatch.Begin( PrimitiveType.LineList );

            foreach ( AI_Connection connection in this.aiConnections ) {
                connection.Draw( Antares.primitiveBatch );
            }

            Antares.primitiveBatch.End();
        }

        public void Update() {
            foreach ( HUD2D item in this.removeList ) {
                if ( item is AI_Connection ) {
                    this.aiConnections.Remove( (AI_Connection)item );
                } else {
                    Remove( item );
                }
            }
            this.removeList.Clear();

            if ( isMouseInBuildingBox() ) {
                if ( this.moveItem == null ) {
                    if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                        foreach ( HUD2D item in this.allChilds ) {
                            if ( item is AI_Item ) {
                                if ( ( (AI_Item)item ).typeString.Intersects( Antares.inputProvider.getMousePos() ) ) {
                                    this.moveItem = (AI_Item)item;
                                    this.moveOffset = item.position - Antares.inputProvider.getMousePos();
                                    break;
                                }
                            }
                        }
                    }
                } else {
                    if ( Antares.inputProvider.isLeftMouseButtonDown() ) {
                        this.moveItem.position = Antares.inputProvider.getMousePos() + this.moveOffset;
                        switch ( this.moveItem.positionType ) {
                            case HUDType.ABSOLUT:
                                this.moveItem.abstractPosition = this.moveItem.position;
                                break;
                            case HUDType.RELATIV:
                                this.moveItem.abstractPosition.X = this.moveItem.position.X / Antares.graphics.GraphicsDevice.Viewport.Width;
                                this.moveItem.abstractPosition.Y = this.moveItem.position.Y / Antares.graphics.GraphicsDevice.Viewport.Height;
                                break;
                        }
                        this.moveItem.ClientSizeChanged();
                    } else {
                        this.moveItem = null;
                    }
                }

                AI_ItemPort port = getMouseOverPort();
                if ( port != null ) {
                    if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {

                        if ( this.moveConnection != null ) {
                            if ( port.portType == this.movePort.portType ) {
                                if ( port.portType == AI_ItemPort.PortType.INPUT && port.connections.Count > 0 ) {
                                    AI_Connection old = port.connections[0];
                                    old.setSource( null );
                                    old.setTarget( null );
                                    this.aiConnections.Remove( old );
                                }

                                port.Add( this.moveConnection );
                                if ( this.moveConnection.getTarget() != null
                                    && this.moveConnection.getSource() != null
                                    && this.moveConnection.getTarget().item == this.moveConnection.getSource().item ) {
                                    this.moveConnection.setTarget( null );
                                    this.moveConnection.setSource( null );
                                    this.aiConnections.Remove( this.moveConnection );
                                }
                                Remove( this.movePort );
                                this.movePort = null;
                                this.moveConnection = null;
                            }
                        } else {
                            this.moveConnection = new AI_Connection();

                            AI_ItemPort.PortType portType = ( port.portType == AI_ItemPort.PortType.INPUT ) ? AI_ItemPort.PortType.OUTPUT : AI_ItemPort.PortType.INPUT;
                            this.movePort = new AI_ItemPort( Antares.inputProvider.getMousePos(), HUDType.ABSOLUT, portType );

                            port.Add( this.moveConnection );
                            this.movePort.Add( this.moveConnection );

                            Add( this.movePort );
                            this.aiConnections.Add( this.moveConnection );
                        }

                    }
                } else {
                    if ( this.moveConnection != null ) {
                        if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                            this.moveConnection.setTarget( null );
                            this.moveConnection.setSource( null );

                            this.aiConnections.Remove( this.moveConnection );
                            Remove( this.movePort );

                            this.moveConnection = null;
                            this.movePort = null;
                        }
                    }
                }

                if ( this.moveConnection != null ) {
                    this.movePort.position = Antares.inputProvider.getMousePos();
                }


                foreach ( AI_Connection con in this.aiConnections ) {
                    if ( con.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        con.color = con.colorHighlight;
                        if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                            con.Delete();
                            this.removeList.Add( con );
                        }
                    } else {
                        con.color = con.colorNormal;
                    }
                }

            }
        }


        private AI_ItemPort getMouseOverPort() {

            foreach ( AI_Item item in this.aiItems ) {
                foreach ( AI_ItemPort port in item.inputs ) {
                    if ( port.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        port.color = Color.Green;
                        return port;
                    } else {
                        port.color = Color.White;
                    }
                }

                foreach ( AI_ItemPort port in item.outputs ) {
                    if ( port.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        port.color = Color.Green;
                        return port;
                    } else {
                        port.color = Color.White;
                    }
                }
            }

            return null;
        }


        private bool isMouseInBuildingBox() {
            bool isWithin = true;
            Vector2 mousePos = Antares.inputProvider.getMousePos();
            if ( mousePos.X < Antares.graphics.GraphicsDevice.Viewport.Width * 0.05f
                || mousePos.X > Antares.graphics.GraphicsDevice.Viewport.Width * 0.8f
                || mousePos.Y < Antares.graphics.GraphicsDevice.Viewport.Height * 0.05f
                || mousePos.Y > Antares.graphics.GraphicsDevice.Viewport.Height * 0.95f ) {
                isWithin = false;
            }

            return isWithin;
        }


        public void ClearAI() {
            foreach ( AI_Connection c in this.aiConnections ) {
                c.Delete();
            }
            this.aiConnections.Clear();

            foreach ( AI_Item i in this.aiItems ) {
                this.allChilds.Remove( i );
            }
            this.aiItems.Clear();
        }

    }

}
