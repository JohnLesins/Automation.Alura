using Automation.Alura.Domain.ValueObjects.v1;

namespace Automation.Alura.Domain.Entities.v1;

public class Course
{
    public Course(string? title, int workload, string? description, string? url, List<Instructor>? instructors)
    {
        Title = title;
        Workload = workload;
        Description = description;
        Url = url;
        Instructors = instructors;
    }

    public Course()
    {

    }

    public Guid Id { get; set; }
    public string? Title { get; set; }
    public int Workload { get; set; }
    public string? Description { get; set; }
    public string? Url { get; set; }

    public virtual List<Instructor>? Instructors { get; set; }
}