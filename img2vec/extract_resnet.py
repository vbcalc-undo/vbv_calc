import pandas as pd
from tensorflow.keras.applications.resnet50 import ResNet50, preprocess_input
from tensorflow.keras.preprocessing import image
import numpy as np
import json
import os
from PIL import Image

# --- CNNモデル（ResNet50、最終層除去、GlobalAveragePooling） ---
model = ResNet50(weights='imagenet', include_top=False, pooling='avg')

# --- CSV読み込み ---
csv_path = "hensei_data/list.csv"
df = pd.read_csv(csv_path, dtype={'id': str})  # IDを文字列として読み込み

def extract_feature_with_color(img_path):
    """
    画像パスから形状特徴＋色特徴ベクトルを抽出
    返り値: 1次元 numpy array
    """
    # --- 画像読み込み ---
    img = Image.open(img_path).convert("RGB")

    # --- 右下30px除外して切り抜き ---
    crop_box = (0, 0, img.width - 30, img.height - 30)
    img = img.crop(crop_box)

    # --- ResNet50入力にリサイズ ---
    img_resized = img.resize((224, 224))

    # --- Keras形式に変換 ---
    x = image.img_to_array(img_resized)
    x_exp = np.expand_dims(x, axis=0)
    x_exp = preprocess_input(x_exp)

    # --- 形状特徴抽出 ---
    shape_feat = model.predict(x_exp).flatten()

    # --- 色特徴抽出 ---
    img_np = np.array(img_resized) / 255.0  # 0-1 正規化

    # RGB平均値
    mean_rgb = np.mean(img_np, axis=(0,1))  # shape=(3,)

    # HSVヒストグラム（8ビン）
    img_hsv = np.array(img_resized.convert("HSV"))
    h_hist, _ = np.histogram(img_hsv[:,:,0], bins=8, range=(0,256))
    s_hist, _ = np.histogram(img_hsv[:,:,1], bins=8, range=(0,256))
    v_hist, _ = np.histogram(img_hsv[:,:,2], bins=8, range=(0,256))
    hsv_hist = np.concatenate([h_hist, s_hist, v_hist]).astype(float)
    hsv_hist /= hsv_hist.sum()  # 正規化

    # --- 形状＋色特徴を結合 ---
    combined = np.concatenate([shape_feat, mean_rgb, hsv_hist])
    return combined

# --- 特徴量抽出 ---
features = {}
for _, row in df.iterrows():
    img_id = str(row['id']).zfill(4)
    img_file = f"hensei_data/bc{img_id}.png"

    if os.path.exists(img_file):
        feat_vec = extract_feature_with_color(img_file)
        features[img_id] = feat_vec.tolist()
        print(f"Processed {img_file}")
    else:
        print(f"File not found: {img_file}")

# --- JSON保存 ---
json_path = "feature_extraction/features_shidan.json"
os.makedirs(os.path.dirname(json_path), exist_ok=True)
with open(json_path, "w", encoding="utf-8") as f:
    json.dump(features, f, ensure_ascii=False, indent=2)

print(f"Features saved to {json_path}")
