using MediatR;
using MediatRSample.Application.Commands;
using MediatRSample.Application.Models;
using MediatRSample.Application.Notifications;
using MediatRSample.Repositories;

namespace MediatRSample.Application.Handlers
{
  public class ExcluiPessoaCommandHandler : IRequestHandler<ExcluiPessoaCommand, string>
  {
    private readonly IMediator _mediator;
    private readonly IPessoaRepository<Pessoa> _repository;
    public ExcluiPessoaCommandHandler(IMediator mediator, IPessoaRepository<Pessoa> repository)
    {
      this._mediator = mediator;
      this._repository = repository;
    }

    public async Task<string> Handle(ExcluiPessoaCommand request, CancellationToken cancellationToken)
    {
      try
      {
        await _repository.Delete(request.Id);

        await _mediator.Publish(new PessoaExcluidaNotification { Id = request.Id, IsEfetivado = true });

        return await Task.FromResult("Pessoa excluída com sucesso");
      }
      catch (Exception ex)
      {
        await _mediator.Publish(new PessoaExcluidaNotification { Id = request.Id, IsEfetivado = false });
        await _mediator.Publish(new ErroNotification
        {
          Excecao = ex.Message,
          PilhaErro = ex.StackTrace ?? "Não foi possível obter a pilha de erros"
        });
        return await Task.FromResult("Ocorreu um erro no momento da exclusão");
      }
    }
  }
}