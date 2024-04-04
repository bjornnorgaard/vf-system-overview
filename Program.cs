using System.Diagnostics;
using System.Text.RegularExpressions;

var sw = Stopwatch.StartNew();

var repositoryDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
Console.WriteLine($"Searching directory: {repositoryDirectory}");

var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var downloadsPath = Path.Combine(userPath, "Downloads", "Projects");
if (!Directory.Exists(downloadsPath)) Directory.CreateDirectory(downloadsPath);

var projects = new List<Project>();
var solutions = new List<Solution>();
var repositories = new List<Repository>();

var versionSearch = new List<string>
{
    "<TargetFramework>(.*?)</TargetFramework>",
    "<TargetFrameworkVersion>(.*?)</TargetFrameworkVersion>",
};

foreach (var directory in Directory.GetDirectories(repositoryDirectory))
{
    if (!Directory.Exists(Path.Combine(directory, ".git"))) continue;

    var files = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);
    foreach (var file in files)
    {
        var projectContent = File.ReadAllText(file);
        var detectedVersion = "";
        foreach (var s in versionSearch)
        {
            var match = Regex.Match(projectContent, s);
            if (!match.Success) continue;
            detectedVersion = match.Groups[1].Value;
        }

        projects.Add(new Project
        {
            Name = Path.GetFileNameWithoutExtension(file),
            Repository = directory,
            FrameworkVersion = detectedVersion,
        });
    }

    files = Directory.GetFiles(directory, "*.sln", SearchOption.AllDirectories);
    foreach (var file in files)
    {
        var solutionContent = File.ReadAllText(file);
        var projectNames = new List<string>();
        foreach (var p in projects)
        {
            if (!solutionContent.Contains(p.Name)) continue;
            projectNames.Add(p.Name);
        }

        solutions.Add(new Solution
        {
            Name = Path.GetFileNameWithoutExtension(file),
            Repository = Path.GetFileName(directory),
            Projects = projectNames.ToList(),
        });
    }
}

foreach (var p in projects)
{
    var content = $"---\n" +
                  $"project: {p.Name}\n" +
                  $"repository: {p.Repository}\n" +
                  $"dotnet: {p.FrameworkVersion}\n" +
                  $"tags: \n" +
                  $"  - project\n" +
                  $"---\n\n" +
                  $"# {p.Name}\n\n" +
                  $"Referenced in solutions: \n\n";

    foreach (var s in solutions)
    {
        if (!s.Projects.Contains(p.Name)) continue;
        content += $"- [[{s.Name}]]\n";
    }

    File.WriteAllText(Path.Combine(downloadsPath, $"{p.Name}.md"), content);
}

foreach (var s in solutions)
{
    var content = $"---\n" +
                  $"solution: {s.Name}\n" +
                  $"repository: {s.Repository}\n" +
                  $"tags: \n" +
                  $"  - solution\n" +
                  $"---\n\n" +
                  $"# {s.Name}\n\n" +
                  $"Contains projects: \n\n";

    foreach (var projectName in s.Projects)
    {
        content += $"- [[{projectName}]]\n";
    }

    var newRepository = repositories.FirstOrDefault(r => r.Name == s.Repository);
    if (newRepository == null)
    {
        newRepository = new Repository { Name = s.Repository, Solutions = [] };
        repositories.Add(newRepository);
    }

    newRepository.Solutions.Add(s.Name);

    File.WriteAllText(Path.Combine(downloadsPath, $"{Path.GetFileNameWithoutExtension(s.Name)}.md"), content);
}

foreach (var r in repositories)
{
    var content = $"---\n" +
                  $"repository: {r.Name}\n" +
                  $"tags: \n" +
                  $"  - repository\n" +
                  $"---\n\n" +
                  $"# {r.Name}\n\n" +
                  $"Contains solutions: \n\n";

    foreach (var solutionName in r.Solutions)
    {
        content += $"- [[{solutionName}]]\n";
    }

    File.WriteAllText(Path.Combine(downloadsPath, $"{Path.GetFileNameWithoutExtension(r.Name)}.md"), content);
}

Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");

internal class Project
{
    public string Name { get; init; } = "";
    public string Repository { get; init; } = "";
    public string FrameworkVersion { get; init; } = "";
}

internal class Repository
{
    public string Name { get; init; } = "";
    public List<string> Solutions { get; init; } = [];
}

internal class Solution
{
    public string Name { get; init; } = "";
    public string Repository { get; init; } = "";
    public List<string> Projects { get; init; } = [];
}
