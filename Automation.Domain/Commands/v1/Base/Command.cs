using MediatR;

namespace Automation.Alura.Domain.Commands.v1.Base;

public class Command<TResponse> : IRequest<TResponse>
{
}