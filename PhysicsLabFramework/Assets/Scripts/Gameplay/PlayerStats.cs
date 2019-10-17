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
    public static bool didPlayerWin;

    public Image playerHealthBar;
    public Text scoreText;
    public Text resultText;
    public GameObject ResultPanel;

    public GameObject shipRearThruster;
    public Animator rearThrustAnim;
    public ParticleSystem leftThrustAnim;
    public ParticleSystem rightThrustAnim;

    // The player will take damage when called
    public static void TakeDamage()
    {
        // If the player has health to spare, knock some off
        if (playerHealth > 0f)
        {
            // Player takes damage equal to .01% of max health
            playerHealth -= PLAYER_HEALTH_MAX * 0.01f;
            // Player loses some score upon taking damage
            RemoveScore();
        }
        // Otherwise, the player is dead
        else
            isPlayerAlive = false;
    }

    private static void RemoveScore()
    {
        playerScore -= 10.0f;
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
        // The player has the maximum possible score
        playerScore = 1000.0f;
        // The player has not won the game yet
        didPlayerWin = false;
    }

    // Initialize external variables
    private void Start()
    {
        // Set the player health bar  fill to max
        playerHealthBar.fillAmount = 1f;
        // Display the player's score on-screen
        scoreText.text = "Score: " + playerScore;
        // The result screen should not be visible initially
        if (ResultPanel.activeSelf)
            ResultPanel.SetActive(false);
    }

    private void Update()
    {
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            shipRearThruster.SetActive(true);
            rearThrustAnim.SetBool("PlayRearThrusterAnimation", true);
        }
        else
        {
            rearThrustAnim.SetBool("PlayRearThrusterAnimation", false);
            shipRearThruster.SetActive(false);
        }

        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            rightThrustAnim.Play();
        }
        else
        {
            rightThrustAnim.Stop();
        }

        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            leftThrustAnim.Play();
        }
        else
        {
            leftThrustAnim.Stop();
        }
    }

    private void LateUpdate()
    {
        if (isPlayerAlive && !didPlayerWin)
        {
            // Update the health bar based on the player's current health
            playerHealthBar.fillAmount = playerHealth / PLAYER_HEALTH_MAX;

            // Update the player health bar color to red in critical condition
            if (playerHealth <= PLAYER_HEALTH_MAX * 0.40f && playerHealthBar.color != Color.red)
            {
                playerHealthBar.color = Color.red;
            }

            scoreText.text = "Score: " + playerScore;
        }
        else if(!isPlayerAlive && !didPlayerWin)
        {
            Camera.main.transform.position = new Vector3(0.0f, 0.0f, 10.0f);
            resultText.text = "You Lose.";
            resultText.color = Color.red;
            ResultPanel.SetActive(true);
        }
        else if (didPlayerWin)
        {
            Camera.main.transform.position = new Vector3(0.0f, 0.0f, 10.0f);

            resultText.text = "You Win!";
            resultText.color = Color.blue;
            ResultPanel.SetActive(true);
        }
    }
}
