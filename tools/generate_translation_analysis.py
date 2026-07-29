#!/usr/bin/env python3
"""
Generate one translation-analysis report for every Markdown-bearing folder.

English is the repository's default-language baseline. Connector documentation
is intentionally excluded:

* topics/api/connectors.md
* topics/api/connectors/**
* topics/designer/connections_settings/connectors_settings.md
* topics/hydra/data_sources.md

The reports are written outside the two-letter documentation roots so that the
documentation validator does not treat them as publishable localized content.
"""

from __future__ import annotations

import argparse
import math
import re
import statistics
from collections import Counter
from dataclasses import dataclass
from pathlib import Path, PurePosixPath
from typing import Iterable, Sequence


SOURCE_LANGUAGE = "en"
LANGUAGE_NAMES = {
    "de": "Deutsch",
    "es": "Español",
    "ja": "日本語",
    "pt": "Português",
    "ru": "Русский",
    "zh": "简体中文",
}

LANGUAGE_ORDER = tuple(LANGUAGE_NAMES)
REPORT_DATE = "2026-07-29"
CONNECTOR_AGGREGATOR_PATHS = {
    "topics/api/connectors.md",
    "topics/designer/connections_settings/connectors_settings.md",
    "topics/hydra/data_sources.md",
}
MANUAL_PATTERN_SUMMARIES = {
    "de": "переведённые API-идентификаторы, ошибки согласования и терминологические кальки",
    "es": "артефакты `XQX`/`lMA`, переведённые API-имена, грамматика и смешение форм обращения",
    "ja": "локализация идентификаторов внутри C#, разнобой терминов и неестественные кальки",
    "pt": "системное смешение pt-PT и pt-BR, переведённые идентификаторы и многочисленные ошибки согласования",
    "ru": (
        "историческая рассинхронизация с текущей английской веткой; "
        "из {auto_findings} автоматических кандидатов вручную подтверждены "
        "{manual_findings} групп реальных поломок или существенных разрывов"
    ),
    "zh": "переведённые идентификаторы и смысловые сужения; большинство сигналов полноты оказалось нормальным объединением китайских абзацев без потери содержания",
}
DEFAULT_MANUAL_SECTION = (
    "Дополнительные подтверждённые наблюдения не внесены.\n\n"
    "Ограничения методики и шкала приоритетов описаны в "
    "`translation-analysis/README.md`."
)

ENGLISH_STOP_WORDS = {
    "a",
    "after",
    "all",
    "also",
    "an",
    "and",
    "are",
    "as",
    "at",
    "be",
    "before",
    "button",
    "by",
    "can",
    "click",
    "configure",
    "data",
    "for",
    "from",
    "has",
    "if",
    "in",
    "into",
    "is",
    "it",
    "market",
    "of",
    "on",
    "open",
    "or",
    "order",
    "select",
    "set",
    "settings",
    "that",
    "the",
    "then",
    "this",
    "to",
    "trading",
    "use",
    "using",
    "when",
    "which",
    "will",
    "window",
    "with",
    "you",
    "your",
}

STRONG_ENGLISH_WORDS = {
    "after",
    "before",
    "button",
    "click",
    "configure",
    "from",
    "into",
    "open",
    "select",
    "settings",
    "that",
    "then",
    "the",
    "this",
    "using",
    "when",
    "which",
    "window",
    "with",
    "you",
    "your",
}

CODE_LANGUAGES = {
    "bash",
    "c#",
    "cs",
    "csharp",
    "css",
    "f#",
    "fs",
    "fsharp",
    "html",
    "javascript",
    "js",
    "json",
    "powershell",
    "ps1",
    "py",
    "python",
    "shell",
    "sql",
    "ts",
    "typescript",
    "xml",
    "yaml",
    "yml",
}
STRICT_EXECUTABLE_CODE_LANGUAGES = {
    "c#",
    "cs",
    "csharp",
    "css",
    "f#",
    "fs",
    "fsharp",
    "html",
    "javascript",
    "js",
    "py",
    "python",
    "sql",
    "ts",
    "typescript",
    "xml",
}

MOJIBAKE_MARKERS = (
    "\ufffd",
    "â€",
    "â„",
    "Ã¡",
    "Ã©",
    "Ã­",
    "Ã³",
    "Ãº",
    "Ã±",
    "Ã£",
    "Ã§",
    "Ð°",
    "Ð±",
    "Ð²",
    "Ñ€",
    "Ñ",
    "Ñ‚",
)

INLINE_CODE_PATTERN = re.compile(r"(?<!`)`([^`\r\n]+)`(?!`)")
FENCE_START_PATTERN = re.compile(r"^\s*(?P<fence>`{3,}|~{3,})(?P<info>.*)$")
HEADING_PATTERN = re.compile(r"^\s*(#{1,6})\s+\S")
ORDERED_LIST_PATTERN = re.compile(r"^\s*\d+[.)]\s+")
UNORDERED_LIST_PATTERN = re.compile(r"^\s*[-+*]\s+")
ADMONITION_PATTERN = re.compile(
    r"^\s*>\s*\[!(NOTE|TIP|IMPORTANT|WARNING|CAUTION)]", re.IGNORECASE
)
WORD_PATTERN = re.compile(r"[A-Za-z]+(?:'[A-Za-z]+)?")
NUMBER_PATTERN = re.compile(r"\d+", re.UNICODE)
CYRILLIC_PATTERN = re.compile(r"[А-Яа-яЁё]{3,}")
CJK_PATTERN = re.compile(r"[\u3040-\u30ff\u3400-\u9fff]{2,}")
TRANSLATION_ARTIFACT_PATTERN = re.compile(r"\bX[A-Z]X(?:\d+)?")
MANUAL_SECTION_PATTERN = re.compile(
    r"^## Ручная языковая проверка[ \t]*\r?\n"
    r"(?P<body>.*?)(?=^##(?!#)\s+\S|\Z)",
    re.MULTILINE | re.DOTALL,
)
MANUAL_FINDING_PATTERN = re.compile(
    r"^- \*\*(?P<priority>P[0-3])\*\*(?:\s|$)", re.MULTILINE
)


@dataclass(frozen=True)
class TextLine:
    line: int
    text: str


@dataclass(frozen=True)
class CodeBlock:
    line: int
    language: str
    body: str


@dataclass(frozen=True)
class Finding:
    priority: str
    location: str
    category: str
    observation: str
    recommendation: str
    confidence: str = "высокая"


@dataclass
class Document:
    relative: str
    path: Path
    raw: str
    visible_lines: list[TextLine]
    visible_text: str
    paragraphs: list[str]
    headings: tuple[int, ...]
    code_blocks: list[CodeBlock]
    link_targets: tuple[str, ...]
    image_targets: tuple[str, ...]
    table_shapes: tuple[tuple[int, int], ...]
    ordered_items: int
    unordered_items: int
    admonitions: tuple[str, ...]
    inline_technical: Counter[str]
    numbers: Counter[str]


@dataclass(frozen=True)
class RatioStats:
    median_log_ratio: float
    mad_log_ratio: float


def parse_args() -> argparse.Namespace:
    parser = argparse.ArgumentParser(
        description="Generate folder-by-folder translation analysis reports."
    )
    parser.add_argument(
        "--language",
        action="append",
        choices=LANGUAGE_ORDER,
        required=True,
        help="Target language. Repeat to generate more than one language.",
    )
    parser.add_argument(
        "--repo-root",
        type=Path,
        default=Path(__file__).resolve().parents[1],
        help="Documentation repository root.",
    )
    return parser.parse_args()


def is_connector_path(relative: str) -> bool:
    return relative in CONNECTOR_AGGREGATOR_PATHS or relative.startswith(
        "topics/api/connectors/"
    )


def normalize_space(value: str) -> str:
    return re.sub(r"\s+", " ", value).strip()


def normalize_comparable_text(value: str) -> str:
    value = value.replace(r"\|", "|").replace(r"\/", "/")
    value = normalize_space(value)
    return value.casefold()


def normalize_url(value: str) -> str:
    value = value.strip().strip("<>").replace("\\", "/")
    value = re.sub(
        r"^(https?://)(?:www\.)?stocksharp\.com/(?:en|ru|de|es|pt|ja|zh)(?=$|[/?#])",
        r"\1stocksharp.com/{lang}",
        value,
        flags=re.IGNORECASE,
    )
    value = re.sub(
        r"^(https?://)docs\.github\.com/(?:en|ru|de|es|pt|ja|zh)(?=$|[/?#])",
        r"\1docs.github.com/{lang}",
        value,
        flags=re.IGNORECASE,
    )
    return value


def enumerate_markdown_links(
    line: str,
) -> Iterable[tuple[bool, str, str, int, int]]:
    """Yield Markdown inline links while respecting parentheses in xref targets."""
    index = 0
    length = len(line)
    while index < length:
        image = line.startswith("![", index)
        if not image and line[index] != "[":
            index += 1
            continue

        label_start = index + 2 if image else index + 1
        cursor = label_start
        bracket_depth = 1
        while cursor < length and bracket_depth:
            if line[cursor] == "\\":
                cursor += 2
                continue
            if line[cursor] == "[":
                bracket_depth += 1
            elif line[cursor] == "]":
                bracket_depth -= 1
            cursor += 1

        if bracket_depth or cursor >= length or line[cursor] != "(":
            index += 2 if image else 1
            continue

        label_end = cursor - 1
        target_start = cursor + 1
        cursor = target_start
        parenthesis_depth = 1
        while cursor < length and parenthesis_depth:
            if line[cursor] == "\\":
                cursor += 2
                continue
            if line[cursor] == "(":
                parenthesis_depth += 1
            elif line[cursor] == ")":
                parenthesis_depth -= 1
            cursor += 1

        if parenthesis_depth:
            index += 2 if image else 1
            continue

        raw_target = line[target_start : cursor - 1].strip()
        if raw_target.startswith("<"):
            target_end = raw_target.find(">")
            target = raw_target[1:target_end] if target_end >= 0 else raw_target
        else:
            target = raw_target.split(maxsplit=1)[0] if raw_target else ""

        yield image, line[label_start:label_end], target, index, cursor
        index = cursor


def replace_markdown_links(line: str) -> str:
    links = list(enumerate_markdown_links(line))
    if not links:
        return line

    parts: list[str] = []
    cursor = 0
    for image, label, _, start, end in links:
        parts.append(line[cursor:start])
        parts.append(" " if image else label)
        cursor = end
    parts.append(line[cursor:])
    return "".join(parts)


def strip_visible_markdown(line: str) -> str:
    line = re.sub(r"<!--.*?-->", " ", line)
    line = replace_markdown_links(line)
    line = re.sub(r"<https?://[^>]+>", " ", line)
    line = re.sub(r"https?://\S+", " ", line)
    line = INLINE_CODE_PATTERN.sub(" ", line)
    line = re.sub(r"<[^>]+>", " ", line)
    line = re.sub(r"^\s*#{1,6}\s*", "", line)
    line = re.sub(r"^\s*>\s*", "", line)
    line = re.sub(r"^\s*(?:[-+*]|\d+[.)])\s+", "", line)
    line = line.replace("**", "").replace("__", "")
    line = line.replace("~~", "")
    line = re.sub(r"(?<!\\)[|]", " ", line)
    return normalize_space(line)


def enumerate_non_code_lines(raw: str) -> Iterable[tuple[int, str]]:
    fence: str | None = None

    for line_number, line in enumerate(raw.splitlines(), start=1):
        match = FENCE_START_PATTERN.match(line)
        if fence is None and match:
            fence = match.group("fence")
            continue

        if fence is not None:
            if re.match(rf"^\s*{re.escape(fence[0])}{{{len(fence)},}}\s*$", line):
                fence = None
            continue

        yield line_number, line


def extract_visible_lines(raw: str) -> list[TextLine]:
    lines: list[TextLine] = []
    for line_number, line in enumerate_non_code_lines(raw):
        text = strip_visible_markdown(line)
        if text:
            lines.append(TextLine(line_number, text))
    return lines


def extract_paragraphs(raw: str) -> list[str]:
    retained: list[str] = []
    fence: str | None = None

    for line in raw.splitlines():
        match = FENCE_START_PATTERN.match(line)
        if fence is None and match:
            fence = match.group("fence")
            retained.append("")
            continue

        if fence is not None:
            if re.match(rf"^\s*{re.escape(fence[0])}{{{len(fence)},}}\s*$", line):
                fence = None
                retained.append("")
            continue

        if HEADING_PATTERN.match(line) or re.match(r"^\s*!\[", line):
            retained.append("")
            continue

        retained.append(line)

    paragraphs: list[str] = []
    for block in re.split(r"\n\s*\n", "\n".join(retained)):
        text = strip_visible_markdown(block)
        if len(text) >= 20:
            paragraphs.append(text)
    return paragraphs


def extract_code_blocks(raw: str) -> list[CodeBlock]:
    lines = raw.splitlines()
    result: list[CodeBlock] = []
    index = 0

    while index < len(lines):
        match = FENCE_START_PATTERN.match(lines[index])
        if not match:
            index += 1
            continue

        fence = match.group("fence")
        info = match.group("info").strip()
        language = info.split()[0].casefold() if info else "none"
        start_line = index + 1
        body: list[str] = []
        index += 1

        while index < len(lines):
            if re.match(
                rf"^\s*{re.escape(fence[0])}{{{len(fence)},}}\s*$", lines[index]
            ):
                break
            body.append(lines[index])
            index += 1

        result.append(CodeBlock(start_line, language, "\n".join(body)))
        index += 1

    return result


def extract_table_shapes(raw: str) -> tuple[tuple[int, int], ...]:
    lines = list(enumerate_non_code_lines(raw))
    shapes: list[tuple[int, int]] = []
    index = 0

    while index + 1 < len(lines):
        _, first = lines[index]
        _, separator = lines[index + 1]
        if "|" not in first or not re.match(
            r"^\s*\|?\s*:?-{3,}:?\s*(?:\|\s*:?-{3,}:?\s*)+\|?\s*$",
            separator,
        ):
            index += 1
            continue

        columns = max(1, len(split_table_row(first)))
        rows = 0
        index += 2
        while index < len(lines) and "|" in lines[index][1] and lines[index][1].strip():
            rows += 1
            index += 1
        shapes.append((columns, rows))

    return tuple(shapes)


def split_table_row(line: str) -> list[str]:
    value = line.strip()
    if value.startswith("|"):
        value = value[1:]
    if value.endswith("|"):
        value = value[:-1]
    return re.split(r"(?<!\\)\|", value)


def extract_inline_technical(raw: str) -> Counter[str]:
    values: list[str] = []
    for _, line in enumerate_non_code_lines(raw):
        line = replace_markdown_links(line)
        for match in INLINE_CODE_PATTERN.finditer(line):
            value = normalize_space(match.group(1))
            if is_technical_inline(value):
                values.append(value)
    return Counter(values)


def is_technical_inline(value: str) -> bool:
    if not value or len(value) > 160:
        return False
    if re.search(r"\s", value):
        return bool(
            re.match(
                r"^(?:dotnet|git|gh|nuget|Install-Package|Get-|Set-|New-|Remove-|\$|--)",
                value,
                re.IGNORECASE,
            )
        )
    if value.startswith("-") or "_" in value or re.search(r"[<>{}\[\]()./\\:=]", value):
        return True
    if re.fullmatch(r"[A-Za-z][A-Za-z0-9+#-]*", value):
        return True
    return False


def extract_numbers(visible_lines: Sequence[TextLine]) -> Counter[str]:
    values: list[str] = []
    for line in visible_lines:
        text = re.sub(r"^\s*\d+[.)]\s+", "", line.text)
        for value in NUMBER_PATTERN.findall(text):
            # Single-digit values are frequently written out in translations
            # and list markers are not always represented identically. Longer
            # values are much stronger signals for versions, limits and dates.
            if len(value) < 2:
                continue
            values.append(str(int(value)))
    return Counter(values)


def read_document(path: Path, relative: str) -> Document:
    raw = path.read_text(encoding="utf-8-sig")
    visible_lines = extract_visible_lines(raw)
    code_blocks = extract_code_blocks(raw)
    links: list[str] = []
    images: list[str] = []

    for _, line in enumerate_non_code_lines(raw):
        for image, _, raw_target, _, _ in enumerate_markdown_links(line):
            target = normalize_url(raw_target)
            if image:
                images.append(target)
            else:
                links.append(target)

    heading_levels: list[int] = []
    ordered_items = 0
    unordered_items = 0
    admonitions: list[str] = []

    for _, line in enumerate_non_code_lines(raw):
        heading = HEADING_PATTERN.match(line)
        if heading:
            heading_levels.append(len(heading.group(1)))
        if ORDERED_LIST_PATTERN.match(line):
            ordered_items += 1
        if UNORDERED_LIST_PATTERN.match(line):
            unordered_items += 1
        admonition = ADMONITION_PATTERN.match(line)
        if admonition:
            admonitions.append(admonition.group(1).upper())

    visible_text = normalize_space(" ".join(line.text for line in visible_lines))

    return Document(
        relative=relative,
        path=path,
        raw=raw,
        visible_lines=visible_lines,
        visible_text=visible_text,
        paragraphs=extract_paragraphs(raw),
        headings=tuple(heading_levels),
        code_blocks=code_blocks,
        link_targets=tuple(sorted(links)),
        image_targets=tuple(images),
        table_shapes=extract_table_shapes(raw),
        ordered_items=ordered_items,
        unordered_items=unordered_items,
        admonitions=tuple(admonitions),
        inline_technical=extract_inline_technical(raw),
        numbers=extract_numbers(visible_lines),
    )


def code_skeleton(block: CodeBlock) -> str:
    text = block.body
    language = block.language

    if language in {"xml", "html"}:
        text = re.sub(r"<!--.*?-->", "", text, flags=re.DOTALL)
    else:
        text = re.sub(r"/\*.*?\*/", "", text, flags=re.DOTALL)

    # Replace strings before stripping line comments so URLs in literals do not
    # look like comments.
    text = re.sub(r'@?\\?"(?:\\.|[^"\\])*"', '""', text)
    text = re.sub(r"'(?:\\.|[^'\\])*'", "''", text)

    if language in {"py", "python", "shell", "bash", "powershell", "ps1"}:
        text = re.sub(r"(?m)#.*$", "", text)
    elif language not in {"xml", "html", "css"}:
        text = re.sub(r"(?m)//.*$", "", text)

    return re.sub(r"\s+", "", text)


def structural_differences(source: Document, target: Document) -> list[str]:
    differences: list[str] = []
    checks = (
        ("уровни заголовков", source.headings, target.headings),
        (
            "языки блоков кода",
            tuple(block.language for block in source.code_blocks),
            tuple(block.language for block in target.code_blocks),
        ),
        ("адреса ссылок", source.link_targets, target.link_targets),
        ("адреса изображений", source.image_targets, target.image_targets),
        ("формы таблиц", source.table_shapes, target.table_shapes),
        (
            "число пунктов списков",
            (source.ordered_items, source.unordered_items),
            (target.ordered_items, target.unordered_items),
        ),
        ("типы admonition-блоков", source.admonitions, target.admonitions),
    )
    for name, expected, actual in checks:
        if expected != actual:
            differences.append(name)
    return differences


def translatable_english_line(text: str) -> bool:
    words = [word.casefold() for word in WORD_PATTERN.findall(text)]
    if len(words) < 4 or sum(len(word) for word in words) < 18:
        return False
    stop_count = sum(word in ENGLISH_STOP_WORDS for word in words)
    strong_count = sum(word in STRONG_ENGLISH_WORDS for word in words)
    return stop_count >= 3 and strong_count >= 1


def likely_english_line(text: str, language: str) -> bool:
    words = [word.casefold() for word in WORD_PATTERN.findall(text)]
    if len(words) < 5:
        return False

    stop_count = sum(word in ENGLISH_STOP_WORDS for word in words)
    strong_count = sum(word in STRONG_ENGLISH_WORDS for word in words)
    if language == "ru":
        if len(re.findall(r"[А-Яа-яЁё]", text)) >= 3:
            return False
        return stop_count >= 2 and strong_count >= 1
    if language in {"ja", "zh"}:
        if len(re.findall(r"[\u3040-\u30ff\u3400-\u9fff]", text)) >= 3:
            return False
        return stop_count >= 2 and strong_count >= 1
    return (
        stop_count >= 4
        and strong_count >= 2
        and stop_count / len(words) >= 0.35
    )


def has_multiline_inline_code(raw: str) -> bool:
    # A few legacy pages use code spans that continue on the next line. The
    # per-line lightweight parser deliberately avoids judging numbers and
    # inline identifiers in those pages because it cannot assign them reliably.
    for _, line in enumerate_non_code_lines(raw):
        cleaned = replace_markdown_links(line)
        runs = re.findall(r"(?<!\\)(`+)", cleaned)
        if sum(len(run) for run in runs) % 2:
            return True
    return False


def robust_ratio_stats(
    source_documents: dict[str, Document], target_documents: dict[str, Document]
) -> RatioStats:
    ratios: list[float] = []
    for relative, source in source_documents.items():
        target = target_documents.get(relative)
        if target is None or len(source.visible_text) < 200 or not target.visible_text:
            continue
        ratios.append(math.log(len(target.visible_text) / len(source.visible_text)))

    if not ratios:
        return RatioStats(0.0, 1.0)

    median = statistics.median(ratios)
    deviations = [abs(value - median) for value in ratios]
    mad = statistics.median(deviations) or 0.05
    return RatioStats(median, mad)


def format_counter_delta(source: Counter[str], target: Counter[str]) -> str:
    missing = list((source - target).elements())
    extra = list((target - source).elements())
    parts: list[str] = []
    if missing:
        parts.append("нет в переводе: " + ", ".join(f"`{v}`" for v in missing[:8]))
    if extra:
        parts.append(
            "только в переводе: " + ", ".join(f"`{v}`" for v in extra[:8])
        )
    return "; ".join(parts)


def first_line_for_pattern(document: Document, pattern: re.Pattern[str]) -> int:
    for line in document.visible_lines:
        if pattern.search(line.text):
            return line.line
    return 1


def analyze_document(
    language: str,
    source: Document,
    target: Document,
    ratio_stats: RatioStats,
) -> list[Finding]:
    findings: list[Finding] = []
    location = PurePosixPath(target.relative).name

    structure = structural_differences(source, target)
    if structure:
        findings.append(
            Finding(
                "P1",
                f"{location}:1",
                "Структура",
                "Не совпадают с английской версией: " + ", ".join(structure) + ".",
                "Сверить Markdown-разметку с английским файлом, не меняя локализованный текст.",
            )
        )

    source_visible = {
        normalize_comparable_text(line.text)
        for line in source.visible_lines
        if translatable_english_line(line.text)
    }
    for line in target.visible_lines:
        normalized = normalize_comparable_text(line.text)
        if normalized and normalized in source_visible:
            findings.append(
                Finding(
                    "P1",
                    f"{location}:{line.line}",
                    "Непереведённый текст",
                    f"Видимая фраза совпадает с английским источником: «{truncate(line.text)}».",
                    "Перевести фразу либо явно зафиксировать её как неизменяемое название интерфейса.",
                )
            )
        elif likely_english_line(line.text, language):
            findings.append(
                Finding(
                    "P1",
                    f"{location}:{line.line}",
                    "Английский остаток",
                    f"Фраза выглядит преимущественно английской: «{truncate(line.text)}».",
                    "Проверить, является ли это реальным названием UI/API; иначе локализовать.",
                    "средняя",
                )
            )

    for line in target.visible_lines:
        artifact = TRANSLATION_ARTIFACT_PATTERN.search(line.text)
        if artifact:
            findings.append(
                Finding(
                    "P1",
                    f"{location}:{line.line}",
                    "Артефакт перевода",
                    f"Обнаружен служебный маркер `{artifact.group(0)}`.",
                    "Удалить маркер и восстановить предполагаемый видимый текст по английскому источнику.",
                )
            )

        if any(marker in line.text for marker in MOJIBAKE_MARKERS) or re.search(
            r"\?{3,}", line.text
        ):
            findings.append(
                Finding(
                    "P1",
                    f"{location}:{line.line}",
                    "Кодировка",
                    f"Обнаружен вероятный артефакт кодировки: «{truncate(line.text)}».",
                    "Восстановить исходный Unicode-текст.",
                )
            )

        foreign = (
            CYRILLIC_PATTERN.search(line.text)
            if language != "ru"
            else CJK_PATTERN.search(line.text)
        )
        if foreign:
            findings.append(
                Finding(
                    "P2",
                    f"{location}:{line.line}",
                    "Смешение языков",
                    f"Обнаружен фрагмент чужой письменности: `{foreign.group(0)}`.",
                    "Проверить контекст; переводить только пользовательский текст, сохраняя API-идентификаторы.",
                    "средняя",
                )
            )

    multiline_inline_code = has_multiline_inline_code(
        source.raw
    ) or has_multiline_inline_code(target.raw)

    if not multiline_inline_code and source.numbers != target.numbers:
        findings.append(
            Finding(
                "P2",
                f"{location}:1",
                "Числа и параметры",
                "Набор числовых значений расходится: "
                + format_counter_delta(source.numbers, target.numbers)
                + ".",
                "Проверить версии, интервалы, лимиты и номера шагов по английскому источнику.",
                "средняя",
            )
        )

    if (
        not multiline_inline_code
        and source.inline_technical != target.inline_technical
    ):
        findings.append(
            Finding(
                "P2",
                f"{location}:1",
                "Технические идентификаторы",
                "Расходятся технические фрагменты в inline code: "
                + format_counter_delta(source.inline_technical, target.inline_technical)
                + ".",
                "Сверить API-имена, CLI-флаги, имена файлов и литералы; видимые UI-подписи оценить вручную.",
                "средняя",
            )
        )

    source_code = [
        block
        for block in source.code_blocks
        if block.language in CODE_LANGUAGES and code_skeleton(block)
    ]
    target_code = [
        block
        for block in target.code_blocks
        if block.language in CODE_LANGUAGES and code_skeleton(block)
    ]
    if len(source_code) == len(target_code):
        changed_blocks = [
            index + 1
            for index, (source_block, target_block) in enumerate(
                zip(source_code, target_code)
            )
            if code_skeleton(source_block) != code_skeleton(target_block)
        ]
        if changed_blocks:
            changed_languages = {
                target_code[index - 1].language for index in changed_blocks
            }
            strict_change = bool(
                changed_languages & STRICT_EXECUTABLE_CODE_LANGUAGES
            )
            findings.append(
                Finding(
                    "P1" if strict_change else "P2",
                    f"{location}:{target_code[changed_blocks[0] - 1].line}",
                    "Кодовый пример",
                    "После удаления комментариев и локализуемых строк отличается каркас "
                    f"блоков кода: {', '.join(map(str, changed_blocks[:10]))}.",
                    "Сверить исполняемый код с английским примером; переводить только комментарии, пользовательские строки и явно условные placeholders.",
                    "высокая" if strict_change else "средняя",
                )
            )

    if len(source.visible_text) >= 200 and target.visible_text:
        log_ratio = math.log(len(target.visible_text) / len(source.visible_text))
        robust_z = (
            0.6745 * (log_ratio - ratio_stats.median_log_ratio) / ratio_stats.mad_log_ratio
        )
        relative_to_typical = math.exp(log_ratio - ratio_stats.median_log_ratio)
        if abs(robust_z) >= 5.0 and (
            relative_to_typical <= 0.78 or relative_to_typical >= 1.28
        ):
            direction = "короче" if relative_to_typical < 1 else "длиннее"
            findings.append(
                Finding(
                    "P2",
                    f"{location}:1",
                    "Полнота",
                    "Объём видимого текста нетипичен для локали: перевод "
                    f"{direction} ожидаемого примерно на {abs(1 - relative_to_typical):.0%}.",
                    "Построчно проверить пропуски, добавления и смысловое дублирование; сам объём не считать доказательством ошибки.",
                    "средняя",
                )
            )

    source_paragraphs = len(source.paragraphs)
    target_paragraphs = len(target.paragraphs)
    paragraph_delta = abs(source_paragraphs - target_paragraphs)
    if (
        source_paragraphs >= 4
        and paragraph_delta >= 3
        and paragraph_delta / source_paragraphs >= 0.25
    ):
        findings.append(
            Finding(
                "P2",
                f"{location}:1",
                "Полнота",
                f"Число смысловых Markdown-блоков различается: en={source_paragraphs}, "
                f"{language}={target_paragraphs}.",
                "Проверить, не объединены ли несвязанные инструкции и не потеряны ли абзацы.",
                "средняя",
            )
        )

    return deduplicate_findings(findings)


def deduplicate_findings(findings: Sequence[Finding]) -> list[Finding]:
    seen: set[tuple[str, str, str]] = set()
    result: list[Finding] = []
    for finding in findings:
        key = (finding.location, finding.category, finding.observation)
        if key in seen:
            continue
        seen.add(key)
        result.append(finding)
    return result


def occurrences(
    documents: Sequence[Document], pattern: re.Pattern[str]
) -> list[tuple[Document, TextLine, str]]:
    result: list[tuple[Document, TextLine, str]] = []
    for document in documents:
        for line in document.visible_lines:
            match = pattern.search(line.text)
            if match:
                result.append((document, line, match.group(0)))
    return result


def examples_for_occurrences(
    values: Sequence[tuple[Document, TextLine, str]], limit: int = 5
) -> str:
    return ", ".join(
        f"`{PurePosixPath(document.relative).name}:{line.line}` ({value})"
        for document, line, value in values[:limit]
    )


def language_specific_findings(
    language: str, documents: Sequence[Document]
) -> list[Finding]:
    findings: list[Finding] = []
    if not documents:
        return findings

    folder_location = PurePosixPath(documents[0].relative).parent.as_posix()
    if folder_location == ".":
        folder_location = language

    if language == "pt":
        brazilian_patterns = {
            "você": re.compile(r"\bvoc[eê]\b", re.IGNORECASE),
            "arquivo": re.compile(r"\barquivos?\b", re.IGNORECASE),
            "senha": re.compile(r"\bsenhas?\b", re.IGNORECASE),
            "usuário": re.compile(r"\busu[aá]rios?\b", re.IGNORECASE),
            "baixar": re.compile(
                r"\b(?:baixar|baixe|baixado|baixada|baixando)\b", re.IGNORECASE
            ),
            "tela": re.compile(r"\btelas?\b", re.IGNORECASE),
            "salvar": re.compile(
                r"\b(?:salvar|salve|salvo|salva)\b", re.IGNORECASE
            ),
        }
        mixed: list[tuple[Document, TextLine, str]] = []
        for pattern in brazilian_patterns.values():
            mixed.extend(occurrences(documents, pattern))
        mixed.sort(key=lambda item: (item[0].relative, item[1].line))
        if mixed:
            first_document, first_line, _ = mixed[0]
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(first_document.relative).name}:{first_line.line}",
                    "Вариант языка",
                    "В корпусе, ориентированном преимущественно на pt-PT, встречаются "
                    f"формы, характерные для pt-BR ({len(mixed)}): "
                    + examples_for_occurrences(mixed)
                    + ".",
                    "Выбрать целевую локаль и унифицировать лексику (например, ficheiro, palavra-passe, utilizador).",
                    "высокая",
                )
            )

        agreement = occurrences(
            documents, re.compile(r"\bvelas\s+hist[oó]ricos\b", re.IGNORECASE)
        )
        if agreement:
            document, line, value = agreement[0]
            findings.append(
                Finding(
                    "P1",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Грамматика",
                    f"Нарушено согласование рода: «{value}».",
                    "Исправить на «velas históricas».",
                    "высокая",
                )
            )

    elif language == "es":
        formal = occurrences(
            documents,
            re.compile(
                r"\b(?:Seleccione|Haga|Introduzca|Arrastre|Utilice|Pulse|Abra|Elija)\b"
            ),
        )
        informal = occurrences(
            documents,
            re.compile(
                r"\b(?:haz|puedes|debes|tienes\s+que|tú|tu)\b",
                re.IGNORECASE,
            ),
        )
        if formal and informal:
            document, line, _ = min(
                formal + informal, key=lambda item: (item[0].relative, item[1].line)
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Стиль обращения",
                    "В одной папке смешаны формальное и неформальное обращение: "
                    f"formal — {examples_for_occurrences(formal, 3)}; "
                    f"informal — {examples_for_occurrences(informal, 3)}.",
                    "Зафиксировать единый стиль документации (usted либо tú) и унифицировать повелительные формы.",
                    "средняя",
                )
            )

    elif language == "ja":
        long_connector = occurrences(documents, re.compile(r"コネクター"))
        short_connector = occurrences(documents, re.compile(r"コネクタ(?!ー)"))
        if long_connector and short_connector:
            document, line, _ = min(
                long_connector + short_connector,
                key=lambda item: (item[0].relative, item[1].line),
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Терминология",
                    "Смешаны варианты `コネクター` и `コネクタ`: "
                    f"{examples_for_occurrences(long_connector, 2)}; "
                    f"{examples_for_occurrences(short_connector, 2)}.",
                    "Выбрать одну форму в японском глоссарии и применять её последовательно.",
                    "высокая",
                )
            )

        native_strategy = occurrences(documents, re.compile(r"戦略"))
        loan_strategy = occurrences(documents, re.compile(r"ストラテジー"))
        if native_strategy and loan_strategy:
            document, line, _ = min(
                native_strategy + loan_strategy,
                key=lambda item: (item[0].relative, item[1].line),
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Терминология",
                    "Для strategy в одной папке используются и `戦略`, и `ストラテジー`: "
                    f"{examples_for_occurrences(native_strategy, 2)}; "
                    f"{examples_for_occurrences(loan_strategy, 2)}.",
                    "Зафиксировать контекстное правило либо один основной термин.",
                    "средняя",
                )
            )

    elif language == "zh":
        ascii_period = occurrences(
            documents, re.compile(r"[\u3400-\u9fff][.](?:\s|$)")
        )
        if ascii_period:
            document, line, _ = ascii_period[0]
            findings.append(
                Finding(
                    "P3",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Типографика",
                    f"После китайского текста используется ASCII-точка ({len(ascii_period)}): "
                    + examples_for_occurrences(ascii_period)
                    + ".",
                    "Заменить знаки конца китайских предложений на `。`, не затрагивая код и URL.",
                    "высокая",
                )
            )

        market_depth = occurrences(documents, re.compile(r"市场深度"))
        order_book = occurrences(documents, re.compile(r"订单簿"))
        if market_depth and order_book:
            document, line, _ = min(
                market_depth + order_book,
                key=lambda item: (item[0].relative, item[1].line),
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Терминология",
                    "Для order book / market depth встречаются разные термины: "
                    f"`市场深度` — {examples_for_occurrences(market_depth, 2)}; "
                    f"`订单簿` — {examples_for_occurrences(order_book, 2)}.",
                    "Уточнить продуктовый глоссарий и разграничить понятия либо унифицировать перевод.",
                    "средняя",
                )
            )

    elif language == "de":
        localized_connectors = occurrences(
            documents, re.compile(r"\bKonnektoren?\b", re.IGNORECASE)
        )
        english_connectors = occurrences(
            documents, re.compile(r"\bConnectors?\b", re.IGNORECASE)
        )
        if localized_connectors and english_connectors:
            document, line, _ = min(
                localized_connectors + english_connectors,
                key=lambda item: (item[0].relative, item[1].line),
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Терминология",
                    "Смешаны немецкая и английская формы `Konnektor` / `Connector`: "
                    f"{examples_for_occurrences(localized_connectors, 2)}; "
                    f"{examples_for_occurrences(english_connectors, 2)}.",
                    "Зафиксировать одну форму для пользовательского текста; API и имена продуктов не менять.",
                    "высокая",
                )
            )

        auftrag = occurrences(documents, re.compile(r"\bAuftr(?:ag|äge|ags|ägen)\b"))
        order = occurrences(documents, re.compile(r"\bOrders?\b", re.IGNORECASE))
        if auftrag and order:
            document, line, _ = min(
                auftrag + order, key=lambda item: (item[0].relative, item[1].line)
            )
            findings.append(
                Finding(
                    "P2",
                    f"{PurePosixPath(document.relative).name}:{line.line}",
                    "Терминология",
                    "В одной папке используются `Auftrag` и `Order`: "
                    f"{examples_for_occurrences(auftrag, 2)}; "
                    f"{examples_for_occurrences(order, 2)}.",
                    "Проверить контекст и унифицировать термин торговой заявки, не смешивая его с задачей/поручением.",
                    "средняя",
                )
            )

    return findings


def truncate(value: str, limit: int = 150) -> str:
    value = normalize_space(value)
    if len(value) <= limit:
        return value
    return value[: limit - 1].rstrip() + "…"


def escape_table(value: str) -> str:
    return (
        value.replace("\\", "\\\\")
        .replace("|", r"\|")
        .replace("\r", " ")
        .replace("\n", "<br>")
    )


def folder_key(relative: str) -> str:
    parent = PurePosixPath(relative).parent.as_posix()
    return "" if parent == "." else parent


def report_path(output_root: Path, language: str, folder: str) -> Path:
    base = output_root / language
    if folder:
        base = base.joinpath(*PurePosixPath(folder).parts)
    return base / "_analysis.md"


def read_existing_manual_section(path: Path) -> str | None:
    if not path.exists():
        return None

    text = path.read_text(encoding="utf-8-sig")
    match = MANUAL_SECTION_PATTERN.search(text)
    if match is None:
        return None
    return match.group("body").strip("\r\n")


def file_set_findings(
    missing: Sequence[str], extra: Sequence[str]
) -> list[Finding]:
    findings: list[Finding] = []
    if missing:
        names = ", ".join(f"`{PurePosixPath(value).name}`" for value in missing)
        findings.append(
            Finding(
                "P1",
                f"{PurePosixPath(missing[0]).name}:1",
                "Состав файлов",
                f"Отсутствуют локализованные файлы: {names}.",
                "Добавить отсутствующие страницы по английскому эталону.",
            )
        )
    if extra:
        names = ", ".join(f"`{PurePosixPath(value).name}`" for value in extra)
        findings.append(
            Finding(
                "P2",
                f"{PurePosixPath(extra[0]).name}:1",
                "Состав файлов",
                f"Обнаружены файлы без английского эталона: {names}.",
                "Проверить, являются ли страницы локальным дополнением или устаревшими файлами.",
                "средняя",
            )
        )
    return findings


def report_status(findings: Sequence[Finding]) -> str:
    priorities = {finding.priority for finding in findings}
    if "P0" in priorities or "P1" in priorities:
        return "Требует исправлений"
    if "P2" in priorities:
        return "Требует ручной проверки"
    if "P3" in priorities:
        return "Есть стилевые замечания"
    return "Сигналов не найдено"


def write_folder_report(
    output_root: Path,
    language: str,
    folder: str,
    expected_count: int,
    actual_count: int,
    missing: Sequence[str],
    extra: Sequence[str],
    findings: Sequence[Finding],
) -> None:
    path = report_path(output_root, language, folder)
    manual_section = read_existing_manual_section(path)
    if manual_section is None:
        manual_section = DEFAULT_MANUAL_SECTION
    path.parent.mkdir(parents=True, exist_ok=True)

    localized_folder = language if not folder else f"{language}/{folder}"
    source_folder = SOURCE_LANGUAGE if not folder else f"{SOURCE_LANGUAGE}/{folder}"
    priority_counts = Counter(finding.priority for finding in findings)
    structure_count = sum(finding.category == "Структура" for finding in findings)
    code_count = sum(
        finding.category in {"Кодовый пример", "Технические идентификаторы"}
        for finding in findings
    )
    residue_count = sum(
        finding.category
        in {"Непереведённый текст", "Английский остаток", "Смешение языков"}
        for finding in findings
    )
    encoding_count = sum(finding.category == "Кодировка" for finding in findings)
    completeness_count = sum(
        finding.category in {"Полнота", "Числа и параметры"} for finding in findings
    )

    lines = [
        f"# Анализ перевода: `{localized_folder}`",
        "",
        "| Параметр | Значение |",
        "| --- | --- |",
        f"| Язык | {LANGUAGE_NAMES[language]} (`{language}`) |",
        f"| Английский эталон | `{source_folder}` |",
        f"| Проверено Markdown-файлов | {actual_count} из {expected_count} |",
        f"| Отсутствуют / лишние | {len(missing)} / {len(extra)} |",
        f"| Автоматический статус | {report_status(findings)} |",
        (
            "| Автоматические сигналы | "
            f"P0: {priority_counts['P0']}, P1: {priority_counts['P1']}, "
            f"P2: {priority_counts['P2']}, P3: {priority_counts['P3']} |"
        ),
        f"| Дата анализа | {REPORT_DATE} |",
        "",
        "## Итог",
        "",
    ]

    if findings:
        lines.append(
            f"Проверены все Markdown-файлы, лежащие непосредственно в папке. "
            f"Найдено сигналов: {len(findings)}; сначала следует разобрать P0/P1, "
            "затем подтвердить контекстные замечания P2/P3."
        )
    else:
        lines.append(
            "Проверены все Markdown-файлы, лежащие непосредственно в папке. "
            "Автоматический сравнительный анализ не обнаружил расхождений или "
            "языковых сигналов; дочерние папки имеют отдельные отчёты."
        )

    lines.extend(
        [
            "",
            "## Автоматические проверки",
            "",
            f"- состав файлов: {'совпадает' if not missing and not extra else 'есть расхождения'};",
            f"- Markdown-структура, ссылки и изображения: {structure_count} расхождений;",
            f"- кодовые примеры и технические идентификаторы: {code_count} сигналов;",
            f"- английские остатки и смешение письменностей: {residue_count} сигналов;",
            f"- артефакты кодировки: {encoding_count} сигналов;",
            f"- полнота, абзацы и числовые значения: {completeness_count} сигналов.",
            "",
            "## Замечания",
            "",
        ]
    )

    if findings:
        lines.extend(
            [
                "| Приоритет | Файл:строка | Категория | Что обнаружено | Рекомендация | Уверенность |",
                "| --- | --- | --- | --- | --- | --- |",
            ]
        )
        for finding in findings:
            lines.append(
                "| "
                + " | ".join(
                    escape_table(value)
                    for value in (
                        finding.priority,
                        finding.location,
                        finding.category,
                        finding.observation,
                        finding.recommendation,
                        finding.confidence,
                    )
                )
                + " |"
            )
    elif not missing and not extra:
        lines.append("Замечаний по применённым проверкам нет.")

    lines.extend(
        [
            "",
            "## Ручная языковая проверка",
            "",
        ]
    )
    lines.extend(manual_section.splitlines())
    lines.append("")

    path.write_text("\n".join(lines), encoding="utf-8", newline="\n")


def collect_paths(root: Path, language: str) -> dict[str, Path]:
    language_root = root / language
    result: dict[str, Path] = {}
    for path in language_root.rglob("*.md"):
        relative = path.relative_to(language_root).as_posix()
        if not is_connector_path(relative):
            result[relative] = path
    return result


def load_documents(paths: dict[str, Path]) -> dict[str, Document]:
    return {
        relative: read_document(path, relative)
        for relative, path in sorted(paths.items())
    }


def generate_language(root: Path, output_root: Path, language: str) -> dict[str, int]:
    source_paths = collect_paths(root, SOURCE_LANGUAGE)
    target_paths = collect_paths(root, language)
    source_documents = load_documents(source_paths)
    target_documents = load_documents(target_paths)
    ratio_stats = robust_ratio_stats(source_documents, target_documents)

    folders = sorted(
        {folder_key(relative) for relative in source_paths}
        | {folder_key(relative) for relative in target_paths}
    )

    all_findings: list[Finding] = []
    status_counts: Counter[str] = Counter()

    for folder in folders:
        expected = sorted(
            relative
            for relative in source_paths
            if folder_key(relative) == folder
        )
        actual = sorted(
            relative
            for relative in target_paths
            if folder_key(relative) == folder
        )
        missing = sorted(set(expected) - set(actual))
        extra = sorted(set(actual) - set(expected))
        findings = file_set_findings(missing, extra)

        for relative in sorted(set(expected) & set(actual)):
            findings.extend(
                analyze_document(
                    language,
                    source_documents[relative],
                    target_documents[relative],
                    ratio_stats,
                )
            )

        direct_documents = [
            target_documents[relative]
            for relative in actual
            if relative in target_documents
        ]
        findings.extend(language_specific_findings(language, direct_documents))
        findings = sorted(
            deduplicate_findings(findings),
            key=lambda finding: (
                int(finding.priority[1]),
                finding.location.casefold(),
                finding.category.casefold(),
            ),
        )

        write_folder_report(
            output_root,
            language,
            folder,
            len(expected),
            len(actual),
            missing,
            extra,
            findings,
        )
        all_findings.extend(findings)
        status_counts[report_status(findings)] += 1

    return {
        "folders": len(folders),
        "source_files": len(source_paths),
        "target_files": len(target_paths),
        "findings": len(all_findings),
        "p0": sum(finding.priority == "P0" for finding in all_findings),
        "p1": sum(finding.priority == "P1" for finding in all_findings),
        "p2": sum(finding.priority == "P2" for finding in all_findings),
        "p3": sum(finding.priority == "P3" for finding in all_findings),
        "clean_folders": status_counts["Сигналов не найдено"],
    }


def expected_folder_keys(root: Path, language: str) -> list[str]:
    source_paths = collect_paths(root, SOURCE_LANGUAGE)
    target_paths = collect_paths(root, language)
    return sorted(
        {folder_key(relative) for relative in source_paths}
        | {folder_key(relative) for relative in target_paths}
    )


def existing_report_paths(
    root: Path, output_root: Path, language: str
) -> list[Path]:
    return [
        path
        for folder in expected_folder_keys(root, language)
        if (path := report_path(output_root, language, folder)).exists()
    ]


def existing_manual_stats(
    root: Path, output_root: Path
) -> dict[str, dict[str, int]]:
    result: dict[str, dict[str, int]] = {}
    for language in LANGUAGE_ORDER:
        reports = existing_report_paths(root, output_root, language)
        if not reports:
            continue

        priorities: Counter[str] = Counter()
        reports_with_findings = 0
        for report in reports:
            text = report.read_text(encoding="utf-8-sig")
            manual_section_match = MANUAL_SECTION_PATTERN.search(text)
            if manual_section_match is None:
                continue
            matches = list(
                MANUAL_FINDING_PATTERN.finditer(
                    manual_section_match.group("body")
                )
            )
            if matches:
                reports_with_findings += 1
            priorities.update(match.group("priority") for match in matches)

        result[language] = {
            "reports": reports_with_findings,
            "p0": priorities["P0"],
            "p1": priorities["P1"],
            "p2": priorities["P2"],
            "p3": priorities["P3"],
            "total": sum(priorities.values()),
        }
    return result


def write_readme(
    root: Path,
    output_root: Path,
    generated: dict[str, dict[str, int]],
) -> None:
    output_root.mkdir(parents=True, exist_ok=True)
    lines = [
        "# Анализ качества переводов",
        "",
        f"Срез документации на {REPORT_DATE}. Английская версия (`en`) используется "
        "как канонический эталон структуры и текущего содержания. Русская версия "
        "исторически развивалась отдельно, поэтому её расхождения отмечаются как "
        "кандидаты на синхронизацию, а не автоматически как ошибки перевода.",
        "",
        "## Область анализа",
        "",
        "- языки: `de`, `es`, `ja`, `pt`, `ru`, `zh`;",
        "- один `_analysis.md` на каждую папку, в которой непосредственно лежат Markdown-файлы;",
        "- дочерние папки оцениваются отдельными отчётами;",
        "- полностью исключены `topics/api/connectors.md`, `topics/api/connectors/**` "
        "и однотипные списки `topics/designer/connections_settings/connectors_settings.md` "
        "и `topics/hydra/data_sources.md`;",
        "- служебные `toc.yml`, `strings.json`, `language.json` и изображения в этот срез не входят.",
        "",
        "## Сводка",
        "",
        "| Язык | Папки | Страницы | Авто P0 | Авто P1 | Авто P2 | Авто P3 | Папки без автосигналов |",
        "| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |",
    ]
    for language in LANGUAGE_ORDER:
        stats = generated.get(language)
        if stats is None:
            lines.append(
                f"| {language} | — | — | — | — | — | — | ещё не анализировался |"
            )
        else:
            lines.append(
                f"| {language} | {stats['folders']} | {stats['target_files']} | "
                f"{stats['p0']} | {stats['p1']} | {stats['p2']} | {stats['p3']} | "
                f"{stats['clean_folders']} |"
            )

    manual = existing_manual_stats(root, output_root)
    manual_totals: Counter[str] = Counter()
    lines.extend(
        [
            "",
            "## Результат ручной сверки",
            "",
            "| Язык | Отчёты с находками | P0 | P1 | P2 | P3 | Всего групп |",
            "| --- | ---: | ---: | ---: | ---: | ---: | ---: |",
        ]
    )
    for language in LANGUAGE_ORDER:
        stats = manual.get(
            language,
            {
                "reports": 0,
                "p0": 0,
                "p1": 0,
                "p2": 0,
                "p3": 0,
                "total": 0,
            },
        )
        lines.append(
            f"| {language} | {stats['reports']} | {stats['p0']} | "
            f"{stats['p1']} | {stats['p2']} | {stats['p3']} | "
            f"{stats['total']} |"
        )
        manual_totals.update(stats)

    lines.extend(
        [
            (
                f"| **Итого** | **{manual_totals['reports']}** | "
                f"**{manual_totals['p0']}** | **{manual_totals['p1']}** | "
                f"**{manual_totals['p2']}** | **{manual_totals['p3']}** | "
                f"**{manual_totals['total']}** |"
            ),
            "",
            "Одна ручная группа может объединять один системный дефект в нескольких "
            "строках или страницах. Автоматические и ручные числа не складываются: "
            "верхняя таблица содержит кандидаты, а эта — подтверждённые при чтении "
            "наблюдения.",
            "",
            "Основные паттерны:",
            "",
        ]
    )
    for language in LANGUAGE_ORDER:
        summary = MANUAL_PATTERN_SUMMARIES[language]
        if language == "ru":
            auto_stats = generated.get(language, {})
            auto_findings = sum(
                int(auto_stats.get(priority, 0))
                for priority in ("p0", "p1", "p2", "p3")
            )
            manual_findings = manual.get(language, {}).get("total", 0)
            summary = summary.format(
                auto_findings=auto_findings,
                manual_findings=manual_findings,
            )
        punctuation = "." if language == LANGUAGE_ORDER[-1] else ";"
        lines.append(f"- `{language}`: {summary}{punctuation}")

    lines.extend(
        [
            "",
            "## Что проверяется",
            "",
            "- наличие файлов и соответствие английскому набору;",
            "- уровни заголовков, code fences, ссылки, изображения, таблицы, списки и admonition-блоки;",
            "- сохранность исполняемого каркаса примеров, API-идентификаторов, CLI-флагов и чисел;",
            "- видимые английские остатки, чужая письменность и повреждённая кодировка;",
            "- сильные выбросы объёма и числа смысловых блоков как сигналы пропусков или добавлений;",
            "- отдельные языковые маркеры: терминология, регистр обращения, вариант португальского и CJK-типографика.",
            "",
            "## Приоритеты",
            "",
            "- `P0` — искажение действия, условия, числа или технического смысла, опасное для использования продукта;",
            "- `P1` — пропуск, существенная смысловая/структурная ошибка, неверный идентификатор или непереведённый фрагмент;",
            "- `P2` — терминология, вариант языка, неестественная или неоднозначная формулировка;",
            "- `P3` — пунктуация, регистр, типографика и мелкая стилистика.",
            "",
            "## Ограничения",
            "",
            "Автоматические сигналы не являются готовыми исправлениями. Выброс длины, "
            "различие абзацев, заимствованный термин или английская UI-подпись требуют "
            "проверки по контексту и реальному интерфейсу. Подтверждённые при чтении "
            "наблюдения добавляются в раздел «Ручная языковая проверка» соответствующего отчёта.",
            "",
            "Отчёты находятся вне двухбуквенных каталогов документации и не публикуются "
            "через TOC. Генератор: `tools/generate_translation_analysis.py`.",
            "",
        ]
    )
    (output_root / "README.md").write_text(
        "\n".join(lines), encoding="utf-8", newline="\n"
    )


def existing_language_stats(
    root: Path, output_root: Path
) -> dict[str, dict[str, int]]:
    result: dict[str, dict[str, int]] = {}
    for language in LANGUAGE_ORDER:
        reports = existing_report_paths(root, output_root, language)
        if not reports:
            continue

        stats = {
            "folders": len(reports),
            "source_files": 0,
            "target_files": 0,
            "findings": 0,
            "p0": 0,
            "p1": 0,
            "p2": 0,
            "p3": 0,
            "clean_folders": 0,
        }
        for report in reports:
            text = report.read_text(encoding="utf-8-sig")
            file_count = re.search(
                r"\| Проверено Markdown-файлов \| (?P<actual>\d+) из (?P<expected>\d+) \|",
                text,
            )
            if file_count:
                stats["target_files"] += int(file_count.group("actual"))
                stats["source_files"] += int(file_count.group("expected"))

            priorities = re.search(
                r"\| Автоматические сигналы \| P0: (?P<p0>\d+), P1: (?P<p1>\d+), "
                r"P2: (?P<p2>\d+), P3: (?P<p3>\d+) \|",
                text,
            )
            if priorities:
                for priority in ("p0", "p1", "p2", "p3"):
                    stats[priority] += int(priorities.group(priority))

            if "| Автоматический статус | Сигналов не найдено |" in text:
                stats["clean_folders"] += 1

        stats["findings"] = (
            stats["p0"] + stats["p1"] + stats["p2"] + stats["p3"]
        )
        result[language] = stats
    return result


def main() -> int:
    args = parse_args()
    root = args.repo_root.resolve()
    output_root = root / "translation-analysis"
    generated = existing_language_stats(root, output_root)

    for language in args.language:
        generated[language] = generate_language(root, output_root, language)
        stats = generated[language]
        print(
            f"{language}: folders={stats['folders']} files={stats['target_files']} "
            f"findings={stats['findings']} "
            f"(P0={stats['p0']}, P1={stats['p1']}, "
            f"P2={stats['p2']}, P3={stats['p3']})"
        )

    write_readme(root, output_root, generated)
    return 0


if __name__ == "__main__":
    raise SystemExit(main())
