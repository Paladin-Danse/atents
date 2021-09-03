// D3D.cpp : 애플리케이션에 대한 진입점을 정의합니다.
//

#pragma comment(lib, "d3d9.lib")
#pragma comment(lib, "d3dx9.lib")

#include <d3d9.h>   // 기본 DirectX9
#include <d3dx9.h>  // 확장 DirectX9


#include "framework.h"

#define MAX_LOADSTRING 100

WCHAR szWindowClass[MAX_LOADSTRING];            // 기본 창 클래스 이름입니다.

LRESULT CALLBACK    WndProc(HWND, UINT, WPARAM, LPARAM);

HRESULT InitD3D(HWND);
void Cleanup();
void Update(const float& elapsed);
void Render();
HRESULT InitVertexBuffer();
HRESULT InitIndexBuffer();

int APIENTRY wWinMain(_In_ HINSTANCE hInstance,
                     _In_opt_ HINSTANCE hPrevInstance,
                     _In_ LPWSTR    lpCmdLine,
                     _In_ int       nCmdShow)
{
    UNREFERENCED_PARAMETER(hPrevInstance);
    UNREFERENCED_PARAMETER(lpCmdLine);

    // 전역 문자열을 초기화합니다.
    LoadStringW(hInstance, IDC_D3D, szWindowClass, MAX_LOADSTRING);

    // Windows 클래스 등록.
    WNDCLASSEXW wcex = { sizeof(WNDCLASSEX), CS_HREDRAW | CS_VREDRAW, WndProc, 0, 0, hInstance, 0, 0, 0, NULL, szWindowClass, NULL };
    RegisterClassExW(&wcex);
    // Windows 생성.
    HWND hWnd = CreateWindowW(szWindowClass, L"D3D", WS_CAPTION | WS_SYSMENU | WS_MINIMIZEBOX,
        CW_USEDEFAULT, 0, CW_USEDEFAULT, 0, nullptr, nullptr, hInstance, nullptr);
    if (!hWnd) return FALSE;

    // InitD3D() 함수에서 Dirext3D 초기화.
    if (SUCCEEDED(InitD3D(hWnd)))
    {
        // Vertex 버퍼 초기화.
        if (SUCCEEDED(InitVertexBuffer()))
        {
            // Index 버퍼 초기화.
            if (SUCCEEDED(InitIndexBuffer()))
            {
                ShowWindow(hWnd, nCmdShow);
                UpdateWindow(hWnd);

                MSG msg;
                ZeroMemory(&msg, sizeof(msg));
                ULONGLONG frameTime, limitFrameTime = GetTickCount64();

                // 게임 루프.
                while (WM_QUIT != msg.message)
                {
                    if (PeekMessage(&msg, NULL, 0U, 0U, PM_REMOVE))
                    {
                        TranslateMessage(&msg);
                        DispatchMessage(&msg);
                    }
                    else
                    {
                        frameTime = GetTickCount64();
                        if (!(limitFrameTime > frameTime))
                        {
                            auto elapsed = frameTime - limitFrameTime;
                            limitFrameTime = frameTime + 30;

                            // TODO: 여기에 코드를 입력합니다.
                            Update(elapsed * 0.01f);
                            Render();
                        }
                    }
                }// while()
            }// InitIndexBuffer
        }// InitVertexBuffer
    }// InitD3D

    Cleanup();

    return 0;
}


LPDIRECT3D9             g_pD3D = NULL;          // D3D Device를 생성할 D3D 객체 변수.
LPDIRECT3DDEVICE9       g_pd3dDevice = NULL;    // 렌더링에 사용될 D3D Device.
LPDIRECT3DVERTEXBUFFER9 g_pVB = NULL;           // 정점 버퍼.
LPDIRECT3DINDEXBUFFER9  g_pIB = NULL;           // 인덱스 버퍼.

// Direct3D 초기화.
HRESULT InitD3D(HWND hWnd)
{
    // Device를 생성하기 위한 D3D객체.
    if (NULL == (g_pD3D = Direct3DCreate9(D3D_SDK_VERSION))) return E_FAIL;

    // Device 생성을 위한 구조체.
    // Present는 백 버퍼의 내용을 스크린에 보여주는 작업을 한다.
    D3DPRESENT_PARAMETERS d3dpp;
    ZeroMemory(&d3dpp, sizeof(d3dpp)); // 메모리의 쓰레기 값을 반드시 지원야 한다.

    d3dpp.Windowed = TRUE;                      // 창 모드로 생성.
    d3dpp.SwapEffect = D3DSWAPEFFECT_DISCARD;   // 가장 효율적인 SWAP 효과. 백 버퍼의 내용을 프론트 버퍼로 Swap하는 방식.
    d3dpp.BackBufferFormat = D3DFMT_UNKNOWN;    // 런타임에, 현재 디스플레이 모드에 맞춰 백 버퍼를 생성.
    d3dpp.EnableAutoDepthStencil = TRUE;        // D3D에서 프로그램의 깊이 버퍼(Z-Buffer)를 관리하게 한다.
                                                // 깊이 버퍼:화면에 그려질 각 픽셀의 z(깊이) 값을 저장하기위한 버퍼. 주로 절두체에서 픽셀의 깊이 값을 기록하는데 사용.
    d3dpp.AutoDepthStencilFormat = D3DFMT_D16;  // 16비트의 깊이 버퍼를 사용.


    // 해당 하드웨어에서 사용 가능한 멀티 생플링 레벨을 확인하여 적용.
    // 16 ~ 1까지 확인.
    DWORD level;
    for (auto type = (int)D3DMULTISAMPLE_16_SAMPLES; 0 < type; type--)
    {
        if (SUCCEEDED(g_pD3D->CheckDeviceMultiSampleType(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, D3DFMT_D16, FALSE, (D3DMULTISAMPLE_TYPE)type, &level)))
        {
            d3dpp.MultiSampleQuality = level - 1;
            d3dpp.MultiSampleType = (D3DMULTISAMPLE_TYPE)type;

            break;
        }
    }

    // Device 생성.
    // D3DADAPTER_DEFAULT : 기본 그래픽카드를 사용.
    // HAL(Hardware Abstraction Layer):하드웨어 추상 층.
    // REF(Reference rasterizer):소프트웨어로 애뮬레이트를 한다.
    // D3DDEVTYPE_HAL : 하드웨어 가속장치를 지원한다(그래픽카드 제조사에서 DirectX가 세부적인 부분을 제어할 필요가 없도록 지원하는 기능).
    // D3DCREATE_SOFTWARE_VERTEXPROCESSING : 정점(Vertex) 쉐이더 처리를 소프트웨어에서 한다(하드웨어에서 할 경우 더욱 높은 성능을 발휘:D3DCREATE_HARDWARE_VERTEXPROCESSING)
    if (FAILED(g_pD3D->CreateDevice(D3DADAPTER_DEFAULT, D3DDEVTYPE_HAL, hWnd, D3DCREATE_HARDWARE_VERTEXPROCESSING, &d3dpp, &g_pd3dDevice)))
    {
        return E_FAIL;
    }

    // TODO: 여기에서 Device 상태 정보 처리를 처리를 한다.
    g_pd3dDevice->SetRenderState(D3DRS_CULLMODE, D3DCULL_CCW);  // 컬링 모들를 켠다.
    g_pd3dDevice->SetRenderState(D3DRS_ZENABLE, TRUE);          // Z(깊이) 버퍼 기능을 켠다.
    g_pd3dDevice->SetRenderState(D3DRS_LIGHTING, FALSE);        // 정점(Vertex)에 색 값이 있으므로, 광원 기능을 끈다.
    
    // 멀티 생플링이 가능하면 기능을 활성화 하고, 아니면 끈다.
    g_pd3dDevice->SetRenderState(D3DRS_MULTISAMPLEANTIALIAS, (0 < d3dpp.MultiSampleType));

    return S_OK;
}

// 초기화된 객체를 해제.
void Cleanup()
{
    // 반드시 생성의 역순으로 해제.
    if (NULL != g_pIB) g_pIB->Release();
    if (NULL != g_pVB) g_pVB->Release();
    if (NULL != g_pd3dDevice) g_pd3dDevice->Release();
    if (NULL != g_pD3D) g_pD3D->Release();
}

struct CUSTOMVERTEX
{
    float x, y, z;  // 정점의 좌표.
    DWORD color;    // 정점의 색깔.
};
#define D3DFVF_CUSTOM (D3DFVF_XYZ | D3DFVF_DIFFUSE)
HRESULT InitVertexBuffer()
{
    CUSTOMVERTEX vertices[] =
    {
        // 정육면체. indexbuffer only.
        // 상자(Cube)를 그리기 위한 8개의 정점.
        { -1, 1, 1, 0xffff0000 },   // v0
        { 1, 1, 1, 0xff00ff00 },   // v1
        { 1, 1, -1, 0xff0000ff },   // v2
        { -1, 1, -1, 0xffffff00 },   // v3
        
        { -1, -1, 1, 0xff00ffff },   // v4
        { 1, -1, 1, 0xffff00ff },   // v5
        { 1, -1, -1, 0xff000000 },   // v6
        { -1, -1, -1, 0xffffffff },   // v7

        // 삼각형.
        // 삼각형을 그리기 위한 3개의 정점.
        //{ 0, 0.5f, 1, 0xffff0000 },         // v0
        //{ 0.5f, -0.5f, 1, 0xff00ff00 },     // v1
        //{ -0.5f, -0.5f, 1, 0xff00ffff },    // v2
    };

    // 정점 버퍼 생성.
    // 정점을 보관할 메모리 공간을 할당.
    // FVF를 지정하여 보관할 데이터 형식을 지정.
    // D3DPOOL_DEFAULT:리소스가 가장 적합한 메모리에 놓여진다.
    if (FAILED(g_pd3dDevice->CreateVertexBuffer(sizeof(vertices), 0, D3DFVF_CUSTOM, D3DPOOL_DEFAULT, &g_pVB, NULL)))
    {
        return E_FAIL;
    }

    // 정점 버퍼를 지정한 값으로 채운다.
    // 외부에서 접근하지 못하게 메모리를 Lock() 하고 사용이 끝난 후 Unlock()을 한다.
    LPVOID pVertices;
    if (FAILED(g_pVB->Lock(0, sizeof(vertices), (void**)&pVertices, 0))) return E_FAIL;
    memcpy(pVertices, vertices, sizeof(vertices));
    g_pVB->Unlock();

    return S_OK;
}

struct INDEX
{
    WORD _0, _1, _2;
};
HRESULT InitIndexBuffer()
{
    INDEX indices[] =
    {
        // 상자(Cube)를 그리기 위한 12개의 면(폴리곤의 개수) 선언.
        { 0, 1, 2 }, { 0, 2, 3 },  // 윗면.
        { 4, 6, 5 }, { 4, 7, 6 },  // 아랫면.
        { 0, 3, 7 }, { 0, 7, 4 },  // 왼쪽면.
        { 1, 5, 6 }, { 1, 6, 2 },  // 오른쪽면.
        { 3, 2, 6 }, { 3, 6, 7 },  // 앞면.
        { 0, 4, 5 }, { 0, 5, 1 },  // 뒷면.
    };


    // 인덱스 버퍼 생성.
    // D3DFMT_INDEX16:인덱스의 단위가 16비트. WORD(unsigned short):2byte(16bit).
    if (FAILED(g_pd3dDevice->CreateIndexBuffer(sizeof(indices), 0, D3DFMT_INDEX16, D3DPOOL_DEFAULT, &g_pIB, NULL))) return E_FAIL;

    LPVOID pIndices;
    if (FAILED(g_pIB->Lock(0, sizeof(indices), (void**)&pIndices, 0))) return E_FAIL;
    memcpy(pIndices, indices, sizeof(indices));
    g_pIB->Unlock();

    return S_OK;
}

D3DXMATRIXA16 g_matEarth, g_matMoon;
void Update(const float& elapsed)
{
    auto angle = GetTickCount64() * 0.001f;
    D3DXMATRIXA16 matEarthTr, matEarthRo;
    D3DXMATRIXA16 matMoonTr, matMoonRo, matMoonSc;
    D3DXQUATERNION qt;

    //D3DXMatrixIdentity(&matEarthTr);
    D3DXMatrixTranslation(&matEarthTr, 0, 0, 0);
    //D3DXQuaternionRotationYawPitchRoll(&qt, angle, 0, 0);
    //D3DXQuaternionNormalize(&qt, &qt);
    //D3DXMatrixRotationQuaternion(&g_matEarthRo, &qt);
    D3DXMatrixRotationY(&matEarthRo, angle);
    // 회전(자전) * 이동.
    g_matEarth = matEarthRo * matEarthTr;
    // 이동 * 회전(공전).
    //g_matEarth = matEarthTr * matEarthRo;

    //D3DXQuaternionRotationYawPitchRoll(&qt, angle, 0, 0);
    //D3DXQuaternionNormalize(&qt, &qt);
    //D3DXMatrixRotationQuaternion(&g_matMoonRo, &qt);
    D3DXMatrixRotationY(&matMoonRo, angle * 0.5f);
    D3DXMatrixScaling(&matMoonSc, 0.5f, 0.5f, 0.5f);
    D3DXMatrixTranslation(&matMoonTr, 5, 0, 0);

    // 크기 변경 * 이동 * 회전(공전) * 부모의 정보 추가.
    g_matMoon = matMoonSc * matMoonTr * matMoonRo * g_matEarth;
}

#define Deg2Rad 0.017453293f
void SetupMatrices()
{
    // 월드 스페이스.
    D3DXMATRIXA16 matWorld;         // 월드 행렬.
    D3DXMatrixIdentity(&matWorld);  // 단위행렬로 변경.
    g_pd3dDevice->SetTransform(D3DTS_WORLD, &matWorld); // 월드 스페이스로 변환.

    // 뷰 스페이스.
    D3DXVECTOR3 vEyePt(0.0f, 3.0f, -5.0f);      // 월드 좌표의 카메라의 위치.
    D3DXVECTOR3 vLookAtPt(0.0f, 0.0f, 0.0f);    // 월드 좌표의 카메라가 바라보는 위치.
    D3DXVECTOR3 vUpVector(0.0f, 0.1f, 0.0f);    // 월드 좌표의 하늘 방향을 알기 위한 up vector.
    D3DXMATRIXA16 matView;
    D3DXMatrixLookAtLH(&matView, &vEyePt, &vLookAtPt, &vUpVector);  // 뷰 변환 행렬 계산.
    g_pd3dDevice->SetTransform(D3DTS_VIEW, &matView);               // 뷰 스페이스로 변환.

    // 투영.
    D3DXMATRIXA16 matProj;
    // 투영 변환 행렬 계산.
    // FOV:시야각. 45도(D3DX_PI/4 == 45 * Deg2Rad).
    // 1.0f:종횡비. 1.0f(1:1), 1.33f(4:3), 1.77f/1.78f(16:9),  2.0f(18:9).......
    // 시야 거리는 1.0f(near)에서 100.0f(far)까지.
    D3DXMatrixPerspectiveFovLH(&matProj, 45 * Deg2Rad, 1.77f, 1.0f, 100.0f);
    g_pd3dDevice->SetTransform(D3DTS_PROJECTION, &matProj); // 투영 변환.
}

void DrawMesh(const D3DXMATRIXA16& matrix)
{
    g_pd3dDevice->SetTransform(D3DTS_WORLD, &matrix);
    // 정점의 정보가 담긴 정점 버퍼를 출력 스트림으로 할당.
    g_pd3dDevice->SetStreamSource(0, g_pVB, 0, sizeof(CUSTOMVERTEX));
    // D3D에 정점 쉐이더 정보를 지정. / 대부분의 경우 FVF만 지정한다.
    // FVF는 정점 버퍼를 만들때 정의한 값과 같아야 한다.
    g_pd3dDevice->SetFVF(D3DFVF_CUSTOM);
    // 인덱스 버퍼를 지정.
    g_pd3dDevice->SetIndices(g_pIB);
    g_pd3dDevice->DrawIndexedPrimitive(D3DPT_TRIANGLELIST, 0, 0, 8, 0, 12);
}

// 화면에 그리기.
void Render()
{
    if (NULL == g_pd3dDevice) return;

    // pRects(두 번째 인자)가 NULL이라면 Count(첫 번째 인자)는 0.
    // D3DCLEAR_TARGET:surface를 대상으로 한다. / 서피스(surface):이미지 데이터를 저장하는데 사용되는 버퍼.
    // D3DCLEAR_ZBUFFER:Z(깊이) 버퍼를 대상으로 한다.
    // 버퍼를 파란색[RGB(0, 0, 255)]으로 지운다.
    // Z(깊이) 버퍼를 1로 지운다. (0 ~ 1 사이의 값을 사용)
    // 스텐실(stencil) 버퍼를 0으로 지운다. (0 ~ 2^n-1 사이의 값을 사용) / 스텐실 버퍼:특정 영역이 렌더링 되는 것을 막기위해 사용.
    // 다음과 같은 경우 IDirect3DDevice9 :: Clear() 함수가 실패.
    // - 깊이 버퍼가 연결되지 않은 렌더링 대상의 깊이 버퍼 또는 스텐실 버퍼를 지운다.
    // - 깊이 버퍼에 스텐실 데이터가 포함되지 않은 경우 스텐실 버퍼를 지운다.
    if (SUCCEEDED(g_pd3dDevice->Clear(0, NULL, D3DCLEAR_TARGET | D3DCLEAR_ZBUFFER, D3DCOLOR_XRGB(0, 0, 255), 1, 0)))
    {
        SetupMatrices();

        // 렌더링 시작, 폴리곤을 그리겠다고 D3D에 알림(BeginScene).
        if (SUCCEEDED(g_pd3dDevice->BeginScene()))
        {
            // TODO: 여기에 코드를 입력합니다.
            DrawMesh(g_matEarth);
            DrawMesh(g_matMoon);
            
            //// DrawPrimitive() 함수를 호출하여 출력한다.
            //// D3DPT_TRIANGLELIST:정점을 이어 삼각형을 그린다.
            //// 0 번째의 정점 값부터 사용.
            //// 삼각형을 1개 그린다.
            //g_pd3dDevice->DrawPrimitive(D3DPT_TRIANGLELIST, 0, 1);

            // 렌더링 종료.
            g_pd3dDevice->EndScene();
        }

        // 백 버퍼를 보이는 화면으로 전환.
        g_pd3dDevice->Present(NULL, NULL, NULL, NULL);
    }
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
    switch (message)
    {
    case WM_DESTROY:
        PostQuitMessage(0);
        break;
    default:
        return DefWindowProc(hWnd, message, wParam, lParam);
    }
    return 0;
}
