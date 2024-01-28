using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResultCanvasController : MonoBehaviour
{
    public Image imgDefeat;
    public Text txtDaySurvived;
    public Text txtScore;
    public Text txtNumDaySurvived;
    public Text txtNumScore;
    public Image imgLine;
    public Image btnBackToTitle;
    public Image imgBack;
    public Text txtBackToTitle;

    private float alphaValue = 0;
    private void Start()
    {
        txtNumDaySurvived.text = GameController.day.ToString();
        txtNumScore.text = GameController.score.ToString();
    }
    private void Update()
    {
        imgDefeat.color = new Color(1, 1, 1, alphaValue);
        txtDaySurvived.color = new Color(1, 1, 1, alphaValue);
        txtScore.color = new Color(1, 1, 1, alphaValue);
        txtNumDaySurvived.color = new Color(1, 1, 1, alphaValue);
        txtNumScore.color = new Color(1, 1, 1, alphaValue);
        imgLine.color = new Color(1, 1, 1, alphaValue);
        btnBackToTitle.color = new Color(1, 1, 1, alphaValue);
        imgBack.color = new Color(0.07f, 0.04f, 0.13f, alphaValue);
        txtBackToTitle.color = new Color(1, 1, 1, alphaValue);

        alphaValue += 0.3f * Time.deltaTime;
    }
}
