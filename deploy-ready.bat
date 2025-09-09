@echo off
echo =====================================================
echo       The Next Event API - Deployment Package
echo =====================================================
echo.

echo [1/4] Cleaning previous builds...
rmdir /s /q bin 2>nul
rmdir /s /q obj 2>nul
rmdir /s /q publish 2>nul

echo [2/4] Restoring packages (.NET 8)...
dotnet restore

echo [3/4] Building Release version...
dotnet build --configuration Release

echo [4/4] Publishing for deployment...
dotnet publish --configuration Release --output ./publish

echo.
echo =====================================================
echo             DEPLOYMENT READY!
echo =====================================================
echo.
echo Files ready for upload in: ./publish/
echo.
echo Upload ALL files from publish/ folder to Monster ASP wwwroot
echo.
echo Test endpoints after upload:
echo 1. https://thenextevent.runasp.net/
echo 2. https://thenextevent.runasp.net/health  
echo 3. https://thenextevent.runasp.net/swagger
echo.
echo =====================================================

pause
