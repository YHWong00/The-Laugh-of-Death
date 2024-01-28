using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering;

public class TimeController : MonoBehaviour
{
    public TextMeshProUGUI timeDisplay;
    public TextMeshProUGUI dayDisplay;
    public Volume ppv;

    public float tick;
    public float seconds;
    public int mins;
    public int hours;
    public int days = 1;

    public bool activelights;
    public GameObject[] lights;
    public SpriteRenderer[] stars;

    public GameObject fasterIcon;//º”ÀŸÕº±Í

    public AudioSource timeControllerAS;
    public AudioClip chickenCrow;
    private bool isChickenCrowed;

    // Start is called before the first frame update
    void Start()
    {
        ppv = gameObject.GetComponent<Volume>();
              
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        CalcTime();
        DisplayTime();
        if (lights == null)
        {
            Debug.Log("The lights array is null");
        }

        if (hours >= 8 && hours < 17) 
        {
            GameController.instance.gameControllerAS.mute = true;
            PlayerControllerScript.PCS.speed = 0.01f;
            Time.timeScale = 4f;
            fasterIcon.SetActive(true);
            if(PlayerControllerScript.PCS.isInBuilding == false)
            {
                GameController.instance.EndGame();
            }
        }
        else
        {
            GameController.instance.gameControllerAS.mute = false;
            PlayerControllerScript.PCS.speed = PlayerControllerScript.PCS.oSpeed;
            Time.timeScale = 1f;
            fasterIcon.SetActive(false);
        }

    }

    public void CalcTime()
    {
        seconds += Time.fixedDeltaTime * tick;

        if (seconds >= 60)
        {
            seconds = 0;
            mins += 1;
        }

        if (mins >= 60)
        {
            mins = 0;
            hours += 1;
        }


        if (hours >= 24)
        {
            hours = 0;
            days += 1;
        }
        ControlPPV();
    }

    public void ControlPPV()
    {
        if (hours >= 0 && hours < 1)
        {
            isChickenCrowed = false;
        }
        if (hours >= 17 && hours < 18)
        {
            ppv.weight = (float)mins / 60;

            if (activelights == false)
            {
                if (mins > 45)
                {
                    for (int i = 0; i<lights.Length; i++)
                    {
                        lights[i].SetActive(true);
                    }
                }
                activelights = true;
                //Debug.Log(activelights);
            }
        }
        
        if (hours >= 5 && !timeControllerAS.isPlaying && !isChickenCrowed)
        {
            timeControllerAS.PlayOneShot(chickenCrow);
            isChickenCrowed = true;
        }
        if(hours >= 7 && hours < 8)
        {
            ppv.weight = 1 - (float)mins / 60;
            if (activelights == true)
            {
                if(mins > 45)
                {
                    for (int i = 0; i < lights.Length; i++)
                    {
                        lights[i].SetActive(false);
                    }
                    activelights = false;
                    Debug.Log(activelights);
                }
            }
        }
    }

    public void DisplayTime()
    {
        timeDisplay.text = string.Format("{0:00}:{1:00}", hours, mins);
        dayDisplay.text = "Day:" + days;
    }
}
