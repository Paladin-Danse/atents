#include "Game_MG.h"
#include<time.h>
Game_MG* Game_MG::m_pThis = NULL;

Game_MG::Game_MG()
{
	m_eState = GAME_STATE_WAIT;
	showCount = 0;
}

void Game_MG::Init(HWND hWnd, HINSTANCE hInst)
{
	srand((unsigned)time(NULL));
	m_pRes_MG = new Res_MG();

	HDC hdc = GetDC(hWnd);
	m_pRes_MG->Init(hdc, hInst);
	ReleaseDC(hWnd, hdc);

	int arr[20];
	for (int i = 0; i < 20; i++)
	{
		arr[i] = i % 10;
	}

	for (int i = 0; i < 100; i++)
	{
		int randA = rand() % 20;
		int randB = rand() % 20;
		int temp = 0;

		temp = arr[randA];
		arr[randA] = arr[randB];
		arr[randB] = temp;
	}
	int count = 0;
	for (int j = 1; j < 5; j++)
	{
		for (int i = 1; i < 6; i++)
		{
			Card* p_Card = new Card();
			p_Card->Init(m_pRes_MG, arr[count], (i % 10) * (WIDTH/2) + 10 * (i % 10), j * (HEIGHT/2) + 10 * j, hWnd);
			vec_Card.push_back(p_Card);
			count++;
		}
	}
}

bool Game_MG::UpDate()
{
	if(m_eState == GAME_STATE_SHOW)
	{
		showCount++;
		if (showCount == 10)
		{
			m_pCardONE->CloseCard();
			m_pCardTWO->CloseCard();
			m_pCardONE = NULL;
			m_pCardTWO = NULL;
			showCount = 0;

			m_eState = GAME_STATE_WAIT;
			return true;
		}
	}
	return false;
}
void Game_MG::LBTDown(POINT pt)
{
	for (auto iter = vec_Card.begin(); iter != vec_Card.end(); iter++)
	{
		if ((*iter)->isChak(pt))
		{
			if (m_eState == GAME_STATE_WAIT)
			{
				if ((*iter)->isOpen())
					continue;
				(*iter)->OpenCard();
				m_pCardONE = (*iter);
				m_eState = GAME_STATE_ONE;
			}
			else if (m_eState == GAME_STATE_ONE)
			{
				if ((*iter)->isOpen())
					continue;
				(*iter)->OpenCard();
				m_pCardTWO = (*iter);

				if (m_pCardONE->isSameCard(m_pCardTWO->GetIndex()))
				{
					m_eState = GAME_STATE_WAIT;
					m_pCardONE = NULL;
					m_pCardTWO = NULL;
				}
				else
					m_eState = GAME_STATE_SHOW;
			}
		}
	}
}

void Game_MG::Draw(HDC hdc)
{
	for (auto iter = vec_Card.begin(); iter != vec_Card.end(); iter++)
	{
		(*iter)->Draw(hdc);
	}
}


void Game_MG::Release()
{
	SAFE_DELETE(m_pThis);
}

Game_MG::~Game_MG()
{
	SAFE_DELETE(m_pRes_MG);

}
