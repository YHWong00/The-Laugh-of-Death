using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Building : MonoBehaviour
{
    public SpriteRenderer sr;
    private List<GameObject> edgeBuildings;

    private void Start()
    {
        int roofSprite = Random.Range(0, GameController.instance.roofs.Count);
        sr.sprite = GameController.instance.roofs[roofSprite];
        Invoke("DestroyWall", 0.05f);
    }
    private void DestroyWall()
    {
        edgeBuildings = transform.parent.gameObject.GetComponent<Map>().edgeBuildings;
        if (gameObject == edgeBuildings[0] || gameObject == edgeBuildings[1])
        {
            Destroy(transform.Find("WallB").gameObject);
        }
        else if (gameObject == edgeBuildings[2] || gameObject == edgeBuildings[3])
        {
            Destroy(transform.Find("WallT").gameObject);
        }
        else if (gameObject == edgeBuildings[4] || gameObject == edgeBuildings[5])
        {
            Destroy(transform.Find("WallR").gameObject);
        }
        else if (gameObject == edgeBuildings[5] || gameObject == edgeBuildings[6])
        {
            Destroy(transform.Find("WallL").gameObject);
        }
    }
}
