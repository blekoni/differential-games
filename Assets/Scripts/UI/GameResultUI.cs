using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameResultUI : MonoBehaviour
{
    [SerializeField] private Text m_text;

    // Start is called before the first frame update
    void Start()
    {
        SetVisible(false);
    }

    public void ShowResult(GameManager.GameResult gameResult)
    {
        m_text.text = CreateResultString(gameResult);
        SetVisible(true);
    }

    public void HideResult()
    {
        SetVisible(false);
    }

    private void SetVisible(bool flag)
    {
        this.gameObject.SetActive(flag);
    }

    private string CreateResultString(GameManager.GameResult gameResult)
    {
        var gamRes = GameManager.Get().GetGameResult();

        string resultStr;
        resultStr = "Game has been finished \n";
        resultStr += " Reason: ";
        if (gamRes.result == GameManager.FinishGame.AllEscapersDestroyed)
        {
            resultStr += "all escapers were destroyed \n";
        }
        else if (gamRes.result == GameManager.FinishGame.EscaperOutOfZone)
        {
            resultStr += "all escapers are in escape zonev \n";
        }
        else if (gamRes.result == GameManager.FinishGame.OutOfTime)
        {
            resultStr += "no time left \n";
        }
        else if (gamRes.result == GameManager.FinishGame.StoppedByUser)
        {
            resultStr += "stopped by user \n";
        }

        //resultStr += ("Game time is: " + timerText.text + "\n ");
        resultStr += (" Dist. completed by pursuer: " + gamRes.distanceCompByPursuer.ToString() + "\n");
        resultStr += (" Dist. completed by escaper: " + gamRes.distanceCompByEscaper.ToString() + "\n");

        return resultStr;
    }
}
