@echo off

echo Starting backend...
start "Backend" cmd /k "dotnet run --launch-profile https --project backend\DeviceManagementSystem"

echo Starting frontend...
start "Frontend" cmd /k "cd /d frontend/device-management-ui && npm start"
