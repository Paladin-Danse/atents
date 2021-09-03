#pragma once
#include "point.h"

//두 점을 잇는 직선의 기울기.
float GetSlope(const Point& p1, const Point& p2)
{
	return (p2.GetY() - p1.GetY()) / (p2.GetX() - p1.GetX());
}

// 이 기울기와 직교하는 직선의 기울기.
float PerpSlope(float slope)
{
	return -1 / slope;
}
// 두 직선은 직교인가?
bool IsPerp(float slope1, float slope2)
{
	return (-1 == slope1 * slope2);
}
// 두 직선의 교차점.
Point lineIntersect(const Point& p1, const float& slope1, const Point& p2, const float& slope2)
{
	auto x = (slope1 * p1.GetX() - slope2 * p2.GetX() + p2.GetY() - p1.GetY()) / (slope1 - slope2);
	auto y = slope1 * (x - p1.GetX()) + p1.GetY();
	// or y = slope2 * (x - p2.GetX()) + p2.GetY();

	return Point(x, y);
}