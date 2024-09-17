namespace SplitAspectsTestApp;

using SplitAspectsLib;
using RhoMicro.ApplicationFramework.Aspects;

partial class ConcatServiceImpl
{
    [PartialServiceMethod(RequestType = typeof(Concat), ServiceInterface = typeof(IConcatService))]
    static String Concat(String a, String b) => a + b;
    [PartialServiceMethod(RequestType = typeof(Foo), ServiceInterface = typeof(IFooService))]
    static Int32 Foo() => 1;
}
