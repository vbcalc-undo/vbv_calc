import tensorflow as tf
import tf2onnx
import subprocess

# ResNet50 + GlobalAveragePooling
model = tf.keras.applications.ResNet50(
    include_top=False, weights='imagenet', pooling='avg'
)

# Kerasモデルとして保存
keras_path = "resnet50_saved_model.keras"
model.save(keras_path)

# Keras → ONNX変換
subprocess.run([
    "python", "-m", "tf2onnx.convert",
    "--keras", keras_path,
    "--output", "feature_extraction/resnet50_features.onnx",
    "--opset", "13"
])
