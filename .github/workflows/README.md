# Docs — Local build & dev

Prerequisites
- Python 3.8+ and pip

Install
1. python -m pip install --upgrade pip
2. pip install mkdocs mkdocs-material

Run locally
- mkdocs serve
  Open http://127.0.0.1:8000 in your browser.

Build for preview
- mkdocs build --site-dir site

Notes
- The CI builds the site and runs a link checker. If you add external links, double-check them here before opening PRs.
