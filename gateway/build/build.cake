Task("Clean")
  .Description("Clean Solution")
  .Does(() => 
  {
    DotNetCoreClean(WorkDirectory.Path);
  });

Task("Restore")
  .Description("Restore Solution")
  .Does(() => 
  {
      DotNetCoreRestore(WorkDirectory.Path, new DotNetCoreRestoreSettings 
      {
          PackagesDirectory  = WorkDirectory.Packages
      });
  });

Task("Build")
  .Description("Build Solution")
  .IsDependentOn("Restore")
  .Does(() => 
  {
      DotNetCoreBuild(WorkDirectory.Path, new DotNetCoreBuildSettings 
      {
          NoRestore = true
      });
  });