using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Melee : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OpenCollider() //播放动画中途开启
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = true;

    }

    public void CloseCollider()
    {
        gameObject.GetComponent<PolygonCollider2D>().enabled = false;
    }

    public void EndMelee()
    {
        gameObject.SetActive(false);
    }
}
