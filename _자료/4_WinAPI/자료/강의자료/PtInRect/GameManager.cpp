#include "GameManager.h"
#include "resource.h"


GameManager* GameManager::m_pThis = NULL;

GameManager::GameManager()
{
	m_bClickState = FALSE;
}
void GameManager::Init(HWND hWnd, HINSTANCE hInst)
{
	HDC hdc = GetDC(hWnd);
	m_bt1 = new BitMap;
	m_bt1->Init(hdc, hInst, IDB_BITMAP1);
	m_bt2 = new BitMap;
	m_bt2->Init(hdc, hInst, IDB_BITMAP2);
}
void GameManager::Draw(HDC hdc)
{
	if(m_bClickState)
		m_bt1->Draw(hdc);
	else
		m_bt2->Draw(hdc);
}

void GameManager::LBTClick(POINT pt)
{
	if (PtInRect(&m_bt1->GetRect(), pt))
	{
		if (m_bClickState)
			m_bClickState = FALSE;
		else
			m_bClickState = TRUE;
	}
}

void GameManager::Release()
{
	delete(m_pThis);
}
GameManager::~GameManager()
{
	delete(m_bt1);
}
