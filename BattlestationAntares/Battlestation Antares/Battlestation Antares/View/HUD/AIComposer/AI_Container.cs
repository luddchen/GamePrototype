using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antaris.View.HUD.AIComposer;
using Battlestation_Antares.Control;
using HUD.HUD;
using HUD;

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
            : base( new Vector2( 0, 0 ) ) {

            this.controller = controller;
            this.primitiveBatch = new PrimitiveBatch( Antares.graphics.GraphicsDevice );
            this.state = BuilderState.NORMAL;

            this.removeList = new List<HUD_Item>();
            this.aiItems = new List<AI_Item>();
            this.aiConnections = new List<AI_Connection>();
            this.aiBanks = new List<AI_Bank>();

            this.insertBank = new AI_Bank( new Vector2( 0.9f, 0.1f ), HUDType.RELATIV, new Vector2( 210, 110 ), HUDType.ABSOLUT );
            this.insertBank.SetBackground( Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" ) );
            this.insertBank.SetBackgroundColor( new Color( 120, 128, 112) );
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
            HUDArray addButtonArray = new HUDArray( new Vector2( 0.9f, 0.35f ), HUDType.RELATIV, new Vector2( 0.15f, 0.25f ), HUDType.RELATIV );
            addButtonArray.direction = LayoutDirection.VERTICAL;
            this.Add( addButtonArray );


            HUDButton addInputButton = new HUDButton( "Input", new Vector2(), 0.7f, this.controller );
            addInputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Input( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV) );
            } );
            addInputButton.style = ButtonStyle.BuilderButtonStyle();
            addButtonArray.Add( addInputButton );


            HUDButton addTransformerButton = new HUDButton( "Transformer", new Vector2(), 0.7f, this.controller );
            addTransformerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Transformer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV) );
            } );
            addTransformerButton.style = ButtonStyle.BuilderButtonStyle();
            addButtonArray.Add( addTransformerButton );


            HUDButton addMixerButton = new HUDButton( "Mixer", new Vector2(), 0.7f, this.controller );
            addMixerButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Mixer( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV) );
            } );
            addMixerButton.style = ButtonStyle.BuilderButtonStyle();
            addButtonArray.Add( addMixerButton );


            HUDButton addOutputButton = new HUDButton( "Output", new Vector2(), 0.7f, this.controller );
            addOutputButton.SetPressedAction( delegate() {
                AddInsertItem( new AI_Output( new Vector2( 0.7f, 0.1f ), HUDType.RELATIV) );
            } );
            addOutputButton.style = ButtonStyle.BuilderButtonStyle();
            addButtonArray.Add( addOutputButton );
        }


        private void _initMouseTexture() {
            this.mouseItemTex = new HUDActionTexture(
                delegate() {
                    this.mouseItemTex.AbstractPosition = Antares.inputProvider.getMousePos();
                    this.mouseItemTex.ClientSizeChanged();
                },
                controller );
            this.mouseItemTex.PositionType = HUDType.ABSOLUT;
            this.mouseItemTex.AbstractSize = new Vector2( 200, 100 );
            this.mouseItemTex.SizeType = HUDType.ABSOLUT;
            this.mouseItemTex.color = AI_Bank.NORMAL_COLOR;
            this.Add( this.mouseItemTex );
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

                foreach ( AI_Bank bank in this.aiBanks ) {
                    bank.AbstractPosition = 
                        new Vector2( bank.AbstractPosition.X, this.aiBanks.IndexOf( bank ) * ( 1.0f / this.aiBanks.Count ) + ( 0.5f / this.aiBanks.Count ) );
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
                            bank.SetBackgroundColor( AI_Bank.ENABLED_COLOR );
                        } else {
                            bank.SetBackgroundColor( AI_Bank.DISABLED_COLOR );
                        }
                    } else {
                        bank.SetBackgroundColor( AI_Bank.DISABLED_COLOR );
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
            this.movePort = new AI_ItemPort( Antares.inputProvider.getMousePos(), HUDType.ABSOLUT, portType, this.controller );
            this.movePort.action =
                delegate() {
                    this.movePort.AbstractPosition = Antares.inputProvider.getMousePos();
                    this.movePort.ClientSizeChanged();
                    if ( Antares.inputProvider.isRightMouseButtonPressed() && this.movePort.Intersects( Antares.inputProvider.getMousePos() ) ) {
                        ClearMoveConnection();
                    }
                };
            this.movePort.action();

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
                int portItemBank = this.aiBanks.IndexOf( (AI_Bank)port.item.parent );
                int endPortItemBank;
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

                // if all correct
                if ( doIt ) {
                    this.movePort.action = null;

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


        public override void ClientSizeChanged() {
            base.ClientSizeChanged();

            if ( this.primitiveBatch != null ) {
                this.primitiveBatch.ClientSizeChanged();
            }
        }


        private void _switchBank( AI_Item item, AI_Bank target ) {
            ( (AI_Bank)item.parent ).Remove( item );

            float targetXSize = target.AbstractSize.X * HUD_Item.game.RenderSize().X;
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
