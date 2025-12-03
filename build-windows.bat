@echo off
echo Building Windows EXE...
dotnet publish -f net9.0-windows10.0.19041.0 -c Release -p:RuntimeIdentifierOverride=win10-x64 -p:WindowsPackageType=None -p:WindowsAppSDKSelfContained=true

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
    echo Output location: bin\Release\net9.0-windows10.0.19041.0\win10-x64\publish\
) else (
    echo.
    echo Build failed with error code %ERRORLEVEL%
)

pause
