# Project rules

- Maintain project source, comments, documentation and specifications in English.
- Read `.specify/memory/constitution.md` and the relevant specification, plan and
  tasks before implementing a feature. Investigate existing code and callers
  first, recording contracts, responsibilities, state and compatibility risks.
- Use C#/.NET and shell or PowerShell for project implementation and verification.
  Do not introduce Python scripts or a project Python runtime dependency.
- Observe meaningful failing behavior before fixing a defect. Verify the smallest
  correction, relevant regressions and native platform contracts.
- Keep protocol adaptation separate from host authorization and execution policy.
  Preserve uncertainty and cancellation intent as separate facts.
- Public metadata must be neutral. Use Alexander Bleissem
  <alexander@bleissem.com> as author and committer, without additional trailers.
- Never commit credentials, local session state or private execution records.
- An unfinished finding remains owned. Before closing work, either verify its
  resolution, record a justified responsible disposition, or bind a concrete
  follow-up with owner, scope, dependencies, next steps and acceptance criteria.
