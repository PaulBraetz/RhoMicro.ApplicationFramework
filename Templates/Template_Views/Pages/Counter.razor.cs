namespace Template_Views.Pages;

[RenderModeHelperComponentsAttributeImpl]
public partial class Counter;

file sealed class RenderModeHelperComponentsAttributeImpl : RhoMicro.ApplicationFramework.Presentation.Views.Blazor.RenderModeHelperComponentsAttribute
{
    public override Type FrameType => typeof(RenderModeFrame);
    public override Type WrapperType => typeof(RenderModeWrapper);
}

[RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]
file sealed class RenderModeFrame : Counter
{
    protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)
    {
        builder.OpenComponent<RenderModeWrapper>(0);
        builder.AddComponentRenderMode(Microsoft.AspNetCore.Components.Web.RenderMode.InteractiveAuto);
        builder.CloseComponent();
    }
}
[RhoMicro.ApplicationFramework.Presentation.Views.Blazor.ExcludeComponentFromContainer]
file sealed class RenderModeWrapper : Counter;
