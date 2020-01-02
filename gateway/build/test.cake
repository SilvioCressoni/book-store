#addin "Cake.Docker&version=0.11.0"

Task("Load Test")
  .Does(() => 
  {
    var tests = GetDirectories($"{WorkDirectory.Tests}/*");
    foreach (var test in tests)
    {
      string path = test.ToString();
      if(path.Contains("Acceptance"))
      {
         Parameters.AcceptanceTestsProjects.Add(path);
      }
      else
      {
          Parameters.UnitTestsProjects.Add(path);
      }
    }

    Information($"Acceptance test found: {string.Join(",", Parameters.AcceptanceTestsProjects)}");
    Information($"Unit test found: {string.Join(",", Parameters.UnitTestsProjects)}");
  });

Task("Unit Test")
  .Description("Run Unit Test")
  .IsDependentOn("Build")
  .IsDependentOn("Load Test")
  .DoesForEach(() => Parameters.UnitTestsProjects, testProject => 
  {
    DotNetCoreTest(testProject, new DotNetCoreTestSettings 
    {
      NoRestore = true,
      NoBuild = true,
      ResultsDirectory = WorkDirectory.Coverage
    });
  });

Task("Acceptance Test")
  .Description("Run Acceptance Test")
  .IsDependentOn("Build")
  .IsDependentOn("Load Test")
  .Does(() => 
  {
      try
      {
          Information($"Docke Compose File: {string.Join(":", Docker.DockerCompose)}");
          Information("Building Docker images...");
          DockerComposeBuild(new DockerComposeBuildSettings   
          {
              Files = Docker.DockerCompose
          });

          Information("Starting Docker images...");
          DockerComposeStart(new DockerComposeSettings  
          {
              Files = Docker.DockerCompose
          });

          System.Threading.Thread.Sleep(3_000);

          foreach(var project in Parameters.AcceptanceTestsProjects)
          {
              DotNetCoreTest(project, new DotNetCoreTestSettings 
              {
                  EnvironmentVariables = new Dictionary<string, string>
                  {
                      ["Host"] = $"{Docker.Host}:5100"
                  },
                  NoRestore = true,
                  NoBuild = true
              });
          }
      }
      finally
      {
          DockerComposeDown(new DockerComposeDownSettings 
          {
              Files = Docker.DockerCompose
          });
      }
  });

Task("Test")
  .Description("Run all test(Unit + Acceptance)")
  .IsDependentOn("Build")
  .IsDependentOn("Load Test")
  .IsDependentOn("Unit Test")
  .IsDependentOn("Acceptance Test")
  .Does(() => 
  {

  });