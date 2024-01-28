using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public List<BoxCollider2D> buildingsInThisMap = new List<BoxCollider2D>();
    private Vector2[] cornerBuildingsPos = new Vector2[4];
    private Vector2[] edgeBuildingsPos = new Vector2[8];
    public List<GameObject> cornerBuildings = new List<GameObject>();
    public List<GameObject> edgeBuildings = new List<GameObject>();
    public static int npcSpawnAmount = 5;

    private void Awake()
    {
        Invoke("DestroyTriggerToEntrance", 0.05f);
    }
    private void Start()
    {
        SetBuildingsPos();
        GenerateBuildings();
        DestroyRandomBuildings();
        DestroyBuildingsToEntrance();
    }
    private void Update()
    {
        //if (Input.GetMouseButtonDown(1))
        //{
        //    foreach (var item in buildingsInThisMap)
        //    {
        //        print(item.name);
        //    }
        //}
    }
    
    private void DestroyTriggerToEntrance()
    {
        if (GameController.instance.lastEntranceDir != GameController.Direction.N)
        {
            switch (GameController.instance.lastEntranceDir)
            {
                case GameController.Direction.T:
                    GameObject trigger = gameObject.transform.Find("NextMapB").gameObject;
                    trigger.GetComponent<NextMapTrigger>().SetActive();
                    break;
                case GameController.Direction.B:
                    trigger = gameObject.transform.Find("NextMapT").gameObject;
                    trigger.GetComponent<NextMapTrigger>().SetActive();
                    break;
                case GameController.Direction.L:
                    trigger = gameObject.transform.Find("NextMapR").gameObject;
                    trigger.GetComponent<NextMapTrigger>().SetActive();
                    break;
                case GameController.Direction.R:
                    trigger = gameObject.transform.Find("NextMapL").gameObject;
                    trigger.GetComponent<NextMapTrigger>().SetActive();
                    break;
            }
        }
    }
    private void SetBuildingsPos()
    {
        cornerBuildingsPos[0] = new Vector2(-0.375f, 0.375f); // Top Left Corner
        cornerBuildingsPos[1] = new Vector2(0.375f, 0.375f); // Top Right Corner
        cornerBuildingsPos[2] = new Vector2(-0.375f, -0.375f); // Bot Left Corner
        cornerBuildingsPos[3] = new Vector2(0.375f, -0.375f); // Bot Right Corner

        edgeBuildingsPos[0] = new Vector2(-0.125f, 0.375f); // Top Left Edge
        edgeBuildingsPos[1] = new Vector2(0.125f, 0.375f); // Top Right Edge
        edgeBuildingsPos[2] = new Vector2(-0.125f, -0.375f); // Bot Left Edge
        edgeBuildingsPos[3] = new Vector2(0.125f, -0.375f); // Bot Right Edge
        edgeBuildingsPos[4] = new Vector2(-0.375f, 0.125f); // Left Top Edge
        edgeBuildingsPos[5] = new Vector2(-0.375f, -0.125f); // Left Bot Edge
        edgeBuildingsPos[6] = new Vector2(0.375f, 0.125f); // Right Top Edge
        edgeBuildingsPos[7] = new Vector2(0.375f, -0.125f); // Right Bot Edge
    }
    private void GenerateBuildings()
    {
        for (int i = 0; i < cornerBuildingsPos.Length; i++)
        {
            GameObject building = Instantiate(GameController.instance.building);
            building.transform.parent = transform;
            building.transform.localPosition = cornerBuildingsPos[i];
            building.transform.localScale = new Vector2(0.2285714f, 0.225f);
            cornerBuildings.Add(building);
        }
        for (int i = 0; i < edgeBuildingsPos.Length; i++)
        {
            GameObject building = Instantiate(GameController.instance.building);
            building.transform.parent = transform;
            building.transform.localPosition = edgeBuildingsPos[i];
            building.transform.localScale = new Vector2(0.2285714f, 0.225f);
            edgeBuildings.Add(building);
        }
    }
    private void DestroyRandomBuildings()
    {
        if (GameController.instance.lastEntranceDir != GameController.Direction.N)
        {
            int destroyAmount = Random.Range(3, 5);
            List<int> destroyIndexes = GameController.instance.GenerateDistinctNums(destroyAmount, edgeBuildings.Count);
            foreach (int destroyIndex in destroyIndexes)
            {
                Destroy(edgeBuildings[destroyIndex]);
            }
        }
    }
    private void DestroyBuildingsToEntrance()
    {
        switch (GameController.instance.lastEntranceDir)
        {
            case GameController.Direction.N:
                for (int i = 0; i < edgeBuildings.Count; i++)
                {
                    Destroy(edgeBuildings[i]);
                }
                break;
            case GameController.Direction.T:
                if (edgeBuildings[2] != null) Destroy(edgeBuildings[2]);
                if (edgeBuildings[3] != null) Destroy(edgeBuildings[3]);
                break;
            case GameController.Direction.B:
                if (edgeBuildings[0] != null) Destroy(edgeBuildings[0]);
                if (edgeBuildings[1] != null) Destroy(edgeBuildings[1]);
                break;
            case GameController.Direction.L:
                if (edgeBuildings[6] != null) Destroy(edgeBuildings[6]);
                if (edgeBuildings[7] != null) Destroy(edgeBuildings[7]);
                break;
            case GameController.Direction.R:
                if (edgeBuildings[4] != null) Destroy(edgeBuildings[4]);
                if (edgeBuildings[5] != null) Destroy(edgeBuildings[5]);
                break;
        }
        Invoke("CollectExistingBuildings", 0.05f);
    }
    private void CollectExistingBuildings()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).CompareTag("Building"))
            {
                buildingsInThisMap.Add(gameObject.transform.GetChild(i).GetComponent<BoxCollider2D>());
            }
        }
        Invoke("SpawnNpcs", 0.05f);
    }
    public void SpawnNpcs()
    {
        for (int i = 0; i < npcSpawnAmount; i++)
        {
            float ranPosX = Random.Range(-3.5f, 3.5f);
            float ranPosY = Random.Range(-2, 2);
            Vector3 ranPos = new Vector3(ranPosX, ranPosY, 0);
            int ranNpc = Random.Range(0, GameController.instance.npcs.Count);
            GameObject npc = Instantiate(GameController.instance.npcs[ranNpc]);
            npc.transform.localPosition = transform.position + ranPos;
        }
    }
}
