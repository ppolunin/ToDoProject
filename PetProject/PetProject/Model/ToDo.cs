namespace PetProject.Model
{
    public sealed class ToDo
    {
        public Guid Id { get; init; }
        public string Content { get; set; } = string.Empty;
        public bool IsDone { get; set; } = false;
    }
}
