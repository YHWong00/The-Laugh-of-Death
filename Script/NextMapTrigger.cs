using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NextMapTrigger : MonoBehaviour
{
    private bool isActivated;
    [SerializeField] private SpriteRenderer sr;
    private Vector3 currentMapPos;
    public enum Direction { T, B, L, R };
    [SerializeField] private Direction triggerDir;

    private void Awake()
    {
        currentMapPos = gameObject.transform.parent.transform.position;
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!isActivated && collision.CompareTag("Player"))
        {
            switch (triggerDir)
            {
                case Direction.T:
                    Instantiate(GameController.instance.map, currentMapPos + Vector3.up * 8, Quaternion.identity);
                    GameController.instance.lastEntranceDir = GameController.Direction.T;
                    break;
                case Direction.B:
                    Instantiate(GameController.instance.map, currentMapPos + Vector3.down * 8, Quaternion.identity);
                    GameController.instance.lastEntranceDir = GameController.Direction.B;
                    break;
                case Direction.L:
                    Instantiate(GameController.instance.map, currentMapPos + Vector3.left * 14, Quaternion.identity);
                    GameController.instance.lastEntranceDir = GameController.Direction.L;
                    break;
                case Direction.R:
                    Instantiate(GameController.instance.map, currentMapPos + Vector3.right * 14, Quaternion.identity);
                    GameController.instance.lastEntranceDir = GameController.Direction.R;
                    break;
            }
            SetActive();
        }
    }
    public void SetActive()
    {
        isActivated = true;
        sr.color = new Color(0f, 1f, 1f, 0.1f);
    }
}
