#load "variable.cake"
#load "docker.cake"
#load "build.cake"
#load "test.cake"

var target = Argument("target", "Build");

Parameters.ProjectsToPublish.Add("Gateway.Web");

Docker.Host = Argument("docker-host", "localhost");

Task("CI")
  .Description("Run CI Pipeline")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("Docker Publish")
  .Does(() => { });

RunTarget(target);