namespace moe.yo3explorer.azusa
{
    public interface IAzusaModule
    {
        string IniKey { get; }
        string Title { get; }
        System.Windows.Forms.Control GetSelf();
        void OnLoad();
        int Priority { get; }
    }
}
