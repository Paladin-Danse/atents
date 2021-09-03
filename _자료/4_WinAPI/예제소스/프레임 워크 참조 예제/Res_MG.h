#pragma once
#include"SingleTon.h"
#include"BitMap.h"

namespace ENGINE
{
	class Res_MG : public SingleTon<Res_MG>
	{
	private:
		ENGINE::BitMap* m_pBack;
		vector<ENGINE::BitMap*> m_vBitMap;
	public:
		void Init(HDC hdc, int x, int y);
		void DrawBackDC(HDC hdc);
		inline HDC GetBackDC() { return m_pBack->GetDC(); }
		inline ENGINE::BitMap* GetBitMap(int ImageNum) { return m_vBitMap[ImageNum]; }
		Res_MG();
		~Res_MG();
	};
}
