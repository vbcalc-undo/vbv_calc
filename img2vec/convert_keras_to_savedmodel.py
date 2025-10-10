import tensorflow as tf
import os

# 入力と出力パス
keras_model_path = "resnet50_saved_model.keras"
saved_model_dir = "resnet50_saved_model"

# 既存のディレクトリがあれば削除（安全のため確認を推奨）
if os.path.exists(saved_model_dir):
    import shutil
    shutil.rmtree(saved_model_dir)

# .keras モデルをロード
print(f"Loading Keras model from: {keras_model_path}")
model = tf.keras.models.load_model(keras_model_path)

# SavedModel 形式で保存
print(f"Saving TensorFlow SavedModel to: {saved_model_dir}")
tf.saved_model.save(model, saved_model_dir)

print("✅ SavedModel export complete!")
