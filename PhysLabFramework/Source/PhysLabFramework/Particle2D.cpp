// Fill out your copyright notice in the Description page of Project Settings.


#include "Particle2D.h"

// Sets default values
AParticle2D::AParticle2D()
{
 	// Set this actor to call Tick() every frame.  You can turn this off to improve performance if you don't need it.
	PrimaryActorTick.bCanEverTick = true;

}

// Called when the game starts or when spawned
void AParticle2D::BeginPlay()
{
	Super::BeginPlay();
	
}

// Called every frame
void AParticle2D::Tick(float DeltaTime)
{
	Super::Tick(DeltaTime);

	UpdatePositionExplicitEuler(DeltaTime);
	this->SetActorLocation(FVector(position.X, position.Y, 300));

	acceleration.X = -sin(GStartTime);
}

void AParticle2D::UpdatePositionExplicitEuler(float dt)
{
	position += velocity * dt;

	velocity += acceleration * dt;
}
