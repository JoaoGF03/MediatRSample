using MediatR;
using MediatRSample.Application.Commands;
using MediatRSample.Application.Models;
using MediatRSample.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace MediatRSample.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class PessoaController : ControllerBase
  {
    private readonly IMediator _mediator;
    private readonly IPessoaRepository<Pessoa> _repository;

    public PessoaController(IMediator mediator, IPessoaRepository<Pessoa> repository)
    {
      this._mediator = mediator;
      this._repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> Get()
    {
      return Ok(await _repository.GetAll());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
      return Ok(await _repository.Get(id));
    }

    [HttpPost]
    public async Task<IActionResult> Post(CadastraPessoaCommand command)
    {
      var response = await _mediator.Send(command);

      if (response == "Pessoa já cadastrada")
        return Conflict(response);

      if (response == "Preencha todos os campos")
        return BadRequest(response);

      return Ok(response);
    }

    [HttpPut]
    public async Task<IActionResult> Put(AlteraPessoaCommand command)
    {
      var response = await _mediator.Send(command);
      return Ok(response);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
      var obj = new ExcluiPessoaCommand { Id = id };
      var result = await _mediator.Send(obj);
      return Ok(result);
    }
  }
}