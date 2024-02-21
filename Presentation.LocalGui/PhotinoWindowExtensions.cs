namespace RhoMicro.ApplicationFramework.Presentation.LocalGui;

using PhotinoNET;

internal static class PhotinoWindowExtensions
{
    public static PhotinoWindow Apply(this PhotinoWindow window, DefaultPhotinoMainWindowOptions options)
    {
        options.ApplyTo(window);

        return window;
    }
}
