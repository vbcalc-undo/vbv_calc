import os
import json
import re

def parse_skill_cell(cell: str):
    """
    スキル欄を分割して ["スキル名:値", ...] のリストに変換する
    """
    if not cell.strip():
        return []
    skills = re.split(r"&br;|\n", cell)
    result = []
    for s in skills:
        s = s.strip()
        if not s:
            continue
        result.append(s)
    return result

def parse_equipment(cell: str):
    """
    装備欄を ["片手", "鎧"] のような配列に変換する
    """
    if not cell.strip():
        return []
    return [x.strip() for x in re.split(r"&br;|,", cell) if x.strip()]

def parse_table(md_lines):
    """
    Markdown表の行を受け取って JSON データに変換
    """
    data = []
    for line in md_lines:
        if not line.startswith("|") or line.startswith("|~") or "BGCOLOR" in line:
            continue

        cells = [c.strip() for c in line.strip().strip("|").split("|")]

        while len(cells) < 20:
            cells.append("")

        record = {
            "名称": cells[0],
            "加護": cells[1],
            "職業": cells[2],
            "基本パラメータ_HP": cells[3],
            "基本パラメータ_攻": cells[4],
            "基本パラメータ_防": cells[5],
            "基本パラメータ_速": cells[6],
            "基本パラメータ_知": cells[7],
            "種族": cells[8],
            "特攻": cells[9],
            "装備": parse_equipment(cells[10]),
            "ランク": cells[11],
            "コスト": cells[12],
            "パッシブスキル": parse_skill_cell(cells[13]) + parse_skill_cell(cells[14]),
            "リーダースキル": parse_skill_cell(cells[15]),
            "アシストスキル": parse_skill_cell(cells[16]),
            "内政スキル": cells[17].replace("&br;", ","),
            "スタンス": cells[18].replace("&br;", ""),
            "備考": cells[19].replace("&br;", " ")
        }

        data.append(record)
    return data

def process_file(input_path, output_path):
    try:
        with open(input_path, "r", encoding="utf-8") as f:
            md_lines = f.readlines()

        units = parse_table(md_lines)

        if units:
            with open(output_path, "w", encoding="utf-8") as f:
                json.dump(units, f, ensure_ascii=False, indent=2)
            print(f"✅ {input_path} → {output_path}")
        else:
            print(f"⚠️ データなし: {input_path}")

    except Exception as e:
        print(f"❌ エラー {input_path}: {e}")

if __name__ == "__main__":
    base_dir = "data"
    found = False

    for root, dirs, files in os.walk(base_dir):
        for file in files:
            if file.endswith(".txt"):
                found = True
                input_path = os.path.join(root, file)
                output_path = os.path.splitext(input_path)[0] + ".json"
                print(f"📄 処理対象: {input_path}")
                process_file(input_path, output_path)

    if not found:
        print("❌ data/ 以下に .txt ファイルが見つかりませんでした")
