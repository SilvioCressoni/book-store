#load "variable.cake"
#load "docker.cake"
#load "build.cake"
#load "test.cake"

var target = Argument("target", "Build");

Parameters.ProjectsToPublish.Add("Users.Web");
Parameters.ProjectsToPublish.Add("Users.Migrations");

Docker.Host = Argument("docker-host", "localhost");

Task("CI")
  .Description("Run all test")
  .IsDependentOn("Build")
  .IsDependentOn("Test")
  .IsDependentOn("Docker Publish")
  .Does(() => { });

RunTarget(target);