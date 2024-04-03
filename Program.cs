using System.Diagnostics;
using Overview;

var sw = Stopwatch.StartNew();

var repositoryDirectory = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "..", "..", "..", ".."));
Console.WriteLine($"Searching directory: {repositoryDirectory}");

var userPath = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
var downloadsPath = Path.Combine(userPath, "Downloads", "Projects");
if (!Directory.Exists(downloadsPath)) Directory.CreateDirectory(downloadsPath);

var projects = new List<Project>();
var solutions = new List<Solution>();
var repositories = new List<Repository>();

foreach (var directory in Directory.GetDirectories(repositoryDirectory))
{
    if (!Directory.Exists(Path.Combine(directory, ".git"))) continue;

    var files = Directory.GetFiles(directory, "*.csproj", SearchOption.AllDirectories);
    foreach (var file in files)
    {
        projects.Add(new Project
        {
            Name = Path.GetFileNameWithoutExtension(file),
            Repository = directory,
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
                  $"---\n\n" +
                  $"# {p.Name}\n\n" +
                  $"Referenced in solutions: \n\n";

    foreach (var s in solutions)
    {
        if (!s.Projects.Contains(p.Name)) continue;
        content += $"- [[{s.Name}]]\n";
    }

    File.WriteAllText(Path.Combine(downloadsPath, $"{Path.GetFileNameWithoutExtension(p.Name)}.md"), content);
}

foreach (var s in solutions)
{
    var content = $"---\n" +
                  $"solution: {s.Name}\n" +
                  $"repository: {s.Repository}\n" +
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