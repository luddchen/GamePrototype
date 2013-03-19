﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Container : HUD2DContainer {

        public List<AI_Item> aiItems;

        public List<AI_Connection> aiConnections;

        private AI_Item moveItem;

        private AI_Connection moveConnection;
        private AI_ItemPort movePort;

        private List<AI_Bank> aiBanks;

        private HUD2DTexture mouseItemTex;

        private AI_Item insertItem;

        private AI_Bank insertBank;

        public List<HUD2D> removeList;


        public AI_Container()
            : base( new Vector2( 0, 0 ), HUDType.RELATIV ) {
            this.removeList = new List<HUD2D>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();

            this.insertBank = new AI_Bank( new Vector2( 0.9f, 0.1f ), HUDType.RELATIV, new Vector2( 220, 120 ), HUDType.ABSOLUT );
            this.Add( this.insertBank );

            this.aiBanks = new List<AI_Bank>();
            for ( int i = 0; i < 5; i++ ) {
                this.aiBanks.Add( new AI_Bank( new Vector2( 0.41f, 0.1f + 0.2f * i ), HUDType.RELATIV, new Vector2( 0.8f, 120 ), HUDType.RELATIV_ABSOLUT ) );
            }
            foreach ( AI_Bank bank in this.aiBanks ) {
                this.Add( bank );
                bank.setLayerDepth( this.layerDepth );
            }

            this.mouseItemTex = new HUD2DTexture();
            this.mouseItemTex.positionType = HUDType.ABSOLUT;
            this.mouseItemTex.abstractSize = new Vector2( 200, 100 );
            this.mouseItemTex.sizeType = HUDType.ABSOLUT;
            this.mouseItemTex.color = new Color( 32, 32, 32, 32 );
            this.Add( this.mouseItemTex );
            this.mouseItemTex.layerDepth = 0;
            this.mouseItemTex.isVisible = false;

            this.initButtons();
        }


        public void Add( AI_Item item ) {
            item.container = this;
            this.aiItems.Add( item );
        }


        public void Add( AI_Connection connection ) {
            this.aiConnections.Add( connection );
        }


        public void Remove( AI_Item item ) {
            this.aiItems.Remove( item );
            foreach ( AI_Bank bank in this.aiBanks ) {
                bank.Remove( item );
            }
            this.insertBank.Remove( item );
        }


        public void DrawConnections() {
            Antares.primitiveBatch.Begin( PrimitiveType.LineList );

            foreach ( AI_Connection connection in this.aiConnections ) {
                connection.Draw( Antares.primitiveBatch );
            }

            Antares.primitiveBatch.End();
        }

        public void Update() {
            // remove items if necessary 
            foreach ( HUD2D item in this.removeList ) {
                if ( item is AI_Connection ) {
                    this.aiConnections.Remove( (AI_Connection)item );
                } else if ( item is AI_Item ) {
                    Remove( (AI_Item)item );
                } else {
                    Remove( item );
                }
            }
            this.removeList.Clear();

            // reset bank background color
            foreach ( AI_Bank bank in this.aiBanks ) {
                bank.background.color = new Color( 32, 32, 32, 32 );
            }


            if ( this.moveItem == null ) {
                // drag item if existent
                this.mouseItemTex.isVisible = false;
                if ( Antares.inputProvider.isLeftMouseButtonDown()) {
                    foreach ( AI_Item item in this.aiItems ) {
                        if ( item.typeString.Intersects( Antares.inputProvider.getMousePos() ) ) {
                            this.moveItem = item;
                            break;
                        }
                    }
                }
            } else {
                this.mouseItemTex.isVisible = true;
                this.mouseItemTex.position = Antares.inputProvider.getMousePos();

                AI_Bank targetBank = null;
                foreach ( AI_Bank bank in this.aiBanks ) {
                    if ( bank.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        if ( bank.hasFreePlace( this.moveItem ) ) {
                            bank.background.color = new Color( 32, 64, 32, 32 );
                            targetBank = bank;
                        } else {
                            bank.background.color = new Color( 64, 32, 32, 32 );
                        }
                    }
                }

                if ( targetBank != null ) { //&& ((AI_Bank)this.moveItem.parent) != targetBank) {
                    if ( ( (AI_Bank)this.moveItem.parent ) == this.insertBank ) {
                        this.insertItem = null;
                    }
                    ( (AI_Bank)this.moveItem.parent ).Remove( this.moveItem );

                    float targetXSize = targetBank.abstractSize.X * Antares.graphics.GraphicsDevice.Viewport.Width;
                    float bankPos = ( Antares.inputProvider.getMousePos().X - ( targetBank.position.X - targetXSize / 2 ) ) / targetXSize;
                    targetBank.InsertAt( this.moveItem, bankPos );
                }

                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                    this.moveItem = null;
                    //Console.WriteLine( "AI Items : " + this.aiItems.Count );
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


        private void initButtons() {
            HUD2DArray addButtonArray = new HUD2DArray( new Vector2( 0.9f, 0.5f ), HUDType.RELATIV, new Vector2( 250, 300 ), HUDType.ABSOLUT );
            addButtonArray.direction = LayoutDirection.VERTICAL;
            this.Add( addButtonArray );


            HUD2DButton addInputButton = new HUD2DButton( "Input", new Vector2( 0.9f, 0.3f ), 0.7f );
            addInputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Input( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV ) );
            } );
            addInputButton.positionType = HUDType.RELATIV;
            addButtonArray.Add( addInputButton );


            HUD2DButton addTransformerButton = new HUD2DButton( "Transformer", new Vector2( 0.9f, 0.4f ), 0.7f );
            addTransformerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Transformer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV ) );
            } );
            addTransformerButton.positionType = HUDType.RELATIV;
            addButtonArray.Add( addTransformerButton );


            HUD2DButton addMixerButton = new HUD2DButton( "Mixer", new Vector2( 0.9f, 0.5f ), 0.7f );
            addMixerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Mixer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV ) );
            } );
            addMixerButton.positionType = HUDType.RELATIV;
            addButtonArray.Add( addMixerButton );


            HUD2DButton addOutputButton = new HUD2DButton( "Output", new Vector2( 0.9f, 0.6f ), 0.7f );
            addOutputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Output( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV ) );
            } );
            addOutputButton.positionType = HUDType.RELATIV;
            addButtonArray.Add( addOutputButton );
        }

        private void AddInsertItem(AI_Item item) {
            if ( this.insertItem != null ) {
                this.Remove( this.insertItem );
            }
            this.insertItem = item;
            this.Add( item );
            this.insertBank.Add( this.insertItem );
        }

    }

}
