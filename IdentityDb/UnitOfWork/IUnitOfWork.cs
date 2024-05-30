using IdentityDb.Models;
using IdentityDb.Repositories;

namespace IdentityDb.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
         IGenericRepository<ToDoNoteModel> ToDoRepository { get; }

         Task<int> CompleteAsync();
    }
}
