using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerStats : MonoBehaviour
{
    private const float PLAYER_HEALTH_MAX = 100.0F;

    public static float playerHealth;
    public static bool isPlayerAlive;
    public static float playerScore;

    public Image playerHealthBar;
    public Text scoreText;

    // The player will take damage when called
    public static void TakeDamage()
    {
        // If the player has health to spare, knock some off
        if(playerHealth > 0f)
        {
            // Player takes damage equal to 1% of max health
            playerHealth -= PLAYER_HEALTH_MAX * 0.1f;
        }
        // Otherwise, the player is dead
        else
            isPlayerAlive = false;
    }

    // Add to the player's score depending on
    // size of asteroid killed
    public static void AddScore(float amountToAdd)
    {
        playerScore += amountToAdd;
    }

    // Initialize local variables
    private void Awake()
    {
        // The player is alive and has max health
        isPlayerAlive = true;
        playerHealth = PLAYER_HEALTH_MAX;
        // The player has no existing score
        playerScore = 0.0f;
    }

    // Initialize external variables
    private void Start()
    {
        // Set the player health bar  fill to max
        playerHealthBar.fillAmount = 1f;
        // Display the player's score on-screen
        scoreText.text = "Score: " + playerScore;
    }

    private void LateUpdate() 
    {
        // Update the health bar based on the player's current health
        playerHealthBar.fillAmount = playerHealth / PLAYER_HEALTH_MAX;
        
        // Update the player health bar color to red in critical condition
        if(playerHealth <= PLAYER_HEALTH_MAX * 0.40f && playerHealthBar.color != Color.red)
        {
            playerHealthBar.color = Color.red;
        }

        scoreText.text = "Score: " + playerScore;
    }
}
