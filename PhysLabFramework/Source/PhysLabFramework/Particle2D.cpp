// Fill out your copyright notice in the Description page of Project Settings.

#include "Particle2D.h"

FVector2D position, velocity, acceleration;
float rotation, angularVelocity, angularAccel;


void UpdatePositionExplicitEuler(float dt)
{
	position += velocity * dt;

	velocity += acceleration * dt;
}

// Sets default values for this component's properties
UParticle2D::UParticle2D()
{
	// Set this component to be initialized when the game starts, and to be ticked every frame.  You can turn these features
	// off to improve performance if you don't need them.
	PrimaryComponentTick.bCanEverTick = true;

	// ...
}


// Called when the game starts
void UParticle2D::BeginPlay()
{
	Super::BeginPlay();

	// ...
	
}


// Called every frame
void UParticle2D::TickComponent(float DeltaTime, ELevelTick TickType, FActorComponentTickFunction* ThisTickFunction)
{
	Super::TickComponent(DeltaTime, TickType, ThisTickFunction);

	// ...
	float dt = DeltaTime;
	UpdatePositionExplicitEuler(dt);

	Transform
}

