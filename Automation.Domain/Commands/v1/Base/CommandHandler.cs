using MediatR;

namespace Automation.Alura.Domain.Commands.v1.Base;

public abstract class CommandHandler<TCommand, TResult> : IRequestHandler<TCommand, TResult>
    where TCommand : IRequest<TResult>
{
    public async Task<TResult> Handle(TCommand request, CancellationToken cancellationToken)
    {
        return await HandleCommand(request, cancellationToken);
    }

    protected abstract Task<TResult> HandleCommand(TCommand request, CancellationToken cancellationToken);
}