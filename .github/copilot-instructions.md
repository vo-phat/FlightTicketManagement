# Copilot Instructions for FlightTicketManagement

## Project Overview
- **Type:** Windows Forms Application (.NET 8, C#)
- **Purpose:** Manage flight ticket operations, including user authentication, ticketing, flight management, and statistics.
- **Major Folders:**
  - `BUS/` — Business logic layer
  - `DAO/` — Data access layer
  - `DTO/` — Data transfer objects
  - `GUI/` — UI components and forms (organized by feature)
  - `Database/script.sql` — Database schema and seed data

## Architecture & Patterns
- **Layered Architecture:**
  - `GUI` interacts with `BUS` (business logic), which calls `DAO` for data access. `DTO` objects are used for data transfer between layers.
- **Feature-based UI:**
  - Each feature (e.g., Account, Auth, Flight, Ticket) has its own subfolder in `GUI/Features/` with corresponding forms and controls.
- **Custom Controls:**
  - Reusable UI components are in `GUI/Components/` (e.g., `PrimaryButton`, `UnderlinedTextField`).
- **Resource Management:**
  - Images and static assets are in `GUI/Resources/` and `Properties/Resources.resx`.

## Developer Workflows
- **Build:**
  - Use Visual Studio or run `dotnet build` in the project root.
- **Run:**
  - Use Visual Studio or `dotnet run` (ensure Windows desktop runtime is available).
- **Database:**
  - Schema and seed data in `Database/script.sql`. Update this file for DB changes.
- **Debugging:**
  - Standard Visual Studio debugging applies. Set breakpoints in code-behind files (e.g., `*.cs` in `GUI/Features/`).

## Project-Specific Conventions
- **Naming:**
  - Folders and files are PascalCase. Controls and forms are suffixed with `Control` or `Form`.
- **Separation of Concerns:**
  - Avoid business logic in UI code; keep it in `BUS/`.
- **Resource Usage:**
  - Use `Properties/Resources.resx` for localizable strings and images.
- **Extending Features:**
  - Add new features by creating a new subfolder in `GUI/Features/` and corresponding logic in `BUS/`, `DAO/`, and `DTO/`.

## Integration Points
- **Database:**
  - All data access is via `DAO/` classes. Update `script.sql` for schema changes.
- **External Libraries:**
  - No non-standard dependencies detected; uses .NET built-in libraries.

## Examples
- **Add a new feature:**
  1. Create a new folder in `GUI/Features/` (e.g., `Booking/`).
  2. Add forms/controls for the feature.
  3. Implement business logic in `BUS/Booking/`.
  4. Add data access in `DAO/Booking/` and DTOs in `DTO/Booking/`.

---

**For AI agents:**
- Follow the layered structure and naming conventions.
- Reference existing features for implementation patterns.
- Keep business logic out of UI code.
- Update this file if you discover new project-specific patterns.
