#pragma once
#include"SingleTon.h"
#include"LibMecro.h"

namespace ENGINE
{
	struct KEY
	{
		int KeyCode;
		bool isKeyPress;
	};

	class Input_MG : public SingleTon<Input_MG>
	{
	private:
		HWND m_hWnd;
		KEY m_arrKeyCode[KEY_MAX];
		POINT m_MousePt;
	public:
		void Init(HWND hWnd);
		void Update();
		bool isKeyPress(int KeyCode);
		bool isAllKeyUnPress();
		inline POINT GetMousePt() { return m_MousePt; }
		Input_MG();
		~Input_MG();
	};
}