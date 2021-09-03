#include "Card.h"



Card::Card()
{
	m_eState = CARD_STATE_CLOSE;
}


void Card::Init(Res_MG* m_pRes_MG, int index, int x, int y,HWND hWnd)
{
	m_index = index;
	
	m_pBitMap[CARD_STATE_CLOSE] = m_pRes_MG->GetBitMap(RES_TYPE_CARD_BACK, hWnd);
	m_pBitMap[CARD_STATE_OPEN] = m_pRes_MG->GetBitMap((RES_TYPE)m_index,hWnd);
	
	Bit_Rect.left = x;
	Bit_Rect.top = y;
	Bit_Rect.right = x + WIDTH/2;
	Bit_Rect.bottom = y + HEIGHT/2;
}

void Card::Draw(HDC hdc)
{
	m_pBitMap[m_eState]->Draw(hdc, Bit_Rect.left, Bit_Rect.top);
}


bool Card::isChak(POINT pt)
{
	if (PtInRect(&Bit_Rect, pt))
	{
		return true;
	}
	return false;
}

Card::~Card()
{
}
