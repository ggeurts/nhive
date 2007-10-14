@echo off
Rem This is called by the post-build event. You may modify it to copy the addin
Rem to whatever locations you like. As provided, it copies it to a number of
Rem directories relative to the target, provided they exist.

setlocal
set NUNIT_DEV_DIR= ..\..\..\..\vendor\nunit\current
set NUNIT_PROD_DIR= "%ProgramFiles%\NUnit 2.4.3\bin"
set TDNET_DIR= "%ProgramFiles%\TestDriven.NET 2.0\NUnit\2.4"

Rem See whether we are doing a production or development install
:tdnet
if not exist %TDNET_DIR% goto nunit_dev
echo Installing %1 into TestDriven.NET...
if not exist %TDNET_DIR%\addins mkdir %TDNET_DIR%\addins
copy %1 %TDNET_DIR%\addins

:nunit_dev
if not exist %NUNIT_DEV_DIR% goto nunit_prod
echo Installing %1 into NUnit vendor directory...
if not exist %NUNIT_DEV_DIR%\addins mkdir %NUNIT_DEV_DIR%\addins
copy %1 %NUNIT_DEV_DIR%\addins

:nunit_prod
if not exist %NUNIT_PROD_DIR% goto eof
echo Installing %1 into NUnit 2.4.3...
if not exist %NUNIT_PROD_DIR%\addins mkdir %NUNIT_PROD_DIR%\addins
copy %1 %NUNIT_PROD_DIR%\addins

:end