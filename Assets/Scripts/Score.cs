using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [SerializeField] Text[] scoreText;

    int[] scores = new int[2];

    #region Awake Destroy
    private void Awake()
    {
        Gate.GoalMe += Goal;
    }
    private void OnDestroy()
    {
        Gate.GoalMe -= Goal;
    }
    #endregion

    void Goal(int whoLose)
    {
        int whoWinIndex = (whoLose == 1) ? 1 : 0;
        scores[whoWinIndex]++;
        scoreText[whoWinIndex].text = scores[whoWinIndex].ToString();
    }
}

