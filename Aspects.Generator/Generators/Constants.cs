namespace RhoMicro.ApplicationFramework.Aspects;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;

static class Constants
{
    sealed class R : IRequest<R>;

    public const String ValueTaskTypeName = "global::System.Threading.Tasks.ValueTask";
    public const String TaskTypeName = "global::System.Threading.Tasks.Task";
    public const String CtTypeName = "global::System.Threading.CancellationToken";
    public static readonly String DefaultRequestInterfaceTypeName = $"global::{typeof(IRequest<>).Namespace}.{nameof(IRequest<Object>)}";
    public static readonly String DefaultServiceInjectionInfoAttributeType = $"global::{typeof(ServiceInjectionInfoAttribute).FullName}";
    public static readonly String DefaultServiceInterfaceTypeName = $"global::{typeof(IService<,>).Namespace}.{nameof(IService<R, R>)}";
    public const String DefaultExecuteName = nameof(IService<R, R>.Execute);
}
