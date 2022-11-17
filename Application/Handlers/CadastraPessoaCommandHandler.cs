using MediatR;
using MediatRSample.Application.Commands;
using MediatRSample.Application.Models;
using MediatRSample.Application.Notifications;
using MediatRSample.Repositories;

namespace MediatRSample.Application.Handlers
{
  public class CadastraPessoaCommandHandler : IRequestHandler<CadastraPessoaCommand, string>
  {
    private readonly IMediator _mediator;
    private readonly IPessoaRepository<Pessoa> _repository;
    public CadastraPessoaCommandHandler(IMediator mediator, IPessoaRepository<Pessoa> repository)
    {
      this._mediator = mediator;
      this._repository = repository;
    }

    public async Task<string> Handle(CadastraPessoaCommand request, CancellationToken cancellationToken)
    {
      if (
        string.IsNullOrEmpty(request.Nome)
       || request.Idade == 0
       || !char.IsLetter(request.Sexo)
       )
        return "Preencha todos os campos";

      var pessoa = new Pessoa { Nome = request.Nome, Idade = request.Idade, Sexo = request.Sexo };

      try
      {
        var pessoaExiste = await _repository.GetByNome(request.Nome);

        if (pessoaExiste != null)
          return "Pessoa já cadastrada";

        pessoa = await _repository.Add(pessoa);

        await _mediator.Publish(new PessoaCriadaNotification { Id = pessoa.Id, Nome = pessoa.Nome, Idade = pessoa.Idade, Sexo = pessoa.Sexo });

        return await Task.FromResult("Pessoa criada com sucesso");
      }
      catch (Exception ex)
      {
        await _mediator.Publish(new PessoaCriadaNotification { Id = pessoa.Id, Nome = pessoa.Nome, Idade = pessoa.Idade, Sexo = pessoa.Sexo });
        await _mediator.Publish(new ErroNotification
        {
          Excecao = ex.Message,
          PilhaErro = ex.StackTrace ?? "Não foi possível obter a pilha de erros"
        });
        return await Task.FromResult("Ocorreu um erro no momento da criação");
      }
    }
  }
}