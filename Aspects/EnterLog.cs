namespace RhoMicro.ApplicationFramework.Aspects;

/// <summary>
/// Enters a log to the underlying logger.
/// </summary>
/// <param name="message">The message to log.</param>
/// <param name="parameters">The parameters using which to format the message.</param>
public delegate void EnterLog(String message, params Object[] parameters);
