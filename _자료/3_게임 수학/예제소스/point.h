#pragma once

class Point
{
private:
	float m_fX, m_fY;

public:
	Point() {}
	Point(float x, float y) : m_fX(x), m_fY(y) {}

	Point operator+(Point& point)
	{
		return Point(this->m_fX + point.GetX(), this->m_fY + point.GetY());
	}

	float GetX() const { return m_fX; }
	float GetY() const { return m_fY; }
};