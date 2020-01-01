public static class WorkDirectory
{
    public static string Path => "..";
    public static string Source => $"{Path}/src";
    public static string Tests => $"{Path}/test";
    public static string Artifactory => $"{Path}/.artifacts";
    public static string Publish => $"{Artifactory}/publish";
    public static string Coverage => $"{Artifactory}/coverage";
    public static string Docker => $"{Artifactory}/docker";
    public static string Packages => $"{Path}/packages";
}

public static class Parameters
{
    public static List<string> ProjectsToPublish { get; } = new List<string>();
    public static List<string> UnitTestsProjects { get; } = new List<string>();
    public static List<string> AcceptanceTestsProjects { get; } = new List<string>();
}

public static class Docker
{
    public static string Host = "localhost";
    public static string[] DockerCompose = new [] { $"{WorkDirectory.Path}/../docker-compose.yml" };
    public static Dictionary<string,List<string>> Tags { get; } = new Dictionary<string,List<string>>();
}