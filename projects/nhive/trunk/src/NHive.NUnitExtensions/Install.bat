@echo off
Rem This is called by the post-build event. You may modify it to copy the addin
Rem to whatever locations you like. As provided, it copies it to a number of
Rem directories relative to the target, provided they exist.

setlocal
set NUNIT_DIR= ..\..\..\..\vendor\nunit\current
set TDNET_DIR= "%ProgramFiles%\TestDriven.NET 2.0\NUnit\2.4"

Rem See whether we are doing a production or development install
if exist %NUNIT_DIR% goto prod
echo No install locations found - install %1 manually
goto end

:prod
Rem For production install, ensure we have an addins directory
if not exist %NUNIT_DIR%\addins mkdir %NUNIT_DIR%\addins
copy %1 %NUNIT_DIR%\addins
if not exist %TDNET_DIR%\addins mkdir %TDNET_DIR%\addins
copy %1 %TDNET_DIR%\addins
echo Installed %1 in addins directory
goto end

:end