using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Объекты")]
    [SerializeField] List<Rigidbody> objects;

    [Header("Спавн объектов")]
    [SerializeField] List<GameObject> spawns;

    [Header("Переменные")]
    [SerializeField] float matchDuration;
    [SerializeField] float startEndingMusic;
    [SerializeField] int snowmenCount = 2;

    [Header("Доп объекты")]
    [SerializeField] Animator anim;
    [SerializeField] TimerUI timer;
    [SerializeField] Text endGameScoreText;

    [Header("Музыка")]
    [SerializeField] MusicManager musicManager;
    [SerializeField] AudioClip startGameMusic;
    [SerializeField] AudioClip goalMusic;
    [SerializeField] AudioClip prepareEndingGameMusic;

    List<SnowmanManager> snowmen = new List<SnowmanManager>();

    int _startsGameCount;

    #region Awake Start OnDestroy
    private void Awake()
    {
        Gate.GoalMe += Goal;
        Puck.TheyDontTouchMe += SpawnPuck;
        Savior.RespawnObj += RespawnObject;
        TimerUI.TimerIsZero += EndGame;
        TimerUI.TimeTrigger += PlayEndingMusic;
    }
    private void OnDestroy()
    {
        Gate.GoalMe -= Goal;
        Puck.TheyDontTouchMe -= SpawnPuck;
        Savior.RespawnObj -= RespawnObject;
        TimerUI.TimerIsZero -= EndGame;
    }

    void Start()
    {
        //StartGame();
        RandomRotationPlayerSpawns();

        for (int i = 1; i < objects.Count; i++)
        {
            if (objects[i].TryGetComponent(out SnowmanManager snowman))
            {
                snowmen.Add(snowman);
                snowman.snowmanNumber = i;
                snowman.Setup();
            }
        }

        timer.SetTextColor(snowmen[snowmenCount - 1].color);


    }
    #endregion

    #region Game
    public void NewMatch()
    {
        timer.SetTime(matchDuration);
        timer.SetTimeTriggerAction(startEndingMusic);

        foreach (SnowmanManager snowman in snowmen)
        {
            snowman.ResetScore();
        }

        PrepareGame(); 
        musicManager.NextMusic(startGameMusic);
    }

    public void PrepareGame()
    {
        _startsGameCount++;
        anim.SetTrigger("PrepareGame");
        timer.isTick = false;

        for (int i = 0; i < objects.Count; i++)
        {
            RespawnObject(i);
            objects[i].isKinematic = true;
        }
    }

    // запуск из анимации
    void StartGame()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].isKinematic = false;
        }

        timer.isTick = true;
    }

    void EndGame()
    {
        _startsGameCount--;

        for (int i = 0; i < objects.Count; i++)
        {
            objects[i].isKinematic = true;
        }

        string textOfWin = "TIME OVER\n\n";
        string winner = "";
        int maxScore = 0;

        for (int i = 0; i < snowmen.Count; i++)
        {
            textOfWin += snowmen[i].coloredSnowmanText + " score: " + snowmen[i].GetScoreSTR();
            textOfWin += "\n";
            if(snowmen[i].GetScore() > maxScore)
            {
                maxScore = snowmen[i].GetScore();
                winner = snowmen[i].coloredSnowmanText;
            }
            else if(snowmen[i].GetScore() == maxScore)
            {
                winner += " " + snowmen[i].coloredSnowmanText;
            }
        }

        textOfWin += "\n" + winner + " WIN";
        endGameScoreText.text = textOfWin;

        anim.SetTrigger("Score");
    }
    #endregion

    #region Spawn    
    void RespawnObject(int index)
    {
        objects[index].transform.position = spawns[index].transform.position;
        objects[index].transform.rotation = spawns[index].transform.rotation;
        objects[index].velocity = Vector3.zero;
        objects[index].angularVelocity = Vector3.zero;
    }

    public void RandomRotationPlayerSpawns()
    {
        for (int i = 0; i < objects.Count; i++)
        {
            RespawnObject(i);

            Vector3 rotation = spawns[i].transform.rotation.eulerAngles;
            rotation.x = UnityEngine.Random.value * 360;
            rotation.z = UnityEngine.Random.value * 360;
            objects[i].transform.rotation = Quaternion.Euler(rotation);

            objects[i].isKinematic = false;
        }
    }
    #endregion

    #region Функции для Actions
    void Goal(int whoLose)
    {
        if (_startsGameCount <= 0)
        {
            SpawnPuck();
            return;
        }
        PrepareGame();

        musicManager.NextMusic(goalMusic);
    }

    void SpawnPuck()
    {
        objects[0].transform.position = spawns[0].transform.position;
        objects[0].velocity = Vector3.zero;
    }

    void PlayEndingMusic() => musicManager.NextMusic(prepareEndingGameMusic);
    #endregion

    public void QuitAplication()
    {
        Application.Quit();
    }
}
