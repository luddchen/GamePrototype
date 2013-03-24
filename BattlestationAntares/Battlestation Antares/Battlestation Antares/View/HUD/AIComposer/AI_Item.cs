using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Battlestation_Antares.Control;
using Battlestation_Antaris;

namespace Battlestation_Antares.View.HUD.AIComposer {

    public class AI_Item : HUDContainer, IUpdatableItem {
        public AI_Container container;

        protected Object subType;

        public static int PORT_OFFSET = 4;

        public List<AI_ItemPort> inputs;

        public List<AI_ItemPort> outputs;

        public HUDTexture background;

        public HUDString typeString;

        protected HUDString subTypeString;

        protected float[] parameters;

        private HUDButton nextSubType;

        private HUDButton previousSubType;

        private HUDButton removeButton;

        public Action dragAction;


        public AI_Item( Vector2 abstractPosition, HUDType positionType)
            : base( abstractPosition, positionType) {
            this.sizeType = HUDType.ABSOLUT;
            this.abstractSize = new Vector2( 200, 100 );

            this.inputs = new List<AI_ItemPort>();
            this.outputs = new List<AI_ItemPort>();

            this.background = new HUDTexture();
            this.background.color = new Color( 64, 96, 64 );
            this.background.sizeType = this.sizeType;
            this.background.abstractSize = this.abstractSize;
            this.background.Texture = Antares.content.Load<Texture2D>( "Sprites//builder_bg_temp" );
            Add( this.background );

            this.typeString = new HUDString( "X");
            this.typeString.positionType = this.sizeType;
            this.typeString.scale = 0.6f;
            this.typeString.abstractPosition = new Vector2( 0, -( this.abstractSize.Y - this.typeString.size.Y ) / 2 );
            Add( this.typeString );

            this.removeButton = new HUDButton( "X", new Vector2( this.abstractSize.X / 2 - 8, -( this.abstractSize.Y / 2 ) + 8 ), 0.5f, null);
            this.removeButton.positionType = this.sizeType;
            this.removeButton.style = ButtonStyle.RemoveButtonStyle();
            this.removeButton.SetPressedAction( delegate() {
                Destroy();
            } );
            Add( this.removeButton );

            this.subTypeString = new HUDString( " ");
            this.subTypeString.scale = 0.5f;
            this.subTypeString.positionType = this.sizeType;
            this.subTypeString.abstractSize = new Vector2( this.abstractSize.X, this.subTypeString.size.Y );
            this.subTypeString.abstractPosition = new Vector2( 0, -( this.abstractSize.Y - this.typeString.size.Y * 3 ) / 2 );
            Add( this.subTypeString );

            this.nextSubType = new HUDButton( ">", Vector2.Zero, 0.8f, null);
            this.nextSubType.positionType = this.sizeType;
            this.nextSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.nextSubType.abstractPosition = new Vector2( ( this.abstractSize.X - this.nextSubType.size.X ) / 2 - 2, -( this.abstractSize.Y - this.typeString.size.Y * 3 ) / 2 );
            this.nextSubType.SetPressedAction( delegate() {
                switchToNextSubType();
            } );
            Add( this.nextSubType );

            this.previousSubType = new HUDButton( "<", Vector2.Zero, 0.8f, null);
            this.previousSubType.positionType = this.sizeType;
            this.previousSubType.style = ButtonStyle.NoBackgroundButtonStyle();
            this.previousSubType.abstractPosition = new Vector2( -( this.abstractSize.X - this.nextSubType.size.X ) / 2 + 2, -( this.abstractSize.Y - this.typeString.size.Y * 3 ) / 2 );
            this.previousSubType.SetPressedAction( delegate() {
                switchToPreviousSubType();
            } );
            Add( this.previousSubType );
        }


        public void AddPort( AI_ItemPort.PortType portType ) {
            AI_ItemPort newPort = new AI_ItemPort( Vector2.Zero, HUDType.ABSOLUT, portType, this, null);

            Vector2 portPosition = new Vector2( -this.abstractSize.X / 2, 0 );
            float portOffset = 0;

            switch ( portType ) {
                case AI_ItemPort.PortType.INPUT:
                    this.inputs.Add( newPort );
                    portPosition.Y = -( this.abstractSize.Y / 2 + PORT_OFFSET );
                    portOffset = this.abstractSize.X / ( this.inputs.Count + 1 );
                    foreach ( AI_ItemPort port in this.inputs ) {
                        portPosition.X += portOffset;
                        port.abstractPosition = portPosition;
                    }
                    break;
                case AI_ItemPort.PortType.OUTPUT:
                    this.outputs.Add( newPort );
                    portPosition.Y = ( this.abstractSize.Y / 2 + PORT_OFFSET );
                    portOffset = this.abstractSize.X / ( this.outputs.Count + 1 );
                    foreach ( AI_ItemPort port in this.outputs ) {
                        portPosition.X += portOffset;
                        port.abstractPosition = portPosition;
                    }
                    break;
            }

            Add( newPort );
        }


        public new float LayerDepth {
            set {
                base.LayerDepth = value;
                this.background.LayerDepth = this.layerDepth;
            }
            get {
                return base.LayerDepth;
            }
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
            this.subTypeString.String = this.subType.ToString().Replace( '_', ' ' );
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
