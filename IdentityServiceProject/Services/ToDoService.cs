using IdentityDb.Data;
using IdentityDb.Models;
using IdentityServiceProject.Dtos;
using Microsoft.EntityFrameworkCore;

namespace IdentityServiceProject.Services
{
    public class ToDoService
    {
        private readonly ApplicationContext _context;

        public ToDoService(ApplicationContext context)
        {
            _context = context;
        }

        public IEnumerable<ToDoListShowDto> GetToDoList(string userName)
        {
            if (userName == null)
            {
                throw new NullReferenceException("Please insert User Name");
            }

            var list = _context.ToDos.Where(x => x.UserName == userName);
            List<ToDoListShowDto> showLsit = new List<ToDoListShowDto>();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    showLsit.Add(new ToDoListShowDto
                    {
                        UserName = item.UserName,
                        CreateDate = item.CreateDate,
                        DeadlineDateTime = item.DeadlineDateTime,
                        IsDone = item.IsDone,
                        Description = item.Description,
                        Title = item.Title,
                    });
                }
            }

            return showLsit;
        }

        public ToDoListShowDto GetNote(int id)
        {
            if (id <= 0 )
            {
                throw new ArgumentException();
            }

            var noteDb = _context.ToDos.FirstOrDefault(n => n.Id == id);

            if (noteDb == null)
            {
                throw new Exception("Note with that ID does not exist!!!");
            }

            var note = new ToDoListShowDto
            {
                UserName = noteDb.UserName,
                CreateDate = noteDb.CreateDate,
                DeadlineDateTime = noteDb.DeadlineDateTime,
                IsDone = noteDb.IsDone,
                Description = noteDb.Description,
                Title = noteDb.Title,
            };

            return note;
        }

        public IEnumerable<ToDoListShowDto> GetNotesByTitle(string title, string userName)
        {
            if (string.IsNullOrEmpty(title))
            {
                throw new ArgumentNullException();
            }

            var list = _context.ToDos.Where(n => n.Title == title && n.UserName == userName);
            List<ToDoListShowDto> showLsit = new List<ToDoListShowDto>();

            if (list.Any())
            {
                foreach (var item in list)
                {
                    showLsit.Add(new ToDoListShowDto
                    {
                        UserName = item.UserName,
                        CreateDate = item.CreateDate,
                        DeadlineDateTime = item.DeadlineDateTime,
                        IsDone = item.IsDone,
                        Description = item.Description,
                        Title = item.Title,
                    });
                }
            }

            return showLsit;
        }

        public ToDoListShowDto InsertNote(ToDoServiceInsertDto newNote)
        {
            if (newNote == null)
            {
                throw new ArgumentNullException(nameof(newNote));
            }

            ToDoNoteModel note = new ToDoNoteModel
            {
                UserName = newNote.UserName,
                Title = newNote.Title,
                Description = newNote.Description,
                DeadlineDateTime = newNote.DeadlineDateTime,
                CreateDate = DateTime.Now,
                IsDone = false
            };

            _context.ToDos.Add(note);
            var response = _context.SaveChanges();

            if (response > 0)
            {
                return new ToDoListShowDto
                {
                    UserName = note.UserName,
                    CreateDate = note.CreateDate,
                    DeadlineDateTime = note.DeadlineDateTime,
                    IsDone = note.IsDone,
                    Description = note.Description,
                    Title = note.Title,
                };
            }

            throw new Exception("Creation of new note is failed");
        }

        public int RemoveNote(int noteId, string userName)
        {
            var note = _context.ToDos.FirstOrDefault(n => n.Id == noteId && n.UserName == userName);  

            if (note == null)
            {
                return -1;
            }

            _context.ToDos.Remove(note);
            var response = _context.SaveChanges();

            if(response > 0)
            {
                return noteId;
            }

            return -1;
        }

        public bool UpdateNote(ToDoUpdateDto updatedNote)
        {
            if (updatedNote == null)
            {
                throw new ArgumentNullException(nameof(updatedNote));
            }

            var notedb = _context.ToDos.FirstOrDefault(n => n.Id ==  updatedNote.Id);

            if (notedb == null)
            {
                return false;
            }

            _context.Entry(notedb).State = EntityState.Detached;
            ToDoNoteModel note = new ToDoNoteModel
            {
                Id = updatedNote.Id,
                Title = updatedNote.Title,
                Description = updatedNote.Description,
                IsDone = updatedNote.IsDone,
                DeadlineDateTime = updatedNote.DeadlineDateTime,
                CreateDate = notedb.CreateDate,
                UserName = notedb.UserName,
            };
            _context.ToDos.Attach(note);
            _context.Entry(note).State = EntityState.Modified;
            int response = _context.SaveChanges();
            return response > 0;
        }
    }
}
