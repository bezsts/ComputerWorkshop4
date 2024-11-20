namespace ApiDomain.Repositories.Contracts
{
    public interface IUnitOfWork
    {
        IMovieRepository Movies { get; }
        IUserRepository Users { get; }

        Task<bool> SaveChangesAsync(CancellationToken cancellationToken = default);
    }
}
