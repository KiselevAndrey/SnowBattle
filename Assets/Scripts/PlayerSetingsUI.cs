using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerSetingsUI : MonoBehaviour
{
    [SerializeField] SnowmanManager snowman;

    [Header("UI")]
    [SerializeField] List<Slider> colorScliders;
    [SerializeField] List<Text> textsForColored;
    [SerializeField] List<Image> imagesForAnticolored;
    [SerializeField] Text playerName;

    static Action<Color> ChangeAnticolor;   // подписан сам на себя чтобы изменять цвет камеры
    /// <summary> Передается Average Color двух игроков </summary>
    static public Action<Color> ChangeAverageColor;

    #region Start Awake OnDestroy
    private void Start()
    {
        LoadSnowmanParam();
        SetColor();
    }

    private void Awake()
    {
        ChangeAnticolor += SetAvarageColor;
        LoadSnowmanParam();
    }

    private void OnDestroy()
    {
        ChangeAnticolor -= SetAvarageColor;
    }
    #endregion

    #region Color
    public void ChangeColorRed(float value)
    {
        snowman.ChangeColor("r", value);
        SetColor();
    }
    public void ChangeColorGreen(float value)
    {
        snowman.ChangeColor("g", value);
        SetColor();
    }
    public void ChangeColorBlue(float value)
    {
        snowman.ChangeColor("b", value);
        SetColor();
    }

    void SetColor()
    {
        for (int i = 0; i < textsForColored.Count; i++)
            textsForColored[i].color = snowman.color;

        for (int i = 0; i < imagesForAnticolored.Count; i++)
            imagesForAnticolored[i].color = snowman.anticolor;

        ChangeAnticolor(snowman.anticolor);
    }

    void SetAvarageColor(Color anticolor)
    {
        if (anticolor == snowman.anticolor) return;

        Color newColor = Color.Lerp(snowman.color, anticolor, 0.5f);

        ChangeAverageColor?.Invoke(newColor);
    }
    #endregion

    void LoadSnowmanParam()
    {
        colorScliders[0].value = snowman.color.r;
        colorScliders[1].value = snowman.color.g;
        colorScliders[2].value = snowman.color.b;

        SetPlayerName();
    }

    #region PlayerName
    void SetPlayerName()
    {
        playerName.text = snowman.snowmanName;
    }

    public void SetPlayerName(string newName)
    {
        snowman.ChangeName(newName);
        SetPlayerName();
    }
    #endregion
}
