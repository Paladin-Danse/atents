#include "BitMap.h"
#include"resource.h"



BitMap::BitMap()
{
}

void BitMap::Init(HDC hdc, HINSTANCE hInst,int index)
{
	MemDC = CreateCompatibleDC(hdc);
	m_pMyBitMap = LoadBitmap(hInst, MAKEINTRESOURCE(index));
	m_pOldBitMap = (HBITMAP)SelectObject(MemDC, m_pMyBitMap);

	BITMAP Bit_Info;
	GetObject(m_pMyBitMap,sizeof(Bit_Info), &Bit_Info);
	m_size.cx = Bit_Info.bmWidth;
	m_size.cy = Bit_Info.bmHeight;
}
void BitMap::Draw(HDC hdc, int x, int y,int i)
{
	if (i >= 5)
		y += m_size.cy+20;
	i %= 5;
	x += (x + (i*m_size.cx)) + (10 * i);
	BitBlt(hdc,x, y, m_size.cx, m_size.cy, MemDC, 0, 0, SRCCOPY);
}

BitMap::~BitMap()
{
	SelectObject(MemDC, m_pOldBitMap);
	DeleteObject(m_pMyBitMap);
	DeleteDC(MemDC);
}
