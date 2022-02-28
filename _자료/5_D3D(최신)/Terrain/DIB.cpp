#include <Windows.h>
#include <stdio.h>
#include <math.h>

#include "DIB.h"

//#define  _CRT_SECURE_NO_WARNINGS


LPBYTE DIBLoadHandle(const char* lpFileName)
{
	LPBYTE	lpDib = NULL;
	FILE*		fp;
	// rb : read binary(���̳ʸ� �������� ����)
	// ����(�̹���)�� ����.
	if (!fopen_s(&fp, lpFileName, "rb")) //�����ϸ� 0�� ��ȯ.
	{
		BITMAPFILEHEADER	bmfh;
		// ��ȯ ���� count(1)���� ������ ���� �Ǵ� ���� ��.
		// �Է� stream���� size ������ count �׸���� �а�, ������ buffer�� ����.
		if (1 <= fread(&bmfh, sizeof(BITMAPFILEHEADER), 1, fp))
		{
			int nSize = bmfh.bfSize - sizeof(BITMAPFILEHEADER);
			// Allocation : ���� �Ҵ�.
			// GMEM_FIXED : ������ �޸� ���� �Ҵ�.
			// ������ ũ��� �޸� ������ �Ҵ�.
			if (HGLOBAL hDib = (HGLOBAL)GlobalAlloc(GMEM_FIXED, nSize))
			{
				// �޸� ��.
				if (lpDib = (LPBYTE)GlobalLock(hDib)) fread(lpDib, nSize, 1, fp);
			}
		}
		fclose(fp);
	}
	return lpDib;
}

VOID	DIBDeleteHandle(LPBYTE lpSrc)
{
	if (lpSrc)
	{
		GlobalUnlock(lpSrc);
		GlobalFree(lpSrc);
	}
}