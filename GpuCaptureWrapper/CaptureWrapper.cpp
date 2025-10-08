#include "CaptureWrapper.h"
#include <windows.h>
#include <wincodec.h>
#include <wrl/client.h>

#pragma comment(lib, "windowscodecs.lib")

using Microsoft::WRL::ComPtr;

bool SaveHBitmapToPng(HBITMAP hBitmap, int width, int height, const wchar_t* path)
{
    if (!hBitmap) return false;

    ComPtr<IWICImagingFactory> wicFactory;
    if (FAILED(CoCreateInstance(CLSID_WICImagingFactory, nullptr, CLSCTX_INPROC_SERVER,
        IID_PPV_ARGS(&wicFactory)))) return false;

    ComPtr<IWICBitmap> wicBitmap;
    if (FAILED(wicFactory->CreateBitmapFromHBITMAP(hBitmap, nullptr,
        WICBitmapUseAlpha, &wicBitmap))) return false;

    ComPtr<IWICBitmapEncoder> encoder;
    if (FAILED(wicFactory->CreateEncoder(GUID_ContainerFormatPng, nullptr, &encoder))) return false;

    ComPtr<IWICStream> stream;
    if (FAILED(wicFactory->CreateStream(&stream))) return false;
    if (FAILED(stream->InitializeFromFilename(path, GENERIC_WRITE))) return false;

    if (FAILED(encoder->Initialize(stream.Get(), WICBitmapEncoderNoCache))) return false;

    ComPtr<IWICBitmapFrameEncode> frame;
    if (FAILED(encoder->CreateNewFrame(&frame, nullptr))) return false;
    if (FAILED(frame->Initialize(nullptr))) return false;
    if (FAILED(frame->SetSize(width, height))) return false;

    WICPixelFormatGUID format = GUID_WICPixelFormat32bppBGRA;
    frame->SetPixelFormat(&format);

    if (FAILED(frame->WriteSource(wicBitmap.Get(), nullptr))) return false;

    frame->Commit();
    encoder->Commit();
    return true;
}

int CaptureWindowByTitle(const wchar_t* windowTitle, const wchar_t* outputPath)
{
    HWND hwnd = FindWindowW(nullptr, windowTitle);
    if (!hwnd) return 1;

    RECT rc;
    if (!GetClientRect(hwnd, &rc)) return 2;

    HDC hdcScreen = GetDC(nullptr);
    HDC hdcMem = CreateCompatibleDC(hdcScreen);
    int width = rc.right - rc.left;
    int height = rc.bottom - rc.top;

    HBITMAP hBitmap = CreateCompatibleBitmap(hdcScreen, width, height);
    SelectObject(hdcMem, hBitmap);

    // バックグラウンドも試せるフラグ
    BOOL ok = PrintWindow(hwnd, hdcMem, PW_RENDERFULLCONTENT);
    ReleaseDC(nullptr, hdcScreen);
    DeleteDC(hdcMem);

    if (!ok)
    {
        DeleteObject(hBitmap);
        return 2;
    }

    bool saveOk = SaveHBitmapToPng(hBitmap, width, height, outputPath);
    DeleteObject(hBitmap);

    return saveOk ? 0 : 3;
}
