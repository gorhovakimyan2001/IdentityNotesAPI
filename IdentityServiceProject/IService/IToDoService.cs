using IdentityServiceProject.Dtos;

namespace IdentityServiceProject.IService
{
    public interface IToDoService
    {
        public Task<IEnumerable<ToDoListShowDto>> GetToDoList();

        public Task<ToDoListShowDto> GetNote(int id);

        public Task<IEnumerable<ToDoListShowDto>> GetNotesByTitle(string title);

        public Task<ToDoListShowDto> InsertNote(ToDoBase newNote);

        public Task<int> RemoveNote(int id);

        public Task<bool> EditNote(ToDoUpdateDto updatedNote);
    }
}
