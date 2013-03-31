using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using HUD.HUD;
using HUD;

namespace Battlestation_Antares.View.HUD.AIComposer {

    class AI_Item : HUDContainer, IUpdatableItem {

        public static Vector2 AI_ITEM_SIZE = new Vector2( 0.125f, 0.1f );

        public AI_Container container;

        protected Object subType;

        public static float PORT_OFFSET = AI_ITEM_SIZE.Y / 20f;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;

        public HUDString typeString;

        protected HUDString subTypeString;

        protected float[] parameters;

        private HUDButton nextSubType;

        private HUDButton previousSubType;

        private HUDButton removeButton;

        public Action dragAction;


        public AI_Item() : base( Vector2.Zero ) {
            this.AbstractSize = AI_Item.AI_ITEM_SIZE;

            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();

            SetBackground( "Sprites//builder_bg_temp", new Color( 64, 96, 64 ) );

            Vector2 textSize = new Vector2( this.AbstractSize.X * 0.1f, this.AbstractSize.Y * 0.2f);

            this.typeString = new HUDString( " ", null, null, position: new Vector2( 0, -( this.AbstractSize.Y - textSize.Y ) / 2 ), size: textSize );
            Add( this.typeString );

            this.removeButton = new HUDButton( "X", new Vector2( this.AbstractSize.X / 2 * 0.9f, -( this.AbstractSize.Y - textSize.Y ) / 2 ), textSize * 1.1f, 1f, null );
            this.removeButton.AbstractSize = textSize;
            this.removeButton.style = ButtonStyle.RemoveButtonStyle();
            this.removeButton.SetPressedAction( delegate() {
                Destroy();
            } );
            Add( this.removeButton );

            this.subTypeString = new HUDString( " ", null, null, position: new Vector2( 0, -( this.AbstractSize.Y - textSize.Y * 3 ) / 2 ), size: textSize );
            Add( this.subTypeString );

            this.nextSubType = new HUDButton( ">", Vector2.Zero, textSize * 1.5f, 1f, null);
            this.nextSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.nextSubType.AbstractPosition = new Vector2( this.AbstractSize.X / 2 * 0.9f, this.subTypeString.AbstractPosition.Y );
            this.nextSubType.SetPressedAction( delegate() {
                switchToNextSubType();
            } );
            Add( this.nextSubType );

            this.previousSubType = new HUDButton( "<", Vector2.Zero, textSize * 1.5f, 1f, null);
            this.previousSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.previousSubType.AbstractPosition = new Vector2( -( this.AbstractSize.X / 2 * 0.95f ), this.subTypeString.AbstractPosition.Y );
            this.previousSubType.SetPressedAction( delegate() {
                switchToPreviousSubType();
            } );
            Add( this.previousSubType );
        }


        public void AddPort( AI_ItemPort.PortType portType ) {
            AI_ItemPort newPort = new AI_ItemPort( portType, this, null);

            Vector2 portPosition = new Vector2( -this.AbstractSize.X / 2, this.AbstractSize.Y / 2 + PORT_OFFSET );
            float portOffset = 0;

            switch ( portType ) {
                case AI_ItemPort.PortType.INPUT:
                    this.inputs.Add( newPort );
                    portPosition.Y = -portPosition.Y;
                    portOffset = this.AbstractSize.X / ( this.inputs.Count + 1 );
                    foreach ( AI_ItemPort port in this.inputs ) {
                        portPosition.X += portOffset;
                        port.AbstractPosition = portPosition;
                    }
                    break;
                case AI_ItemPort.PortType.OUTPUT:
                    this.outputs.Add( newPort );
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
