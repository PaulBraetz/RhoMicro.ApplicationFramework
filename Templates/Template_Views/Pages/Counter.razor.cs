namespace Template_Views.Pages;

using System;

using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components.Web;

using RhoMicro.ApplicationFramework.Presentation.Views.Blazor.DependencyInjection;

[RenderModeHelperComponentsAttributeImpl]
public partial class Counter;

file sealed class RenderModeHelperComponentsAttributeImpl : RenderModeHelperComponentsAttribute
{
    public override Type FrameType => typeof(RenderModeFrame);
    public override Type WrapperType => typeof(RenderModeWrapper);
}

[ExcludeComponentFromContainer]
file sealed class RenderModeFrame : Counter
{
    protected override void BuildRenderTree(RenderTreeBuilder builder)
    {
        builder.OpenComponent<RenderModeWrapper>(0);
        builder.AddComponentRenderMode(RenderMode.InteractiveAuto);
        builder.CloseComponent();
    }
}
[ExcludeComponentFromContainer]
file sealed class RenderModeWrapper : Counter;
