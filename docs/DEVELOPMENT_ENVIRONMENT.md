# 🛠️ Development Environment Setup Guide

**Veiled Dominion Engine** — Cloud Development Container Configuration

---

## 📋 Quick Start

This project uses **Visual Studio Code Dev Containers** to provide a unified, containerized development sandbox. No local .NET or Node.js installation required.

### Minimum Requirements
- **Docker Desktop** (installed and running)
- **VS Code** with the [Dev Containers extension](https://marketplace.visualstudio.com/items?itemName=ms-vscode-remote.remote-containers)
- **4GB RAM available** for the container

### Launch the Sandbox (60 seconds)

1. Clone the repository:
   ```bash
   git clone https://github.com/Loptr-Lab/veiled-dominion-engine.git
   cd veiled-dominion-engine
   ```

2. Open in VS Code:
   ```bash
   code .
   ```

3. When prompted: **"Reopen in Container"** (or press `Ctrl+Shift+P` → `Dev Containers: Reopen in Container`)

4. Wait for the container to initialize. The first build takes ~3-5 minutes. Subsequent launches are instant.

5. Verify the setup:
   ```bash
   dotnet --version    # Should output 8.0.x
   node --version      # Should output 20.x
   npm --version       # Should output 10.x
   ```

---

## 🏗️ Container Architecture

### Base Image
- **Runtime:** `mcr.microsoft.com/devcontainers/dotnet:8.0` (Microsoft's official .NET 8 devcontainer)
- **OS:** Debian Bookworm (lightweight, optimized for development)

### Pre-Installed Components

| Component | Version | Purpose |
|-----------|---------|---------|
| **.NET SDK** | 8.0.x | Core game logic compilation & execution |
| **Node.js** | 20 LTS | AT Protocol (`atproto`) tooling & JSON schema validation |
| **npm** | 10.x | Package manager for AT Proto dependencies |
| **Git** | Latest | Version control (pre-configured) |
| **C# Dev Kit** | Latest | Language server, debugging, code analysis |

### VS Code Extensions (Auto-Installed)

| Extension | Purpose |
|-----------|---------|
| `ms-dotnettools.csdevkit` | Official C# development kit (IntelliSense, debugging, refactoring) |
| `ms-dotnettools.csharp` | C# language server |
| `ms-dotnettools.vscode-dotnet-runtime` | Embedded .NET runtime management |
| `davidanson.vscode-markdownlint` | Markdown linting for documentation consistency |
| `ms-vscode.makefile-tools` | Build automation support |

---

## ⚙️ Post-Create Initialization

When the container boots, the `.devcontainer/devcontainer.json` runs:

```bash
dotnet restore && npm install --prefix ./src/network/atproto-layer
```

This command:

1. **`dotnet restore`**
   - Restores all NuGet packages defined in `.csproj` files
   - Creates the `/obj` dependency cache
   - Enables IntelliSense and compilation

2. **`npm install --prefix ./src/network/atproto-layer`**
   - Installs AT Protocol JavaScript dependencies
   - Sets up local PDS (Personal Data Server) simulation tooling
   - Prepares JSON schema validators for match state snapshots

**If initialization fails**, troubleshoot with:
```bash
# Rebuild the container from scratch
Dev Containers: Rebuild Container (VS Code command palette)

# Or from CLI
docker-compose -f .devcontainer/docker-compose.yml build --no-cache
```

---

## 🔗 Networking & Port Forwarding

The devcontainer automatically forwards these ports to `localhost`:

| Port | Service | Use Case |
|------|---------|----------|
| `5000` | HTTP (dev server) | Local game prototype testing |
| `5001` | HTTPS (secure) | AT Protocol handle verification handshakes |

To access the local prototype:
```
http://localhost:5000
```

---

## 📁 Directory Structure Inside Container

Once initialized, you'll have this workspace:

```
/workspaces/veiled-dominion-engine
├── .devcontainer/          # Container configuration (read-only reference)
├── src/
│   ├── /board              # 14x14 grid topology (C#)
│   ├── /pieces             # Piece locomotion classes (C#)
│   ├── /systems            # Radius of Ruin, Veil state machine (C#)
│   ├── /network/
│   │   └── /atproto-layer  # AT Protocol integration (Node.js/TypeScript)
│   └── /input              # Turn phase management UI hooks (C#)
├── docs/                   # Design documentation
├── assets/                 # 3D models, VFX, audio (future)
├── obj/                    # NuGet cache (auto-generated)
├── node_modules/           # npm dependencies (auto-generated)
└── README.md               # Playtest rulebook
```

---

## 🔧 Common Development Workflows

### Building the Project

```bash
# Restore dependencies (run once after clone, or after .csproj changes)
dotnet restore

# Build the solution
dotnet build

# Build with release optimizations
dotnet build -c Release

# Watch mode (auto-rebuild on file changes)
dotnet watch build
```

### Running Tests

```bash
# Run all unit tests
dotnet test

# Run tests for a specific project
dotnet test src/Board.Tests/Board.Tests.csproj

# Run with verbose output
dotnet test --verbosity normal
```

### AT Protocol (Node.js) Commands

```bash
# Navigate to the AT Proto layer
cd src/network/atproto-layer

# Run schema validation against match data
npm run validate:schema

# Simulate a local PDS handle registration
npm run pds:simulate

# Generate TypeScript types from AT Proto schemas
npm run generate:types
```

### Formatting & Linting

```bash
# Format C# code (in-place)
dotnet format

# Check Markdown formatting
npm run lint:markdown -- docs/

# Fix Markdown issues automatically
npm run lint:markdown -- docs/ --fix
```

---

## 🌐 Connecting to AT Protocol (Optional)

If you're testing match state serialization with actual AT Protocol handles:

1. Create a `.env` file in the repository root (git-ignored):
   ```env
   ATPROTO_HANDLE=your-handle.bsky.social
   ATPROTO_APP_PASSWORD=your-app-specific-password
   ```

2. The C# network layer will load these variables at runtime:
   ```csharp
   var handle = Environment.GetEnvironmentVariable("ATPROTO_HANDLE");
   var password = Environment.GetEnvironmentVariable("ATPROTO_APP_PASSWORD");
   ```

3. Run your application. Match state snapshots will be synchronized with your personal AT Protocol data repository.

---

## 🐛 Troubleshooting

### Issue: Container fails to initialize

**Solution:**
```bash
# Force a rebuild
Dev Containers: Rebuild Container

# If that fails, delete and recreate
docker system prune -a
Dev Containers: Rebuild Container
```

### Issue: IntelliSense/C# language server not working

**Solution:**
```bash
# Restart the language server
VS Code > Command Palette > "Omnisharp: Restart Omnisharp"

# Or reload VS Code window
Ctrl+R (Cmd+R on macOS)
```

### Issue: npm packages not installing

**Solution:**
```bash
# Manually trigger npm install
npm install --prefix ./src/network/atproto-layer

# Clear npm cache if corrupted
npm cache clean --force
npm install --prefix ./src/network/atproto-layer
```

### Issue: Port 5000/5001 already in use

**Solution:**
Edit `.devcontainer/devcontainer.json` and change the port mappings:
```json
"forwardPorts": [5002, 5003]
```

Then rebuild the container.

---

## 📚 Additional Resources

- **[Devcontainers Specification](https://containers.dev/)** — Official devcontainer standards
- **[.NET 8 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-8)** — Core language reference
- **[AT Protocol Specs](https://atproto.com/docs)** — Decentralized social protocol specs
- **[VS Code Remote Development](https://code.visualstudio.com/docs/remote/remote-overview)** — Container debugging & advanced workflows

---

## ✅ Verification Checklist

After initialization, verify your environment is ready:

- [ ] Container is running (green indicator in VS Code bottom-left)
- [ ] `dotnet --version` outputs `8.0.x`
- [ ] `node --version` outputs `20.x`
- [ ] `npm install --prefix ./src/network/atproto-layer` completes without errors
- [ ] C# IntelliSense works (type `DateTime.` and see autocomplete)
- [ ] You can open `src/board/Grid.cs` without errors
- [ ] Terminal can access both `dotnet` and `npm` commands

---

## 🚀 Next Steps

Once your sandbox is initialized:

1. **Read** [`CONTRIBUTING.md`](../CONTRIBUTING.md) for contribution guidelines and mechanical design principles
2. **Review** [`README.md`](../README.md) for the official Playtest Rulebook
3. **Start with Phase 1** tasks in [`CONTRIBUTING.md`](../CONTRIBUTING.md#-technical-implementation-phases):
   - Implement the 14×14 cross-shaped grid coordinate system
   - Build base piece movement classes

---

*Last Updated: 2026-07-01*  
*Maintainer: Loptr Lab (@ibloud)*
