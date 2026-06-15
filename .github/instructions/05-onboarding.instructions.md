# Project Onboarding — Do This First

> **This is a fresh boilerplate dropped into a new workspace.** Before generating, scaffolding, or
> editing any application code, the agent must establish the project's identity. Treat this as the
> very first step of the first task in a new workspace.

## First action in a new workspace

1. **Confirm the project name / namespace prefix.** The standards use `YourApp` as a placeholder.
   Ask the user a single question, e.g. _"What is the project's namespace prefix (the `YourApp`
   placeholder)?"_ — unless they have already stated it.
2. **If the user has already given a name** (in the prompt, the folder name, or an existing
   `.csproj`/`.sln`), adopt it without asking.
3. **Record the chosen name** in [`../memory_bank/projectBrief.md`](../memory_bank/projectBrief.md)
   and [`../memory_bank/techContext.md`](../memory_bank/techContext.md) so it persists across
   sessions.

## Apply the name everywhere

Once the name is known, replace the `YourApp` placeholder as you touch each area (no bulk rename
needed up front — substitute as you create files):

- Namespaces in all new `.cs` files (`namespace <Name>.Domain;` etc.).
- Project and folder names under `src/` and `tests/` (`src/<Name>.Domain`, …).
- `companyName` in [`../stylecop.json`](../stylecop.json), if it should differ.
- Any `<Name>`/`YourApp` references in config you create.

## Confirm the other project facts

While onboarding, confirm (or accept sensible defaults for) the values listed under **Project facts**
in [`00-overview.instructions.md`](00-overview.instructions.md): target framework, primary database engine, and test framework.
Update that section and the `memory_bank/` files with the answers.

> Do not run `dotnet build`/`test`/`publish` in this workspace — the maintainer builds elsewhere.
