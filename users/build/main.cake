#load "variable.cake"
#load "build.cake"
#load "test.cake"
#load "docker.cake"


var target = Argument("target", "Build");

Parameters.ProjectsToPublish.Add("Users.Web");
Parameters.ProjectsToPublish.Add("Users.Migrations");


Task("Publish")
  .Description("Publish Project")
  .IsDependentOn("Test")
  .DoesForEach(() => Parameters.ProjectsToPublish, project =>
  {
      DotNetCorePublish($"{WorkDirectory.Source}/{project}", new DotNetCorePublishSettings 
      {
          OutputDirectory = $"{WorkDirectory.Publish}/{project}"
      });
  });

RunTarget(target);