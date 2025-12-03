@echo off
echo Building Android APK...
dotnet publish -f net9.0-android -c Release -p:AndroidSigningKeyStore=AIBrainstorm.keystore -p:AndroidSigningKeyAlias=AIBrainstorm -p:AndroidSigningKeyPass=bk1994 -p:AndroidSigningStorePass=bk1994

if %ERRORLEVEL% EQU 0 (
    echo.
    echo Build successful!
    echo Output location: bin\Release\net9.0-android\publish\
) else (
    echo.
    echo Build failed with error code %ERRORLEVEL%
)

pause
