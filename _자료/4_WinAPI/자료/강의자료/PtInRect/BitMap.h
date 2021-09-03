#pragma once
#include<Windows.h>
class BitMap
{
private:
	HDC		MemDC;
	HBITMAP	m_pMyBitMap;
	HBITMAP	m_pOldBitMap;
	SIZE	m_size;
	RECT	m_rect;
public:
	BitMap();
	void Init(HDC hdc, HINSTANCE hInst, int index);
	void Draw(HDC hdc);
	inline RECT GetRect()
	{
		return m_rect;
	}
	~BitMap();
};

