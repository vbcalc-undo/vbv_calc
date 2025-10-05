import os
import json
from itertools import zip_longest

def convert_markdown_file_to_json(input_file, output_file):
    """
    Markdownテーブルを含むファイルを読み込み、JSON形式に変換して保存します。
    """
    try:
        if not os.path.exists(input_file):
            print(f"エラー: 入力ファイル '{input_file}' が見つかりませんでした。")
            return False

        with open(input_file, 'r', encoding='utf-8') as f:
            markdown_content = f.read()

        lines = [line.strip() for line in markdown_content.strip().split('\n')]
        
        headers = ["メダリオン", "レア", "二つ名", "接頭", "接尾", "攻撃", "防御", "速度", "知力", "特攻", "加護", "追加スキル"]
        
        data = []
        data_lines = [line for line in lines[3:] if line.strip()]
        
        for line in data_lines:
            # 各行を'|'で分割。先頭と末尾の空の要素を削除
            values = line.split('|')[1:-1]
            values = [v.strip() for v in values]
            
            # ヘッダーと値のペアを確実に作成
            item_data = list(zip_longest(headers, values, fillvalue=''))

            item = {}
            for key, value in item_data:
                item[key] = value

            processed_item = {
                "メダリオン": item.get("メダリオン"),
                "レア": int(item.get("レア")) if item.get("レア") and item.get("レア").isdigit() else 0,
                "二つ名": item.get("二つ名"),
                "接続": {
                    "接頭": item.get("接頭") if item.get("接頭") == "●" else "",
                    "接尾": item.get("接尾") if item.get("接尾") == "●" else ""
                },
                "ステータス変化": {
                    "攻撃": int(item.get("攻撃")) if item.get("攻撃") and item.get("攻撃").lstrip('-').isdigit() else 0,
                    "防御": int(item.get("防御")) if item.get("防御") and item.get("防御").lstrip('-').isdigit() else 0,
                    "速度": int(item.get("速度")) if item.get("速度") and item.get("速度").lstrip('-').isdigit() else 0,
                    "知力": int(item.get("知力")) if item.get("知力") and item.get("知力").lstrip('-').isdigit() else 0,
                    "特攻": item.get("特攻") 
                },
                "能力付与": {
                    "加護": item.get("加護", None) if item.get("加護") else None,
                    "追加スキル": item.get("追加スキル", None) if item.get("追加スキル") else None
                }
            }
            data.append(processed_item)

        with open(output_file, 'w', encoding='utf-8') as f:
            json.dump(data, f, ensure_ascii=False, indent=2)

        print(f"✅ '{input_file}' から '{output_file}' への変換が完了しました。")
        return True

    except Exception as e:
        print(f"❌ ファイル '{input_file}' の処理中にエラーが発生しました: {e}")
        return False

def process_data_directory(directory):
    """
    指定されたディレクトリ内のすべての .txt ファイルをJSONに変換します。
    """
    if not os.path.isdir(directory):
        print(f"エラー: ディレクトリ '{directory}' が見つかりませんでした。")
        return

    print(f"🔍 ディレクトリ '{directory}' 内の .txt ファイルを検索中...")
    
    processed_count = 0
    for filename in os.listdir(directory):
        if filename.endswith(".txt"):
            input_path = os.path.join(directory, filename)
            base_name = os.path.splitext(filename)[0]
            output_path = os.path.join(directory, f"{base_name}.json")
            
            if convert_markdown_file_to_json(input_path, output_path):
                processed_count += 1
    
    print(f"\n🎉 変換処理が完了しました。合計 {processed_count} 個のファイルが変換されました。")

if __name__ == '__main__':
    if not os.path.exists('data'):
        os.makedirs('data')

    process_data_directory('data')