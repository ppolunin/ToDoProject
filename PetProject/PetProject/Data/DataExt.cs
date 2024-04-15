using PetProject.DTO;
using PetProject.Model;

namespace PetProject.Data
{
    public static class DataExt
    {
        public static ToDo ToEntity(this DTOToDo dto) => new()
        {
            Id = dto.Id,
            Content = dto.Content,
            IsDone = dto.IsDone
        };

        public static ToDo ToEntity(this DTOToDoForPost dto, in Guid id) => new()
        {
            Id = id,
            Content = dto.Content,
            IsDone = dto.IsDone
        };

        public static DTOToDo ToDTO(this ToDo entity) => new(
            Id: entity.Id, 
            Content: entity.Content, 
            IsDone: entity.IsDone);
    }
}
