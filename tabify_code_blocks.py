import sys
import re

if len(sys.argv) < 2:
    print("Usage: python tabify_code_blocks.py file1.md [file2.md ...]")
    sys.exit(1)

# Convert leading spaces to tabs
INDENT_SIZE = 4

code_fence_re = re.compile(r'^```')
leading_spaces_re = re.compile(r'^( +)')

for filename in sys.argv[1:]:
    try:
        with open(filename, 'r', encoding='utf-8') as f:
            lines = f.readlines()
    except UnicodeDecodeError:
        with open(filename, 'r', encoding='utf-8', errors='ignore') as f:
            lines = f.readlines()
    in_code = False
    new_lines = []
    for line in lines:
        if code_fence_re.match(line):
            in_code = not in_code
            new_lines.append(line)
            continue
        if in_code:
            m = leading_spaces_re.match(line)
            if m:
                spaces = len(m.group(1))
                tabs = (spaces + INDENT_SIZE - 1) // INDENT_SIZE
                new_indent = '\t' * tabs
                line = new_indent + line[spaces:]
        new_lines.append(line)
    with open(filename, 'w', encoding='utf-8') as f:
        f.writelines(new_lines)
    print(f"Processed {filename}")
