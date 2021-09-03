#pragma once
#include<cmath>

#pragma intrinsic (sin, cos, sqrtf, asin, cosf, sinf)

#define PI		3.14159265f
#define Rad2Deg	57.29577951f
#define Deg2Rad	0.017453293f
#define GRAVITY 9.81f

// 데카르트 좌표.
class Vector2
{
public:
	float x, y;

	Vector2() : x(0), y(0) {}
	Vector2(float x, float y) : x(x), y(y) {}

	Vector2 operator+(const Vector2& v)
	{
		Vector2 temp;
		temp.x = this->x + v.x;
		temp.y = this->y + v.y;

		return temp;
	}
	Vector2 operator-(const Vector2& v)
	{
		Vector2 temp;
		temp.x = this->x - v.x;
		temp.y = this->y - v.y;

		return temp;
	}
	Vector2 operator*(const float& value)
	{
		Vector2 temp;
		temp.x = this->x * value;
		temp.y = this->y * value;

		return temp;
	}
	void operator+=(const Vector2& v)
	{
		this->x = this->x + v.x;
		this->y = this->y + v.y;
	}
	Vector2 normalize()
	{
		float magnitude = sqrtf(pow(x, 2) + pow(y, 2));
		return Vector2(x / magnitude, y / magnitude);
	}
};
// 극 좌표.
class Polar
{
public:
	float magnitude, angle;

	Polar() : magnitude(0), angle(0) {}
};

Vector2 Normalize(const Vector2& v)
{
	float magnitude = sqrtf(pow(v.x, 2) + pow(v.y, 2));
	return Vector2(v.x / magnitude, v.y / magnitude);
}

Vector2 ScalarMultiply(const Vector2& v, const float& value)
{
	Vector2 temp;
	temp.x = v.x * value;
	temp.y = v.y * value;

	return temp;
}

Vector2 SumVector(const Vector2& v1, const Vector2& v2)
{
	Vector2 temp;
	temp.x = v1.x + v2.x;
	temp.y = v1.y + v2.y;

	return temp;
}

// 벡터의 내적.
float DotProduct(const Vector2& v1, const Vector2& v2)
{
	return v1.x * v2.x + v1.y * v2.y;
}

Vector2 Polar2Comp(const Polar& p)
{
	Vector2 temp;
	temp.x = p.magnitude * cos(p.angle * Deg2Rad);
	temp.y = p.magnitude * -sin(p.angle * Deg2Rad);

	return temp;
}

Polar Comp2Polar(const Vector2& v)
{
	Polar temp;
	// 피타고라스의 정리를 이용하여 크기를 계산.
	temp.magnitude = sqrtf(pow(v.x, 2) + pow(v.y, 2));
	// divide-by-zero(분모가 0) 에러를 막기위한 작업.
	if (0 == temp.magnitude) return temp;
	// 각을 계산. 제1, 4분면의 각을 리턴하는 asin을 사용.
	temp.angle = Rad2Deg * asin(v.y / temp.magnitude);
	// 각이 제2, 3분면에 있을 경우.
	if (0 > v.x) temp.angle += 180;
	// 제4분면에 있을 경우.
	else if (0 < v.x && 0 > v.y) temp.angle += 360;

	return temp;
}