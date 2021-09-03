#pragma once
#include"BitMap.h"
#include<Windows.h>

enum RES_TYPE
{
	RES_TYPE_CARD_00,
	RES_TYPE_CARD_01,
	RES_TYPE_CARD_02,
	RES_TYPE_CARD_03,
	RES_TYPE_CARD_04,
	RES_TYPE_CARD_05,
	RES_TYPE_CARD_06,
	RES_TYPE_CARD_07,
	RES_TYPE_CARD_08,
	RES_TYPE_CARD_09,
	RES_TYPE_CARD_BACK,
	RES_TYPE_CARD_END
};

class Res_MG
{
private:
	BitMap* m_ResBit[RES_TYPE_CARD_END];
public:
	Res_MG();
	void Init(HDC hdc, HINSTANCE hInst);
	BitMap* GetBitMap(RES_TYPE type,HWND hWnd);
	~Res_MG();
};

