#include "GameManager.h"


GameManager* GameManager::m_pThis = NULL;

GameManager::GameManager()
{
}
void GameManager::Init(HWND hWnd, HINSTANCE hInst)
{
	HDC hdc = GetDC(hWnd);
	m_pResource = new ResourceManager;
	m_pResource->Init(hdc, hInst);
	return;
}
void GameManager::Draw(HDC hdc, int x, int y)
{
	BitMap* tmp;
	for (int i = 0; i < IMG_NUMBER_END; i++)
	{
		tmp = m_pResource->GetBitMap((IMG_NUMBER)i);
		tmp->Draw(hdc, x, y,i);
	}
}

void GameManager::Release()
{
	delete(m_pThis);
}
GameManager::~GameManager()
{
	delete(m_pResource);
}
