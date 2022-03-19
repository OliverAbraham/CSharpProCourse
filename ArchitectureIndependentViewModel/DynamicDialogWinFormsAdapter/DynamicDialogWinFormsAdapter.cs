using System.Collections.Generic;
using System.Windows.Forms;

namespace Abraham.UI
{
    public class DynamicDialogWinFormsAdapter<T> : DynamicDialog, IFramework
    {
        private Form _Parent;
        private IDynamicDialog _ViewModel;
        private bool _DebugColors = false;

        public DynamicDialogWinFormsAdapter(IDynamicDialog viewModel) : base(viewModel)
        {
            _ViewModel = viewModel;
            _ViewModel.Init(this);
        }

        public void DoModal(Form parent)
        {
            FlowLayoutPanel MyStackPanel = new FlowLayoutPanel();
            MyStackPanel.FlowDirection = FlowDirection.TopDown;
            MyStackPanel.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.WhiteSmoke);
            MyStackPanel.Padding   = new Padding(20);
            
            ProcessRows(parent, MyStackPanel, _LayoutDefinition.Tree);
            
            MyStackPanel.Width  = parent.Width;  
            MyStackPanel.Height = parent.Height;  
            MyStackPanel.Anchor = AnchorStyles.Left | AnchorStyles.Top | AnchorStyles.Right | AnchorStyles.Bottom;
            parent.Controls.Add(MyStackPanel);
            parent.Margin = new Padding(50);
            parent.ShowDialog();
        }

        private void ProcessRows(Form parentWindow, FlowLayoutPanel parent, List<LayoutElement> elements)
        {
            foreach (var Element in elements)
            {
                switch (Element.XMLType)
                {
                    case "Dialog":
                        parentWindow.Width = (int)Element.Width;
                        parentWindow.Height = (int)Element.Height;
                        break;

                    case "Row":
                        FlowLayoutPanel Row = new FlowLayoutPanel();
                        Row.BackColor = System.Drawing.SystemColors.ControlLight;
                        Row.FlowDirection = FlowDirection.LeftToRight;
                        Row.Width = parent.Width;
                        Row.Height = 25;
                        Row.Padding = new Padding(0);
                        ProcessChildren(Row, Element.Children);
                        parent.Controls.Add(Row);
                        break;
                }
            }
        }

        private void ProcessChildren(FlowLayoutPanel parent, List<LayoutElement> elements)
        {
            foreach (var Element in elements)
            {
                if (Element.Width == 0)
                    Element.Width = 50;

                switch (Element.XMLType)
                {
                    case "Label":
                        {
                            Label c      = new Label();
                            c.BackColor  = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Gray);
                            c.Text       = Element.Content;
                            c.Margin     = new Padding(0, 1, 10, 0);
                            c.Width      = (int)(parent.Width * Element.Width / 100);
                            parent.Controls.Add(c);
                            break;
                        }
                    case "TextBox":
                        {
                            TextBox c    = new TextBox();
                            if (_DebugColors) c.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Gray);
                            //c.MinWidth   = 10;
                            c.Margin     = new Padding(0, 4, 10, 0);
                            c.Width      = (int)(parent.Width * Element.Width / 100);
                            if (Element.Content.StartsWith("{Binding:"))
                            {
                                string PropertyPath = GetBindingArgument(Element.Content);
                                Binding myBinding = CreateTwoWayBinding(PropertyPath, _ViewModel);
                                //BindingOperations.SetBinding(c, TextBox.TextProperty, myBinding);
                            }
                            else
                            {
                                if (Element.Watermark != "")
                                    c.Text = Element.Watermark;
                                else
                                    c.Text = Element.Content;
                            }

                            parent.Controls.Add(c);
                            break;
                        }
                    case "ComboBox":
                        {
                            ComboBox c    = new ComboBox();
                            if (_DebugColors) c.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.BlueViolet);
                            //c.MinWidth   = 10;
                            c.Margin     = new Padding(0, 4, 10, 0);
                            c.Width      = (int)(parent.Width * Element.Width / 100);
                            if (Element.ItemsSource.StartsWith("{Binding:"))
                            {
                                string PropertyPath = GetBindingArgument(Element.ItemsSource);
                                Binding myBinding = CreateTwoWayBinding(PropertyPath, _ViewModel);
                                //BindingOperations.SetBinding(c, ComboBox.ItemsSourceProperty, myBinding);
                            }
                            if (Element.SelectedItem.StartsWith("{Binding:"))
                            {
                                string PropertyPath = GetBindingArgument(Element.SelectedItem);
                                Binding myBinding = CreateTwoWayBinding(PropertyPath, _ViewModel);
                                //BindingOperations.SetBinding(c, ComboBox.SelectedItemProperty, myBinding);
                            }

                            parent.Controls.Add(c);
                            break;
                        }
                    case "Button":
                        {
                            Button c     = new Button();
                            if (_DebugColors) c.BackColor = System.Drawing.Color.FromKnownColor(System.Drawing.KnownColor.Blue);
                            c.Text       = Element.Content;
                            c.Margin     = new Padding(10);
                            c.Width      = (int)(parent.Width * Element.Width / 100);
                            c.Height     = 25;
                            if (Element.Command != "")
                            {
                                //c.Click += new RoutedEventHandler(
                                //    delegate(object sender, RoutedEventArgs e)
                                //    {
                                //        _ViewModel.Command(Element.Command);
                                //    });
                            }
                            parent.Controls.Add(c);
                            break;
                        }
                }
            }
        }

        private Binding CreateTwoWayBinding(string PropertyPath, object source)
        {
            Binding myBinding = new Binding(PropertyPath, source, PropertyPath);
            //myBinding.Source = source;
            //myBinding.Path = new PropertyPath(PropertyPath);
            //myBinding.Mode = BindingMode.TwoWay;
            //myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            return myBinding;
        }

        private static string GetBindingArgument(string xml)
        {
            int Start = "{Binding:".Length;
            int End = xml.IndexOf('}', Start);
            string PropertyPath = xml.Substring(Start, End - Start);
            return PropertyPath;
        }

        #region IFramework
        MessageResult IFramework.Message(string text, string caption)
        {
            MessageBox.Show(text, caption);
            return MessageResult.OK;
        }

        void IFramework.Close(int result)
        {
            _Parent.Close();
        }
        #endregion
    }
}
