#include "ResourceManager.h"
#include "resource.h"



ResourceManager::ResourceManager()
{
}


void ResourceManager::Init(HDC hdc, HINSTANCE hInst)
{
	for (int i = 0; i < IMG_NUMBER_END; i++)
	{
		m_Bitmap[i] = new BitMap;
		m_Bitmap[i]->Init(hdc, hInst, IDB_BITMAP1 + i);
	}
}
BitMap* ResourceManager::GetBitMap(IMG_NUMBER Number)
{
	return m_Bitmap[Number];
}

ResourceManager::~ResourceManager()
{
	for (int i = 0; i < IMG_NUMBER_END; i++)
	{
		delete(m_Bitmap[i]);
	}
}
