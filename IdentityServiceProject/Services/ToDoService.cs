using IdentityDb.Models;
using IdentityDb.UnitOfWork;
using IdentityServiceProject.Dtos;
using IdentityServiceProject.Helpers;
using IdentityServiceProject.IService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace IdentityServiceProject.Services
{
    public class ToDoService : IToDoService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserHelper _userHelper;

        public ToDoService(IUnitOfWork unitOfWork, UserManager<IdentityUser> userManager,
                            IHttpContextAccessor httpContextAccessor)
        {
            _userHelper = new UserHelper(userManager, httpContextAccessor);
            _unitOfWork = unitOfWork;
        }

        public async Task<IEnumerable<ToDoListShowDto>> GetToDoList()
        {
            var userDb = await _userHelper.GetCurrentUser();
            if (userDb == null)
            {
                return new List<ToDoListShowDto>();
            }

            var toDoList = await _unitOfWork.ToDoRepository.GetAllAsync();
            var list = toDoList.Where(x => x.UId == userDb.Id).Select(item => new ToDoListShowDto
            {
                UserName = item.User.UserName ?? string.Empty,
                CreateDate = item.CreateDate,
                DeadlineDateTime = item.DeadlineDateTime,
                IsDone = item.IsDone,
                Description = item.Description,
                Title = item.Title,
            });

            return list;
        }

        public async Task<ToDoListShowDto> GetNote(int id)
        {
            var user = await _userHelper.GetCurrentUser();
            var noteDb = await _unitOfWork.ToDoRepository.GetAsync(id);

            if (noteDb == null || user == null || user.Id != noteDb.UId)
            {
                return null;
            }

            return new ToDoListShowDto
            {
                UserName = noteDb.User.UserName ?? string.Empty,
                CreateDate = noteDb.CreateDate,
                DeadlineDateTime = noteDb.DeadlineDateTime,
                IsDone = noteDb.IsDone,
                Description = noteDb.Description,
                Title = noteDb.Title,
            };
        }

        public async Task<IEnumerable<ToDoListShowDto>> GetNotesByTitle(string title)
        {
            var user = await _userHelper.GetCurrentUser();
            var toDoList = await _unitOfWork.ToDoRepository.GetAllAsync();

            var list = toDoList.Where(n => n.Title == title && n.UId == user.Id)
                                            .Select(item => new ToDoListShowDto
            {
                UserName = item.User.UserName ?? string.Empty,
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
            var user = await _userHelper.GetCurrentUser();

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
                    UserName = note.User.UserName ?? string.Empty,
                    CreateDate = note.CreateDate,
                    DeadlineDateTime = note.DeadlineDateTime,
                    IsDone = note.IsDone,
                    Description = note.Description,
                    Title = note.Title,
                };
            }

            return null;
        }

        public async Task<int> RemoveNote(int id)
        {
            var user = await _userHelper.GetCurrentUser();

            if (user == null)
            {
                return -1;
            }

            var note = await _unitOfWork.ToDoRepository.GetAsync(id);  

            if (note == null)
            {
                return -1;
            }

            _unitOfWork.ToDoRepository.RemoveAsync(note); 
            var response = await _unitOfWork.CompleteAsync();

            if (response > 0)
            {
                return id;
            }

            return -1;
        }

        public async Task<bool> EditNote(ToDoUpdateDto updatedNote)
        {
            var notedb = await _unitOfWork.ToDoRepository.GetAsync(updatedNote.Id);
            var user = await _userHelper.GetCurrentUser();

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
