namespace Dr.Peril.Interpreter;

/// <summary>
///   <para>
///     A parsed command from the user.
///   </para>
///   <para>
///     All superfluous information has been removed.
///   </para>
///   <remarks>
///     All returned values are guaranteed to be lowercase.
///   </remarks>
/// </summary>
/// <param name="IsSuccessful">
///   False when cannot parse the users input.  In this case all other will contain an empty string.
/// </param>
/// <param name="Verb">Always supplied, when successful.</param>
/// <param name="Object">An optional object to apply the action to.</param>
/// <param name="Adjunct">An optional modifier.</param>
/// <example>
///   <code>
///     open the door
///   </code>
///   returns: verb = "open, object = "door", adjunct = ""
/// </example>
/// <example>
///   <code>
///     please open the door with the key
///   </code>
///   returns: verb = "open", object = "door", adjunct = "key"
///   </example>
public readonly record struct Command(
    bool IsSuccessful,
    string Verb,
    string Object,
    string Adjunct);
