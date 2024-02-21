namespace RhoMicro.ApplicationFramework.Common;

/// <summary>
/// Helper class for converting boolean states between boolean and integer representations for the purpose of enabling atomic exchange operations.
/// </summary>
public static class BooleanState
{
    /// <summary>
    /// Gets the value an instance of <see cref="Int32"/> must contain in order to be considered <see langword="true"/>.
    /// </summary>
    public const Int32 TrueState = 1;
    /// <summary>
    /// Gets the value an instance of <see cref="Int32"/> must contain in order to be considered <see langword="false"/>.
    /// </summary>
    public const Int32 FalseState = 0;
    /// <summary>
    /// Converts an integer state its boolean state representation.
    /// </summary>
    /// <param name="state">The integer state to convert.</param>
    /// <returns>The boolean state representation of <paramref name="state"/>.</returns>
    public static Boolean ToBooleanState(Int32 state)
    {
        CheckStateValidity(state);
        return state == TrueState;
    }
    /// <summary>
    /// Converts a boolean state representation to its integer state representation.
    /// </summary>
    /// <param name="state">The boolean state to convert.</param>
    /// <returns>The integer state representation of <paramref name="state"/>.</returns>
    public static Int32 FromBooleanState(Boolean state) => state ? TrueState : FalseState;
    /// <summary>
    /// Gets a value indicating whether an instance of <see cref="Int32"/> represents a valid state, that is, 
    /// its value is either <see cref="TrueState"/> or <see cref="FalseState"/>.
    /// </summary>
    /// <param name="state">The integer state to assess.</param>
    /// <returns><see langword="true"/> if the value is either <see cref="TrueState"/> or <see cref="FalseState"/>; otherwise, <see langword="false"/>.</returns>
    public static Boolean IsValidState(Int32 state) => state is TrueState or FalseState;
    private static void CheckStateValidity(Int32 state)
    {
        if(!IsValidState(state))
            throw new ArgumentOutOfRangeException(nameof(state), state, $"{nameof(state)} must be either {TrueState} or {FalseState}.");
    }
}
