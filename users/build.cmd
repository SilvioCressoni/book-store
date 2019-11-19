@echo off
powershell powershell -ExecutionPolicy ByPass -NoProfile -command "'%~dp0build\build.ps1' -restore -build %*"
exit /b %ErrorLevel%