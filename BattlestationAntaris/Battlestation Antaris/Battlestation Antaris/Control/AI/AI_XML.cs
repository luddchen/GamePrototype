using System;
using System.Xml;
using System.Collections.Generic;
using Battlestation_Antaris.View.HUD.AIComposer;
using Microsoft.Xna.Framework;
using Battlestation_Antaris.View.HUD;

namespace Battlestation_Antaris.Control.AI
{

    public class AI_XML
    {

        public static void WriteAIContainer(String fileName, AI_Container aiContainer)
        {
            XmlTextWriter writer = new XmlTextWriter(fileName, null);

            writer.WriteStartDocument();
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement("AI");

                writer.WriteStartElement("Items");
                foreach (AI_Item item in aiContainer.aiItems)
                {
                    writer.WriteStartElement("Item");
                        writer.WriteAttributeString("Type", item.GetType().ToString());

                        writer.WriteStartElement("SubType");
                            writer.WriteString(item.subType.ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Position");
                            writer.WriteAttributeString("x", item.abstractPosition.X.ToString());
                            writer.WriteAttributeString("y", item.abstractPosition.Y.ToString());
                        writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

                writer.WriteStartElement("Connections");
                foreach (AI_Connection connection in aiContainer.aiConnections)
                {
                    writer.WriteStartElement("Connection");

                        writer.WriteStartElement("Source");
                            writer.WriteAttributeString("Item", aiContainer.aiItems.IndexOf(connection.getSource().item).ToString());
                            writer.WriteAttributeString("Port", connection.getSource().item.outputs.IndexOf(connection.getSource()).ToString());
                        writer.WriteEndElement();

                        writer.WriteStartElement("Target");
                            writer.WriteAttributeString("Item", aiContainer.aiItems.IndexOf(connection.getTarget().item).ToString());
                            writer.WriteAttributeString("Port", connection.getTarget().item.inputs.IndexOf(connection.getTarget()).ToString());
                        writer.WriteEndElement();

                    writer.WriteEndElement();
                }
                writer.WriteEndElement();

            writer.WriteEndElement();
            writer.WriteEndDocument();
            writer.Close();
        }


        public static void ReadAIContainer(String fileName, AI_Container aiContainer, Game1 game)
        {
            aiContainer.ClearAI();

            XmlTextReader reader = new XmlTextReader(fileName);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name == "Item")
                    {
                        AI_Item item = null;

                        Type type = Type.GetType(reader.GetAttribute(0));
                        Object[] parameters = new Object[3];
                        parameters[0] = new Vector2( 0.5f, 0.5f);
                        parameters[1] = HUDType.RELATIV;
                        parameters[2] = game;
                        item = (AI_Item)Activator.CreateInstance(type, parameters);

                        ContinueToNode(reader, "SubType");
                        Type subType = item.subType.GetType();
                        item.subType = Enum.Parse(subType, reader.ReadString());
                        item.subTypeString.String = item.subType.ToString().Replace('_',' ');

                        ContinueToNode(reader, "Position");
                        item.abstractPosition.X = float.Parse(reader.GetAttribute(0));
                        item.abstractPosition.Y = float.Parse(reader.GetAttribute(1));

                        aiContainer.Add(item);

                    }

                    if (reader.Name == "Connection")
                    {
                        AI_Connection connection = new AI_Connection(game);

                        ContinueToNode(reader, "Source");
                        int sourceItemIndex = int.Parse(reader.GetAttribute(0));
                        int sourcePortIndex = int.Parse(reader.GetAttribute(1));

                        ContinueToNode(reader, "Target");
                        int targetItemIndex = int.Parse(reader.GetAttribute(0));
                        int targetPortIndex = int.Parse(reader.GetAttribute(1));

                        connection.setSource(aiContainer.aiItems[sourceItemIndex].outputs[sourcePortIndex]);
                        connection.setTarget(aiContainer.aiItems[targetItemIndex].inputs[targetPortIndex]);

                        aiContainer.aiConnections.Add(connection);
                    }
                }
            }

            reader.Close();
        }


        private static void ContinueToNode(XmlTextReader reader, String nodeName)
        {
            while (reader.Read() && reader.Name != nodeName) { }
        }


    }

}
