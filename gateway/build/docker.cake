#addin "Cake.Docker&version=0.11.0"

Task("Load Docker Tags")
  .Description("Load Docker Tags")
  .DoesForEach(() => Parameters.ProjectsToPublish, project =>
  {
      var tags = new List<string>();
      tags.Add($"{project.ToLower().Replace(".", "-")}:unstable");
      Docker.Tags.Add(project, tags);

      Information($"Tags for {project}:"); 
      Information(string.Join("\n", tags));

      Information("");
  });

Task("Docker Build")
  .Description("Build Docker image")
  .IsDependentOn("Load Docker Tags")
  .DoesForEach(() => Parameters.ProjectsToPublish, project => 
  {
    DockerBuild(new DockerImageBuildSettings 
    { 
      File = $"{WorkDirectory.Source}/{project}/Dockerfile",
      Tag = Docker.Tags[project].ToArray()
    }, Directory(WorkDirectory.Source));
  });

Task("Docker Save")
  .Description("Save Docker image")
  .IsDependentOn("Docker Build")
  .DoesForEach(() => Parameters.ProjectsToPublish, project => 
  {
    if(!DirectoryExists(WorkDirectory.Docker))
    {
        CreateDirectory(WorkDirectory.Docker);
    }

    var dockerImage = project.ToLower().Replace(".", "-");

    DockerSave(new DockerImageSaveSettings 
    { 
      Output = $"{WorkDirectory.Docker}/{dockerImage}"
    }, dockerImage);
  });

Task("Docker Publish")
  .Description("Publish Docker images")
  .IsDependentOn("Docker Build")
  .IsDependentOn("Docker Save")
  .Does(() => 
  {
    Information("Do nothing...");
  });