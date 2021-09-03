#pragma once
#include"Res_MG.h"
#include<Windows.h>
#include<vector>
#include"Card.h"
using namespace std;

enum GAME_STATE
{
	GAME_STATE_WAIT,
	GAME_STATE_ONE,
	GAME_STATE_SHOW
};

class Res_MG;
class Card;
class Game_MG
{
private:
	static Game_MG*	m_pThis;
	Res_MG*			m_pRes_MG;
	Game_MG();

	vector<Card*> vec_Card;
	GAME_STATE m_eState;
	Card* m_pCardONE;
	Card* m_pCardTWO;
	int showCount;

public:
	static Game_MG* GetInstance()
	{
		if(m_pThis == NULL)
		{
			m_pThis = new Game_MG;
		}
		return m_pThis;
	}

	void Init(HWND hWnd, HINSTANCE hInst);
	void Draw(HDC hdc);	
	void LBTDown(POINT pt);
	bool UpDate();
	void Release();
	~Game_MG();
};

