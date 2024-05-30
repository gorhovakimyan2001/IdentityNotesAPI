namespace IdentityDb.Repositories
{
    public interface IGenericRepository<T>
    {
        public Task<IEnumerable<T>> GetAllAsync();

        public Task<T> GetAsync(int id);

        public void EditAsync(T entity);

        public void RemoveAsync(T entity);

        public Task<T> InsertAsync(T entity);

        public void StateDetach(T entity);

        public void StateModify(T entity);
    }
}
