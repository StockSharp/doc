#!/usr/bin/env python3
"""
Add or remove a connector's links across every localized index in this repository.

A connector is listed in four places, once per language, and a page that is
reachable from none of them is invisible while a link to a page that is gone
breaks the build:

* topics/toc.yml                                          - the tree, plus the
                                                            Hydra video entry
* topics/hydra/data_sources.md                            - a table row
* topics/designer/connections_settings/connectors_settings.md - a table row
* topics/hydra/videos/sources_samples.md                  - a bullet, only for
                                                            connectors that have
                                                            a video

Wording is never invented here. "add" copies the entries of a connector that is
already listed and substitutes the name, so each language keeps the phrasing its
own file already uses. That is why --like is required.

The connector's own pages are neither written nor deleted: this manages links,
and a delisted connector keeps its documentation, reachable by its own address.

Nothing outside this repository is touched. The connector table in the core
repository's README is a separate file in a separate repository and is not
managed here.

Usage
-----
    python tools/connector_links.py check  bitmex
    python tools/connector_links.py remove bitmex            # prints the plan
    python tools/connector_links.py remove bitmex --apply    # writes it
    python tools/connector_links.py add    kraken2 --like kraken --name Kraken2 --apply

Without --apply nothing is written; the plan is printed instead.
"""

from __future__ import annotations

import argparse
import re
import sys
from dataclasses import dataclass
from difflib import SequenceMatcher
from pathlib import Path


REPO = Path(__file__).resolve().parent.parent

TOC = "topics/toc.yml"
TABLES = (
    "topics/hydra/data_sources.md",
    "topics/designer/connections_settings/connectors_settings.md",
)
VIDEO_INDEX = "topics/hydra/videos/sources_samples.md"

CONNECTOR_HREF = re.compile(r"api/connectors/([a-z_]+)/([a-z0-9_.-]+)\.md")
VIDEO_HREF = "hydra/videos/sources_samples/{slug}.md"


def languages() -> list[str]:
    """The two-letter documentation roots, in a stable order."""
    return sorted(
        p.name for p in REPO.iterdir()
        if p.is_dir() and len(p.name) == 2 and (p / "topics").is_dir()
    )


@dataclass
class Edit:
    """One file's worth of change, kept as whole lines so newlines survive."""
    path: Path
    newline: bytes
    before: list[bytes]
    after: list[bytes]
    what: str

    @property
    def changed(self) -> bool:
        return self.before != self.after

    def lines_changed(self) -> list[tuple[str, bytes]]:
        """
        The lines this edit drops and adds, so a dry run shows the text itself.

        Positional, not by membership: a line such as `items:` occurs all over
        the file, and testing whether it occurs elsewhere would hide it here.
        """
        out = []
        matcher = SequenceMatcher(a=self.before, b=self.after, autojunk=False)
        for tag, i1, i2, j1, j2 in matcher.get_opcodes():
            if tag in ("delete", "replace"):
                out += [("-", l) for l in self.before[i1:i2]]
            if tag in ("insert", "replace"):
                out += [("+", l) for l in self.after[j1:j2]]
        return out

    def write(self) -> None:
        self.path.write_bytes(self.newline.join(self.after))


def read_lines(path: Path) -> tuple[list[bytes], bytes]:
    raw = path.read_bytes()
    newline = b"\r\n" if raw.count(b"\r\n") * 2 >= raw.count(b"\n") else b"\n"
    return raw.split(newline), newline


def indent_of(line: bytes) -> int:
    return len(line) - len(line.lstrip())


# --------------------------------------------------------------------------- #
# locating
# --------------------------------------------------------------------------- #

def toc_block(lines: list[bytes], slug: str) -> tuple[int, int, str] | None:
    """The `- name:` item owning the connector page, as [start, end) and section."""
    for i, line in enumerate(lines):
        m = CONNECTOR_HREF.search(line.decode("utf-8", "replace"))
        if not m or m.group(2) != slug:
            continue
        if not line.strip().startswith(b"href:"):
            continue
        start = i - 1
        if start < 0 or not lines[start].strip().startswith(b"- name:"):
            continue
        own = indent_of(lines[start])
        end = i + 1
        while end < len(lines):
            cur = lines[end]
            if not cur.strip():
                end += 1
                continue
            ind = indent_of(cur)
            if ind < own or (ind == own and cur.strip().startswith(b"- ")):
                break
            end += 1
        return start, end, m.group(1)
    return None


def toc_video_block(lines: list[bytes], slug: str) -> tuple[int, int] | None:
    want = VIDEO_HREF.format(slug=slug).encode()
    for i, line in enumerate(lines):
        if line.strip() == b"href: " + want:
            start = i - 1
            if start >= 0 and lines[start].strip().startswith(b"- name:"):
                return start, i + 1
    return None


def table_rows(lines: list[bytes], slug: str) -> list[int]:
    """Indices of table rows whose first cell links to the connector page."""
    hit = []
    for i, line in enumerate(lines):
        if not line.startswith(b"| ["):
            continue
        m = CONNECTOR_HREF.search(line.decode("utf-8", "replace"))
        if m and m.group(2) == slug:
            hit.append(i)
    return hit


def video_bullets(lines: list[bytes], slug: str) -> list[int]:
    want = ("sources_samples/%s.md" % slug).encode()
    return [i for i, l in enumerate(lines) if l.lstrip().startswith(b"- [") and want in l]


def display_name(row: bytes) -> str:
    """The link text of a table row's first cell, which is the connector's name."""
    m = re.match(r"\|\s*\[([^\]]+)\]", row.decode("utf-8", "replace"))
    return m.group(1) if m else ""


def toc_display_name(line: bytes) -> str:
    return line.decode("utf-8", "replace").split("- name:", 1)[1].strip()


# --------------------------------------------------------------------------- #
# rewriting
# --------------------------------------------------------------------------- #

def substitute(block: list[bytes], old_slug: str, new_slug: str,
               old_name: str, new_name: str) -> list[bytes]:
    out = []
    for line in block:
        text = line.decode("utf-8")
        text = text.replace("/%s.md" % old_slug, "/%s.md" % new_slug)
        text = text.replace("/%s/" % old_slug, "/%s/" % new_slug)
        text = text.replace("_%s.md" % old_slug, "_%s.md" % new_slug)
        if old_name:
            text = text.replace(old_name, new_name)
        out.append(text.encode("utf-8"))
    return out


def repad(row: bytes, template: bytes) -> bytes:
    """Keep a table's columns lined up after the text in them changed length."""
    cells = row.decode("utf-8").split("|")
    widths = [len(c) for c in template.decode("utf-8").split("|")]
    if len(cells) != len(widths):
        return row
    out = []
    for cell, width in zip(cells, widths):
        stripped = cell.strip()
        if not stripped:
            out.append(cell)
            continue
        out.append(" " + stripped.ljust(max(width - 2, len(stripped))) + " ")
    return "|".join(out).rstrip().encode("utf-8")


def insertion_point(lines: list[bytes], candidates: list[int], name: str,
                    name_of) -> int:
    """Where `name` belongs among rows that are already sorted by name."""
    for i in candidates:
        if name_of(lines[i]).lower() > name.lower():
            return i
    return candidates[-1] + 1 if candidates else len(lines)


CONNECTORS_DIR = "topics/api/connectors"
CLOSED_SET_FILE = "tests/DocumentationValidationTests.cs"
CLOSED_SET_MARKER = "_retiredConnectors = new(StringComparer.OrdinalIgnoreCase)"
BANNER_HEAD = "> [!CAUTION]"


def connector_pages(lang: str, slug: str) -> tuple[str, list[Path]]:
    """The overview page and its detail pages, plus the category they sit in."""
    root = REPO / lang / CONNECTORS_DIR
    if not root.is_dir():
        return "", []
    for category in sorted(p.name for p in root.iterdir() if p.is_dir()):
        overview = root / category / ("%s.md" % slug)
        if not overview.exists():
            continue
        details = sorted((root / category / slug).glob("*.md")) if (root / category / slug).is_dir() else []
        return category, [overview] + details
    return "", []


def banner_of(path: Path) -> bytes | None:
    """The notice line of a page that already carries one."""
    lines, _ = read_lines(path)
    if len(lines) > 1 and lines[0].strip() == BANNER_HEAD.encode() and lines[1].startswith(b"> **"):
        return lines[1]
    return None


def apply_banner(path: Path, notice: bytes) -> Edit | None:
    lines, nl = read_lines(path)
    after = list(lines)
    if banner_of(path) is not None:
        if after[1] == notice:
            return None
        after[1] = notice
        what = "notice replaced"
    else:
        after = [BANNER_HEAD.encode(), notice, b""] + after
        what = "notice added"
    return Edit(path, nl, lines, after, "%s: %s" % (path.relative_to(REPO).as_posix(), what))


def register_closed(category: str, slug: str) -> Edit | None:
    """Record the connector in the set the documentation test reads."""
    path = REPO / CLOSED_SET_FILE
    if not path.exists():
        return None
    lines, nl = read_lines(path)
    entry = ('		"%s/%s",' % (category, slug)).encode()
    if entry in lines:
        return None
    at = next((i for i, l in enumerate(lines) if CLOSED_SET_MARKER.encode() in l), None)
    if at is None:
        return None
    start = at + 2                      # past the opening brace
    end = next(i for i in range(start, len(lines)) if lines[i].strip() == b"};")

    # A comment belongs to the entry under it, so entries are moved as blocks
    # rather than as lines; sorting the lines alone would strand the comments.
    blocks, pending = [], []
    for line in lines[start:end]:
        pending.append(line)
        if line.strip().startswith(b'"'):
            blocks.append(pending)
            pending = []
    blocks.append([entry])
    blocks.sort(key=lambda b: b[-1].strip().lower())
    after = lines[:start] + [l for b in blocks for l in b] + pending + lines[end:]
    return Edit(path, nl, lines, after, "%s: %s/%s registered as closed" % (CLOSED_SET_FILE, category, slug))


def plan_close(slug: str, like: str, name: str, langs: list[str]) -> list[Edit]:
    edits: list[Edit] = []
    category = ""

    for lang in langs:
        found, pages = connector_pages(lang, slug)
        if not pages:
            print("  %s: no pages for %r, skipped" % (lang, slug))
            continue
        category = category or found

        notice = None
        for page in pages:
            notice = banner_of(page)
            if notice is not None:
                break

        if notice is None:
            if not like:
                print("  %s: %r carries no notice yet and --like was not given, skipped" % (lang, slug))
                continue
            _, template_pages = connector_pages(lang, like)
            template = next((banner_of(p) for p in template_pages if banner_of(p) is not None), None)
            if template is None:
                print("  %s: template %r carries no notice either, skipped" % (lang, like))
                continue
            notice = substitute([template], like, slug, display_name_of(template_pages[0]), name)[0]

        for page in pages:
            edit = apply_banner(page, notice)
            if edit:
                edits.append(edit)

    edits += plan_remove(slug, langs)
    if category:
        registered = register_closed(category, slug)
        if registered:
            edits.append(registered)
    return edits


def display_name_of(overview: Path) -> str:
    """A page's own title, which is what its notice calls the venue."""
    for line in overview.read_text(encoding="utf-8", errors="replace").splitlines():
        if line.startswith("# "):
            return line[2:].strip()
    return overview.stem


# --------------------------------------------------------------------------- #
# operations
# --------------------------------------------------------------------------- #

def plan_remove(slug: str, langs: list[str]) -> list[Edit]:
    edits = []
    for lang in langs:
        toc = REPO / lang / TOC
        if toc.exists():
            lines, nl = read_lines(toc)
            after = list(lines)
            removed = []
            block = toc_block(after, slug)
            if block:
                start, end, _ = block
                removed.append("tree entry (%d lines)" % (end - start))
                del after[start:end]
            video = toc_video_block(after, slug)
            if video:
                start, end = video
                removed.append("video entry (%d lines)" % (end - start))
                del after[start:end]
            if removed:
                edits.append(Edit(toc, nl, lines, after, "%s: %s" % (TOC, ", ".join(removed))))

        for rel in TABLES + (VIDEO_INDEX,):
            path = REPO / lang / rel
            if not path.exists():
                continue
            lines, nl = read_lines(path)
            drop = table_rows(lines, slug) if rel in TABLES else video_bullets(lines, slug)
            if not drop:
                continue
            after = [l for i, l in enumerate(lines) if i not in set(drop)]
            edits.append(Edit(path, nl, lines, after, "%s: %d line(s)" % (rel, len(drop))))
    return edits


def plan_add(slug: str, like: str, name: str, langs: list[str]) -> list[Edit]:
    edits = []
    for lang in langs:
        toc = REPO / lang / TOC
        if toc.exists():
            lines, nl = read_lines(toc)
            if toc_block(lines, slug):
                print("  %s/%s: already listed, left alone" % (lang, TOC))
            else:
                src = toc_block(lines, like)
                if not src:
                    print("  %s/%s: template %r is not listed, skipped" % (lang, TOC, like))
                else:
                    start, end, section = src
                    template_name = toc_display_name(lines[start])
                    block = substitute(lines[start:end], like, slug, template_name, name)
                    siblings = []
                    for i, l in enumerate(lines):
                        if not l.strip().startswith(b"- name:"):
                            continue
                        if indent_of(l) != indent_of(lines[start]):
                            continue
                        nxt = lines[i + 1] if i + 1 < len(lines) else b""
                        m = CONNECTOR_HREF.search(nxt.decode("utf-8", "replace"))
                        if m and m.group(1) == section:
                            siblings.append(i)
                    at = insertion_point(lines, siblings, name, toc_display_name)
                    after = lines[:at] + block + lines[at:]
                    edits.append(Edit(toc, nl, lines, after, "%s: tree entry (%d lines)" % (TOC, len(block))))

        for rel in TABLES:
            path = REPO / lang / rel
            if not path.exists():
                continue
            lines, nl = read_lines(path)
            if table_rows(lines, slug):
                print("  %s/%s: already listed, left alone" % (lang, rel))
                continue
            src = table_rows(lines, like)
            if not src:
                print("  %s/%s: template %r is not listed, skipped" % (lang, rel, like))
                continue
            template = lines[src[0]]
            section = CONNECTOR_HREF.search(template.decode("utf-8", "replace")).group(1)
            row = substitute([template], like, slug, display_name(template), name)[0]
            row = repad(row, template)
            same = [i for i in table_rows_any(lines) if row_section(lines[i]) == section]
            at = insertion_point(lines, same, name, display_name)
            after = lines[:at] + [row] + lines[at:]
            edits.append(Edit(path, nl, lines, after, "%s: table row" % rel))
    return edits


def table_rows_any(lines: list[bytes]) -> list[int]:
    return [i for i, l in enumerate(lines) if l.startswith(b"| [") and CONNECTOR_HREF.search(l.decode("utf-8", "replace"))]


def row_section(line: bytes) -> str:
    m = CONNECTOR_HREF.search(line.decode("utf-8", "replace"))
    return m.group(1) if m else ""


def report_check(slug: str, langs: list[str]) -> int:
    total = 0
    for lang in langs:
        found = []
        toc = REPO / lang / TOC
        if toc.exists():
            lines, _ = read_lines(toc)
            if toc_block(lines, slug):
                found.append("toc tree")
            if toc_video_block(lines, slug):
                found.append("toc video")
        for rel in TABLES + (VIDEO_INDEX,):
            path = REPO / lang / rel
            if not path.exists():
                continue
            lines, _ = read_lines(path)
            hits = table_rows(lines, slug) if rel in TABLES else video_bullets(lines, slug)
            if hits:
                found.append("%s (%d)" % (Path(rel).name, len(hits)))
        total += len(found)
        print("  %-3s %s" % (lang, ", ".join(found) if found else "not listed"))
    return total


def main() -> int:
    # The tables carry Russian, Japanese and Chinese; a console in a single-byte
    # code page must not be able to stop the run.
    try:
        sys.stdout.reconfigure(encoding="utf-8", errors="replace")
    except AttributeError:
        pass

    parser = argparse.ArgumentParser(description=__doc__.splitlines()[1])
    parser.add_argument("action", choices=["check", "add", "remove", "close"])
    parser.add_argument("slug", help="the connector page's file name without .md, e.g. bitmex")
    parser.add_argument("--like", help="an already listed connector to copy the wording from (add only)")
    parser.add_argument("--name", help="display name; defaults to the slug capitalised (add only)")
    parser.add_argument("--langs", nargs="*", help="limit to these languages; default is all")
    parser.add_argument("--apply", action="store_true", help="write the changes; without it they are only printed")
    args = parser.parse_args()

    langs = args.langs or languages()

    if args.action == "check":
        print("%s is listed in:" % args.slug)
        return 0 if report_check(args.slug, langs) else 1

    if args.action == "close":
        edits = plan_close(args.slug, args.like, args.name or args.slug.capitalize(), langs)
    elif args.action == "add":
        if not args.like:
            parser.error("add needs --like, so the wording of each language comes from that language's own file")
        edits = plan_add(args.slug, args.like, args.name or args.slug.capitalize(), langs)
    else:
        edits = plan_remove(args.slug, langs)

    edits = [e for e in edits if e.changed]
    if not edits:
        print("nothing to do")
        return 0

    for e in edits:
        print("  %s" % e.path.relative_to(REPO).as_posix())
        print("      %s" % e.what)
        for sign, line in e.lines_changed():
            print("      %s %s" % (sign, line.decode("utf-8", "replace").rstrip()[:150]))

    if not args.apply:
        print("\n%d file(s) would change. Re-run with --apply to write." % len(edits))
        return 0

    for e in edits:
        e.write()
    print("\n%d file(s) written." % len(edits))
    return 0


if __name__ == "__main__":
    sys.exit(main())
