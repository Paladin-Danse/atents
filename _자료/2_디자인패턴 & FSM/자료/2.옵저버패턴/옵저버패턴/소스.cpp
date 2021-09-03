#include "Data.h"
#include"DataGraph.h"
#include"DataTable.h"

void main()
{
	Data* data = new Data;
	DataTable* DT = new DataTable;
	DataGraph* DG = new DataGraph;

	cout << "[갱신 1]" << endl;
	data->AddObserver(DT);
	data->setData(6, 2, 3);
	data->NotifyObserver();



	cout << "\n[갱신 2]" << endl;
	data->AddObserver(DG);
	data->setData(10, 12, 7);
	data->NotifyObserver();

	delete data;
	delete DT;
	delete DG;
}