namespace PetProject.Model
{
    public record NewToDoRecord
    {
        public string Content { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
    }

    public sealed record ToDoRecord() : NewToDoRecord
    {
        public ToDoRecord(NewToDoRecord from, Guid id) : this()
        {
            Content = from.Content;
            IsDone = from.IsDone;
            Id = id;
        }

        public Guid Id { get; init; }
    }
}
