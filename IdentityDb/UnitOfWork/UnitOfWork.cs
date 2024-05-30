using IdentityDb.Data;
using IdentityDb.Models;
using IdentityDb.Repositories;

namespace IdentityDb.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationContext _context;
        private readonly IGenericRepository<ToDoNoteModel> _toDoNoteRepository;

        public UnitOfWork(ApplicationContext context)
        {
            _context = context;
            _toDoNoteRepository = new GenericRepository<ToDoNoteModel>(_context);
        }

        public IGenericRepository<ToDoNoteModel> ToDoRepository => _toDoNoteRepository;

        public Task<int> CompleteAsync()
        {
            return _context.SaveChangesAsync();
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
