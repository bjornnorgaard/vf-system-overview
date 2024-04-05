using System.Diagnostics;
using System.Text.RegularExpressions;

var sw = Stopwatch.StartNew();

var repositoryDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
Console.WriteLine($"Searching directory: {repositoryDirectory}");

var outputDirectory = Path.Combine(repositoryDirectory, "flying-circus-docs", "Generated");
if (!Directory.Exists(outputDirectory)) Directory.CreateDirectory(outputDirectory);
foreach (var file in Directory.GetFiles(outputDirectory))
{
    File.Delete(file);
}

var projects = new List<Project>();
var solutions = new List<Solution>();
var repositories = new List<Repository>();

var versionSearch = new List<string>
{
    "<TargetFramework>(.*?)</TargetFramework>",
    "<TargetFrameworkVersion>(.*?)</TargetFrameworkVersion>",
};

foreach (var repository in Directory.GetDirectories(repositoryDirectory))
{
    if (!Directory.Exists(Path.Combine(repository, ".git"))) continue;

    var projectFiles = Directory.GetFiles(repository, "*.csproj", SearchOption.AllDirectories);
    foreach (var project in projectFiles)
    {
        var projectContent = File.ReadAllText(project);
        var detectedVersion = "";
        foreach (var s in versionSearch)
        {
            var match = Regex.Match(projectContent, s);
            if (!match.Success) continue;
            detectedVersion = match.Groups[1].Value;
        }

        projects.Add(new Project
        {
            Name = Path.GetFileName(project),
            CleanName = Path.GetFileName(project).PruneCharacters(),
            Repository = Path.GetFileName(repository).PruneCharacters(),
            FrameworkVersion = detectedVersion.PruneCharacters(),
        });
    }

    var solutionFiles = Directory.GetFiles(repository, "*.sln", SearchOption.AllDirectories);
    foreach (var solution in solutionFiles)
    {
        var solutionContent = File.ReadAllText(solution);
        var projectNames = new List<string>();
        foreach (var p in projects)
        {
            if (!solutionContent.Contains(p.Name)) continue;
            projectNames.Add(p.Name);
        }

        solutions.Add(new Solution
        {
            Name = Path.GetFileName(solution),
            CleanName = Path.GetFileName(solution).PruneCharacters(),
            Repository = Path.GetFileName(repository).PruneCharacters(),
            Projects = projectNames.ToList(),
        });
    }
}

foreach (var p in projects)
{
    var content = $"---\n" +
                  // $"project: {p.Name}\n" +
                  // $"repository: {p.Repository}\n" +
                  // $"dotnet: {p.FrameworkVersion}\n" +
                  $"tags: \n" +
                  $"  - project/{p.CleanName}\n" +
                  $"  - repository/{p.Repository}\n" +
                  $"  - dotnet/{p.FrameworkVersion}\n" +
                  $"---\n\n" +
                  $"# {p.Name}\n\n" +
                  $"Referenced in solutions: \n\n";

    foreach (var s in solutions)
    {
        if (!s.Projects.Contains(p.Name)) continue;
        content += $"- [[{s.CleanName}]]\n";
    }

    File.WriteAllText(Path.Combine(outputDirectory, $"{p.CleanName}.md"), content);
}

foreach (var s in solutions)
{
    var content = $"---\n" +
                  // $"solution: {s.CleanName}\n" +
                  // $"repository: {s.Repository}\n" +
                  $"tags: \n" +
                  $"  - solution/{s.CleanName}\n" +
                  $"  - repository/{s.Repository}\n" +
                  $"---\n\n" +
                  $"# {s.Name}\n\n" +
                  $"Contains projects: \n\n";

    foreach (var projectName in s.Projects)
    {
        content += $"- [[{projectName.PruneCharacters()}]]\n";
    }

    var newRepository = repositories.FirstOrDefault(r => r.Name == s.Repository);
    if (newRepository == null)
    {
        newRepository = new Repository
        {
            Name = s.Repository,
            CleanName = Path.GetFileName(s.Repository).PruneCharacters(),
            Solutions = [],
        };
        repositories.Add(newRepository);
    }

    newRepository.Solutions.Add(s.Name);

    File.WriteAllText(Path.Combine(outputDirectory, $"{s.CleanName}.md"), content);
}

foreach (var r in repositories)
{
    var content = $"---\n" +
                  // $"repository: {r.CleanName}\n" +
                  $"tags: \n" +
                  $"  - repository/{r.CleanName}\n" +
                  $"---\n\n" +
                  $"# {r.Name}\n\n" +
                  $"Contains solutions: \n\n";

    foreach (var solutionName in r.Solutions)
    {
        content += $"- [[{solutionName.PruneCharacters()}]]\n";
    }

    File.WriteAllText(Path.Combine(outputDirectory, $"{r.CleanName}.md"), content);
}

Console.WriteLine($"Elapsed time: {sw.ElapsedMilliseconds} ms");

public static class StringExtensions
{
    public static string PruneCharacters(this string str)
    {
        var replace = Regex.Replace(str, "[^a-zA-Z0-9]", "_");
        if (replace.StartsWith("_"))
        {
            replace = replace[1..];
        }

        return replace.ToLower();
    }
}

internal class Project
{
    public required string Name { get; init; } = "";
    public required string CleanName { get; init; } = "";
    public required string Repository { get; init; } = "";
    public required string FrameworkVersion { get; init; } = "";
}

internal class Repository
{
    public required string Name { get; init; } = "";
    public required string CleanName { get; init; } = "";
    public List<string> Solutions { get; init; } = [];
}

internal class Solution
{
    public required string Name { get; init; } = "";
    public required string CleanName { get; init; } = "";
    public required string Repository { get; init; } = "";
    public required List<string> Projects { get; init; } = [];
}