#!/usr/bin/env bash
# Verify the toolchain required by docs/superpowers/specs/2026-04-26-monogame-research-design.md.
# Exits non-zero on any blocking issue. Run from anywhere.

set -u

REQUIRED_DOTNET_MAJOR=10
TEMPLATE_PACKAGE="MonoGame.Templates.CSharp"
TEMPLATE_SHORT_NAME="mgdesktopgl"
SCRIPT_DIR="$(cd "$(dirname "${BASH_SOURCE[0]}")" && pwd)"
REPO_ROOT="$(cd "$SCRIPT_DIR/.." && pwd)"

red()   { printf '\033[31m%s\033[0m\n' "$*"; }
green() { printf '\033[32m%s\033[0m\n' "$*"; }
yellow(){ printf '\033[33m%s\033[0m\n' "$*"; }

fail=0
warn=0

check_macos() {
  if [[ "$(uname -s)" != "Darwin" ]]; then
    yellow "[warn] Phase 1 targets macOS DesktopGL; current OS is $(uname -s)."
    warn=$((warn + 1))
  else
    green "[ok]   macOS detected ($(uname -m))."
  fi
}

check_dotnet() {
  if ! command -v dotnet >/dev/null 2>&1; then
    red "[fail] dotnet not on PATH. Install .NET ${REQUIRED_DOTNET_MAJOR} SDK from https://dotnet.microsoft.com/download/dotnet/${REQUIRED_DOTNET_MAJOR}.0"
    fail=$((fail + 1))
    return 1
  fi
  local version major
  version=$(cd "$REPO_ROOT" && dotnet --version 2>/dev/null)
  major=${version%%.*}
  if [[ -z "$version" ]]; then
    red "[fail] 'dotnet --version' produced no output."
    fail=$((fail + 1))
    return 1
  fi
  if [[ "$major" != "$REQUIRED_DOTNET_MAJOR" ]]; then
    red "[fail] .NET SDK ${REQUIRED_DOTNET_MAJOR}.x required; found ${version}. If you have ${REQUIRED_DOTNET_MAJOR}.x installed alongside, the repo's global.json should select it - re-run from the repo root."
    fail=$((fail + 1))
    return 1
  fi
  green "[ok]   .NET SDK ${version}."
  return 0
}

check_template() {
  local templates
  templates=$(cd "$REPO_ROOT" && dotnet new list 2>/dev/null)
  if printf '%s\n' "$templates" | awk -v short_name="$TEMPLATE_SHORT_NAME" '
      /MonoGame/ {
        for (i = 1; i <= NF; i++) {
          gsub(/,/, "", $i)
          if ($i == short_name) {
            found = 1
          }
        }
      }
      END { exit found ? 0 : 1 }
    '; then
    green "[ok]   Template '${TEMPLATE_SHORT_NAME}' installed."
    return 0
  fi
  red "[fail] Template '${TEMPLATE_SHORT_NAME}' not found. Install with: dotnet new install ${TEMPLATE_PACKAGE}"
  fail=$((fail + 1))
  return 1
}

check_global_json() {
  if [[ ! -f "$REPO_ROOT/global.json" ]]; then
    red "[fail] global.json missing at repo root. The spec requires SDK pinning."
    fail=$((fail + 1))
    return 1
  fi
  if grep -Eq '"version"[[:space:]]*:[[:space:]]*"10\.' "$REPO_ROOT/global.json"; then
    green "[ok]   global.json pins .NET ${REQUIRED_DOTNET_MAJOR}.x."
  else
    red "[fail] global.json exists but does not pin .NET ${REQUIRED_DOTNET_MAJOR}.x."
    fail=$((fail + 1))
    return 1
  fi
}

note_mgcb_editor() {
  # MGCB Editor is project-local in current MonoGame templates: scaffolded
  # projects ship a .config/dotnet-tools.json. There is no global tool to check
  # for at this layer - restore happens per project via `dotnet tool restore`.
  yellow "[note] MGCB Editor is installed per-project. After scaffolding, run 'dotnet tool restore' inside the project, then 'dotnet mgcb-editor'."
  if [[ "$(uname -m)" == "arm64" ]]; then
    yellow "       Apple Silicon: verify editor launch on the first M-series machine; if arm64 launch fails, try x64 .NET via Rosetta."
  fi
}

echo "MonoGame research project - environment check"
echo "---------------------------------------------"
check_macos
check_global_json
if check_dotnet; then
  check_template
else
  yellow "[skip] template check skipped because dotnet is missing."
  warn=$((warn + 1))
fi
note_mgcb_editor
echo "---------------------------------------------"

if (( fail > 0 )); then
  red "${fail} blocking issue(s); ${warn} warning(s)."
  exit 1
fi
if (( warn > 0 )); then
  yellow "0 blocking issues; ${warn} warning(s)."
  exit 0
fi
green "Environment ready."
