# Monstertacle.Communication

Communication adapters between applications and controlled agent execution.

This repository contains the specification workflow and project conventions.
No protocol adapter or runtime package is implemented yet.

AG-UI is an initial integration candidate. Protocol types and transports should
reuse maintained upstream packages. Execution admission, authorization and
application policy remain responsibilities of the consuming host.

## Specification workflow

- Project principles: [constitution](.specify/memory/constitution.md).
- Feature documents: `specs/<number>-<name>/`.
- Shared Spec Kit templates: `.specify/templates/`.
- Feature and plan helpers: `.specify/scripts/bash/` and
  `.specify/scripts/powershell/`.

The shared scaffolding is based on Spec Kit 1.2.2 and uses no editor-specific
integration. Define and review a feature specification, architecture research,
contracts, implementation plan and acceptance tasks before adding product code.

Shell helper example (preview only):

```sh
bash .specify/scripts/bash/create-new-feature.sh --dry-run --json \
  --number 1 --short-name protocol-adapter 'Describe a protocol adapter'
```

## Status

Repository initialization only. API, package boundaries and the first feature
remain to be specified. See [project rules](AGENTS.md).

## Runtime target requirement

The maintained target scope is .NET 10 **and additionally .NET 11**. The first
adapter implementation must qualify both runtimes on native Linux and Windows,
including protocol contracts, rejection, cancellation and recovery. Reuse compatible
maintained upstream packages and keep execution admission in the consuming host.

There is currently no C# project, adapter, test suite or runtime package in this
repository. These are requirements for future implementation, not a claim of
implemented runtime support. See [runtime support scope](specs/001-runtime-support-scope/spec.md).
