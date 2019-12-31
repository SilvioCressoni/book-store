public static class WorkDirectory
{
    public static string Path => "..";
    public static string Source => $"{Path}/src";
    public static string Tests => $"{Path}/test";
    public static string Artifactory => $"{Path}/.artifacts";
    public static string Publish => $"{Artifactory}/publish";
    public static string Coverage => $"{Artifactory}/coverage";
    public static string Packages => $"{Path}/packages";
}

public static class Parameters
{
    public static List<string> ProjectsToPublish = new List<string>();
    public static List<string> UnitTestsProjects = new List<string>();
    public static List<string> AcceptanceTestsProjects = new List<string>();
}