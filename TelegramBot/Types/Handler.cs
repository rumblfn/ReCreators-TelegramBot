namespace TelegramBot.Types;

/// <summary>
/// Base update handler.
/// </summary>
public abstract class Handler
{
    /// <summary>
    /// Method for validating update event.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>Is valid.</returns>
    public abstract bool Validate(Context context);
    
    /// <summary>
    /// Method for handling update event.
    /// </summary>
    /// <param name="context">Update context.</param>
    /// <returns>Handled Task.</returns>
    public abstract Task Handle(Context context);
}