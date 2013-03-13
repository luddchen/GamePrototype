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
            //writer.WriteComment("AI Container");
            writer.Formatting = Formatting.Indented;
            writer.WriteStartElement("AI");

            writer.WriteStartElement("Items");
            foreach (AI_Item item in aiContainer.aiItems)
            {
                writer.WriteStartElement("Item");

                    writer.WriteStartElement("Type");
                        writer.WriteString(item.GetType().ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("SubType");
                        writer.WriteString(item.subType.ToString());
                    writer.WriteEndElement();

                    writer.WriteStartElement("Position");
                        writer.WriteStartElement("X");
                            writer.WriteString(item.abstractPosition.X.ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("Y");
                            writer.WriteString(item.abstractPosition.Y.ToString());
                        writer.WriteEndElement();
                    writer.WriteEndElement();

                writer.WriteEndElement();
            }
            writer.WriteEndElement();

            writer.WriteStartElement("Connections");
            foreach (AI_Connection connection in aiContainer.aiConnections)
            {
                writer.WriteStartElement("Connection");

                    writer.WriteStartElement("Source");
                        writer.WriteStartElement("ItemIndex");
                            writer.WriteString(aiContainer.aiItems.IndexOf(connection.getSource().item).ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("PortIndex");
                            writer.WriteString(connection.getSource().item.outputs.IndexOf(connection.getSource()).ToString());
                        writer.WriteEndElement();
                    writer.WriteEndElement();

                    writer.WriteStartElement("Target");
                        writer.WriteStartElement("ItemIndex");
                            writer.WriteString(aiContainer.aiItems.IndexOf(connection.getTarget().item).ToString());
                        writer.WriteEndElement();
                        writer.WriteStartElement("PortIndex");
                            writer.WriteString(connection.getTarget().item.inputs.IndexOf(connection.getTarget()).ToString());
                        writer.WriteEndElement();
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
                switch (reader.NodeType)
                {
                    case XmlNodeType.Element :
                        if (reader.Name == "Item")
                        {
                            AI_Item item = null;

                            reader.Read();// Type Node
                            reader.Read();
                            Type type = Type.GetType(reader.ReadString());
                            Object[] parameters = new Object[3];
                            parameters[0] = new Vector2( 0.5f, 0.5f);
                            parameters[1] = HUDType.RELATIV;
                            parameters[2] = game;
                            item = (AI_Item)Activator.CreateInstance(type, parameters);

                            reader.Read();// SubType Node
                            reader.Read();
                            Type subType = item.subType.GetType();
                            item.subType = Enum.Parse(subType, reader.ReadString());
                            item.subTypeString.String = item.subType.ToString().Replace('_',' ');

                            reader.Read();// Position Node
                            reader.Read();
                            reader.Read();// X Node
                            reader.Read();
                            item.abstractPosition.X = float.Parse(reader.ReadString());
                            reader.Read();// Y Node
                            reader.Read();
                            item.abstractPosition.Y = float.Parse(reader.ReadString());

                            aiContainer.Add(item);
                        }
                        if (reader.Name == "Connection")
                        {
                            AI_Connection connection = new AI_Connection(game);

                            reader.Read();// Source Node
                            reader.Read();
                            reader.Read();// Item Index Node
                            reader.Read();
                            int sourceItemIndex = int.Parse(reader.ReadString());
                            reader.Read();// Port Index Node
                            reader.Read();
                            int sourcePortIndex = int.Parse(reader.ReadString());

                            reader.Read();
                            reader.Read();
                            reader.Read();
                            reader.Read();// Target Node
                            reader.Read();
                            reader.Read();// Item Index Node
                            reader.Read();
                            int targetItemIndex = int.Parse(reader.ReadString()); 
                            reader.Read();// Port Index Node
                            reader.Read();
                            int targetPortIndex = int.Parse(reader.ReadString());

                            connection.setSource(aiContainer.aiItems[sourceItemIndex].outputs[sourcePortIndex]);
                            connection.setTarget(aiContainer.aiItems[targetItemIndex].inputs[targetPortIndex]);

                            aiContainer.aiConnections.Add(connection);
                        }

                        break;
                }
            }

            Console.WriteLine();
            reader.Close();
        }


    }

}
