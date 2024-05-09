namespace RhoMicro.CodeAnalysis.Library.Text;
using System;

using static IndentedStringBuilder.Appendables;

partial class IndentedStringBuilder
{
    public IndentedStringBuilder AppendProxy(
        String className,
        String typeParametersString,
        String[] typeConstraints) =>
        ( Operators +
        "[global::RhoMicro.ApplicationFramework.Hosting.ExcludeComponentFromContainer]" + NewLine +
        "[RenderModeWrapperAttributeImpl]" + NewLine +
        "file sealed class RenderModeProxy" + typeParametersString + " : " + className + typeParametersString + NewLine +
        Appendables.AppendJoinLines(StringOrChar.Empty, typeConstraints) + ';' )
        .Builder;
    public IndentedStringBuilder AppendWrapper(
        String className,
        String typeParametersString,
        String noOpRenderModeType,
        String[] typeConstraints) =>
        ( Operators +
        "[global::RhoMicro.ApplicationFramework.Hosting.ExcludeComponentFromContainer]" + NewLine +
        "[RenderModeProxyAttributeImpl]" + NewLine +
        "file sealed class RenderModeWrapper" + typeParametersString + " : " + className + typeParametersString + NewLine +
        Appendables.AppendJoinLines(separator: StringOrChar.Empty, typeConstraints) +
        Appendables.OpenBracesBlock() +

            "/// <inheritdoc/>" + NewLine +
            "[global::RhoMicro.ApplicationFramework.Hosting.Injected]" + NewLine +
            "public global::RhoMicro.ApplicationFramework.Hosting.IRenderModeInterceptor RenderModeInterceptor { get; set; }" + NewLine +
        
            "static RenderModeWrapper()" +
            Appendables.OpenBracesBlock() +
                "_parameters = [];" + NewLine +
                "var propertyInfos = typeof(" + className + typeParametersString + ").GetProperties(global::System.Reflection.BindingFlags.Instance | global::System.Reflection.BindingFlags.Public);" + NewLine +
                "foreach(var propertyInfo in propertyInfos)" +
                Appendables.OpenBracesBlock() +
                    Appendables.AppendJoinLines(separator: StringOrChar.Empty, [
                        "if(global::System.Reflection.CustomAttributeExtensions.GetCustomAttribute<global::Microsoft.AspNetCore.Components.ParameterAttribute>(propertyInfo) == null) continue;",
                        "var accessorInfo = propertyInfo.GetMethod;",
                        "if(accessorInfo == null) continue;"]) + NewLine +
                    "var parameter = global::System.Linq.Expressions.Expression.Parameter(typeof(" + className + typeParametersString + "));" + NewLine +
                    "var callExpr = global::System.Linq.Expressions.Expression.Call(parameter, accessorInfo);" + NewLine +
                    "var castExpr = global::System.Linq.Expressions.Expression.Convert(callExpr, typeof(global::System.Object));" + NewLine +
                    "var getAccessor = (global::System.Func<" + className + typeParametersString + ", global::System.Object?>)global::System.Linq.Expressions.Expression.Lambda(castExpr, parameter).Compile();" + NewLine +
                    "var name = propertyInfo.Name;" + NewLine +
                    "_parameters.Add((name, getAccessor));" +
                Appendables.CloseBlock() +
            Appendables.CloseBlock() +
            //TODO: generate static parameter list instead of reflection -> symbol not entirely available due to razor generator, so no compile time list
            "private static readonly global::System.Collections.Generic.List<(global::System.String name, global::System.Func<" + className + typeParametersString + ", global::System.Object?> getAccessor)> _parameters;" + NewLine +
            "protected override void BuildRenderTree(global::Microsoft.AspNetCore.Components.Rendering.RenderTreeBuilder builder)" +
            Appendables.OpenBracesBlock() +
                "var actualRenderMode = this.RenderModeInterceptor.GetRenderMode((__IOptionalRenderModeComponent)this);" + NewLine +

                "((__IHasFileScopeMembers)this).ReceiveActualRenderMode(actualRenderMode);" + NewLine +

                "if(actualRenderMode is " + noOpRenderModeType + ')' +
                Appendables.OpenBracesBlock() +
                    "base.BuildRenderTree(builder);" + NewLine +
                    "return;" +
                Appendables.CloseBlock() +

                "builder.OpenComponent(1, typeof(RenderModeProxy" + typeParametersString + "));" + NewLine +
                "for(var i = 0; i < _parameters.Count; i++)" +
                Appendables.OpenBracesBlock() +
                    Appendables.AppendJoinLines(StringOrChar.Empty, [
                    "var (name, getAccessor) = _parameters[i];",
                    "var value = getAccessor.Invoke(this);",
                    "builder.AddComponentParameter(i + 2, name, value);" ]) +
                Appendables.CloseBlock() +
                "if(actualRenderMode is not __NoOpRenderMode)" +
                Appendables.OpenBracesBlock() +
                    "builder.AddComponentRenderMode(actualRenderMode);" +
                Appendables.CloseBlock() +
                "builder.CloseComponent();" + NewLine +
            Appendables.CloseBlock() +
        Appendables.CloseBlock() )
        .Builder;
    public IndentedStringBuilder AppendAttributes(
        String className,
        String typeParametersOpenString,
        String[] typeParameters) =>
        ( Operators +
        "file sealed class RenderModeHelperComponentsAttributeImpl : global::RhoMicro.ApplicationFramework.Hosting.RenderModeHelperComponentsAttribute" +
        Appendables.OpenBracesBlock() +
            "public override global::System.Type OpenWrapperType { get; } = typeof(RenderModeWrapper" + typeParametersOpenString + ");" + NewLine +
            "public override global::System.Type OpenProxyType { get; } = typeof(RenderModeProxy" + typeParametersOpenString + ");" +
        Appendables.CloseBlock() +
        "file sealed class RenderModeProxyAttributeImpl : global::RhoMicro.ApplicationFramework.Hosting.RenderModeProxyAttribute" +
        Appendables.OpenBracesBlock() +
            "public override global::System.Type GetConstructedComponentType(global::System.Type proxyType) => typeof(" + className + typeParametersOpenString + ')' + Appendables.Append(b =>
            {
                if(typeParameters.Length == 0)
                    return;
                b.AppendCore(".MakeGenericType(proxyType.GetGenericArguments())");
            }) + ';' +
        Appendables.CloseBlock() +
        "file sealed class RenderModeWrapperAttributeImpl : global::RhoMicro.ApplicationFramework.Hosting.RenderModeWrapperAttribute" +
        Appendables.OpenBracesBlock() +
            "public override global::System.Type GetConstructedComponentType(global::System.Type wrapperType) => typeof(" + className + typeParametersOpenString + ')' + Appendables.Append(b =>
            {
                if(typeParameters.Length == 0)
                    return;
                b.AppendCore(".MakeGenericType(wrapperType.GetGenericArguments())");
            }) + ';' +
        Appendables.CloseBlock() )
        .Builder;
    public IndentedStringBuilder AppendComponent(
        String className,
        String typeParametersString,
        String fullyQualifiedRenderModeExpr,
        String[] typeConstraints) =>
        ( Operators +
        "file interface __IHasFileScopeMembers" +
        Appendables.OpenBracesBlock() +
            "void ReceiveActualRenderMode(__IComponentRenderMode? actualRenderMode);" + NewLine +
        Appendables.CloseBlock() +
        "[RenderModeHelperComponentsAttributeImpl]" + NewLine +
        "partial class " + className + typeParametersString + " : __IOptionalRenderModeComponent, __IHasFileScopeMembers" +
        Appendables.AppendJoinLines(separator: StringOrChar.Empty, typeConstraints) +
        Appendables.OpenBracesBlock() +
            "/// <inheritdoc/>" + NewLine +
            "[global::Microsoft.AspNetCore.Components.Parameter]" + NewLine +
            "public __IComponentRenderMode? OptionalRenderMode { get; set; } = " + fullyQualifiedRenderModeExpr + ';' + NewLine +

            "/// <summary>Invoked upon receiving the actual render mode applied to the component. This event will be raised whenever the component is about to be rendered. It is therefore critical not to cause rerenders in handlers; lest a stack overflow is the desired behavior.</summary>" + NewLine +
            "private event global::System.EventHandler<__IComponentRenderMode?>? ActualRenderModeReceived;" + NewLine +
            "/// <summary>Receives the actual render mode applied to the component. Depending on the runtime context, this value could differ from <see cref=\"OptionalRenderMode\"/>.</summary>" + NewLine +
            "/// <param name=\"actualRenderMode\">The render mode actually applied to the component.</param>" + NewLine +
            "void __IHasFileScopeMembers.ReceiveActualRenderMode(__IComponentRenderMode? actualRenderMode) => ActualRenderModeReceived?.Invoke(this, actualRenderMode);" +
        Appendables.CloseBlock() )
        .Builder;
    public IndentedStringBuilder AppendUsings(String[] usings) =>
        ( Operators +
        Appendables.Append(b =>
        {
            for(var i = 0; i < usings.Length; i++)
            {
                b.Append("using ").Append(usings[i]).Append(';').AppendLineCore();
            }
        }) +
        "using __IComponentRenderMode = global::Microsoft.AspNetCore.Components.IComponentRenderMode;" + NewLine +
        "using __IOptionalRenderModeComponent = global::RhoMicro.ApplicationFramework.Hosting.IOptionalRenderModeComponent;" + NewLine +
        "using __NoOpRenderMode = global::RhoMicro.ApplicationFramework.Hosting.NoOpRenderMode;" + NewLine )
        .Builder;
}
