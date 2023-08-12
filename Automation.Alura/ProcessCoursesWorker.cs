using Automation.Alura.Domain.Commands.v1;
using MediatR;

namespace Automation.Presentation.Alura;

public class ProcessCoursesWorker : BackgroundService
{
    private readonly ILogger<ProcessCoursesWorker> _logger;
    private readonly IMediator _mediator;

    public ProcessCoursesWorker(ILogger<ProcessCoursesWorker> logger, IMediator mediator)
    {
        _logger = logger;
        _mediator = mediator;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation($"{nameof(ProcessCoursesWorker)} Started.");
        
        await _mediator.Send(new ProcessCoursesCommand(), stoppingToken);
        
        _logger.LogInformation($"{nameof(ProcessCoursesWorker)} Finished.");
    }
}