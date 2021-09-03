#pragma once
#include<Windows.h>

#include"BitMap.h"

enum IMG_NUMBER
{
	IMG_NUMBER_1,
	IMG_NUMBER_2,
	IMG_NUMBER_3,
	IMG_NUMBER_4,
	IMG_NUMBER_5,
	IMG_NUMBER_6,
	IMG_NUMBER_7,
	IMG_NUMBER_8,
	IMG_NUMBER_9,
	IMG_NUMBER_10,
	IMG_NUMBER_END
};
class ResourceManager
{
	BitMap* m_Bitmap[IMG_NUMBER_END];
public:
	ResourceManager();
	void Init(HDC hdc, HINSTANCE hInst);
	BitMap* GetBitMap(IMG_NUMBER Number);
	~ResourceManager();
};

