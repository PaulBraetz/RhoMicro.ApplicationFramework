namespace RhoMicro.ApplicationFramework.Presentation.Models;

using System.Text;

using RhoMicro.ApplicationFramework.Common;
using RhoMicro.ApplicationFramework.Presentation.Models.Abstractions;

/// <summary>
/// Contains extensions for the <c>RhoMicro.ApplicationFramework.Presentation.Models</c> namespace.
/// </summary>
public static class Extensions
{
    /// <summary>
    /// Returns a vlue indicating whether the validity of an input group has been set to 
    /// <see cref="InputValidityType.None"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
    /// <typeparam name="TError">The type of error displayed by this model.</typeparam>
    /// <param name="model">The model whose validity to check.</param>
    /// <returns>
    /// <see langword="true"/> if the models validity is set to 
    /// <see cref="InputValidityType.None"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static Boolean IsValidityUnset<TValue, TError>(this IInputModel<TValue, TError> model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return model.Validity == InputValidityType.None;
    }

    /// <summary>
    /// Returns a vlue indicating whether the validity of an input group has been set to 
    /// <see cref="InputValidityType.Valid"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
    /// <typeparam name="TError">The type of error displayed by this model.</typeparam>
    /// <param name="model">The model whose validity to check.</param>
    /// <returns>
    /// <see langword="true"/> if the models validity is set to 
    /// <see cref="InputValidityType.Valid"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static Boolean IsValid<TValue, TError>(this IInputModel<TValue, TError> model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return model.Validity == InputValidityType.Valid;
    }

    /// <summary>
    /// Returns a vlue indicating whether the validity of an input group has been set to 
    /// <see cref="InputValidityType.Invalid"/>.
    /// </summary>
    /// <typeparam name="TValue">The type of value obtained by this model.</typeparam>
    /// <typeparam name="TError">The type of error displayed by this model.</typeparam>
    /// <param name="model">The model whose validity to check.</param>
    /// <returns>
    /// <see langword="true"/> if the models validity is set to 
    /// <see cref="InputValidityType.Invalid"/>; otherwise,
    /// <see langword="false"/>.
    /// </returns>
    public static Boolean IsInvalid<TValue, TError>(this IInputModel<TValue, TError> model)
    {
        ArgumentNullException.ThrowIfNull(model);

        return model.Validity == InputValidityType.Invalid;
    }

    /// <summary>
    /// Obtains the values currently represented by a multi control model.
    /// </summary>
    /// <typeparam name="TSubControl">The type of subcontrol contained in the multicontrol.</typeparam>
    /// <typeparam name="TValue">The type of value to extract from the subcontrols contained in the multicontrol.</typeparam>
    /// <param name="multiControl">The multicontrol whose values to obtain.</param>
    /// <param name="getValue">The projection to apply to the subcontrols contained in the multicontrol in order to obtain values.</param>
    /// <returns>The values currently represented by the multicontrol.</returns>
    public static IEnumerable<TValue> GetValues<TSubControl, TValue>(this IMultiControlModel<TSubControl> multiControl, Func<TSubControl, TValue> getValue)
    {
        ArgumentNullException.ThrowIfNull(multiControl);

        var result = multiControl.Items.Select(getValue);

        return result;
    }
    /// <summary>
    /// Gets the full path to this node.
    /// </summary>
    /// <param name="node">The node whose full path to get.</param>
    /// <returns>The nodes full path.</returns>
    public static String GetFullPath(this INavigationTreeNode node)
    {
        ArgumentNullException.ThrowIfNull(node);

        var resultBuilder = new StringBuilder();
        appendPath(node);
        var result = resultBuilder.ToString();
        return result;

        void appendPath(INavigationTreeNode node)
        {
            if(node is INavigationTreeChildNode child)
            {
                appendPath(child.Parent);
            }

            _ = resultBuilder.Append(node.Path);
        }
    }
    /// <summary>
    /// Disables a button for the lifetime of the disposable result.
    /// </summary>
    /// <param name="button">The button to disable.</param>
    /// <returns>A disposable that, upon disposing, re-enables the button.</returns>
    public static IDisposable Disable(this IButtonModel button)
    {
        ArgumentNullException.ThrowIfNull(button);

        button.Disabled = true;
        var result = new CallbackDisposable(() => button.Disabled = false);

        return result;
    }
}
