using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Transform[,] graph = new Transform[100, 100];
    int[,] map = new int[100, 100];
    public int row;
    public int col;

    Queue<Vector2> q = new Queue<Vector2>();
    public Vector2 startPos;
    public Vector2 goalPos;
    Vector2 current;

    int[] directionX = { -1, 1, 0, 0 };
    int[] directionY = { 0, 0, -1, 1 };

    Stack<Vector2> hashStack = new Stack<Vector2>();
    Hashtable hash = new Hashtable();
    Vector2 before;
    Vector2 poppedStack;
    float nextX;
    float nextY;

    // Use this for initialization
    void Start()
    {
        for (int i = 0; i < row; i++)
        {
            for (int j = 0; j < col; j++)
            {
                graph[i, j] = GameObject.Find("GameObject (" + i + ")").transform.Find("Quad (" + j + ")");
                print("graph[" + i + ", " + j + "] : " + graph[i, j].position);
                if (graph[i, j].gameObject.activeSelf)
                {
                    map[i, j] = 1;
                }
                else
                    map[i, j] = 0;
            }
        }
    }
    IEnumerator Move()
    {
        while (hashStack.Count != 0)
        {
            poppedStack = hashStack.Pop();
            print(poppedStack);
            transform.position = new Vector3(poppedStack.x, poppedStack.y, 0);
            yield return new WaitForSeconds(0.5f);
        }
    }
    IEnumerator BFS(Vector2 startPos, Vector2 goalPos)
    {
        current = startPos;
        q.Enqueue(current);
        map[0, 0] = 1;

        while (q.Count != 0)
        {
            current = q.Dequeue();


            if (current == goalPos)
            {
                graph[(int)current.y, (int)current.x].gameObject.GetComponent<MeshRenderer>().material.color = new Color(2, 34, 54);
                hashStack.Push(current);
                before = (Vector2)hash[current];
                graph[(int)before.y, (int)before.x].gameObject.GetComponent<MeshRenderer>().material.color = new Color(2, 34, 54);
                hashStack.Push(before);
                while (true)
                {
                    before = (Vector2)hash[before];
                    graph[(int)before.y, (int)before.x].gameObject.GetComponent<MeshRenderer>().material.color = new Color(2, 34, 54);
                    hashStack.Push(before);

                    if (before == startPos)
                        break;
                }
                print("goal found" + map[(int)current.y, (int)current.x]);
                yield break;
            }

            for (int k = 0; k < 4; k++)
            {
                nextX = current.x + directionX[k];
                nextY = current.y + directionY[k];
                Vector2 nextPos = new Vector2(nextX, nextY);

                if (nextX < 0 || nextX > col - 1 || nextY < 0 || nextY > row - 1)
                    continue;
                if (map[(int)nextY, (int)nextX] == 1)
                {
                    q.Enqueue(nextPos);
                    map[(int)nextY, (int)nextX] = map[(int)current.y, (int)current.x] + 1;
                    hash.Add(nextPos, current);

                }
            }
        }
    }

    public void Click()
    {
        StartCoroutine(BFS(startPos, goalPos));
        StartCoroutine(Move());
    }

    // Update is called once per frame
    void Update()
    {

    }
}
