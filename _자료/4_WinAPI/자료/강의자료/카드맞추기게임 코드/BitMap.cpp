#include "BitMap.h"



BitMap::BitMap()
{
}

void BitMap::Init(HDC hdc, HINSTANCE hInst,int id)
{
	MemDC = CreateCompatibleDC(hdc);
	m_pBitMap = (HBITMAP)LoadBitmap(hInst, MAKEINTRESOURCE(id));
	m_pBitOld = (HBITMAP)SelectObject(MemDC, m_pBitMap);

	BITMAP Bit_Info;
	GetObject(m_pBitMap, sizeof(Bit_Info), &Bit_Info);
	m_size.cx = Bit_Info.bmWidth;
	m_size.cy = Bit_Info.bmHeight;
}

void BitMap::Draw(HDC hdc, int x, int y)
{
	StretchBlt(hdc, x, y, m_size.cx/2, m_size.cy/2, MemDC, 0, 0,m_size.cx,m_size.cy, SRCCOPY);
}

BitMap::~BitMap()
{
	SelectObject(MemDC, m_pBitOld);
	DeleteObject(m_pBitMap);
	DeleteDC(MemDC);
}
