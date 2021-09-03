#pragma once
#include "vector2.h"


class GameObject
{
private:
	Vector2 pos;
	Vector2 velocity;

public:
	void UpdatePos()
	{
		pos += velocity;
	}

	// 수직 방향 가속도 업데이트.
	// 데카르트 좌표에 따라 가속도가 양수면 아래로 떨어진다.
	void UpdateVelocity(const float& accel, const float& deltaTime)
	{
		velocity.y = velocity.y + accel * deltaTime;
	}
};

// 질량으로 물체의 무게를 계산.
Vector2 Weight(const float& mass)
{
	Vector2 weight;

	weight.y = mass * GRAVITY;

	return weight;
}

// 물체가 경사면을 미끄러지는지 체크.
// 미끄러지면 true, 아니면 false
bool IsSlopeSlip(const float& angle, const float& weight, const float& coeffFric)
{
	// 경사면에서 물체에 작용하는 수직항력 계산.
	float normal = weight * cosf(angle * Deg2Rad);
	// 경사면에 평행하게 물체에 작용하는 힘 계산.
	float perpForce = weight * sinf(angle * Deg2Rad);
	// 물체가 정지해 있도록 하는 정지마찰력 계산.
	float staticFriction = coeffFric * normal;
	
	return (perpForce > staticFriction);
}

// 경사면에서 미끄러져 내려가는 가속도 계산.
float SlopeSlipAccel(const float& angle, const float& weight, const float& coeffFric, const float& mass)
{
	// 경사면에서 물체에 작용하는 수직항력 계산.
	float normal = weight * cosf(angle * Deg2Rad);
	// 경사면에 평행하게 물체에 작용하는 힘 계산.
	float perpForce = weight * sinf(angle * Deg2Rad);
	// 물체에 작용하는 운동마찰력 계산.
	float kineticFriction = coeffFric * normal;
	// 물체에 작용하는 힘 계산.
	float totalForce = perpForce - kineticFriction;
	// 물체의 가속도.
	return (totalForce / mass);
}

// 일(물체에 힘을 작용해 힘의 방향으로 물체를 이동시키는 것)의 양.
float Work(const float& force, const float& friction, const float& displacement)
{
	float netForce = force - friction;
	return displacement * netForce;
}

// 각을 고려하여 힘을 계산.
float AngleWork(const Polar& p, const float& friction, const float& displacement)
{
	float temp = cos(p.angle * Deg2Rad);
	// 힘의 수평 성분 계산.
	float horizForce = p.magnitude * temp;
	
	return Work(horizForce, friction, displacement);
}

// inital:물체에 작용된 힘, chage:외부의 요인으로 인하여 실제 작용한 힘.
float AngleWorkFromVector(const Polar& inital, const Polar& chage)
{
	// 극 좌표를 데카르트 좌표로 변경.
	auto force = Polar2Comp(inital);
	auto displacement = Polar2Comp(chage);

	// 변위의 정규화.
	auto displacementNormal = displacement.normalize();

	// 투영된 길이. 변위의 단위벡터와 힘의 내적.
	auto projectionLenth = DotProduct(force, displacementNormal);

	// 투영된 길이와 변위의 단위벡터의 곱.
	auto temp = displacementNormal * projectionLenth;
	// 일을 계산하기 위한 극좌표 형식으로 다시 변환.
	auto polarTemp = Comp2Polar(temp);

	return polarTemp.magnitude * chage.magnitude;
}

// 질량과 속력을 받아 운동 에너지 계산.
float KineticEnergy(const float& mass, const float& speed)
{
	return (mass * 0.5f) * (pow(speed, 2));
}

// 일-에너지 정리.
// 물체의 최종 속도를 계산.
float WorkEnergy(const float& force, const float& mass, const float& displacement, const float& velocityInital)
{
	// 마찰력이 존재하지 않는 상황을 가정.
	float work = Work(force, 0, displacement);
	float result = work / (mass * 0.5f) - KineticEnergy(mass, velocityInital);
	// 최종 속도.
	return sqrtf(result);
}

// 포텐셜 에너지를 줄 단위로 계산.
float PotentialEnergy(const float& mass, const float& height)
{
	return mass * GRAVITY * height;
}

// 축이 정렬된 상태의 반사 방향.
Vector2 AxisAlignedCollision(const Vector2& v, bool isHorizen)
{
	Vector2 temp = v;
	if (!isHorizen) temp.x *= -1;
	else temp.y *= -1;

	return temp;
}

// 축이 비정렬 상태의 반사.
// v1:반사면에 수직인 벡터, v2:입사벡터
Vector2 NonAxisAlignedCollision(const Vector2& v1, const Vector2& v2)
{
	// 벡터 정규화.
	auto normal = Normalize(v1);

	// 벡터 반전.
	auto revalsal = ScalarMultiply(v2, -1);

	// 내적으로 투영의 길이를 계산.
	auto projection = DotProduct(revalsal, normal);

	auto length = ScalarMultiply(normal, projection);

	// 반사 벡터를 구하기 위해서 2배로 곱.
	auto reflection = ScalarMultiply(length, 2);

	return SumVector(reflection, revalsal);
}