# Monstertacle.Communication Constitution

## I. Focused communication boundary

The project connects applications with controlled agent execution. Protocol
adapters translate commands, observations and results. The consuming host owns
authorization, execution admission and application policy.

## II. Explicit contracts and compatibility

Specify public contracts and observable behavior before implementation. Reuse
maintained upstream protocol packages and existing Monstertacle capabilities.
Keep protocol dependencies at adapter boundaries. Document compatibility and
persisted-format effects before changing them.

## III. Evidence before completion

Distinguish accepted input, started execution, observed progress, uncertain
outcome and confirmed completion. A disconnected client or cancellation request
does not establish that external work ended. Reconnection must not silently
resubmit uncertain work.

## IV. Tests and quality

Demonstrate a meaningful failing test before implementing a defect correction.
Cover protocol mapping, rejection, duplicate input, interruption and recovery
with offline contract tests. Establish native Linux and Windows qualification
for supported behavior before release. Missing tests are not passing evidence.

## V. Least authority and data minimization

Client-supplied messages, tools and state are inputs for validation, not execution
authority. Project only authorized information. Keep credentials and internal
execution context out of provider messages, public events and source control.

## VI. Maintainable delivery

Use English for maintained project content, neutral public metadata and explicit
source ownership. Record changes, actual verification and all remaining work.
Open acceptance blockers prevent closure.

## Governance

Apply this constitution to every feature specification and plan. Amendments must
state their reason, affected contracts and verification. Project initialization
does not approve a runtime API or claim an implemented integration.

**Version**: 1.0.0 | **Ratified**: 2026-11-03 | **Last Amended**: 2026-11-03
