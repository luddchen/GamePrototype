using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Container : HUD2DContainer {

        public List<AI_Item> aiItems;

        public List<AI_Connection> aiConnections;

        private AI_Item moveItem;
        private Vector2 moveOffset;

        private AI_Connection moveConnection;
        private AI_ItemPort movePort;

        private List<AI_Bank> aiBanks;

        public List<HUD2D> removeList;


        public AI_Container()
            : base( new Vector2( 0, 0 ), HUDType.RELATIV ) {
            this.removeList = new List<HUD2D>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();

            this.aiBanks = new List<AI_Bank>();
            for ( int i = 0; i < 5; i++ ) {
                this.aiBanks.Add( new AI_Bank( new Vector2( 0.41f, 0.1f + 0.2f * i ), HUDType.RELATIV, new Vector2( 0.8f, 120 ), HUDType.RELATIV_ABSOLUT ) );
            }
            foreach ( AI_Bank bank in this.aiBanks ) {
                this.Add( bank );
                bank.setLayerDepth( this.layerDepth );
            }

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
                foreach ( AI_Bank bank in this.aiBanks ) {
                    bank.Remove( element );
                }
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

            foreach ( AI_Bank bank in this.aiBanks ) {
                bank.background.color = new Color( 32, 32, 32, 32 );
            }

            if ( this.moveItem == null ) {
                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                    // check banks
                    foreach ( AI_Bank bank in this.aiBanks ) {
                        foreach ( HUD2D item in bank.allChilds ) {
                            if ( item is AI_Item ) {
                                if ( ( (AI_Item)item ).typeString.Intersects( Antares.inputProvider.getMousePos() ) ) {
                                    this.moveItem = (AI_Item)item;
                                    this.moveOffset = item.position - Antares.inputProvider.getMousePos();
                                    bank.Remove( item );
                                    base.Add( item );
                                    break;
                                }
                            }
                        }
                    }
                    // if not in a bank check this
                    if ( this.moveItem == null ) {
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
                }
            } else {
                bool enablePlacing = false;
                foreach ( AI_Bank bank in this.aiBanks ) {
                    if ( bank.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        if ( bank.hasFreePlace( this.moveItem ) ) {
                            bank.background.color = new Color( 32, 64, 32, 32 );
                            enablePlacing = true;
                        } else {
                            bank.background.color = new Color( 64, 32, 32, 32 );
                        }
                    } else {
                        bank.background.color = new Color( 32, 32, 32, 32 );
                    }
                }
                if ( Antares.inputProvider.isLeftMouseButtonDown() || !(enablePlacing)) {
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
                    foreach ( AI_Bank bank in this.aiBanks ) {
                        if ( bank.Intersects( Antares.inputProvider.getMousePos() + this.moveOffset )) {
                            ( (AI_Container)this.moveItem.parent ).Remove( this.moveItem );
                            bank.Add( this.moveItem );
                            if ( bank.allChilds.Contains( this.moveItem ) ) {
                                this.aiItems.Add( this.moveItem );
                                this.moveItem = null;
                            } else {
                                this.Add( this.moveItem );
                            }
                            break;
                        }
                    }
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
