#pragma once
#include "vector2.h"


// 단위 시간당 속도에 의한 이동 거리.
float Displacement(const float& velocity, const float& time)
{
	return velocity * time;
}

// 시작점과 도착점, 이동한 시간의 평균 속도.
float AvgVelocity(const float& start, const float& end, const float& time)
{
	return (end - start) / time;
}

// 단위 시간당 가속도를 구한다.
// 시작 속도, 현재 속도, 시작 부터 현재까지 걸린 시간.
float AccelerationSeconds(const float& startVel, const float& endVel, const float& time)
{
	return (endVel - startVel) / time;
}