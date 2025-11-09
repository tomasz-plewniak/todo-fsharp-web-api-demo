/// <summary>
/// Represents the lifecycle status of a Todo item.
/// </summary>
module Domain.Todos.Status

/// <summary>
/// Discriminated union representing the possible lifecycle states of a Todo.
/// </summary>
type Status =
    | NotStarted   // Todo has been created but work hasn't begun
    | InProgress   // Todo is actively being worked on
    | Completed    // Todo has been finished successfully
    | Cancelled    // Todo was abandoned or is no longer relevant
