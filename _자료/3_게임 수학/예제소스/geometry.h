#pragma once
#include "point.h"
#include <cmath>

#define PI		3.14159265f
#define Rad2Deg	57.29577951f
#define Deg2Rad	0.017453293f

// 내장 함수를 오버헤드 없이 호출하여 더 빠르게 사용이 가능하다.
#pragma intrinsic (sqrt, pow, atan, sin, cos)

class Circle
{
public:
	Point center;
	float radius;
};

// 두 점 사이의 거리.
float Distance2D(const Point& p1, const Point& p2)
{
	return ((float)sqrt(pow(p2.GetX() - p1.GetX(), 2) + pow(p2.GetY() - p1.GetY(), 2)));
}

// 두 점 사이의 중점.
Point MidPoint2D(const Point& p1, const Point& p2)
{
	auto x = (p1.GetX() + p2.GetX()) * 0.5f;
	auto y = (p1.GetY() + p2.GetY()) * 0.5f;

	return Point(x, y);
}

bool CircleCollision(const Circle& c1, const Circle& c2)
{
	return (Distance2D(c1.center, c2.center) <= (c1.radius + c2.radius));
}

// 두 점 사이의 각도(Degree)
float CalculatorAngle2D(const Point& p1, const Point& p2)
{
	//각도 계산.
	float angle = (float)atan((p2.GetY() - p1.GetY()) / (p2.GetX() - p1.GetX())) * Rad2Deg;
	// 데카르트좌표계 기준, 제1사분면(x,y가 양수)에 있는 각일 때.
	if (p2.GetY() < p1.GetY() && p2.GetX() > p1.GetX()) return angle;
	// 제2(x 음수, y 양수), 3(x, y 음수)사분면에 있는 각일 때.
	else if ((p2.GetY() < p1.GetY() && p2.GetX() < p1.GetX()) ||
		(p2.GetY() > p1.GetY() && p2.GetX() < p1.GetX())) return angle + 180;
	// 제4사분면(x 양수, y 음수)
	else return angle + 360;
}

Point Direction2DWithRad(const float& radian)
{
	auto x = cos(radian);
	auto y = -sin(radian);

	return Point(x, y);
}
Point Direction2DWithDeg(const float& degree)
{
	return Direction2DWithRad(degree * Deg2Rad);
}