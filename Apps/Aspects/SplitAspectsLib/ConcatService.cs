namespace SplitAspectsLib;

using RhoMicro.ApplicationFramework.Aspects;
#pragma warning disable

partial class ConcatService
{
    [ServiceMethod(Visibility = ServiceVisibility.Public, ServiceInterfaceName = "IConcatService")]
    static String Concat(String a, String b) => throw new NotSupportedException("The service is not supported client-side. Make sure to register an api client implementation instead.");
    [ServiceMethod(Visibility = ServiceVisibility.Public, ServiceInterfaceName = "IFooService")]
    static int Foo() => throw new NotSupportedException("The service is not supported client-side. Make sure to register an api client implementation instead.");
}
