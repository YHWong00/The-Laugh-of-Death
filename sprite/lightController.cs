using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class lightController : MonoBehaviour
{
    public GameObject lightObj;
    public GameObject Gvol;
    public bool lightact;
    public TimeController script;
    public bool getB;

    private void Start()
    {
        script = gameObject.GetComponent<TimeController>();
    }

    // Update is called once per frame
    void Update()
    {
        lightact = lightObj.activeSelf;
        getB = script.activelights;

        if (lightact == true && getB == false)
        {
            lightObj.SetActive(true);
        }
        if (lightact == false && getB == true)
        {
            lightObj.SetActive(false);
        }
    }
}
