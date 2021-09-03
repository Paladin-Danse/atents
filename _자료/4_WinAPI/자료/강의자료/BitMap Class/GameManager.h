#pragma once
#include<Windows.h>
#include"BitMap.h"
class GameManager
{
private:
	static GameManager* m_pThis;
	BitMap* m_bt;
public:
	static GameManager* GetInstans()
	{
		if (m_pThis == NULL)
		{
			m_pThis = new GameManager;
		}
		return m_pThis;
	}
	GameManager();
	void Init(HWND hWnd, HINSTANCE hInst);
	void Draw(HDC hdc, int x, int y);
	void Release();
	~GameManager();
};

