#include "GameManager.h"


GameManager* GameManager::m_pThis = NULL;

GameManager::GameManager()
{
}
void GameManager::Init(HWND hWnd, HINSTANCE hInst)
{
	HDC hdc = GetDC(hWnd);
	m_bt = new BitMap;
	m_bt->Init(hdc, hInst);
}
void GameManager::Draw(HDC hdc, int x, int y)
{
	m_bt->Draw(hdc, x, y);
}

void GameManager::Release()
{
	delete(m_pThis);
}
GameManager::~GameManager()
{
	delete(m_bt);
}
