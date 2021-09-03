#include "Res_MG.h"
#include"Mecro.h"



Res_MG::Res_MG()
{
}

void Res_MG::Init(HDC hdc, HINSTANCE hInst)
{
	for (int i = 0 ; i < RES_TYPE_CARD_END; i++)
	{
		m_ResBit[i] = new BitMap();
		m_ResBit[i]->Init(hdc, hInst, IDB_BITMAP1 + i);
	}
}

BitMap* Res_MG::GetBitMap(RES_TYPE type,HWND hWnd)
{
	if (type >= RES_TYPE_CARD_END || type < RES_TYPE_CARD_00)
	{
		MessageBox(hWnd, "카드가없습니다.", "Error.", MB_OK);
		return NULL;
	}
	return m_ResBit[type];
}

Res_MG::~Res_MG()
{
}
