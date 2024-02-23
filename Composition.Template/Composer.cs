namespace RhoMicro.ApplicationFramework.Composition.Template;

public static class Composers
{
    public static IComposer DefaultComposer { get; }
        = Composer.Create(c =>
        {

        });
}
