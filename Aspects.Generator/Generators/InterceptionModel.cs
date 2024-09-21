namespace RhoMicro.ApplicationFramework.Aspects;

using RhoMicro.CodeAnalysis.Library;

readonly record struct InterceptionModel(
    EquatableList<InterceptedParameterModel> InterceptedParameters,
    EquatableList<InterceptorTypeArgumentModel> RequiredInterceptorTypeArguments)
{
    public Boolean HasAny => InterceptedParameters.Count > 0;
}
