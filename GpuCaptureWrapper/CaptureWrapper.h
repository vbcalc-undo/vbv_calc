#pragma once
#include <windows.h>

// �߂�l�R�[�h
// 0: ����
// 1: �E�B���h�E��������Ȃ�
// 2: �L���v�`�����s
// 3: PNG �ۑ����s
extern "C" __declspec(dllexport)
int CaptureWindowByTitle(const wchar_t* windowTitle, const wchar_t* outputPath);
