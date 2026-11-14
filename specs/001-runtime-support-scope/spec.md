# Runtime support scope

The owner requested additional .NET 11 support throughout the product family,
retaining .NET 10. This repository currently contains no adapter implementation,
project, tests or runtime package. Its runtime scope must therefore distinguish
future requirements from implemented support.

FR-001: Future maintained .NET adapters must support .NET 10 and .NET 11, using
matching compatible upstream protocol packages and isolated host-policy boundaries.
FR-002: Native Linux/Windows and each supported runtime require actual contract,
rejection, cancellation and recovery evidence before a support claim or release.
FR-003: Do not create a placeholder runtime package or claim execution from scaffolding.

Acceptance: maintained README records this scope and explicitly preserves the
unimplemented status; the current constitution's evidence and policy boundaries
remain unchanged. No runtime API or protocol implementation is introduced.
