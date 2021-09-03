#pragma once
#include<Windows.h>
#include"BitMap.h"
class GameManager
{
private:
	static GameManager* m_pThis;
	BitMap*				m_bt1;
	BitMap*				m_bt2;
	bool				m_bClickState;
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
	void Draw(HDC hdc);
	void LBTClick(POINT pt);
	void Release();
	~GameManager();
};

