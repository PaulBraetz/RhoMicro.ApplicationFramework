namespace RhoMicro.ApplicationFramework.Presentation.Models;
using Microsoft.AspNetCore.Http;

using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Navigates to a static path upon calling <see cref="Navigate"/>.
/// </summary>
/// <remarks>
/// Initializes a new instance.
/// </remarks>
/// <param name="navigationManager">The navigation manager to use when invoking <see cref="Navigate"/>.</param>
/// <param name="path">The path to navigate to upon invoking <see cref="Navigate"/>.</param>
public sealed class NavigationModel(INavigationManager navigationManager, IPathModel path) : INavigationModel
{
    private readonly IPathModel _path = path;

    /// <inheritdoc/>
    public void Navigate(QueryString? query = null)
    {
        String path;
        if(_path.IsAbsolute)
        {
            var builder = new UriBuilder(_path.Path);
            if(query.HasValue)
            {
                var builderQuery = new QueryString(builder.Query);
                builder.Query = query.Value.Add(builderQuery).ToString();
            }

            path = builder.Uri.ToString();
        } else
        {
            //TODO: support fragments/user credentials
            path = query.HasValue ?
                _path.Path + query.Value.ToString() :
                _path.Path;
        }

        navigationManager.NavigateTo(path);
    }
}
