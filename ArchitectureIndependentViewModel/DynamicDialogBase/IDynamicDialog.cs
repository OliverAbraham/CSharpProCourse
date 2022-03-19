namespace Abraham.UI
{
    public interface IDynamicDialog
    {
        void Init(IFramework framework);
        void Loading();
        void Loaded();

        LayoutDefinition GetLayoutDefinition();

        void Command(string source);
    }
}
