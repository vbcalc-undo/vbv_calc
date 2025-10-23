import pandas as pd
from tensorflow.keras.applications import MobileNetV2
from tensorflow.keras.applications.mobilenet_v2 import preprocess_input
from tensorflow.keras.preprocessing import image
import numpy as np
import json
import os
from PIL import Image

# CNNモデル（最終層除去）
model = MobileNetV2(weights='imagenet', include_top=False, pooling='avg')

csv_path = "equipment_data/equipment_list.csv"
df = pd.read_csv(csv_path, dtype={'id': str})

def extract_cnn_feature(img):
    # MobileNetV2は[-1,1]正規化
    img_resized = img.resize((224,224))
    x = image.img_to_array(img_resized)
    x = np.expand_dims(x, axis=0)
    x = preprocess_input(x)
    return model.predict(x).flatten()

def extract_color_feature(img, bins_hsv=16, bins_rgb=8):
    img_resized = img.resize((224,224))
    arr = np.array(img_resized)/255.0

    # RGB平均
    rgb_mean = arr.mean(axis=(0,1))

    # RGBヒストグラム
    r_hist, _ = np.histogram(arr[:,:,0], bins=bins_rgb, range=(0,1), density=True)
    g_hist, _ = np.histogram(arr[:,:,1], bins=bins_rgb, range=(0,1), density=True)
    b_hist, _ = np.histogram(arr[:,:,2], bins=bins_rgb, range=(0,1), density=True)

    # HSVヒストグラム
    hsv_img = img_resized.convert("HSV")
    hsv_arr = np.array(hsv_img)/255.0
    h_hist, _ = np.histogram(hsv_arr[:,:,0], bins=bins_hsv, range=(0,1), density=True)
    s_hist, _ = np.histogram(hsv_arr[:,:,1], bins=bins_hsv, range=(0,1), density=True)
    v_hist, _ = np.histogram(hsv_arr[:,:,2], bins=bins_hsv, range=(0,1), density=True)

    color_feat = np.concatenate([rgb_mean, r_hist, g_hist, b_hist, h_hist, s_hist, v_hist])
    return color_feat

features = {}
for _, row in df.iterrows():
    img_id = str(row['id'])
    img_file = f"equipment_data/{img_id}.png"
    if os.path.exists(img_file):
        img = Image.open(img_file).convert("RGB")
        cnn_feat = extract_cnn_feature(img)
        color_feat = extract_color_feature(img)
        features[img_id] = np.concatenate([cnn_feat, color_feat]).tolist()
        print(f"Processed {img_file}")
    else:
        print(f"File not found: {img_file}")

# JSON保存
os.makedirs("feature_extraction", exist_ok=True)
with open("feature_extraction/equipment_feature.json", "w", encoding="utf-8") as f:
    json.dump(features, f, ensure_ascii=False)

print("Features saved.")
