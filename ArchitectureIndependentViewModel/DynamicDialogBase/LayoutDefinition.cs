using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace Abraham.UI
{
    public class LayoutDefinition
    {
        public string Definition { get; set; }

        public List<LayoutElement> Tree;

        private Stack<LayoutElement> RowStack = null;
        private int CurrentDepth = 0;
        private LayoutElement CurrentElement;

        public LayoutDefinition()
        {
            Definition = "";
        }

        public LayoutDefinition(string definitionXML)
        {
            Definition = definitionXML;
        }

        internal void Convert_layout_XML_to_element_tree()
        {
            Tree = new List<LayoutElement>();
            RowStack = new Stack<LayoutElement>();
            XmlTextReader reader = null;
            try
            {
                // Load the reader with the data file and ignore all white space nodes.         
                File.WriteAllText("tmp", Definition);
                reader = new XmlTextReader("tmp");
                reader.WhitespaceHandling = WhitespaceHandling.None;

                // Parse the file and display each of the nodes.
                while (reader.Read())
                {
                    switch (reader.NodeType)
                    {
                        case XmlNodeType.Element:
                            ProcessElementBegin(reader);
                            break;
                        case XmlNodeType.Text:
                            //Console.Write(reader.Value);
                            break;
                        case XmlNodeType.CDATA:
                            //Console.Write("<![CDATA[{0}]]>", reader.Value);
                            break;
                        case XmlNodeType.ProcessingInstruction:
                            //Console.Write("<?{0} {1}?>", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.Comment:
                            //.Write("<!--{0}-->", reader.Value);
                            break;
                        case XmlNodeType.XmlDeclaration:
                            //Console.Write("<?xml version='1.0'?>");
                            break;
                        case XmlNodeType.Document:
                            break;
                        case XmlNodeType.DocumentType:
                            //Console.Write("<!DOCTYPE {0} [{1}]", reader.Name, reader.Value);
                            break;
                        case XmlNodeType.EntityReference:
                            //Console.Write(reader.Name);
                            break;
                        case XmlNodeType.EndElement:
                            //Console.Write("</{0}>", reader.Name);
                            ProcessElementEnd(reader);
                            break;
                        default:
                            break;
                    }
                }
            }

            finally
            {
                if (reader != null)
                    reader.Close();
            }
        }

        private void ProcessElementBegin(XmlTextReader element)
        {
            LayoutElement e = new LayoutElement();
            e.XMLType       = element.Name;
            e.Depth         = element.Depth;
            e.Name          = element.Name;
            e.Id            = element.GetAttribute("Id");
            e.Width         = Convert.ToDouble(element.GetAttribute("Width"));
            e.Height        = Convert.ToDouble(element.GetAttribute("Height"));
            e.Name          = element.GetAttribute("Name");
            e.Content       = element.GetAttribute("Content");
            e.Watermark     = element.GetAttribute("Watermark");
            e.Command       = element.GetAttribute("Command");
            e.ItemsSource   = element.GetAttribute("ItemsSource");
            e.SelectedItem  = element.GetAttribute("SelectedItem");
            e.IsChecked     = element.GetAttribute("IsChecked");

            switch (element.Name)
            {
                case "Dialog":
                {
                    RowStack.Push(e);
                    CurrentElement = e;
                    Tree.Add(CurrentElement);
                    break;
                }

                case "Row":
                {
                    RowStack.Push(e);
                    CurrentElement = e;
                    break;
                }

                case "Label":
                case "TextBox":
                case "CheckBox":
                case "ComboBox":
                case "ListBox":
                case "Button":
                {
                    CurrentElement.Children.Add(e);
                    break;
                }
            }
        }

        private void ProcessElementEnd(XmlTextReader element)
        {
            switch (element.Name)
            {
                case "Dialog":
                {
                    break;
                }

                case "Row":
                {
                    RowStack.Pop();
                    Tree.Add(CurrentElement);
                    break;
                }

                case "Label":
                {
                    break;
                }

                case "Entryfield":
                {
                    break;
                }
            }
        }
    }
}
