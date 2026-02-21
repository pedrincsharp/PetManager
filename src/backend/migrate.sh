#!/usr/bin/env bash
# migrate.sh
# Usage: ./migrate.sh <MigrationName> [-p project.csproj] [-s startup.csproj] [-c DbContextName]
# Defaults (relative to this script):
#   Project:  PetManager.Infrastructure/PetManager.Infrastructure.csproj
#   Startup:  PetManager.Api/PetManager.Api.csproj

set -euo pipefail

usage() {
  cat <<EOF
Usage: $(basename "$0") <MigrationName> [-p project.csproj] [-s startup.csproj] [-c DbContextName]
Example: ./migrate.sh AddPetEntity
EOF
  exit 1
}

if [[ ${1:-} == "-h" || ${1:-} == "--help" || -z ${1:-} ]]; then
  usage
fi

NAME="$1"
shift || true

SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"

while [[ $# -gt 0 ]]; do
  case "$1" in
    -p|--project)
      PROJECT="$2"
      shift 2
      ;;
    -s|--startup)
      STARTUP="$2"
      shift 2
      ;;
    -c|--context)
      CONTEXT="$2"
      shift 2
      ;;
    *)
      echo "Unknown option: $1"
      usage
      ;;
  esac
done

PROJECT=${PROJECT:-"$SCRIPT_DIR/PetManager.Infrastructure/PetManager.Infrastructure.csproj"}
STARTUP=${STARTUP:-"$SCRIPT_DIR/PetManager.Api/PetManager.Api.csproj"}

if ! command -v dotnet >/dev/null 2>&1; then
  echo "dotnet não está disponível no PATH. Instale o .NET SDK e o tool 'dotnet-ef' se necessário." >&2
  exit 2
fi

echo "Running: dotnet ef migrations add $NAME"
echo " - Project: $PROJECT"
echo " - Startup: $STARTUP"
[[ -n "${CONTEXT:-}" ]] && echo " - Context: $CONTEXT"

args=(ef migrations add "$NAME" -p "$PROJECT" -s "$STARTUP")
if [[ -n "${CONTEXT:-}" ]]; then
  args+=( -c "$CONTEXT" )
fi

dotnet "${args[@]}"
rc=$?
if [[ $rc -ne 0 ]]; then
  echo "dotnet ef falhou com o código de saída $rc" >&2
  exit $rc
fi

echo "Migration '$NAME' criada com sucesso."

echo "Aplicando migration ao banco de dados (dotnet ef database update)..."
update_args=(ef database update -p "$PROJECT" -s "$STARTUP")
if [[ -n "${CONTEXT:-}" ]]; then
  update_args+=( -c "$CONTEXT" )
fi

dotnet "${update_args[@]}"
rc2=$?
if [[ $rc2 -ne 0 ]]; then
  echo "dotnet ef database update falhou com o código de saída $rc2" >&2
  exit $rc2
fi

echo "Database atualizado com sucesso."
