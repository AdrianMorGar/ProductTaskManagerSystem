# Script de Automatización de Despliegue
# Autor: AdrianMorGar
# Fecha: 2026-01-22

$ErrorActionPreference = "Stop"
Write-Host ">>> INICIANDO PROCESO DE AUTOMATIZACIÓN Y DESPLIEGUE <<<" -ForegroundColor Cyan

# 1. Definir rutas
$rootPath = ".."  # Asumiendo que ejecutamos desde la carpeta Scripts
$deployPath = "$rootPath\Deploy"
$webProject = "$rootPath\WebApp\Gestion.Web\Gestion.Web.csproj"
$desktopProject = "$rootPath\DesktopApp\Gestion.Desktop\Gestion.Desktop.csproj"

# 2. Limpiar carpeta de despliegue previa
if (Test-Path $deployPath) {
    Write-Host "Limpiando carpeta de despliegue anterior..." -ForegroundColor Yellow
    Remove-Item $deployPath -Recurse -Force
}
New-Item -ItemType Directory -Force -Path $deployPath | Out-Null

# 3. Publicar Aplicación Web (Backend)
Write-Host "--- Compilando y Publicando Web API ---" -ForegroundColor Green
dotnet publish $webProject -c Release -o "$deployPath\Web" --nologo

# 4. Publicar Aplicación Desktop (Cliente)
Write-Host "--- Compilando y Publicando Desktop App (Windows) ---" -ForegroundColor Green
dotnet publish $desktopProject -c Release -f net8.0-windows10.0.19041.0 -o "$deployPath\Desktop" --nologo -p:WindowsPackageType=None -p:SelfContained=true

# 5. Copiar Scripts de Base de Datos
Write-Host "--- Empaquetando Scripts de BD ---" -ForegroundColor Green
New-Item -ItemType Directory -Force -Path "$deployPath\DatabaseScripts" | Out-Null
Copy-Item ".\01_Schema.sql" -Destination "$deployPath\DatabaseScripts"
Copy-Item ".\02_SemillaDatos.sql" -Destination "$deployPath\DatabaseScripts"

Write-Host ">>> DESPLIEGUE COMPLETADO CON ÉXITO <<<" -ForegroundColor Cyan
Write-Host "Los archivos generados están en: $deployPath" -ForegroundColor White