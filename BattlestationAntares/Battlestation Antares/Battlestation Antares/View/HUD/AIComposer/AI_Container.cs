using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;
using Battlestation_Antares.Control;
using HUD.HUD;
using HUD;
using System;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Container : HUDContainer, IUpdatableItem {

        private enum BuilderState {
            NORMAL,
            ITEM,
            CONNECTION
        }

        public static Vector2 basePosition = new Vector2( 0.41f, 0.5f );

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
            : base( basePosition, basePosition * 2 ) {

            this.controller = controller;
            this.primitiveBatch = new PrimitiveBatch( HUDService.Device );
            this.state = BuilderState.NORMAL;

            SetBackground( "Sprites//builder_bg_temp", new Color( 120, 128, 112 ) );

            this.removeList = new List<HUD_Item>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();
            this.aiBanks = new List<AI_Bank>();

            this.insertBank = new AI_Bank( new Vector2( 0.9f, 0.1f ), AI_Item.AI_ITEM_SIZE * 1.2f );
            this.insertBank.SetBackground( "Sprites//builder_bg_temp", new Color( 120, 128, 112 ) );
            this.insertBank.LayerDepth = 0.3f;
            this.controller.view.Add( this.insertBank );

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
            this.controller.Register( item );
        }


        public void Add( AI_Connection connection ) {
            this.aiConnections.Add( connection );
            connection.action =
                delegate() {
                    if ( connection.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        connection.color = connection.colorHighlight;
                        if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                            connection.Delete();
                            this.removeList.Add( connection );
                        }
                    } else {
                        connection.color = connection.colorNormal;
                    }
                };
            this.controller.Register( connection );
        }


        public void Remove( AI_Item item ) {
            this.aiItems.Remove( item );
            foreach ( AI_Bank bank in this.aiBanks ) {
                bank.Remove( item );
            }
            this.insertBank.Remove( item );
            this.controller.Unregister( item );
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
                    this.controller.Unregister( (AI_Connection)item );
                } else if ( item is AI_Item ) {
                    Remove( (AI_Item)item );
                } else {
                    Remove( item );
                }
            }
            this.removeList.Clear();

            int zoomValue = HUDService.Input.getMouseWheelChange();
            if ( zoomValue > 0 ) {
                this.AbstractScale *= 1.05f;
                if ( this.AbstractScale > 2.0f ) {
                    this.AbstractScale = 2.0f;
                }
            }
            if ( zoomValue < 0 ) {
                this.AbstractScale *= 0.95f;
                this.AbstractPosition -= ( this.AbstractPosition - AI_Container.basePosition ) * 0.05f;
                if ( this.AbstractScale < 0.5f ) {
                    this.AbstractScale = 0.5f;
                }
            }

            if ( this.AbstractScale <= 1.0f ) {
                this.AbstractPosition = AI_Container.basePosition;
            } else {
                Vector2 mousePos = AI_Container.basePosition - HUD_Item.ConcreteToAbstract( HUDService.Input.getMousePos() );
                if ( Math.Abs( mousePos.X ) > AI_Container.basePosition.X * 0.75 && Math.Abs( mousePos.X ) < AI_Container.basePosition.X
                    || Math.Abs( mousePos.Y ) > AI_Container.basePosition.Y * 0.75 && Math.Abs( mousePos.Y ) < AI_Container.basePosition.Y ) {
                    // todo : stop scrolling at border
                    this.AbstractPosition += mousePos * this.Scale * 0.01f * mousePos.Length();
                }
            }
        }


        public void ClearAI() {
            foreach ( AI_Connection c in this.aiConnections ) {
                c.Delete();
                this.removeList.Add( c );
            }

            foreach ( AI_Item i in this.aiItems ) {
                this.removeList.Add( i );
            }
        }


        private void _initButtons() {
            HUDArray addButtonArray = new HUDArray( new Vector2( 0.9f, 0.35f ), new Vector2( 0.15f, 0.25f ) );
            addButtonArray.direction = LayoutDirection.VERTICAL;
            addButtonArray.borderSize = new Vector2( 0, 0.01f );
            addButtonArray.LayerDepth = 0.3f;
            this.controller.view.Add( addButtonArray );


            HUDButton addInputButton = new HUDButton( "Input", new Vector2(), 0.7f, this.controller );
            addInputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Input() );
            } );
            addInputButton.style = ButtonStyle.BuilderButtonStyle();
            addInputButton.SetBackground( "Sprites//builder_button" );
            addButtonArray.Add( addInputButton );


            HUDButton addTransformerButton = new HUDButton( "Transformer", new Vector2(), 0.7f, this.controller );
            addTransformerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Transformer() );
            } );
            addTransformerButton.style = ButtonStyle.BuilderButtonStyle();
            addTransformerButton.SetBackground( "Sprites//builder_button" );
            addButtonArray.Add( addTransformerButton );


            HUDButton addMixerButton = new HUDButton( "Mixer", new Vector2(), 0.7f, this.controller );
            addMixerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Mixer() );
            } );
            addMixerButton.style = ButtonStyle.BuilderButtonStyle();
            addMixerButton.SetBackground( "Sprites//builder_button" );
            addButtonArray.Add( addMixerButton );


            HUDButton addOutputButton = new HUDButton( "Output", new Vector2(), 0.7f, this.controller );
            addOutputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Output() );
            } );
            addOutputButton.style = ButtonStyle.BuilderButtonStyle();
            addOutputButton.SetBackground( "Sprites//builder_button" );
            addButtonArray.Add( addOutputButton );
        }


        private void _initMouseTexture() {
            this.mouseItemTex = new HUDActionTexture(
                delegate() {
                    this.mouseItemTex.AbstractPosition = HUD_Item.ConcreteToAbstract( Antares.inputProvider.getMousePos() );
                    this.mouseItemTex.AbstractScale = this.Scale;
                    this.mouseItemTex.RenderSizeChanged();
                },
                controller );
            this.mouseItemTex.AbstractSize = AI_Item.AI_ITEM_SIZE;
            this.mouseItemTex.color = AI_Bank.NORMAL_COLOR;
            this.controller.view.Add( this.mouseItemTex );
            this.mouseItemTex.LayerDepth = 0;
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
                AI_Bank newBank = new AI_Bank( Vector2.Zero,  new Vector2( 0.8f, AI_Item.AI_ITEM_SIZE.Y * 1.2f ) );
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

                foreach ( AI_Bank bank in this.aiBanks ) {
                    bank.AbstractPosition = 
                        new Vector2( 
                            bank.AbstractPosition.X, 
                            -this.AbstractSize.Y / 2.0f + this.aiBanks.IndexOf( bank ) * ( 1.0f / this.aiBanks.Count ) + ( 0.5f / this.aiBanks.Count ) );
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
                if ( bank != (AI_Bank)this.moveItem.Parent ) {
                    if ( bank.hasFreePlace( this.moveItem ) ) {
                        if ( ( (AI_Bank)this.moveItem.Parent ) == this.insertBank ) {
                            this.insertItem = null;
                        }
                        bool insert = true;
                        int targetBankIndex = this.aiBanks.IndexOf( bank );
                        foreach ( AI_ItemPort itemPort in this.moveItem.inputs ) {
                            foreach ( AI_Connection con in itemPort.connections ) {
                                if ( this.aiBanks.IndexOf( (AI_Bank)con.getSource().item.Parent ) >= targetBankIndex ) {
                                    insert = false;
                                }
                            }
                        }
                        foreach ( AI_ItemPort itemPort in this.moveItem.outputs ) {
                            foreach ( AI_Connection con in itemPort.connections ) {
                                if ( this.aiBanks.IndexOf( (AI_Bank)con.getTarget().item.Parent ) <= targetBankIndex ) {
                                    insert = false;
                                }
                            }
                        }
                        if ( insert ) {
                            _switchBank( this.moveItem, bank );
                            bank.SetBackground( AI_Bank.ENABLED_COLOR );
                        } else {
                            bank.SetBackground( AI_Bank.DISABLED_COLOR );
                        }
                    } else {
                        bank.SetBackground( AI_Bank.DISABLED_COLOR );
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


        public void PortPressed( AI_ItemPort port ) {
            switch ( this.state ) {
                case BuilderState.NORMAL :
                    _startConnection( port );
                    break;

                case BuilderState.CONNECTION :
                    _stopConnection( port );
                    break;
            }
        }


        private void _startConnection( AI_ItemPort port ) {
            this.state = BuilderState.CONNECTION;

            this.moveConnection = new AI_Connection();

            AI_ItemPort.PortType portType = AI_ItemPort.Inverse( port.portType );
            this.movePort = new AI_ItemPort( portType, this.controller );
            this.movePort.Action =
                delegate() {
                    this.movePort.AbstractPosition = HUD_Item.ConcreteToAbstract( Antares.inputProvider.getMousePos() );
                    this.movePort.RenderSizeChanged();
                    if ( Antares.inputProvider.isRightMouseButtonPressed() && this.movePort.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        ClearMoveConnection();
                    }
                };
            this.movePort.Update( null );

            if ( port.portType == AI_ItemPort.PortType.INPUT && port.connections.Count > 0) {
                this.removeList.Add( port.connections[0] );
                port.connections[0].Delete();
            }
            port.Add( this.moveConnection );
            this.movePort.Add( this.moveConnection );

            Add( this.moveConnection );
        }


        private void _stopConnection( AI_ItemPort port ) {

            // check type of ports (only source -> target)
            if ( port.portType == this.movePort.portType ) {

                // check flow direction (only top to bottom)
                bool doIt = true;
                int portItemBank = this.aiBanks.IndexOf( (AI_Bank)port.item.Parent );
                int endPortItemBank;
                if ( port.portType == AI_ItemPort.PortType.INPUT ) {
                    endPortItemBank = this.aiBanks.IndexOf( (AI_Bank)this.moveConnection.getSource().item.Parent );
                    if ( portItemBank <= endPortItemBank ) {
                        doIt = false;
                    }
                } else {
                    endPortItemBank = this.aiBanks.IndexOf( (AI_Bank)this.moveConnection.getTarget().item.Parent );
                    if ( portItemBank >= endPortItemBank ) {
                        doIt = false;
                    }
                }

                // if all correct
                if ( doIt ) {
                    this.movePort.Action = null;

                    // if end port is an input with an existing connection, remove that connection
                    if ( port.portType == AI_ItemPort.PortType.INPUT && port.connections.Count > 0 ) {
                        AI_Connection old = port.connections[0];
                        old.setSource( null );
                        old.setTarget( null );
                        this.aiConnections.Remove( old );
                    }

                    // bind connection to end port
                    port.Add( this.moveConnection );
                    this.controller.Unregister( this.movePort );
                    this.movePort = null;
                    this.moveConnection = null;

                    this.state = BuilderState.NORMAL;
                }
            }
        }


        public void ClearMoveConnection() {
            if ( this.state == BuilderState.CONNECTION ) {
                this.moveConnection.setTarget( null );
                this.moveConnection.setSource( null );

                this.removeList.Add( this.moveConnection );

                this.moveConnection = null;
                this.controller.Unregister( this.movePort );
                this.movePort = null;

                this.state = BuilderState.NORMAL;
            }
        }


        public override void RenderSizeChanged() {
            base.RenderSizeChanged();

            if ( this.primitiveBatch != null ) {
                this.primitiveBatch.ClientSizeChanged();
            }
        }


        private void _switchBank( AI_Item item, AI_Bank target ) {
            ( (AI_Bank)item.Parent ).Remove( item );

            float targetXSize = target.AbstractSize.X * HUDService.RenderSize.X;
            float bankPos = ( Antares.inputProvider.getMousePos().X - ( target.Position.X - targetXSize / 2 ) ) / targetXSize;
            target.InsertAt( item, bankPos );
        }


        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
