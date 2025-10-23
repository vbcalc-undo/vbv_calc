import pandas as pd
from tensorflow.keras.applications.resnet50 import ResNet50, preprocess_input
from tensorflow.keras.preprocessing import image
import numpy as np
import json
import os
from PIL import Image

# --- CNNモデル（最終層除去） ---
model = ResNet50(weights='imagenet', include_top=False, pooling='avg')

# --- CSV読み込み ---
csv_path = "hensei_data/list.csv"
df = pd.read_csv(csv_path, dtype={'id': str})

# --- CNN特徴抽出 ---
def extract_cnn_feature(img):
    img = img.resize((224, 224))
    x = image.img_to_array(img)
    x = np.expand_dims(x, axis=0)
    x = preprocess_input(x)
    return model.predict(x).flatten()

# --- カラー特徴抽出（chara_dataと同一構成） ---
def extract_color_feature(img, bins_hsv=16, bins_rgb=8):
    img = img.resize((224, 224))
    arr = np.array(img) / 255.0

    # RGB平均値（3）
    rgb_mean = arr.mean(axis=(0, 1))

    # RGBヒストグラム（各8ビン ×3 = 24）
    r_hist, _ = np.histogram(arr[:, :, 0], bins=bins_rgb, range=(0, 1), density=True)
    g_hist, _ = np.histogram(arr[:, :, 1], bins=bins_rgb, range=(0, 1), density=True)
    b_hist, _ = np.histogram(arr[:, :, 2], bins=bins_rgb, range=(0, 1), density=True)

    # HSVヒストグラム（各16ビン ×3 = 48）
    hsv_img = img.convert("HSV")
    hsv_arr = np.array(hsv_img) / 255.0
    h_hist, _ = np.histogram(hsv_arr[:, :, 0], bins=bins_hsv, range=(0, 1), density=True)
    s_hist, _ = np.histogram(hsv_arr[:, :, 1], bins=bins_hsv, range=(0, 1), density=True)
    v_hist, _ = np.histogram(hsv_arr[:, :, 2], bins=bins_hsv, range=(0, 1), density=True)

    # 結合（合計 3 + 24 + 48 = 75次元）
    color_feat = np.concatenate([rgb_mean, r_hist, g_hist, b_hist, h_hist, s_hist, v_hist])
    return color_feat

# --- 特徴量抽出ループ ---
features = {}
for _, row in df.iterrows():
    img_id = str(row['id']).zfill(4)
    img_file = f"hensei_data/bc{img_id}.png"

    if os.path.exists(img_file):
        # 画像読み込み＋右下30px除外（元の仕様を保持）
        img = Image.open(img_file).convert("RGB")
        crop_box = (0, 0, img.width - 30, img.height - 30)
        img = img.crop(crop_box)

        # 特徴抽出
        cnn_feat = extract_cnn_feature(img)
        color_feat = extract_color_feature(img)
        features[img_id] = np.concatenate([cnn_feat, color_feat]).tolist()

        print(f"Processed {img_file}")
    else:
        print(f"File not found: {img_file}")

# --- JSON保存 ---
json_path = "feature_extraction/features_shidan.json"
os.makedirs(os.path.dirname(json_path), exist_ok=True)
with open(json_path, "w", encoding="utf-8") as f:
    json.dump(features, f, ensure_ascii=False, indent=2)

print(f"Features saved to {json_path}")