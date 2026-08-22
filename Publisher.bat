@echo off
title SeeSharPublisher
echo Checking for run type...
echo %cmdcmdline% | findstr /i /c:"\"%~f0\"" >nul
if %errorlevel% EQU 0 (
	echo Double clicking a file
	set /a PublisherMode=0
) else (
	echo CMD window
	set /a PublisherMode=1
)
echo Creating new publishable version...
dotnet publish -c Release
if %errorlevel% NEQ 0 (
	echo Oops!
	echo Exe generation failed
	if %PublisherMode% EQU 0 (
		cmd /k
	) else (
		exit /b 1
	)
)
echo Exe generation completed successfully, copying to root...
xcopy .\bin\Release\net10.0-windows\win-x64\publish\SeeSharp.exe .\ /y /c
if %errorlevel% NEQ 0 (
	echo Oops!
	echo Exe copuing failed
	if %PublisherMode% EQU 0 (
		cmd /k
	) else (
		exit /b 2
	)
	
)
echo Done copying!
echo Publish process completed :)

if %PublisherMode% EQU 0 (
timeout /t 3 /nobreak >nul
) else (
echo Exiting
)
exit /b 0

