#pragma once
#include<iostream>
#include"Look.h"
#include"Skill.h"
#include"Stat.h"
using namespace std;
class Character
{
	Skill sk;
	Look look;
	Stat st;
public:
	Character();
	~Character();
};

