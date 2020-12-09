using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; //required for text class

public class GameController : MonoBehaviour
{
    public float timeLeft;
    public float health;
    public Text timeText;
    public Text targetText;
    public Text healthText;
    public Text levelOverText;
    public Text reloadText;
    public Text finalTimeText;
    public Text countdownText;
    public Button Level2Button;
    public Button RestartButton;
    public GameObject endGamePanel;
    
    AudioSource timeOverAudio;
    GameObject[] targets;

    // Start is called before the first frame update
    void Start()
    {
        //make cursor invisible and start countdown
        Cursor.visible = false;
        endGamePanel.SetActive(false);
        StartCoroutine(StartCountDown(4));
        timeOverAudio = gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        targets = GameObject.FindGameObjectsWithTag("Enemy");
        int targetNum = targets.Length; //Calculate number of targets left

        timeLeft -= Time.deltaTime;
        // if there is no time left, end the level
        if (timeLeft < 0 )
        {
            Debug.Log("game lost");
            Level2Button.gameObject.SetActive(false);
            EndGameUI();
            levelOverText.color = Color.red;
            levelOverText.text = "GAME OVER";
            finalTimeText.text = "You ran out of time";
        } else if(health < 1)
        {
            //if there is no health left, end the level
            Debug.Log("game lost");
            Level2Button.gameObject.SetActive(false);
            EndGameUI();
            levelOverText.color = Color.red;
            levelOverText.text = "GAME OVER";
            finalTimeText.text = "You died";
        }
        else if (targetNum.Equals(0))
        {
            //if there is no enemies left, end the level
            Debug.Log("Level Won!");
            RestartButton.gameObject.SetActive(false);
            levelOverText.color = Color.green;
            levelOverText.text = "LEVEL COMPLETE!";
            finalTimeText.text = "Congratulations! You won with " + timeLeft.ToString("0") + " seconds remaining";
            EndGameUI();
        }

        // format to a string with no decimal places
        timeText.text = "Time Left: " + timeLeft.ToString("0");
        targetText.text = "Targets Left: " + targetNum.ToString();
        healthText.text = "Health: " + health.ToString();

        // Change the colour of the health text depending on health
        switch (health)
        {
            case 100:
                healthText.color = Color.green;
                break;
            case 60:
                healthText.color = Color.yellow;
                break;
            case 30:
                healthText.color = Color.red;
                break;
        }

        //start countdown clock if less than 10 seconds left
        if(timeLeft < 10)
        {
            if (!timeOverAudio.isPlaying)
            {
                Debug.Log("10 seconds left");
                timeOverAudio.Play();
                timeText.color = Color.red;
            }
        }
    }

    public void TargetDestroyed(int timeBonus)
    {
        //give a time bonus when an object is destroyed
        timeLeft += timeBonus;
    }

    public void PlayerHit(float damage)
    {
        //remove player health when hit
        health -= damage;
    }
    IEnumerator StartCountDown(int countdownTime)
    {
        // Countdown at start of game
        Time.timeScale = 0.1f;
        Cursor.lockState = CursorLockMode.Locked;
        reloadText.gameObject.SetActive(false);
        targetText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        timeText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(true);
        float startEndTime = Time.realtimeSinceStartup + countdownTime;

        while(Time.realtimeSinceStartup < startEndTime)
        {
            float timeToDisplay = startEndTime - Time.realtimeSinceStartup;
            float seconds = Mathf.FloorToInt(timeToDisplay % 60);
            countdownText.text = seconds.ToString();
            yield return 0;
        }
        countdownText.text = "GO!!!!";
        yield return new WaitForSeconds(0.1f);
        countdownText.gameObject.SetActive(false);
        targetText.gameObject.SetActive(true);
        healthText.gameObject.SetActive(true);
        timeText.gameObject.SetActive(true );
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.None;
    }

    public void EndGameUI()
    {
        //removes game UI and activates End Game UI panel
        Time.timeScale = 0;
        Cursor.visible = true;
        timeOverAudio.Stop();
        endGamePanel.gameObject.SetActive(true);
        timeText.gameObject.SetActive(false);
        reloadText.gameObject.SetActive(false);
        healthText.gameObject.SetActive(false);
        countdownText.gameObject.SetActive(false);
        targetText.gameObject.SetActive(false);
    }
}
