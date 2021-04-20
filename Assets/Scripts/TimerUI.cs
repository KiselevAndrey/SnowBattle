using System;
using UnityEngine;
using UnityEngine.UI;

public class TimerUI : MonoBehaviour
{
    [Header("Стартовые данные")]
    public bool isTick;
    public bool isRunDown;

    [Header("Связывающие объекты")]
    [SerializeField] Text timerText;

    float _time;
    int _minuts;
    int _sec;
    int _millisec;

    float _triggerTime;     // когда сработает триггер
    bool _triggering;       // будет ли вообще триггерить
    bool _isTriggering;     // сработал ли уже триггер

    public static Action TimerIsZero;
    public static Action TimeTrigger;

    void Update()
    {
        if (!isTick || Time.timeScale == 0) return;

        _time += isRunDown ? -Time.deltaTime : Time.deltaTime;

        FloatToTime();

        if(_triggering && !_isTriggering)
        {
            if ((isRunDown && _time <= _triggerTime) || (!isRunDown && _time >= _triggerTime))
            {
                _isTriggering = true;
                TimeTrigger?.Invoke();
            }
        }

        if (isRunDown && _time <= 0)
        {
            TimerIsZero?.Invoke();
            isTick = false;
        }
    }

    #region Time
    void FloatToTime()
    {
        _minuts = (int)_time / 60;
        _sec = (int)_time % 60;
        _millisec = Math.Max(0, (int)(_time % 1 * 100));

        DrawTime();
    }

    void DrawTime()
    {
        string temp = IntToString(_minuts )+ " : ";
        temp += IntToString(_sec) + " : ";
        temp += IntToString(_millisec);

        timerText.text = temp;
    }

    string IntToString(int value)
    {
        if (value < 10) return "0" + value;
        else return value.ToString();
    }

    public float GetTime() => _time;
    public void SetTime(float time)
    {
        _time = time;
        FloatToTime();
    }
    #endregion

    public void SetTextColor(Color color)
    {
        timerText.color = color;
    }

    public void SetTimeTriggerAction(float time)
    {
        _triggering = true;
        _isTriggering = false;
        _triggerTime = time;
    }
}
