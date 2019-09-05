// Fill out your copyright notice in the Description page of Project Settings.

#pragma once

#include "CoreMinimal.h"
#include "GameFramework/Actor.h"
#include "Particle2D.generated.h"

UCLASS()
class PHYSLABFRAMEWORK_API AParticle2D : public AActor
{
	GENERATED_BODY()
	
public:	
	// Sets default values for this actor's properties
	AParticle2D();

protected:
	// Called when the game starts or when spawned
	virtual void BeginPlay() override;

public:	
	// Called every frame
	virtual void Tick(float DeltaTime) override;

	virtual void UpdatePositionExplicitEuler(float dt);

	UPROPERTY(EditAnywhere)
		FVector position; 
	UPROPERTY(EditAnywhere)
		FVector velocity; 
	UPROPERTY(EditAnywhere)
		FVector acceleration; 
		enum IntegrationType
	{
		EULER,
		KINEMATIC
	};
	// float rotation, angularVelocity, angularAcceleration;
};