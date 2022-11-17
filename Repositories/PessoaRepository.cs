using MediatRSample.Application.Models;

namespace MediatRSample.Repositories
{
  public class PessoaRepository : IPessoaRepository<Pessoa>
  {
    private static Dictionary<int, Pessoa> pessoas = new Dictionary<int, Pessoa>();

    public async Task<IEnumerable<Pessoa>> GetAll()
    {
      return await Task.Run(() => pessoas.Values.ToList());
    }

    public async Task<Pessoa> Get(int id)
    {
      return await Task.Run(() => pessoas.GetValueOrDefault(id));
    }

    public async Task<Pessoa> Add(Pessoa pessoa)
    {
      return await Task.Run(() =>
      {
        var id = pessoas.Count() + 1;
        pessoa.Id = id;
        pessoas.Add(id, pessoa);
        return pessoa;
      });
    }

    public async Task Edit(Pessoa pessoa)
    {
      await Task.Run(() =>
      {
        pessoas.Remove(pessoa.Id);
        pessoas.Add(pessoa.Id, pessoa);
      });
    }

    public async Task Delete(int id)
    {
      await Task.Run(() => pessoas.Remove(id));
    }

    public async Task<Pessoa> GetByNome(string nome)
    {
      return await Task.Run(() => pessoas.Values.FirstOrDefault(x => x.Nome == nome));
    }
  }
}