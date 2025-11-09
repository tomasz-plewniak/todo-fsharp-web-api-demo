/// <summary>
/// Domain model for Todo entities.
/// Contains value objects, entities, and factory functions for working with todos.
/// </summary>
module Domain.Todos.TodoEntity

open System
open Domain.Todos.Status
open Domain.Todos.Priority

// ============================================================================
// VALUE OBJECTS
// ============================================================================
// Value objects provide type safety and encapsulation of domain rules.
// They are immutable and their constructors are private to enforce validation.

/// <summary>
/// Unique identifier for a Todo item.
/// Wraps a Guid to provide type safety and prevent mixing IDs from different entities.
/// </summary>
type TodoId = TodoId of Guid

/// <summary>
/// Todo title value object.
/// Private constructor ensures all titles are validated through the Title.create function.
/// </summary>
type Title = private Title of string

/// <summary>
/// Module containing functions for working with Title value objects.
/// </summary>
module Title =
    /// <summary>
    /// Creates a new Title with validation.
    /// Ensures the title is not empty and does not exceed 200 characters.
    /// </summary>
    /// <param name="value">The title string to validate and wrap</param>
    /// <returns>Result with Title on success, or error message on validation failure</returns>
    let create (value: string) =
        if String.IsNullOrWhiteSpace(value) then
            Error "Title cannot be empty"
        elif value.Length > 200 then
            Error "Title cannot exceed 200 characters"
        else
            Ok (Title value)

    /// <summary>
    /// Extracts the string value from a Title.
    /// Uses pattern matching to unwrap the private constructor.
    /// </summary>
    /// <param name="Title v">The Title to unwrap</param>
    /// <returns>The underlying string value</returns>
    let value (Title v) = v

/// <summary>
/// Todo description value object.
/// Wraps an optional string - None represents an empty description.
/// Private constructor ensures consistent handling of empty/null descriptions.
/// </summary>
type Description = private Description of string option

/// <summary>
/// Module containing functions for working with Description value objects.
/// </summary>
module Description =
    /// <summary>
    /// Creates a new Description from a string.
    /// Automatically converts null/whitespace to None for consistent handling.
    /// </summary>
    /// <param name="value">The description string</param>
    /// <returns>Description value object (None if empty/whitespace)</returns>
    let create (value: string) =
        if String.IsNullOrWhiteSpace(value) then
            Description None
        else
            Description (Some value)

    /// <summary>
    /// Represents an empty description.
    /// </summary>
    let empty = Description None

    /// <summary>
    /// Extracts the optional string value from a Description.
    /// </summary>
    /// <param name="Description v">The Description to unwrap</param>
    /// <returns>The underlying optional string value</returns>
    let value (Description v) = v

// ============================================================================
// MAIN TODO ENTITY
// ============================================================================

/// <summary>
/// Main Todo aggregate root entity.
/// Represents a task or item to be completed with all its properties and metadata.
/// This is an immutable F# record - all updates create new instances.
/// </summary>
type Todo = {
    /// Unique identifier for this todo
    Id: TodoId

    /// The todo's title (required, validated)
    Title: Title

    /// Optional detailed description
    Description: Description

    /// Current lifecycle status
    Status: Status

    /// Priority level for organization
    Priority: Priority

    /// Optional due date for deadline tracking
    DueDate: DateTime option

    /// Timestamp when the todo was first created (UTC)
    CreatedAt: DateTime

    /// Timestamp of the last modification (UTC)
    UpdatedAt: DateTime

    /// Timestamp when the todo was completed (None if not completed)
    CompletedAt: DateTime option
}

// ============================================================================
// FACTORY FUNCTIONS AND BEHAVIORS
// ============================================================================

/// <summary>
/// Module containing factory functions and behaviors for Todo entities.
/// All functions are pure and return new instances rather than mutating state.
/// </summary>
module Todo =
    /// <summary>
    /// Creates a new Todo with default values.
    /// The todo starts in NotStarted status with Medium priority.
    /// </summary>
    /// <param name="title">Validated title for the todo</param>
    /// <returns>A new Todo entity with initialized default values</returns>
    let create (title: Title) =
        {
            Id = TodoId (Guid.NewGuid())
            Title = title
            Description = Description.empty
            Status = NotStarted
            Priority = Medium
            DueDate = None
            CreatedAt = DateTime.UtcNow
            UpdatedAt = DateTime.UtcNow
            CompletedAt = None
        }

    /// <summary>
    /// Marks a Todo as completed.
    /// Updates the status to Completed and records the completion timestamp.
    /// </summary>
    /// <param name="todo">The todo to mark as complete</param>
    /// <returns>A new Todo instance with updated status and timestamps</returns>
    let complete (todo: Todo) =
        { todo with
            Status = Completed
            CompletedAt = Some DateTime.UtcNow
            UpdatedAt = DateTime.UtcNow }

    /// <summary>
    /// Updates the title of a Todo.
    /// </summary>
    /// <param name="title">The new validated title</param>
    /// <param name="todo">The todo to update</param>
    /// <returns>A new Todo instance with the updated title</returns>
    let updateTitle (title: Title) (todo: Todo) =
        { todo with
            Title = title
            UpdatedAt = DateTime.UtcNow }

    /// <summary>
    /// Changes the priority level of a Todo.
    /// </summary>
    /// <param name="priority">The new priority level</param>
    /// <param name="todo">The todo to update</param>
    /// <returns>A new Todo instance with the updated priority</returns>
    let setPriority (priority: Priority) (todo: Todo) =
        { todo with
            Priority = priority
            UpdatedAt = DateTime.UtcNow }
