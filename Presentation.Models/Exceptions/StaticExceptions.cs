namespace RhoMicro.ApplicationFramework.Presentation.Models.Exceptions;

internal static class StaticExceptions
{
    public static class InvalidOperation
    {
        public static InvalidOperationException ButtonDisabled => new("The button may not be clicked while disabled.");
    }
}
