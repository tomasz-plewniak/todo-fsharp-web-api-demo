/// <summary>
/// Represents the priority level of a Todo item.
/// Used for sorting and organizing todos by importance.
/// </summary>
module Domain.Todos.Priority

/// <summary>
/// Discriminated union representing the priority levels for todos.
/// </summary>
type Priority =
    | Low          // Nice to have, can be done later
    | Medium       // Normal priority (default)
    | High         // Important, should be done soon
    | Critical     // Urgent, requires immediate attention
