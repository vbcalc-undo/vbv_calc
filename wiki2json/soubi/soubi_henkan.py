import os
import json
import re

def parse_wiki_table(wiki_table_string):
    lines = wiki_table_string.strip().split('\n')
    data = []
    headers = []
    
    header_start_index = -1
    for i, line in enumerate(lines):
        if line.startswith('|~レア'):
            header_start_index = i
            break
    
    if header_start_index == -1 or header_start_index + 2 >= len(lines):
        print("エラー: Wikiテーブルのヘッダーが見つからないか、不完全です。")
        return []

    header_line = lines[header_start_index].strip()
    sub_header_line = lines[header_start_index + 1].strip()

    headers_raw = re.findall(r'~([^|]+)', header_line)
    headers = [headers_raw[0], headers_raw[1]]
    headers.append('購入')
    headers.append('売却')

    sub_headers_raw = re.findall(r'~([^|]+)', sub_header_line)
    for stat in ['攻', '防', '速', '知']:
        headers.append(f"性能変化_{stat}")
    headers.extend(headers_raw[4:])
    
    data_lines = lines[header_start_index + 2:]
    for line in data_lines:
        line = line.strip()
        if not line.startswith('|'):
            continue

        values = line.split('|')[1:-1]
        if len(values) != len(headers):
            continue

        item = {}
        for i, header in enumerate(headers):
            value = values[i].strip()
            
            if value.replace('-', '').isdigit():
                value = int(value)
            elif value == '-':
                value = None
            
            if header == '能力付加':
                value = [s.strip() for s in value.split('&br;') if s.strip()]
            
            item[header] = value

        data.append(item)
        
    return data

def process_file_to_json(input_filename, output_filename):
    try:
        with open(input_filename, 'r', encoding='utf-8') as f:
            wiki_table_string = f.read()
        
        parsed_data = parse_wiki_table(wiki_table_string)
        
        if parsed_data:
            json_output = json.dumps(parsed_data, ensure_ascii=False, indent=2)
            with open(output_filename, 'w', encoding='utf-8') as f:
                f.write(json_output)
            print(f"✅ {input_filename} → {output_filename}")
        else:
            print(f"❌ 解析失敗: {input_filename}")
            
    except Exception as e:
        print(f"エラー {input_filename}: {e}")

if __name__ == "__main__":
    base_dir = "data"
    
    for root, dirs, files in os.walk(base_dir):
        for file in files:
            if file.endswith(".txt"):
                input_path = os.path.join(root, file)
                output_path = os.path.splitext(input_path)[0] + ".json"
                process_file_to_json(input_path, output_path)
