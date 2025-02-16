import re
import sys

def increase_links(content):
    # Search for links in the format (../../... and insert an extra "../"
    # Groups: 1 – opening parenthesis, 2 – sequence of "../", 3 – rest of the link
    def repl(match):
        return match.group(1) + "../" + match.group(2) + match.group(3)
    pattern = r"(\()((?:\.\.\/)+)([^)]+)"
    return re.sub(pattern, repl, content)

if __name__ == '__main__':
    if len(sys.argv) < 2:
        print("Usage: python increase_links.py file1.md [file2.md ...]")
        sys.exit(1)
    for filename in sys.argv[1:]:
        with open(filename, "r", encoding="utf-8") as f:
            content = f.read()
        new_content = increase_links(content)
        with open(filename, "w", encoding="utf-8") as f:
            f.write(new_content)
        print(f"Processed file: {filename}")
