namespace RhoMicro.ApplicationFramework.Composition;

using SimpleInjector;

public static partial class Root
{
    private sealed class Strategy(Action<Container> strategy) : IRoot
    {
        public void Compose(Container container) => strategy.Invoke(container);
    }
}
