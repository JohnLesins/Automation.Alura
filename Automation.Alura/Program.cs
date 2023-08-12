using Automation.Alura.Domain.Commands.v1.Base;
using Automation.Alura.Domain.Contracts.Helpers.v1;
using Automation.Alura.Domain.Contracts.Repositories.v1;
using Automation.Alura.Domain.Helpers.v1;
using Automation.Alura.Infrastructure.Repository;
using Automation.Alura.Infrastructure.Repository.Repositories.v1;
using Automation.Presentation.Alura;
using Microsoft.EntityFrameworkCore;

await Host.CreateDefaultBuilder(args)
    .ConfigureServices(services =>
    {
        services.AddDbContextFactory<AluraDbContext>(opt =>
        {
            opt.UseInMemoryDatabase("DB");
            opt.EnableSensitiveDataLogging();
            opt.UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking);
        });

        services.AddTransient<IJsonHelper, JsonHelper>();
        services.AddTransient<ICourseRepository, CourseRepository>();
        services.AddHostedService<ProcessCoursesWorker>();
        services.AddMediatR(x => x.RegisterServicesFromAssemblies(typeof(Program).Assembly, typeof(Command<>).Assembly));
    })
    .Build()
    .RunAsync();