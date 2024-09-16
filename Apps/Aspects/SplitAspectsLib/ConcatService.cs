namespace SplitAspectsLib;

using RhoMicro.ApplicationFramework.Aspects;
using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Common.Abstractions;
#pragma warning disable

partial class ConcatService
{
    [ServiceMethod(Visibility = ServiceVisibility.Public)]
    static String Concat(String a, String b) => throw new NotSupportedException("The service is not supported client-side. Make sure to register an api client implementation instead.");
}
