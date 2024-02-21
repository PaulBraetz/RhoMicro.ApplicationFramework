namespace RhoMicro.ApplicationFramework.Tests.Common;

using Microsoft.VisualStudio.TestTools.UnitTesting;

/// <summary>
/// Contains assertions for tests.
/// </summary>
public static class Assertions
{
    /// <summary>
    /// Asserts that an action will throw an ArgumentNullException with an expected exception parameter name.
    /// </summary>
    /// <param name="expected">The expected exception parameter name.</param>
    /// <param name="action">The action to test if a ArgumentNullException is thrown.</param>
    public static void AssertArgumentNullException(String expected, Action action)
    {
        ArgumentNullException.ThrowIfNull(action);

        //Act
        var actual = String.Empty;
        try
        {
            action.Invoke();
        } catch(ArgumentNullException ex)
        {
            actual = ex.ParamName;
        }

        //Assert
        Assert.AreEqual(expected, actual);
    }
}
