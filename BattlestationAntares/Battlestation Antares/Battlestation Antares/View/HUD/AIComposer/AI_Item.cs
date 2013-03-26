using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Item : HUDContainer, IUpdatableItem {
        public AI_Container container;

        protected Object subType;

        public static int PORT_OFFSET = 4;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;

        public HUDString typeString;

        protected HUDString subTypeString;

        protected float[] parameters;

        private HUDButton nextSubType;

        private HUDButton previousSubType;

        private HUDButton removeButton;

        public Action dragAction;


        public AI_Item( Vector2 abstractPosition, HUDType positionType) : base( abstractPosition ) {
            this.SizeType = HUDType.ABSOLUT;
            this.AbstractSize = new Vector2( 200, 100 );

            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();

            SetBackground( "Sprites//builder_bg_temp", new Color( 64, 96, 64 ) );

            this.typeString = new HUDString( "X");
            this.typeString.PositionType = this.SizeType;
            this.typeString.AbstractSize = new Vector2( this.AbstractSize.X, this.AbstractSize.Y / 5f );
            this.typeString.AbstractPosition = new Vector2( 0, -( this.AbstractSize.Y - this.typeString.AbstractSize.Y ) / 2 );
            Add( this.typeString );

            this.removeButton = new HUDButton( "X", new Vector2( this.AbstractSize.X / 2 - 8, -( this.AbstractSize.Y / 2 ) + 8 ), 1f, null);
            this.removeButton.PositionType = this.SizeType;
            this.removeButton.AbstractSize = new Vector2( 16, 16 );
            this.removeButton.style = ButtonStyle.RemoveButtonStyle();
            this.removeButton.SetPressedAction( delegate() {
                Destroy();
            } );
            Add( this.removeButton );

            this.subTypeString = new HUDString( " ");
            this.subTypeString.PositionType = this.SizeType;
            this.subTypeString.AbstractSize = new Vector2( this.AbstractSize.X, this.AbstractSize.Y / 5f);
            this.subTypeString.AbstractPosition = new Vector2( 0, -( this.AbstractSize.Y - this.typeString.Size.Y * 3 ) / 2 );
            Add( this.subTypeString );

            this.nextSubType = new HUDButton( ">", Vector2.Zero, 1, null);
            this.nextSubType.PositionType = this.SizeType;
            this.nextSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.nextSubType.AbstractPosition = new Vector2( this.AbstractSize.X / 2 - 8, this.subTypeString.AbstractPosition.Y );
            this.nextSubType.AbstractSize = new Vector2( 24, 24 );
            this.nextSubType.SetPressedAction( delegate() {
                switchToNextSubType();
            } );
            Add( this.nextSubType );

            this.previousSubType = new HUDButton( "<", Vector2.Zero, 1, null);
            this.previousSubType.PositionType = this.SizeType;
            this.previousSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.previousSubType.AbstractPosition = new Vector2( -( this.AbstractSize.X / 2 - 8 ), this.subTypeString.AbstractPosition.Y );
            this.previousSubType.AbstractSize = new Vector2( 24, 24 );
            this.previousSubType.SetPressedAction( delegate() {
                switchToPreviousSubType();
            } );
            Add( this.previousSubType );
        }


        public void AddPort( AI_ItemPort.PortType portType ) {
            AI_ItemPort newPort = new AI_ItemPort( Vector2.Zero, HUDType.ABSOLUT, portType, this, null);

            Vector2 portPosition = new Vector2( -this.AbstractSize.X / 2, 0 );
            float portOffset = 0;

            switch ( portType ) {
                case AI_ItemPort.PortType.INPUT:
                    this.inputs.Add( newPort );
                    portPosition.Y = -( this.AbstractSize.Y / 2 + PORT_OFFSET );
                    portOffset = this.AbstractSize.X / ( this.inputs.Count + 1 );
                    foreach ( AI_ItemPort port in this.inputs ) {
                        portPosition.X += portOffset;
                        port.AbstractPosition = portPosition;
                    }
                    break;
                case AI_ItemPort.PortType.OUTPUT:
                    this.outputs.Add( newPort );
                    portPosition.Y = ( this.AbstractSize.Y / 2 + PORT_OFFSET );
                    portOffset = this.AbstractSize.X / ( this.outputs.Count + 1 );
                    foreach ( AI_ItemPort port in this.outputs ) {
                        portPosition.X += portOffset;
                        port.AbstractPosition = portPosition;
                    }
                    break;
            }

            Add( newPort );
        }


        protected virtual void switchToNextSubType() {
            if ( this.subType is Enum ) {
                Type type = this.subType.GetType();

                int oldSubType = (int)this.subType;
                Array subTypes = Enum.GetValues( type );
                Object newSubType = null;
                if ( oldSubType + 1 < subTypes.Length ) {
                    newSubType = subTypes.GetValue( oldSubType + 1 );
                } else {
                    newSubType = subTypes.GetValue( 0 );
                }
                SetSubType( newSubType );
            }
        }


        protected virtual void switchToPreviousSubType() {
            if ( this.subType is Enum ) {
                Type type = this.subType.GetType();

                int oldSubType = (int)this.subType;
                Array subTypes = Enum.GetValues( type );
                Object newSubType = null;
                if ( oldSubType - 1 >= 0 ) {
                    newSubType = subTypes.GetValue( oldSubType - 1 );
                } else {
                    newSubType = subTypes.GetValue( subTypes.Length - 1 );
                }
                SetSubType( newSubType );
            }
        }

        public virtual void SetSubType( Object subType ) {
            this.subType = subType;
            this.subTypeString.Text = this.subType.ToString().Replace( '_', ' ' );
        }

        public Object GetSubType() {
            return this.subType;
        }


        private void Destroy() {
            foreach ( AI_ItemPort port in this.inputs ) {
                while ( port.connections.Count > 0 ) {
                    this.container.removeList.Add( port.connections[0] );
                    port.connections[0].Delete();
                }
                port.connections.Clear();
            }

            foreach ( AI_ItemPort port in this.outputs ) {
                while ( port.connections.Count > 0 ) {
                    this.container.removeList.Add( port.connections[0] );
                    port.connections[0].Delete();
                }
                port.connections.Clear();
            }

            this.container.removeList.Add( this );

        }

        public void SetParameterCount( int count ) {
            this.parameters = new float[count];
        }

        public int GetParameterCount() {
            if ( this.parameters == null ) {
                return 0;
            }
            return this.parameters.Length;
        }

        public virtual void SetParameter( int index, float value ) {
            if ( this.parameters != null && index < this.parameters.Length ) {
                this.parameters[index] = value;
            }
        }

        public virtual float GetParameter( int index ) {
            if ( this.parameters != null && index < this.parameters.Length ) {
                return this.parameters[index];
            }
            return 0;
        }


        public virtual void Update( GameTime gameTime ) {
            if ( this.dragAction != null ) {
                if ( this.typeString.Intersects( Antares.inputProvider.getMousePos() ) ) {
                    if ( Antares.inputProvider.isLeftMouseButtonPressed() ) {
                        this.dragAction();
                    }
                }
            }
            foreach ( AI_ItemPort port in this.inputs ) {
                port.Update( gameTime );
            }
            foreach ( AI_ItemPort port in this.outputs ) {
                port.Update( gameTime );
            }
            this.removeButton.Update( gameTime );
            this.previousSubType.Update( gameTime );
            this.nextSubType.Update( gameTime );
        }

        public bool Enabled {
            get {
                return this.IsVisible;
            }
        }
    }

}
