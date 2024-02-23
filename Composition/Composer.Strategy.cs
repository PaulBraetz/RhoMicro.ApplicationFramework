namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

public static partial class Composer
{
    private sealed class Strategy(Action<Container> strategy) : IComposer
    {
        public void Compose(Container container) => strategy.Invoke(container);
    }
}
