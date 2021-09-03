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
	m_rect.left = 500;
	m_rect.top = 200;
	m_rect.right = m_rect.left + m_size.cx;
	m_rect.bottom = m_rect.top + m_size.cy;
}
void BitMap::Draw(HDC hdc)
{
	BitBlt(hdc, m_rect.left, m_rect.top, m_size.cx, m_size.cy, MemDC, 0, 0, SRCCOPY);
}

BitMap::~BitMap()
{
	SelectObject(MemDC, m_pOldBitMap);
	DeleteObject(m_pMyBitMap);
	DeleteDC(MemDC);
}
