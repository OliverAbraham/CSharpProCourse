using System;
using System.Collections.Generic;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;

namespace Abraham.UI
{
    public class DynamicDialogWpfAdapter<T> : DynamicDialog, IFramework
    {
        #region ------------- Properties ---------------------------------------------------------
        #endregion



        #region ------------- Fields --------------------------------------------------------------
        private Window _Parent;
        private IDynamicDialog _ViewModel;
        private bool _DebugColors = false;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public DynamicDialogWpfAdapter(IDynamicDialog viewModel) : base(viewModel)
        {
            _ViewModel = viewModel;
            _ViewModel.Init(this);
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public void DoModal(Window parent)
        {
            _Parent = parent;
            _ViewModel.Loading();

            StackPanel MyStackPanel = new StackPanel();
            MyStackPanel.Orientation = Orientation.Vertical;
            MyStackPanel.Margin = new Thickness(20);
            MyStackPanel.Width = parent.Width;  
            
            CreateAllRows(parent, MyStackPanel, _LayoutDefinition.Tree);
            
            parent.Content = MyStackPanel;
            parent.ShowDialog();
            _ViewModel.Loaded();
        }
        #endregion



        #region ------------- Implementation ------------------------------------------------------
        private void CreateAllRows(Window parentWindow, StackPanel parent, List<LayoutElement> elements)
        {
            foreach (var Element in elements)
            {
                switch (Element.XMLType)
                {
                    case "Dialog":
                        parentWindow.Width  = Element.Width;
                        parentWindow.Height = Element.Height;
                        break;

                    case "Row":
                        StackPanel Row = new StackPanel();
                        if (_DebugColors) Row.Background = Brushes.LightSalmon;
                        Row.Orientation = Orientation.Horizontal;
                        Row.Width = parent.Width;  
                        Row.MinWidth  = 100;
                        Row.MinHeight = 25;
                        CreateControlsInARow(Row, Element.Children);
                        parent.Children.Add(Row);
                        break;
                }
            }
        }

        private void CreateControlsInARow(StackPanel parent, List<LayoutElement> elements)
        {
            foreach (var Element in elements)
            {
                if (Element.Width == 0)
                    Element.Width = 50;

                switch (Element.XMLType)
                {
                    case "Label":
                        Create_Label(parent, Element);
                        break;
                    case "TextBox":
                        Create_Textbox(parent, Element);
                        break;
                    case "CheckBox":
                        Create_Checkbox(parent, Element);
                        break;
                    case "ComboBox":
                        Create_Combobox(parent, Element);
                        break;
                    case "ListBox":
                        Create_Listbox(parent, Element);
                        break;
                    case "Button":
                        Create_Button(parent, Element);
                        break;
                    default: 
                        break;
                }
            }
        }

        private void Create_Label(StackPanel parent, LayoutElement Element)
        {
            Label c = new Label();
            if (_DebugColors) c.Background = Brushes.LightGray;
            c.Content = Element.Content;
            c.MinWidth = 10;
            c.Margin = new Thickness(0, 0, 10, 0);
            c.Width = parent.Width * Element.Width / 100;
            parent.Children.Add(c);
        }

        private void Create_Textbox(StackPanel parent, LayoutElement Element)
        {
            TextBox c = new TextBox();
            if (_DebugColors) c.Background = SystemColors.GradientActiveCaptionBrush;
            c.MinWidth = 10;
            c.Margin = new Thickness(0, 4, 10, 0);
            c.Width = parent.Width * Element.Width / 100;
            if (!CreatePropertyBinding(Element.Content, c, TextBox.TextProperty))
            {
                if (Element.Watermark != "")
                    c.Text = Element.Watermark;
                else
                    c.Text = Element.Content;
            }

            parent.Children.Add(c);
        }

        private void Create_Checkbox(StackPanel parent, LayoutElement Element)
        {
            CheckBox c = new CheckBox();
            if (_DebugColors) c.Background = SystemColors.GradientActiveCaptionBrush;
            c.MinWidth = 10;
            c.Margin = new Thickness(0, 4, 10, 0);
            c.Width = parent.Width * Element.Width / 100;
            CreatePropertyBinding(Element.IsChecked, c, CheckBox.IsCheckedProperty);
            parent.Children.Add(c);
        }

        private void Create_Combobox(StackPanel parent, LayoutElement Element)
        {
            ComboBox c = new ComboBox();
            if (_DebugColors) c.Background = SystemColors.GradientActiveCaptionBrush;
            c.MinWidth = 10;
            c.Margin = new Thickness(0, 4, 10, 0);
            c.Width = parent.Width * Element.Width / 100;
            CreatePropertyBinding(Element.ItemsSource, c, ComboBox.ItemsSourceProperty);
            CreatePropertyBinding(Element.SelectedItem, c, ComboBox.SelectedItemProperty);
            parent.Children.Add(c);
        }

        private void Create_Listbox(StackPanel parent, LayoutElement Element)
        {
            DataGrid c = new DataGrid();
            if (_DebugColors) c.Background = SystemColors.GradientActiveCaptionBrush;
            c.MinWidth = 10;
            c.Margin = new Thickness(0, 4, 10, 0);
            c.Width = parent.Width * Element.Width / 100;
            if (Element.Height > 0)
            {
                c.Height = Element.Height;
                if (parent.Height < c.Height)
                    parent.Height = c.Height;
            }
            CreatePropertyBinding(Element.ItemsSource, c, DataGrid.ItemsSourceProperty);
            CreatePropertyBinding(Element.SelectedItem, c, DataGrid.SelectedItemProperty);
            parent.Children.Add(c);
        }

        private void Create_Button(StackPanel parent, LayoutElement Element)
        {
            Button c = new Button();
            if (_DebugColors) c.Background = Brushes.LightSkyBlue;
            c.Content = Element.Content;
            c.Margin = new Thickness(10);
            c.Width = parent.Width * Element.Width / 100;
            c.Height = 25;
            if (Element.Command != "")
            {
                c.Click += new RoutedEventHandler(
                    delegate (object sender, RoutedEventArgs e)
                    {
                        _ViewModel.Command(Element.Command);
                    });
            }
            parent.Children.Add(c);
        }

        private bool CreatePropertyBinding(string tag, Control control, DependencyProperty dp)
        {
            if (tag.StartsWith("{Binding:"))
            {
                string PropertyPath = GetBindingArgument(tag);
                Binding myBinding = CreateTwoWayBinding(PropertyPath, _ViewModel);
                BindingOperations.SetBinding(control, dp, myBinding);
                return true;
            }
            else
            {
                return false;
            }
        }

        private Binding CreateTwoWayBinding(string PropertyPath, object source)
        {
            Binding myBinding = new Binding();
            myBinding.Source = source;
            myBinding.Path = new PropertyPath(PropertyPath);
            myBinding.Mode = BindingMode.TwoWay;
            myBinding.UpdateSourceTrigger = UpdateSourceTrigger.PropertyChanged;
            return myBinding;
        }

        private static string GetBindingArgument(string xml)
        {
            int Start = "{Binding:".Length;
            int End = xml.IndexOf('}', Start);
            string PropertyPath = xml.Substring(Start, End - Start);
            return PropertyPath;
        }
        #endregion



        #region ------------- IFramework ----------------------------------------------------------
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
