namespace RhoMicro.ApplicationFramework.Common.Tests;

using RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// Contains tests for the <see cref="Progress"/> class.
/// </summary>
[TestClass]
public class ProgressTests
{
    /// <summary>
    /// Asserts that reports made in the same call will be reported sequentially.
    /// </summary>
    [TestMethod]
    public void RegistersOnSameFrame()
    {
        var reports = new List<String>();
        _ = Progress.Register<String>(reports.Add);
        Progress.Report("1");
        Progress.Report("2");
        Progress.Report("3");
        Assert.AreEqual("1", reports[0]);
        Assert.AreEqual("2", reports[1]);
        Assert.AreEqual("3", reports[2]);
    }
}
