using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnakeGameManager : MonoBehaviour
{
    private Grid<PathNode> grid;
    private bool gameover = true;

    private int width = 20;
    private int height = 20;

    private Vector2 foodPos;

    private float updateTime = 1f;
    private float updateTimeMax;

    [SerializeField] private Snake snake;
    // Start is called before the first frame update
    void Start()
    {
        grid = new Grid<PathNode>(width, height, 1f, new Vector3(-(width/2), -(height/2), 0f),
             (Grid<PathNode> g, int x, int y) => new PathNode(g, x, y));
        updateTimeMax = updateTime;
    }

    // Update is called once per frame
    void Update()
    {
        updateTime += Time.deltaTime;
        if (updateTime >= updateTimeMax)
        {
            SpawnFood();
            updateTime -= updateTimeMax;
        }
    }

    public (int, int) GetGridSize()
    {
        return (width, height);
    }

    private void SpawnFood()
    {
        int w = width / 2;
        int h = height / 2;
        foodPos = new Vector2(Random.Range(-w,w), Random.Range(-h,h));

        GameObject foodGameObject = new GameObject("Food", typeof(SpriteRenderer));
        foodGameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load("Sprites/Food", typeof(Sprite)) as Sprite;
        foodGameObject.GetComponent<SpriteRenderer>().color = Color.red;
        foodGameObject.transform.position = new Vector3(foodPos.x + .5f, foodPos.y + .5f);
    }
}
