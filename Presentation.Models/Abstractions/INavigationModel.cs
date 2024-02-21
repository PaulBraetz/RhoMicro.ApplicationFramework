namespace RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;
using Microsoft.AspNetCore.Http;

/// <summary>
/// Represents a model that navigates to a static path.
/// </summary>
public interface INavigationModel
{
    /// <summary>
    /// Navigates to the path.
    /// </summary>
    /// <param name="query">The optional query to append to the static navigation path.</param>
    void Navigate(QueryString? query = null);
}
