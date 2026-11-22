# Implementation plan

The [Hosting rollout plan](https://github.com/monstertacle/monstertacle.hosting/blob/main/specs/011-staged-secret-protection/plan.md)
defines the common check. At source revision 623ebb2bb77a0ee2b129eb276133ddb2df8e6dba, AGENTS.md
already bars credentials and local state from commits. The Constitution's
least-authority principle establishes custody. Add a contributor check where
Git's index supplies the input and redacted status is the only scanner output.
The protocol adapter and its host contracts have no caller or dependency on
this workflow.

Amend principle V to require staged review and a fail-closed scan. Qualify the
scanner with an isolated synthetic fixture and verify the Linux and Windows
checks in the existing Build and test workflow.
