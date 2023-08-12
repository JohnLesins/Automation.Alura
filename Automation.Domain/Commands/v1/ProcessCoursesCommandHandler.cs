using Automation.Alura.Domain.Commands.v1.Base;
using Automation.Alura.Domain.Contracts.Helpers.v1;
using Automation.Alura.Domain.Contracts.Repositories.v1;
using Automation.Alura.Domain.Entities.v1;
using Automation.Alura.Domain.Extensions.v1;
using Automation.Alura.Domain.ValueObjects.v1;
using Automation.Domain.Extensions;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using System.Text;
using System.Web;

namespace Automation.Alura.Domain.Commands.v1;

public class ProcessCoursesCommandHandler : CommandHandler<ProcessCoursesCommand, Unit>
{
    private readonly ICourseRepository _courseRepository;
    private readonly IConfiguration _configuration;
    private readonly ILogger<ProcessCoursesCommandHandler> _logger;
    private readonly IJsonHelper _jsonHelper;

    public ProcessCoursesCommandHandler(ICourseRepository courseRepository, IConfiguration configuration, ILogger<ProcessCoursesCommandHandler> logger, IJsonHelper jsonHelper)
    {
        _courseRepository = courseRepository;
        _configuration = configuration;
        _logger = logger;
        _jsonHelper = jsonHelper;
    }

    protected override async Task<Unit> HandleCommand(ProcessCoursesCommand request, CancellationToken cancellationToken)
    {
        using var chromeDriver = new ChromeDriver();
        var baseAddressAlura = _configuration["urlBaseAlura"];
        chromeDriver.Navigate().GoToUrl(baseAddressAlura);

        var nameCourse = ".net";

        GetCoursesList(chromeDriver, baseAddressAlura, nameCourse);

        var coursesSearched = (await chromeDriver.WaitFindElements(By.ClassName("busca-resultado"), cancellationToken: cancellationToken))
            .Where(x =>
            {
                try
                {
                    var hasHref = !string.IsNullOrEmpty(x.WaitFindElement(By.ClassName("busca-resultado-link")).Result?.GetAttribute("href"));
                    var hasDescricao = !string.IsNullOrEmpty(x.WaitFindElement(By.ClassName("busca-resultado-descricao")).Result?.Text);

                    return hasHref && hasDescricao;
                }
                catch
                {
                    return false;
                }
            });

        var urlAddressCoursesTasks = coursesSearched.Select(async x => (await x.WaitFindElement(By.ClassName("busca-resultado-link"))
            )?.GetAttribute("href"));

        var urlAddressCourses = (await Task.WhenAll(urlAddressCoursesTasks)).ToList();

        var courses = new List<Course>(urlAddressCourses.Count);

        urlAddressCourses.ForEach(async urlAddressCourse =>
        {
            if (string.IsNullOrEmpty(urlAddressCourse))
                return;

            chromeDriver.Navigate().GoToUrl(urlAddressCourse);

            var isCourseFormation = urlAddressCourse.Contains("formacao");
            var isCourse = urlAddressCourse.Contains("curso");

            if (isCourseFormation)
            {
                var curso = await GetCourseFormation(chromeDriver, urlAddressCourse, cancellationToken);

                courses.Add(curso);
            }

            if (isCourse)
            {
                var curso = await GetCourse(chromeDriver, urlAddressCourse, cancellationToken);

                courses.Add(curso);
            }
        });

        await _courseRepository.SaveAsync(courses, cancellationToken);

        var cursosSalvos = await _courseRepository.GetAllAsync(cancellationToken);

        //logging the courses just to demonstrate that data is being saved
        _logger.LogInformation(await _jsonHelper.SerializeAsync(cursosSalvos));

        return Unit.Value;
    }

    private void GetCoursesList(ChromeDriver chromeDriver, string? baseAddressAlura, string nomeCursoParaPesquisa)
    {
        try
        {
            var inputSearchCourses = chromeDriver.FindElement(By.Id("header-barraBusca-form-campoBusca"));

            inputSearchCourses.SendKeys(nomeCursoParaPesquisa);
            inputSearchCourses.Submit();
        }
        catch (Exception ex)
        {
            _logger.LogError($"Erro ao processar o campo de busca. {ex.Message}");

            chromeDriver.Navigate().GoToUrl($"{baseAddressAlura}busca?query={HttpUtility.UrlEncode(nomeCursoParaPesquisa)}");
        }
    }

    private static async Task<Course> GetCourseFormation(ChromeDriver chromeDriver, string? urlAddressCourse, CancellationToken cancellationToken = default)
    {
        int.TryParse((await chromeDriver.WaitFindElement(By.ClassName("formacao__info-destaque"), cancellationToken))?.Text?.RemoveLetters(), out int cargaHoraria);

        var title = chromeDriver.FindElement(By.ClassName("formacao-headline-titulo"))?.Text;
        var description = chromeDriver.FindElement(By.ClassName("formacao-sobre"))?.Text;

        var instructors = chromeDriver.FindElements(By.ClassName("formacao-instrutor-nome"))
            .Where(x => !string.IsNullOrEmpty(x.Text))
            .Select(x => new Instructor(x.Text))
            .ToList();

        var course = new Course(title, cargaHoraria, description, urlAddressCourse, instructors);

        return course;
    }

    private async Task<Course> GetCourse(ChromeDriver chromeDriver, string? urlAddressCourse, CancellationToken cancellationToken = default)
    {
        var builderTitle = new StringBuilder();
        builderTitle.Append((await chromeDriver.WaitFindElement(By.ClassName("curso-banner-course-title"), cancellationToken))?.Text);
        builderTitle.Append($" {chromeDriver.FindElement(By.ClassName("course--banner-text-category"))?.Text}");

        var title = builderTitle.ToString();

        int.TryParse(chromeDriver.FindElement(By.ClassName("courseInfo-card-wrapper-infos"))?.Text?.RemoveLetters(), out int cargaHoraria);

        var builderDescription = new StringBuilder();
        builderDescription.AppendLine(chromeDriver.FindElement(By.ClassName("container-list--width"))?.Text ?? "");
        builderDescription.AppendLine(chromeDriver.FindElement(By.ClassName("courseInfo-container"))?.Text ?? "");

        var description = builderDescription.ToString();
        var instructors = chromeDriver.FindElements(By.ClassName("instructor-title--name"))
            .Where(x => !string.IsNullOrEmpty(x.Text))
            .Select(x => new Instructor(x.Text))
            .ToList();

        var course = new Course(title, cargaHoraria, description, urlAddressCourse, instructors);

        return course;
    }
}