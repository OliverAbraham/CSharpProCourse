using System.Collections.Generic;

namespace Abraham.UI
{
    public class LayoutElement
    {
        public enum Type
        {
            Unknown,
            Row,
            Column,
            Label,
            TextBox,
            Radiobutton,
            Checkbox,
            Button,
            GroupBox
        }

        public List<LayoutElement> Children { get; private set; }

        public Type ElementType { get; set; }
        public string XMLType { get; set; }
        public int Depth { get; set; }
        public string Name { get; set; }
        public string Id { get; set; }
        public string Content { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public string Watermark { get; set; }
        public string Command { get; set; }
        public string ItemsSource { get; internal set; }
        public string SelectedItem { get; internal set; }
        public string IsChecked { get; internal set; }

        public LayoutElement()
        {
            ElementType = Type.Unknown;
            Children = new List<LayoutElement>();
        }
    }
}