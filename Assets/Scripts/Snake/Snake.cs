using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snake : MonoBehaviour
{
    private Vector2 gridPos;
    private Vector2Int moveDirection;

    private float moveTime;
    private float moveTimeMax;

    private SnakeGameManager gameManager;

    public void Setup(SnakeGameManager gameManager)
    {
        this.gameManager = gameManager;
    }
    // Start is called before the first frame update
    void Awake()
    {
        gridPos = new Vector2(.5f,.5f);
        moveTime = 1f;
        moveTimeMax = moveTime;
        moveDirection = Vector2Int.right;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            moveDirection.x = 0;
            moveDirection.y = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            moveDirection.x = 0;
            moveDirection.y = -1;
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            moveDirection.x = -1;
            moveDirection.y = 0;
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            moveDirection.x = 1;
            moveDirection.y = 0;
        }

        moveTime += Time.deltaTime;
        if (moveTime >= moveTimeMax)
        {
            gridPos += moveDirection;
            moveTime -= moveTimeMax;
            transform.position = new Vector3(gridPos.x, gridPos.y, 0f);

            gameManager.OnSnakeMoved(gridPos);
        }
    }
}
