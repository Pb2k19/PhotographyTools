namespace Photography_Tools.Components.DataTemplateSelectors;

public class PlatformTemplateSelector : DataTemplateSelector
{
    public DataTemplate DesktopTemplate { get; set; } = new();
    public DataTemplate MobileTemplate { get; set; } = new();

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
#if ANDROID || IOS
        return MobileTemplate;
#else
        return DesktopTemplate;
#endif
    }
}
