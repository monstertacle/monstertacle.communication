# Tasks: MCP Streamable HTTP adapter

- [x] T001 Inventory Communication and the Hosting session feature. Specify
  the transport and host responsibility boundary before changing source.
- [x] T002 Add the dual-target package project and SDK integration.
- [x] T003 Cover tool discovery, typed calls, rejected requests, identity
  isolation, cancellation, duplicates and fresh-client recovery.
- [x] T004 Verify both runtime targets, configure native Linux and Windows
  qualification, and verify local package/source consumption.
- [x] T005 Publish the verified branch and pull request. Record the immutable
  commit and integration contract for Hosting.

The [native CI run](https://github.com/monstertacle/monstertacle.communication/actions/runs/44828320914)
passed eight cases on each framework on both Linux and Windows and built the
package on both platforms. A standalone .NET 11 host built against the public
source and the local package. See the [pull request](https://github.com/monstertacle/monstertacle.communication/pull/2).
