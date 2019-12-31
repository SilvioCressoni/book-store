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
      NoBuild = true
    });
  });

Task("Acceptance Test")
  .Description("Run Acceptance Test")
  .IsDependentOn("Build")
  .IsDependentOn("Load Test")
  .Does(() => {});

Task("Test")
  .Description("Run all test(Unit + Acceptance)")
  .IsDependentOn("Build")
  .IsDependentOn("Load Test")
  .IsDependentOn("Unit Test")
  .IsDependentOn("Acceptance Test")
  .Does(() => 
  {

  });