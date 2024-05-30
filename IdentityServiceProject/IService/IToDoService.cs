using IdentityDb.Models;
using IdentityServiceProject.Dtos;
using Microsoft.EntityFrameworkCore;
namespace IdentityServiceProject.IService
{
    public interface IToDoService
    {
        public Task<IEnumerable<ToDoListShowDto>> GetToDoList(string userName);

        public Task<ToDoListShowDto> GetNote(ToDoRemoveDto note);

        public Task<IEnumerable<ToDoListShowDto>> GetNotesByTitle(string title, string userName);

        public Task<ToDoListShowDto> InsertNote(ToDoBase newNote);

        public Task<int> RemoveNote(ToDoRemoveDto note);

        public Task<bool> EditNote(ToDoUpdateDto updatedNote);
    }
}
