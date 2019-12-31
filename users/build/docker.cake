#addin "Cake.Docker"
Task("Docker build")
  .Description("Build Docker image")
  .DoesForEach(() => Parameters.ProjectsToPublish, project => 
  {
    DockerBuild(new DockerImageBuildSettings 
      { 
        File = $"{project}/Dockerfile")
      }, Directory(WorkDirectory.Source));
  });

Task("Docker Publish")
  .Description("Publish Docker images")
  .IsDependentOn("Docker build")
  .Does(() => 
  {
    Information("Do nothing...");
  });