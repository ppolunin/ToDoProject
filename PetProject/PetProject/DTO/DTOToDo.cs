namespace PetProject.DTO
{
    public sealed record DTOToDoForPost(
        string Content,
        bool IsDone);

    public sealed record DTOToDo(
        Guid Id,
        string Content,
        bool IsDone);
}
