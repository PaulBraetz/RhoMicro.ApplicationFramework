namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

public static partial class Composer
{
    sealed class EmptyComposer : IComposer
    {
        public void Compose(Container container) { }
    }
}
