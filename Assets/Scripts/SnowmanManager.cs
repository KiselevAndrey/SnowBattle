using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SnowmanManager : MonoBehaviour
{
    [Header("Цвет и объекты связанные с ним")]
    public Color color;
    [HideInInspector] public Color anticolor;
    [SerializeField] Text scoreText;
    [SerializeField] List<Light> lights;
    [SerializeField] Material wall;
    [SerializeField] Material gate;

    [Header("Осн параметры")]
    public string snowmanName = "SNOWMAN";

    //[Header("Осн объекты")]
    //public GameObject spawn;

    [HideInInspector] public int snowmanNumber;
    [HideInInspector] public string coloredSnowmanText;

    int _score;

    #region Awake Destroy Start
    private void Awake()
    {
        Gate.GoalMe += Goal;
        color.a = 1;
        anticolor.a = 0.5f;
    }
    private void OnDestroy()
    {
        Gate.GoalMe -= Goal;
    }

    private void Start()
    {
        
    }
    #endregion

    #region Setup
    public void Setup()
    {
        SetupColor();
    }

    public void SetupColor()
    {
        ChangeColoredName();

        for (int i = 0; i < lights.Count; i++)
            lights[i].color = color;

        wall.color = color;
        gate.color = color;
        scoreText.color = color;
    }
    #endregion

    #region Score
    void Goal(int whoLose)
    {
        if (whoLose == snowmanNumber) return;

        _score++;
        scoreText.text = _score.ToString();
    }

    public void ResetScore()
    {
        _score = 0;
        scoreText.text = "0";
    }

    public int GetScore() => _score;
    public string GetScoreSTR() => scoreText.text;
    #endregion

    #region Changes
    void ChangeColoredName() => coloredSnowmanText = "<color=#" + ColorUtility.ToHtmlStringRGB(color) + "> " + snowmanName + "</color>";

    public void ChangeColor(string firstColorLeter, float value)
    {
        switch (firstColorLeter.ToLower())
        {
            case "r":
                color.r = value;
                anticolor.r = 1 - value;
                SetupColor();
                break;

            case "g":
                color.g = value;
                anticolor.g = 1 - value;
                SetupColor();
                break;

            case "b":
                color.b = value;
                anticolor.b = 1 - value;
                SetupColor();
                break;
        }
    }

    public void ChangeName(string newName)
    {
        snowmanName = newName;
        ChangeColoredName();
    }
    #endregion
}
