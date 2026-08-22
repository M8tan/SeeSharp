@echo off
echo Creating new publishable version...
dotnet publish -c Release
 if %errorlevel% NEQ 0 (
echo Oops!
echo Exe generation failed
exit /b
)
echo Exe generation completed successfully, copying to root...
xcopy .\bin\Release\net10.0-windows\win-x64\publish\SeeSharp.exe .\ /y /c
 if %errorlevel% NEQ 0 (
echo Oops!
echo Exe copuing failed
exit /b
)
echo Done copying!
echo Publish process completed :)