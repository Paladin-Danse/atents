#pragma once
#include "BitMap.h"
#include "Res_MG.h"
#include "Mecro.h"
#include<Windows.h>

enum CARD_STATE
{
	CARD_STATE_CLOSE,
	CARD_STATE_OPEN,
	CARD_STATE_END,
};

class BitMap;
class Res_MG;
class Card
{
private:
	BitMap* m_pBitMap[CARD_STATE_END];
	int m_index;
	CARD_STATE m_eState;
	RECT Bit_Rect;
public:
	void Init(Res_MG* m_pRes_MG, int index, int x, int y,HWND hWnd);
	void Draw(HDC hdc);
	bool isChak(POINT pt);
	inline void OpenCard()
	{
		m_eState = CARD_STATE_OPEN;
	}
	inline void CloseCard()
	{
		m_eState = CARD_STATE_CLOSE;
	}
	inline bool isOpen()
	{
		if (m_eState == CARD_STATE_OPEN)
			return true;
		return false;
	}
	inline int GetIndex()
	{
		return m_index;
	}
	inline bool isSameCard(int index)
	{
		if (m_index == index)
			return true;
		return false;
	}
	Card();
	~Card();
};

