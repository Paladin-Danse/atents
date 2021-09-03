#include"IntManager.h"
#include"Test.h"
#include<Windows.h>

void main()
{
	int Select;
	while (true)
	{
		system("cls");
		cout << "자료 갯수 :" << IntManager::GetInstace()->GetSize() << endl;
		cout << "===메뉴===" << endl;
		cout << "1.값 추가" << endl;
		cout << "2.보기" << endl;
		cout << "3.종료" << endl;
		cin >> Select;
		switch (Select)
		{
		case 1:
			IntManager::GetInstace()->Insert();
			break;
		case 2:
			IntManager::GetInstace()->ShowList();
			system("pause");
			break;
		case 3:
		{
			Test t;
			delete IntManager::GetInstace();
			return;
		}
		}
	}
}

//클래스가 어디서든 쓸 수 있게 만드는 목적
//하나 이상 못만들게하려고
//클래스 자체가 객체화 되는 것.