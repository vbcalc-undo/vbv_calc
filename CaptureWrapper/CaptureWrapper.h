#pragma once
#include <windows.h>

// 戻り値コード
// 0: 成功
// 1: ウィンドウが見つからない
// 2: キャプチャ失敗
// 3: PNG 保存失敗
extern "C" __declspec(dllexport)
int CaptureWindowByTitle(const wchar_t* windowTitle, const wchar_t* outputPath);
