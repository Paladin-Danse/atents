#include "Input_MG.h"

namespace ENGINE
{
	Input_MG::Input_MG()
	{
	}
	Input_MG::~Input_MG()
	{
	}

	void Input_MG::Init(HWND hWnd)
	{
		//키등록
		for (int i = KEY_START; i < KEY_MAX; i++)
		{
			switch (i)
			{
			case KEY_LEFT:
				m_arrKeyCode[i].KeyCode = VK_LEFT;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			case KEY_RIGHT:
				m_arrKeyCode[i].KeyCode = VK_RIGHT;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			case KEY_UP:
				m_arrKeyCode[i].KeyCode = VK_UP;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			case KEY_DOWN:
				m_arrKeyCode[i].KeyCode = VK_DOWN;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			case KEY_ESCAPE:
				m_arrKeyCode[i].KeyCode = VK_ESCAPE;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			case KEY_LBUTTON:
				m_arrKeyCode[i].KeyCode = VK_LBUTTON;
				m_arrKeyCode[i].isKeyPress = false;
				break;
			}
		}
		//hWnd등록
		m_hWnd = hWnd;
	}

	void Input_MG::Update()
	{
		GetCursorPos(&m_MousePt);
		ScreenToClient(m_hWnd, &m_MousePt);
		for (int i = KEY_START; i < KEY_MAX; i++)
		{
			if (GetKeyState(m_arrKeyCode[i].KeyCode) & 0x8000)
				m_arrKeyCode[i].isKeyPress = true;
			else
				m_arrKeyCode[i].isKeyPress = false;
		}
	}

	bool Input_MG::isKeyPress(int KeyCode)
	{
		for (int i = KEY_START; i < KEY_MAX; i++)
		{
			if (m_arrKeyCode[i].KeyCode == KeyCode)
				return m_arrKeyCode[i].isKeyPress;
		}
		return false;
	}

	bool Input_MG::isAllKeyUnPress()
	{
		for (int i = KEY_START; i < KEY_MAX; i++)
		{
			if (m_arrKeyCode[i].isKeyPress == true)
				return false;
		}
		return true;
	}
}