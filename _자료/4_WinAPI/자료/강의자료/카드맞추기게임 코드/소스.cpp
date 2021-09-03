#include<Windows.h>
#include"Game_MG.h"
#include"resource.h"
LRESULT CALLBACK WndProc(HWND, UINT, WPARAM, LPARAM);
HINSTANCE g_hInst;
LPCTSTR lpszClass = TEXT("First");

enum GAME_SET
{
	GAME_SET_READY,
	GAME_SET_START
};

GAME_SET State = GAME_SET_READY;

int APIENTRY WinMain(HINSTANCE hInstance, HINSTANCE hPervlnstance, LPSTR lpszCmdParam, int nCmdShow)
{
	_CrtSetDbgFlag(_CRTDBG_ALLOC_MEM_DF | _CRTDBG_LEAK_CHECK_DF);
	_CrtDumpMemoryLeaks();
	//_CrtSetBreakAlloc(559);
	HWND hWnd;
	MSG Message;
	WNDCLASS WndClass;
	g_hInst = hInstance;

	WndClass.cbClsExtra = 0;
	WndClass.cbWndExtra = 0;
	WndClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);	//배경색
	WndClass.hCursor = LoadCursor(NULL, IDC_ARROW);	//커서
	WndClass.hIcon = LoadIcon(NULL, IDI_APPLICATION);	//아이콘 모양
	WndClass.hInstance = hInstance;
	WndClass.lpfnWndProc = WndProc;	//프로세스 함수 호출
	WndClass.lpszClassName = lpszClass;	//클레스 이름
	WndClass.lpszMenuName = NULL;
	WndClass.style = CS_HREDRAW | CS_VREDRAW;
	RegisterClass(&WndClass);

	hWnd = CreateWindow(lpszClass, lpszClass, WS_OVERLAPPEDWINDOW,470,30,590,800,
		NULL, (HMENU)NULL, hInstance, NULL);
	ShowWindow(hWnd, nCmdShow);

	while (GetMessage(&Message, NULL, 0, 0))
	{
		TranslateMessage(&Message);
		DispatchMessage(&Message);
	}
	return (int)Message.wParam;
}

LRESULT CALLBACK WndProc(HWND hWnd, UINT iMessage, WPARAM wParam, LPARAM lParam)
{
	HDC hdc,MemDC;
	PAINTSTRUCT ps;
	HBITMAP My_BITMAP,Old_BITMAP;
	SIZE m_size;
	RECT StartBox = { 250,350,340,370 };
	char menu[10] = "게임 시작";
	switch (iMessage)
	{
	case WM_CREATE:
		SetTimer(hWnd, 1, 100, NULL);
		Game_MG::GetInstance()->Init(hWnd, g_hInst);
		return 0;
	case WM_TIMER:
		if (Game_MG::GetInstance()->UpDate())
		{
			InvalidateRect(hWnd, NULL, true);
		}
		return 0;
	case WM_PAINT:
		hdc = BeginPaint(hWnd, &ps);
		if (State == GAME_SET_START)
		{
			MemDC = CreateCompatibleDC(hdc);
			My_BITMAP = LoadBitmap(g_hInst, MAKEINTRESOURCE(IDB_BITMAP12));
			Old_BITMAP = (HBITMAP)SelectObject(MemDC, My_BITMAP);

			BITMAP Info;
			GetObject(My_BITMAP, sizeof(Info), &Info);
			m_size.cx = Info.bmWidth;
			m_size.cy = Info.bmHeight;
			StretchBlt(hdc, 0, 0, 590, 800, MemDC, 0, 0, m_size.cx, m_size.cy, SRCCOPY);
			Game_MG::GetInstance()->Draw(hdc);
			SelectObject(hdc, Old_BITMAP);
			DeleteObject(My_BITMAP);
			DeleteDC(MemDC);
		}
		else if (State == GAME_SET_READY)
		{
			Rectangle(hdc, StartBox.left, StartBox.top, StartBox.right, StartBox.bottom); 
			DrawText(hdc, menu, -1, &StartBox, DT_CENTER);
		}
		EndPaint(hWnd, &ps);
		return 0;
	case WM_RBUTTONDOWN:
		SendMessage(hWnd, WM_DESTROY, 1, 0);
		return 0;
	case WM_LBUTTONDOWN:
		POINT pt;
		pt.x = LOWORD(lParam);
		pt.y = HIWORD(lParam);
		if (State == GAME_SET_START)
		{
			Game_MG::GetInstance()->LBTDown(pt);
			InvalidateRect(hWnd, NULL, true);
		}
		else if (State == GAME_SET_READY)
		{
			if (PtInRect(&StartBox, pt))
			{
				State = GAME_SET_START;
				InvalidateRect(hWnd, NULL, true);
				return 0;
			}
		}
		return 0;
	case WM_DESTROY:
		KillTimer(hWnd, 1);
		Game_MG::GetInstance()->Release();
		PostQuitMessage(0);
		return 0;
	}
	return(DefWindowProc(hWnd, iMessage, wParam, lParam));
}