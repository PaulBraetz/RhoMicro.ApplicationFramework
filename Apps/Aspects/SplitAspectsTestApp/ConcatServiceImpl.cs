namespace SplitAspectsTestApp;

using SplitAspectsLib;
using RhoMicro.ApplicationFramework.Aspects;

partial class ConcatServiceImpl
{
    [ServiceMethodImplementation(Request = typeof(Concat), Service = typeof(IConcatService))]
    static String Concat([Intercept] String a, String b)
    {
        return a + b;
    }

    [ServiceMethodImplementation(Request = typeof(Foo), Service = typeof(IFooService))]
    static Int32 Foo() => 1;
}
