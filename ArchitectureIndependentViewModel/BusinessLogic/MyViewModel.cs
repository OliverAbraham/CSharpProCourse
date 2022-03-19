using Abraham.UI;
using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace BusinessLogic
{
    public class MyViewModel : IDynamicDialog, INotifyPropertyChanged
    {
        #region ------------- Properties ---------------------------------------------------------
        public string Ef_1 { get; set; }
        public string Ef_2 { get; set; }
        public List<MyCombobox1> Cb_1_Items { get; set; }
        public int Cb_1_SelectedItem { get; set; }
        public List<MyListbox1> Lb_1_Items { get; set; }
        public int Lb_1_SelectedItem { get; set; }
        public bool Cb_1_IsChecked { get; set; }
        #endregion



        #region ------------- Fields --------------------------------------------------------------
        private IFramework F;
        #endregion



        #region ------------- Init ----------------------------------------------------------------
        public void Init(IFramework framework)
        {
            F = framework;
        }
        #endregion



        #region ------------- Methods -------------------------------------------------------------
        public void Loading()
        {
            Ef_1 = "MyValue1";
            Ef_2 = "MyValue2";
            NotifyPropertyChanged(nameof(Ef_1));
            NotifyPropertyChanged(nameof(Ef_2));
            Cb_1_Items = new List<MyCombobox1>();
            Cb_1_Items.Add(new MyCombobox1() { ID=1, Name="one"});
            Cb_1_Items.Add(new MyCombobox1() { ID=2, Name="two"});
            Cb_1_Items.Add(new MyCombobox1() { ID=3, Name="three"});
            Lb_1_Items = new List<MyListbox1>();
            Lb_1_Items.Add(new MyListbox1() { ID=4, Name="four"});
            Lb_1_Items.Add(new MyListbox1() { ID=5, Name="five"});
            Lb_1_Items.Add(new MyListbox1() { ID=6, Name="six"});
            Cb_1_IsChecked = true;
        }

        public void Loaded()
        {
        }
     
        public LayoutDefinition GetLayoutDefinition()
        {
            LayoutDefinition Ld = new LayoutDefinition(@"
            <Dialog Width='400' Height='400' >
                <Row Id='1'>
                    <Label      Name='Lbl_Ef_1'   Content='FirstRow'                     Width='25' />
                    <TextBox    Name='Ef_1'       Content='{Binding:Ef_1}'  Watermark='' Width='60' />
                </Row>                            
                <Row Id='2'>                      
                    <Label      Name='Lbl_Ef_2'   Content='SecondRow'                    Width='25' />
                    <TextBox    Name='Ef_2'       Content='{Binding:Ef_2}'  Watermark='' Width='60' />
                </Row>                            
                <Row Id='3'>
                    <Label      Name='Lbl_Cb_1'   Content='Combobox'                     Width='25' />
                    <ComboBox   Name='Cb_1'       ItemsSource='{Binding:Cb_1_Items}' SelectedItem='{Binding:Cb_1_SelectedItem}' Width='60' />
                </Row>                            
                <Row Id='4'>
                    <Label      Name='Lbl_Cb_1'   Content='Checkbox'                     Width='25' />
                    <CheckBox   Name='Ch_1'       IsChecked='{Binding:Cb_1_IsChecked}'   Width='60' />
                </Row>                            
                <Row Id='5'>
                    <Label      Name='Lbl_Lb_1'   Content='Listbox'                      Width='25' />
                    <ListBox    Name='Lb_1'       ItemsSource='{Binding:Lb_1_Items}' SelectedItem='{Binding:Lb_1_SelectedItem}' Width='60' Height='150'/>
                </Row>                            
                <Row Id='6'>
                </Row>                            
                <Row>                             
                    <Button     Name='btn_Save'   Content='Save'   Command='SaveClicked'    Width='30' />
                    <Button     Name='btn_Cancel' Content='Cancel' Command='CancelClicked'  Width='30' />
                </Row>
            </Dialog>
            ");
            return Ld;
        }

        public void Command(string source)
        {
            F.Message($"Command {source} raised!", "My title");
            if (source == "SaveClicked")
                Validate();
            if (source == "CancelClicked")
                F.Close();
        }
        #endregion



        #region ------------- Implementation ------------------------------------------------------
        private void Validate()
        {
            if (Ef_1.Length == 0 || Ef_2.Length == 0)
                F.Message($"Please enter values in both fields!", "Error");
        }
        #endregion



        #region ------------- INotifyPropertyChanged ---------------------------

        [NonSerialized]
        private PropertyChangedEventHandler _PropertyChanged;
        public event PropertyChangedEventHandler PropertyChanged
        {
            add
            {
                _PropertyChanged += value;
            }
            remove
            {
                _PropertyChanged -= value;
            }
        }

        public void NotifyPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler Handler = _PropertyChanged; // avoid race condition
            if (Handler != null)
                Handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion       

    }

    public class MyCombobox1
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{ID}  {Name}";
        }
    }

    public class MyListbox1
    {
        public int ID { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return $"{ID}  {Name}";
        }
    }
}
