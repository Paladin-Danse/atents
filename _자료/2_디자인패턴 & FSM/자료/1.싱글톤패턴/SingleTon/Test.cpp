#include "Test.h"



Test::Test()
{
	cout << IntManager::GetInstace()->GetSize();
	IntManager::GetInstace()->ShowList();
}


Test::~Test()
{
}
