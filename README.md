# Unity Helpers

A collection of small, focused helper packages for Unity.

Each helper is distributed as a **separate git branch** containing a standalone Unity package.  
The `main` branch is **documentation-only** and serves as an entry point to the project.

---

## Repository structure

- **main** — documentation only
- **one branch = one Unity package**
- Packages are installed via [`helpers-installer`](https://github.com/NateArasti/UnityHelpers/tree/helpers-installer) or directly from their branch via Unity Package Manager

---

## Helpers

- [`helpers-installer`](https://github.com/NateArasti/UnityHelpers/tree/helpers-installer) — Editor window for installing other helpers from this and other repos
- [`helper-functions`](https://github.com/NateArasti/UnityHelpers/tree/helper-functions) — Common utility extensions (math, vectors, etc.)
- [`pretty-logging`](https://github.com/NateArasti/UnityHelpers/tree/pretty-logging) — Improved / formatted logging helpers
- [`extended-file-logger`](https://github.com/NateArasti/UnityHelpers/tree/extended-file-logger) — File logging support
- [`unotes`](https://github.com/NateArasti/UnityHelpers/tree/unotes) — Lightweight editor notes utility

---

## Installation

Install [`helpers-installer`](https://github.com/NateArasti/UnityHelpers/tree/helpers-installer) first if you want a GUI-based way to add other helpers.

### Unity Package Manager (recommended)

1. Open **Window → Package Manager**
2. Click **+ → Add package from git URL…**
3. Use the repo URL with a branch name:

```
https://github.com/NateArasti/UnityHelpers.git#helpers-installer
```

### Via `manifest.json`

Add desired row to `manifest.json` like this:

```json
{
  "dependencies": {
    "com.natearasti.helpers-installer":
      "https://github.com/NateArasti/UnityHelpers.git#helpers-installer"
  }
}
