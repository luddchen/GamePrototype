using System;
using System.Xml;
using System.Collections.Generic;
using Battlestation_Antares.View.HUD.AIComposer;
using Microsoft.Xna.Framework;
using Battlestation_Antares.View.HUD;
using Battlestation_Antaris.View.HUD.AIComposer;

namespace Battlestation_Antares.Control.AI {

    public class AI_XML {

        public static void WriteAIContainer( String fileName, AI_Container aiContainer ) {
            XmlTextWriter writer = new XmlTextWriter( fileName, null );

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement( "AI" );

            writer.WriteStartElement( "Items" );
            foreach ( AI_Item item in aiContainer.aiItems ) {
                writer.WriteStartElement( "Item" );
                writer.WriteAttributeString( "type", item.GetType().ToString() );

                writer.WriteStartElement( "SubType" );
                writer.WriteString( item.GetSubType().ToString() );
                writer.WriteEndElement();

                writer.WriteStartElement( "Position" );
                writer.WriteAttributeString( "x", item.abstractPosition.X.ToString() );
                writer.WriteAttributeString( "y", item.abstractPosition.Y.ToString() );
                writer.WriteEndElement();

                writer.WriteStartElement( "Bank" );
                writer.WriteAttributeString("nr", aiContainer.aiBanks.IndexOf( (AI_Bank)item.parent ).ToString() );
                writer.WriteEndElement();

                int paramCount = 0;
                if ( item.GetParameterCount() > 0 ) {
                    paramCount = item.GetParameterCount();
                }
                writer.WriteStartElement( "Parameters" );
                writer.WriteAttributeString( "count", paramCount.ToString() );
                for ( int i = 0; i < paramCount; i++ ) {
                    writer.WriteStartElement( "Param" );
                    writer.WriteAttributeString( "value", item.GetParameter( i ).ToString() );
                    writer.WriteEndElement();
                }

                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement( "Connections" );
            foreach ( AI_Connection connection in aiContainer.aiConnections ) {
                writer.WriteStartElement( "Connection" );

                writer.WriteStartElement( "Source" );
                writer.WriteAttributeString( "item", aiContainer.aiItems.IndexOf( connection.getSource().item ).ToString() );
                writer.WriteAttributeString( "port", connection.getSource().item.outputs.IndexOf( connection.getSource() ).ToString() );
                writer.WriteEndElement();

                writer.WriteStartElement( "Target" );
                writer.WriteAttributeString( "item", aiContainer.aiItems.IndexOf( connection.getTarget().item ).ToString() );
                writer.WriteAttributeString( "port", connection.getTarget().item.inputs.IndexOf( connection.getTarget() ).ToString() );
                writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }


        public static void ReadAIContainer( String fileName, AI_Container aiContainer ) {
            aiContainer.ClearAI();

            XmlTextReader reader = new XmlTextReader( fileName );

            try {
                while ( reader.Read() ) {
                    if ( reader.NodeType == XmlNodeType.Element ) {
                        if ( reader.Name == "Item" ) {
                            AI_Item item = null;

                            Type type = Type.GetType( reader.GetAttribute( 0 ) );
                            Object[] parameters = new Object[2];
                            parameters[0] = new Vector2( 0.5f, 0.5f );
                            parameters[1] = HUDType.RELATIV;
                            item = (AI_Item)Activator.CreateInstance( type, parameters );

                            ContinueToNode( reader, "SubType" );
                            item.SetSubType( Enum.Parse( item.GetSubType().GetType(), reader.ReadString() ) );

                            ContinueToNode( reader, "Position" );
                            item.abstractPosition.X = float.Parse( reader.GetAttribute( 0 ) );
                            item.abstractPosition.Y = float.Parse( reader.GetAttribute( 1 ) );

                            ContinueToNode( reader, "Bank" );
                            int bankNr = int.Parse( reader.GetAttribute( 0 ) );

                            ContinueToNode( reader, "Parameters" );
                            int count = int.Parse( reader.GetAttribute( 0 ) );
                            item.SetParameterCount( count );
                            for ( int index = 0; index < count; index++ ) {
                                ContinueToNode( reader, "Param" );
                                item.SetParameter( index, float.Parse( reader.GetAttribute( 0 ) ) );
                            }

                            aiContainer.Add( item );
                            aiContainer.aiBanks[bankNr].Add( item );

                        }

                        if ( reader.Name == "Connection" ) {
                            AI_Connection connection = new AI_Connection();

                            ContinueToNode( reader, "Source" );
                            int sourceItemIndex = int.Parse( reader.GetAttribute( 0 ) );
                            int sourcePortIndex = int.Parse( reader.GetAttribute( 1 ) );

                            ContinueToNode( reader, "Target" );
                            int targetItemIndex = int.Parse( reader.GetAttribute( 0 ) );
                            int targetPortIndex = int.Parse( reader.GetAttribute( 1 ) );

                            connection.setSource( aiContainer.aiItems[sourceItemIndex].outputs[sourcePortIndex] );
                            connection.setTarget( aiContainer.aiItems[targetItemIndex].inputs[targetPortIndex] );

                            aiContainer.aiConnections.Add( connection );
                        }
                    }
                }

                reader.Close();
            } catch ( Exception e ) {
                Console.WriteLine( e );
            }
        }


        private static void ContinueToNode( XmlTextReader reader, String nodeName ) {
            while ( reader.Read() && reader.Name != nodeName ) {
            }
        }


    }

}
