import json
from janome.tokenizer import Tokenizer
from janome.analyzer import Analyzer
from janome.charfilter import *
from janome.tokenfilter import *

def convert_markdown_to_json(input_file, output_file):
    """
    Markdown形式の表を形態素解析で分割し、指定されたJSONフォーマットに変換して出力する。
    助詞は前の単語と結合する。
    """
    try:
        with open(input_file, 'r', encoding='utf-8') as f:
            lines = f.readlines()
    except FileNotFoundError:
        print(f"エラー: 入力ファイル '{input_file}' が見つかりません。")
        return

    # フィルタリングを設定（助詞フィルタは削除）
    t = Tokenizer()
    analyzer = Analyzer(
        char_filters=[UnicodeNormalizeCharFilter()],
        tokenizer=t,
        token_filters=[]
    )

    data_lines = [line for line in lines if line.strip().startswith('|') and '|~' not in line]
    converted_data = []

    for line in data_lines:
        parts = [p.strip() for p in line.strip('|').split('|')]
        if len(parts) < 8:
            continue

        title = parts[0]
        if "※" in parts[0]:
	        converted_data.append({
	            "メダリオン": "キャラ",
	            "レア": 8,
	            "二つ名": title,
	            "接続": {
	                "接頭": "●",
	                "接尾": ""
	            },
	            "ステータス変化": {
	                "攻撃": 0,
	                "防御": 0,
	                "速度": 0,
	                "知力": 0,
	                "特攻": 0
	            },
	            "能力付与": {
	                "加護": None,
	                "追加スキル": None
	            }
	        })

	        # 2つ目の要素
	        converted_data.append({
	            "メダリオン": "キャラ",
	            "レア": 8,
	            "二つ名": title,
	            "接続": {
	                "接頭": "",
	                "接尾": "●"
	            },
	            "ステータス変化": {
	                "攻撃": 0,
	                "防御": 0,
	                "速度": 0,
	                "知力": 0,
	                "特攻": ""
	            },
	            "能力付与": {
	                "加護": None,
	                "追加スキル": None
	            }
	        })            
        else:
	        attack = int(parts[1])
	        defense = int(parts[2])
	        speed = int(parts[3])
	        intellect = int(parts[4])
	        special_attack_type = parts[5]
	        skill1_str = parts[6]
	        skill2_str = parts[7]
	        # 追加スキルの文字列を分割
	        def parse_skill(skill_str):
	            if '[' in skill_str and ']' in skill_str:
	                name, value_str = skill_str.split('[')
	                value = int(value_str.replace(']', ''))
	                return f"{name.strip()}:{value}"
	            else:
	                return skill_str.strip()

	        skill1 = parse_skill(skill1_str)
	        skill2 = parse_skill(skill2_str)

	        # 形態素解析で称号を分割し、手動で助詞を結合
	        tokens = list(analyzer.analyze(title))
	        
	        title1_tokens = []
	        title2_tokens = []
	        
	        # 最初の「名詞」とそれに続く「助詞」を結合して title1 に入れる
	        found_main_noun = False
	        for i, token in enumerate(tokens):
	            if found_main_noun == False and token.part_of_speech.startswith('名詞'):
	                title1_tokens.append(token.surface)
	                found_main_noun = True
	            elif found_main_noun == True and (token.part_of_speech.startswith('助詞') or token.surface == 'の' or token.surface == 'を'):
	                title1_tokens.append(token.surface)
	                title2_tokens.extend([t.surface for t in tokens[i+1:]])
	                break
	            elif found_main_noun == False and token.part_of_speech.startswith('動詞') or token.part_of_speech.startswith('形容詞'):
	                # 動詞や形容詞から始まる場合も最初の部分を title1 に
	                title1_tokens.append(token.surface)
	                title2_tokens.extend([t.surface for t in tokens[i+1:]])
	                break
	            else:
	                title1_tokens.append(token.surface)

	        title1 = "".join(title1_tokens)
	        title2 = "".join(title2_tokens)
	        
	        # もし分割がうまくいかなければ、元の文字列をそのまま使用
	        if not title2:
	            title1 = tokens[0].surface
	            title2 = "".join(token.surface for token in tokens[1:])

	        # 1つ目の要素
	        converted_data.append({
	            "メダリオン": "キャラ",
	            "レア": 8,
	            "二つ名": title1,
	            "接続": {
	                "接頭": "●",
	                "接尾": ""
	            },
	            "ステータス変化": {
	                "攻撃": attack,
	                "防御": defense,
	                "速度": speed,
	                "知力": intellect,
	                "特攻": special_attack_type
	            },
	            "能力付与": {
	                "加護": None,
	                "追加スキル": skill1
	            }
	        })

	        # 2つ目の要素
	        converted_data.append({
	            "メダリオン": "キャラ",
	            "レア": 8,
	            "二つ名": title2,
	            "接続": {
	                "接頭": "",
	                "接尾": "●"
	            },
	            "ステータス変化": {
	                "攻撃": 0,
	                "防御": 0,
	                "速度": 0,
	                "知力": 0,
	                "特攻": ""
	            },
	            "能力付与": {
	                "加護": None,
	                "追加スキル": skill2
	            }
	        })

    with open(output_file, 'w', encoding='utf-8') as f:
        json.dump(converted_data, f, ensure_ascii=False, indent=2)

    print(f"変換が完了しました。結果は '{output_file}' に出力されました。")

if __name__ == "__main__":
    convert_markdown_to_json('./busho_data/busho_shogo.txt', './data/busho_shogo.json')