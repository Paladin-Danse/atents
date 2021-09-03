#include "Res_MG.h"

namespace ENGINE
{
	Res_MG::Res_MG()
	{
	}
	Res_MG::~Res_MG()
	{
		for (auto iter = m_vBitMap.begin(); iter != m_vBitMap.end(); iter++)
			delete (*iter);
		delete m_pBack;
	}

	void Res_MG::Init(HDC hdc, int x, int y)
	{
		//BackDC설정
		m_pBack = new ENGINE::BitMap;
		m_pBack->InitBackDC(hdc, x, y);
		//그 외 비트맵 리소스로드
		char buf[256];
		for (int i = RES_START; i != RES_END; i++)
		{
			ENGINE::BitMap* NewBitMap = new ENGINE::BitMap;
			wsprintf(buf, "Game/%d.bmp", i);
			NewBitMap->InitBitMap(buf);
			m_vBitMap.push_back(NewBitMap);
		}
	}

	void Res_MG::DrawBackDC(HDC hdc)
	{
		m_pBack->DrawBackDC(hdc);
	}
}