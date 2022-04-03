using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Manager : MonoBehaviour
{
    float time = 20f;
    [SerializeField]
    private TMP_Text timer;

    [SerializeField]
    private TMP_Text scoreBoard;
    private int curScore;

    private int[] tierScores = { 250, 600, 1200, 2000, 3000, 5000, 7500, 12000, 20000 };
    private int curTier = 0;

    public static int enemyCount = 0;
    public static PlayerController globalPlayer;

    float timermultiplier = 1f;

    [SerializeField]
    private ManagerChannel managerChannel;
    [SerializeField]
    private TheMaw maw;

    [SerializeField]
    private GameObject[] spawners;
    [SerializeField]
    private GameObject endPanel;

    public static bool alertstate = false;
    [SerializeField]
    private AudioSource alertsound;

    private bool gameRunning = false;

    private void Awake()
    {
        scoreBoard.text = "Score: " + 0;
        

        managerChannel.onGameEnd += OnGameEnd;
    }

    public void AddToScore(int amount)
    {
        curScore += amount;
        scoreBoard.text = "Score: " + curScore;
        if(curScore > tierScores[curTier])
        {
            managerChannel.NotifyTierUpgrade(++curTier);
            Debug.Log("Upgraded Tier!");
            timermultiplier += 0.2f;
        }
    }
    private void Update()
    {
        if(gameRunning) UpdateTimer();
        if (Input.GetButtonDown("Cancel")) Application.Quit();
    }

    private void UpdateTimer()
    {
        time -= Time.deltaTime * timermultiplier;
        string timeString = time.ToString("00.00").Replace(',', ':');

        if (time < 0f && !maw.unleashed)
        {
            maw.StartUnleash();
            maw.unleashed = true;
        }

        if(time < 10f && !alertstate)
        {
            alertstate = true;
            alertsound.Play();
        }
        else if(time > 10f && alertstate)
        {
            alertstate = false;
            alertsound.Pause();
        }

        timer.text = timeString;
    }

    public void AddTime(float amount)
    {
        time = Mathf.Min(99.99f, time + amount);
    }

    private void OnGameEnd()
    {
        endPanel.SetActive(true);
        foreach(GameObject spawner in spawners)
        {
            spawner.SetActive(false);
        }
        gameRunning = false;
    }


    [SerializeField]
    private GameObject GameWorld;
    [SerializeField]
    private GameObject MainMenu;
    [SerializeField]
    private PointCarrier[] leftovers;


    public void StartGame()
    {
        GameWorld.SetActive(true);
        MainMenu.SetActive(false);

        globalPlayer = FindObjectOfType<PlayerController>();

        managerChannel.NotifyTierUpgrade(0);

        foreach (GameObject spawner in spawners)
        {
            spawner.SetActive(true);
        }



        gameRunning = true;

        globalPlayer.enabled = true;
    }

    public void BackToMainMenu()
    {
        leftovers = FindObjectsOfType<PointCarrier>();

        for(int i = leftovers.Length - 1; i >= 0; i--)
        {
            Destroy(leftovers[i].gameObject);
        }

        alertsound.Stop();

        maw.GetComponent<BoxCollider2D>().offset = new Vector2(0f, -2.5f);

        GameWorld.SetActive(false);
        MainMenu.SetActive(true);
        endPanel.SetActive(false);
        AddToScore(-curScore);
        time = 20f;
        timermultiplier = 1f;

        globalPlayer.transform.position = Vector3.zero;
        maw.unleashed = false;
        maw.transform.position = new Vector3(0f, 8.3f, 0f);

        globalPlayer.ResetAbilities();
    }
}
