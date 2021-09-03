#pragma once
#include<Windows.h>
class BitMap
{
private:
	HDC		MemDC;
	HBITMAP	m_pMyBitMap;
	HBITMAP	m_pOldBitMap;
	SIZE	m_size;
public:
	BitMap();
	void Init(HDC hdc, HINSTANCE hInst);
	void Draw(HDC hdc, int x, int y);
	~BitMap();
};

