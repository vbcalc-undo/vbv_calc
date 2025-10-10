import pandas as pd
from tensorflow.keras.applications.resnet50 import ResNet50, preprocess_input
from tensorflow.keras.preprocessing import image
import numpy as np
import json
import os
from PIL import Image

# CNNモデル（最終層除去、GlobalAveragePooling）
model = ResNet50(weights='imagenet', include_top=False, pooling='avg')

# CSV読み込み（IDを文字列として読み込む）
csv_path = "hensei_data/list.csv"
df = pd.read_csv(csv_path, dtype={'id': str})  # 文字列として読み込み

def extract_feature(img_path):
    """画像パスから特徴量ベクトルを抽出"""
    # PILで画像読み込み
    img = Image.open(img_path)

    # RGBAならRGBに変換
    if img.mode != "RGB":
        img = img.convert("RGB")

    # 右下30pxを除外して切り抜き
    crop_box = (0, 0, img.width - 30, img.height - 30)
    img = img.crop(crop_box)

    # ResNet50入力にリサイズ
    img = img.resize((224, 224))

    # Keras形式に変換
    x = image.img_to_array(img)
    x = np.expand_dims(x, axis=0)
    x = preprocess_input(x)

    # 特徴量抽出
    feature = model.predict(x)
    return feature.flatten()

features = {}
for _, row in df.iterrows():
    # IDを4桁ゼロ埋め
    img_id = str(row['id']).zfill(4)
    img_file = f"hensei_data/bc{img_id}.png"  # ファイル名を bcXXXX.png に変更

    if os.path.exists(img_file):
        features[img_id] = extract_feature(img_file).tolist()
        print(f"Processed {img_file}")
    else:
        print(f"File not found: {img_file}")

# JSON保存
json_path = "feature_extraction/features.json"
with open(json_path, "w", encoding="utf-8") as f:
    json.dump(features, f, ensure_ascii=False)

print(f"Features saved to {json_path}")
