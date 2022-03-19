namespace Abraham.UI
{
    public abstract class DynamicDialog
    {
        public IDynamicDialog Instance;
        protected LayoutDefinition _LayoutDefinition;

        public DynamicDialog(IDynamicDialog instance)
        {
            Instance = instance;
            _LayoutDefinition = Instance.GetLayoutDefinition();
            _LayoutDefinition.Convert_layout_XML_to_element_tree();
        }
    }
}
