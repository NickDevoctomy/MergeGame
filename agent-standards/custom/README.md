# Your Custom Standards

This folder is **reserved for you**. Add your own standards files here and they become part of the
canonical rule set without touching anything else.

## How to add a standard

1. Create a new markdown file in this folder, e.g. `logging.md` or `git-workflow.md`.
2. Write your rules as clear, scannable bullet points (see the sibling files in `agent-standards/`
   for the house style).
3. Link it from the **Standards index** in [`../00-overview.md`](../00-overview.md) so it's
   discoverable.
4. (Optional) If the standard should be auto-applied for specific file types in a particular tool,
   add a one-line reference to it from that tool's adapter:
   - GitHub Copilot: a new `.github/instructions/<name>.instructions.md` with an `applyTo` glob.
   - Cursor: a new `.cursor/rules/<name>.mdc` with `globs`/`alwaysApply` frontmatter.
   - Cline / Roo / Windsurf: a new file in `.clinerules/`, `.roo/rules/`, or `.windsurf/rules/`.

   Each adapter only needs to say *"Follow `agent-standards/custom/<name>.md`."* — keep the content
   here, not in the adapter.

## Why keep custom rules here?

Everything stays DRY: the rule lives in exactly one place, and every AI assistant reads it through
the existing wiring. You never have to duplicate a rule across six tool-specific files.
