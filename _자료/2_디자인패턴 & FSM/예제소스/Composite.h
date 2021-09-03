#pragma once
#include <list>
#include <string>

// InpuComponent.h
class GameObject;
class InputComponent
{
public:
	virtual ~InputComponent() {}
	virtual void Update(GameObject&) abstract;
};
class PlayerInputComponent : public InputComponent
{
public:
	virtual void Update(GameObject& go) override
	{
		// 유저의 input을 확인하여 처리.
	}
};
class EnemyInputComponent : public InputComponent
{
public:
	virtual void Update(GameObject& go) override
	{
		// AI가 알아서 조종한다.
	}
};

//PhysicsComponent.h
class GameObject;
class PhysicsComponent
{
public:
	virtual ~PhysicsComponent() {}
	virtual void Update(GameObject&) abstract;
};

//PhysicsComponent.h
class GameObject;
class GraphicsComponent
{
public:
	virtual ~GraphicsComponent() {}
	virtual void Update(GameObject&) abstract;
};

//GameObject.h
class InputComponent;
class PhysicsComponent;
class GraphicsComponent;
class GameObject
{
private:
	int m_nX, m_nY;
	InputComponent* m_pInput;
	PhysicsComponent* m_pPhysics;
	GraphicsComponent* m_pGraphics;

public:
	GameObject(InputComponent* input, PhysicsComponent* physics, GraphicsComponent* graphics)
		: m_nX(0), m_nY(0), m_pInput(input), m_pPhysics(physics), m_pGraphics(graphics) {}
	virtual ~GameObject() {}

	int GetX() const { return this->m_nX; }
	int GetY() const { return this->m_nY; }
	void Upadte()
	{
		if(NULL != m_pInput) m_pInput->Update(*this);
		if(NULL != m_pPhysics) m_pPhysics->Update(*this);
		if(NULL != m_pGraphics) m_pGraphics->Update(*this);
	}
};

