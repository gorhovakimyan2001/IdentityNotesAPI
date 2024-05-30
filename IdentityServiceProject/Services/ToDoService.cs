using IdentityDb.Models;
using IdentityDb.UnitOfWork;
using IdentityServiceProject.Dtos;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceProject.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<IdentityUser> _userManager;

        public ToDoService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public async Task<IEnumerable<ToDoListShowDto>> GetToDoList(string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var toDoList = await _unitOfWork.ToDoRepository.GetAllAsync();

            var list = toDoList.Where(x => x.UId == user.Id).Select(item => new ToDoListShowDto
            {
                UserName = item.User.UserName ?? "",
                CreateDate = item.CreateDate,
                DeadlineDateTime = item.DeadlineDateTime,
                IsDone = item.IsDone,
                Description = item.Description,
                Title = item.Title,
            });

            return list;
        }

        public async Task<ToDoListShowDto> GetNote(ToDoRemoveDto note)
        {
            var noteDb = await _unitOfWork.ToDoRepository.GetAsync(note.Id);
            var user = await _userManager.FindByNameAsync(note.UserName);

            if (noteDb == null || user == null || user.Id != noteDb.UId)
            {
                return null;
            }

            return new ToDoListShowDto
            {
                UserName = noteDb.User.UserName ?? "",
                CreateDate = noteDb.CreateDate,
                DeadlineDateTime = noteDb.DeadlineDateTime,
                IsDone = noteDb.IsDone,
                Description = noteDb.Description,
                Title = noteDb.Title,
            };
        }

        public async Task<IEnumerable<ToDoListShowDto>> GetNotesByTitle(string title, string userName)
        {
            var user = await _userManager.FindByNameAsync(userName);
            var toDoList = await _unitOfWork.ToDoRepository.GetAllAsync();

            var list = toDoList.Where(n => n.Title == title && n.UId == user.Id)
                                            .Select(item => new ToDoListShowDto
            {
                UserName = item.User.UserName ?? "",
                CreateDate = item.CreateDate,
                DeadlineDateTime = item.DeadlineDateTime,
                IsDone = item.IsDone,
                Description = item.Description,
                Title = item.Title,
            });

            return list;
        }

        public async Task<ToDoListShowDto> InsertNote(ToDoBase newNote)
        {
            var user = await _userManager.FindByNameAsync(newNote.UserName);

            ToDoNoteModel note = new ToDoNoteModel
            {
                UId = user.Id,
                Title = newNote.Title,
                Description = newNote.Description,
                DeadlineDateTime = newNote.DeadlineDateTime,
                CreateDate = DateTime.Now,
                IsDone = false
            };

            await _unitOfWork.ToDoRepository.InsertAsync(note);
            var response = await _unitOfWork.CompleteAsync();

            if (response > 0)
            {
                return new ToDoListShowDto
                {
                    UserName = note.User.UserName ?? "",
                    CreateDate = note.CreateDate,
                    DeadlineDateTime = note.DeadlineDateTime,
                    IsDone = note.IsDone,
                    Description = note.Description,
                    Title = note.Title,
                };
            }

            return null;
        }

        public async Task<int> RemoveNote(ToDoRemoveDto noteIdAndUserName)
        {
            var user = await _userManager.FindByNameAsync(noteIdAndUserName.UserName);

            if (user == null)
            {
                return -1;
            }

            var note = await _unitOfWork.ToDoRepository.GetAsync(noteIdAndUserName.Id);  

            if (note == null)
            {
                return -1;
            }

            _unitOfWork.ToDoRepository.RemoveAsync(note); 
            var response = await _unitOfWork.CompleteAsync();

            if (response > 0)
            {
                return noteIdAndUserName.Id;
            }

            return -1;
        }

        public async Task<bool> EditNote(ToDoUpdateDto updatedNote)
        {
            var notedb = await _unitOfWork.ToDoRepository.GetAsync(updatedNote.Id);
            var user = await _userManager.FindByNameAsync(updatedNote.UserName);

            if (notedb == null || user == null || user.Id != notedb.UId)
            {
                return false;
            }

            _unitOfWork.ToDoRepository.StateDetach(notedb);
            ToDoNoteModel note = new ToDoNoteModel
            {
                Id = updatedNote.Id,
                Title = updatedNote.Title,
                Description = updatedNote.Description,
                IsDone = updatedNote.IsDone,
                DeadlineDateTime = updatedNote.DeadlineDateTime,
                CreateDate = notedb.CreateDate,
                UId = notedb.UId,
            };
            _unitOfWork.ToDoRepository.EditAsync(note);
            _unitOfWork.ToDoRepository.StateModify(note);
            int response = await _unitOfWork.CompleteAsync();
            return response > 0;
        }
    }
}
