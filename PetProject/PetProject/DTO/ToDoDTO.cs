namespace PetProject.DTO
{
    public sealed record CreateToDoDTO(
        string Content,
        bool IsDone);

    public sealed record ToDoDTO(
        Guid Id,
        string Content,
        bool IsDone);
}
