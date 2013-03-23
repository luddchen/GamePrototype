﻿using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;
using Battlestation_Antares.Control;
using Battlestation_Antaris;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Container : HUDContainer, IUpdatableItem {

        private enum BuilderState {
            NORMAL,
            ITEM,
            CONNECTION
        }

        private PrimitiveBatch primitiveBatch;

        public const int maxBanks = 6;

        public List<AI_Item> aiItems;

        public List<AI_Connection> aiConnections;

        private AI_Item moveItem;

        private AI_Connection moveConnection;
        private AI_ItemPort movePort;

        public List<AI_Bank> aiBanks;

        private HUDActionTexture mouseItemTex;

        private AI_Item insertItem;

        private AI_Bank insertBank;

        public List<HUD_Item> removeList;

        private SituationController controller;

        private BuilderState state;

        public AI_Container(SituationController controller)
            : base( new Vector2( 0, 0 ), HUDType.RELATIV ) {

            this.controller = controller;
            this.primitiveBatch = new PrimitiveBatch( Antares.graphics.GraphicsDevice );
            this.state = BuilderState.NORMAL;

            this.removeList = new List<HUD_Item>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();
            this.aiBanks = new List<AI_Bank>();

            this.insertBank = new AI_Bank( new Vector2( 0.9f, 0.1f ), HUDType.RELATIV, new Vector2( 210, 110 ), HUDType.ABSOLUT );
            this.insertBank.background.Texture = Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" );
            this.insertBank.background.color = new Color( 120, 128, 112);
            this.Add( this.insertBank );

            _addBanks(maxBanks);
            _initMouseTexture();
            _initButtons();

            if ( controller != null ) {
                controller.Register( this );
            }
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
            this.primitiveBatch.Begin( PrimitiveType.LineList );

            foreach ( AI_Connection connection in this.aiConnections ) {
                connection.Draw( this.primitiveBatch );
            }

            this.primitiveBatch.End();
        }

        public void Update( GameTime gameTime ) {
            // remove items if necessary 
            foreach ( HUD_Item item in this.removeList ) {
                if ( item is AI_Connection ) {
                    this.aiConnections.Remove( (AI_Connection)item );
                } else if ( item is AI_Item ) {
                    Remove( (AI_Item)item );
                } else {
                    Remove( item );
                }
            }
            this.removeList.Clear();
        }

        public void TempUpdate( GameTime gameTime ) {
            AI_ItemPort port = getMouseOverPort();
            if ( port != null ) {
                if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {

                    if ( this.moveConnection != null ) {
                        if ( port.portType == this.movePort.portType ) {
                            bool doIt = true;
                            int portItemBank = this.aiBanks.IndexOf( (AI_Bank)port.item.parent );
                            int endPortItemBank = 0;
                            if ( port.portType == AI_ItemPort.PortType.INPUT ) {
                                endPortItemBank = this.aiBanks.IndexOf( (AI_Bank)this.moveConnection.getSource().item.parent );
                                if ( portItemBank <= endPortItemBank ) {
                                    doIt = false;
                                }
                            } else {
                                endPortItemBank = this.aiBanks.IndexOf( (AI_Bank)this.moveConnection.getTarget().item.parent );
                                if ( portItemBank >= endPortItemBank ) {
                                    doIt = false;
                                }
                            }

                            if ( doIt ) {
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
                foreach ( AI_Bank bank in this.aiBanks ) {
                    bank.Remove( i );
                }
                this.insertBank.Remove( i );
            }
            this.aiItems.Clear();
        }

        private void _initButtons() {
            HUDArray addButtonArray = new HUDArray( new Vector2( 0.9f, 0.35f ), HUDType.RELATIV, new Vector2( 0.15f, 0.25f ), HUDType.RELATIV );
            addButtonArray.direction = LayoutDirection.VERTICAL;
            this.Add( addButtonArray );


            HUDButton addInputButton = new HUDButton( "Input", new Vector2(), 0.7f, this.controller );
            addInputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Input( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV, this.controller ) );
            } );
            addInputButton.style = ButtonStyle.BuilderButtonStyle();
            addInputButton.SetBackgroundTexture( "Sprites//builder_button" );
            addButtonArray.Add( addInputButton );


            HUDButton addTransformerButton = new HUDButton( "Transformer", new Vector2(), 0.7f, this.controller );
            addTransformerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Transformer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV, this.controller ) );
            } );
            addTransformerButton.style = ButtonStyle.BuilderButtonStyle();
            addTransformerButton.SetBackgroundTexture( "Sprites//builder_button" );
            addButtonArray.Add( addTransformerButton );


            HUDButton addMixerButton = new HUDButton( "Mixer", new Vector2(), 0.7f, this.controller );
            addMixerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Mixer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV, this.controller ) );
            } );
            addMixerButton.style = ButtonStyle.BuilderButtonStyle();
            addMixerButton.SetBackgroundTexture( "Sprites//builder_button" );
            addButtonArray.Add( addMixerButton );


            HUDButton addOutputButton = new HUDButton( "Output", new Vector2(), 0.7f, this.controller );
            addOutputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Output( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV, this.controller ) );
            } );
            addOutputButton.style = ButtonStyle.BuilderButtonStyle();
            addOutputButton.SetBackgroundTexture( "Sprites//builder_button" );
            addButtonArray.Add( addOutputButton );
        }

        private void _initMouseTexture() {
            this.mouseItemTex = new HUDActionTexture(
                delegate() {
                    this.mouseItemTex.abstractPosition = Antares.inputProvider.getMousePos();
                    this.mouseItemTex.ClientSizeChanged();
                },
                controller );
            this.mouseItemTex.positionType = HUDType.ABSOLUT;
            this.mouseItemTex.abstractSize = new Vector2( 200, 100 );
            this.mouseItemTex.sizeType = HUDType.ABSOLUT;
            this.mouseItemTex.color = AI_Bank.NORMAL_COLOR;
            this.Add( this.mouseItemTex );
            this.mouseItemTex.layerDepth = 0;
            this.mouseItemTex.IsVisible = false;
        }

        private void AddInsertItem(AI_Item item) {
            if ( this.insertItem != null ) {
                this.Remove( this.insertItem );
                this.controller.Unregister( this.insertItem );
                this.insertItem = null;
            }
            this.insertItem = item;
            this.Add( item );
            this.insertBank.Add( this.insertItem );
            item.dragAction = 
                delegate() {
                    DragItem( item );
                };
        }

        private void _addBanks(int count) {
            for ( int i = 0; i < count && i < maxBanks; i++ ) {
                AI_Bank newBank = new AI_Bank( new Vector2( 0.41f, 0.5f), HUDType.RELATIV, new Vector2( 0.8f, 110 ), HUDType.RELATIV_ABSOLUT );
                if ( this.controller != null ) {
                    this.controller.Register( newBank );
                }
                newBank.mouseOverAction = 
                    delegate() {
                        BankMouseOver( newBank );
                    };

                newBank.mousePressedAction =
                    delegate() {
                        BankMousePressed( newBank );
                    };

                this.aiBanks.Add( newBank );
                this.Add( newBank );
                newBank.setLayerDepth( this.layerDepth );

                foreach ( AI_Bank bank in this.aiBanks ) {
                    bank.abstractPosition.Y = this.aiBanks.IndexOf( bank ) * ( 1.0f / this.aiBanks.Count ) + ( 0.5f / this.aiBanks.Count );
                    bank.ClientSizeChanged();
                }
            }
        }

        public void DragItem( AI_Item item ) {
            if ( this.state == BuilderState.NORMAL ) {
                this.moveItem = item;
                this.mouseItemTex.IsVisible = true;
                this.state = BuilderState.ITEM;
            }
        }


        public void BankMouseOver( AI_Bank bank ) {
            if ( this.state == BuilderState.ITEM ) {
                if ( bank != (AI_Bank)this.moveItem.parent ) {
                    if ( bank.hasFreePlace( this.moveItem ) ) {
                        if ( ( (AI_Bank)this.moveItem.parent ) == this.insertBank ) {
                            this.insertItem = null;
                        }
                        bool insert = true;
                        int targetBankIndex = this.aiBanks.IndexOf( bank );
                        foreach ( AI_ItemPort itemPort in this.moveItem.inputs ) {
                            foreach ( AI_Connection con in itemPort.connections ) {
                                if ( this.aiBanks.IndexOf( (AI_Bank)con.getSource().item.parent ) >= targetBankIndex ) {
                                    insert = false;
                                }
                            }
                        }
                        foreach ( AI_ItemPort itemPort in this.moveItem.outputs ) {
                            foreach ( AI_Connection con in itemPort.connections ) {
                                if ( this.aiBanks.IndexOf( (AI_Bank)con.getTarget().item.parent ) <= targetBankIndex ) {
                                    insert = false;
                                }
                            }
                        }
                        if ( insert ) {
                            _switchBank( this.moveItem, bank );
                            bank.background.color = AI_Bank.ENABLED_COLOR;
                        } else {
                            bank.background.color = AI_Bank.DISABLED_COLOR;
                        }
                    } else {
                        bank.background.color = AI_Bank.DISABLED_COLOR;
                    }
                } else {
                    _switchBank( this.moveItem, bank );
                }
            }
        }


        public void BankMousePressed( AI_Bank bank ) {
            if ( this.state == BuilderState.ITEM ) {
                this.moveItem = null;
                this.mouseItemTex.IsVisible = false;
                this.state = BuilderState.NORMAL;
            }
        }


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            if ( this.primitiveBatch != null ) {
                this.primitiveBatch.ClientSizeChanged();
            }
        }


        private void _switchBank( AI_Item item, AI_Bank target ) {
            ( (AI_Bank)item.parent ).Remove( item );

            float targetXSize = target.abstractSize.X * Antares.RenderSize.X;
            float bankPos = ( Antares.inputProvider.getMousePos().X - ( target.position.X - targetXSize / 2 ) ) / targetXSize;
            target.InsertAt( item, bankPos );
        }


        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
