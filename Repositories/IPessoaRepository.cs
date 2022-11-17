namespace MediatRSample.Repositories
{
  public interface IPessoaRepository<T> : IRepository<T>
  {
    Task<T> GetByNome(string nome);
  }
}