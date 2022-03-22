using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Agent : MonoBehaviour
{
    public float speed = 3.0f;
    public Game game;
    private Vector3Int trgTile;
    private bool turbo = false;
    private List<Vector3Int> path;

    public bool CanWalkOnFurniture;

    private float moveDistLeft = 0;
    private Vector3 trgPos;
    private Vector3 moveDir;

    public void SetTurbo(bool t)
    {
        turbo = t;
    }

    // Start is called before the first frame update
    void Start()
    {
        path = new List<Vector3Int>();
        Stop();
    }

    private List<Vector3Int> FindPath(Vector3Int end)
    {
        int bork = 10000;
        int w = game.map[0].Length;
        int h = game.map.Length;
        int[,] cost = new int[w, h];
        Vector3Int[,] comeFrom = new Vector3Int[w, h];
        bool[,] touched = new bool[w, h];
        for (int i=0;i<w;i++)
            for (int j=0;j<h;j++)
            {
                cost[i, j] = 100000;
                comeFrom[i, j] = Vector3Int.one;
                touched[i, j] = false;
            }
        Vector3Int o = GetGridPos();
        touched[o.x, o.y] = true;
        List<Vector3Int> toVisit = new List<Vector3Int>();
        toVisit.Add(o);
        cost[o.x, o.y] = 0;
        while (toVisit.Count > 0)
        {
            bork--;
            if (bork == 0)
                throw new System.Exception("AAA");
            Vector3Int here = toVisit[0];
            toVisit.RemoveAt(0);
            foreach (Vector3Int n in GetNeighbors(here.x, here.y, true))
            {
                if (cost[here.x, here.y] + n.z < cost[n.x, n.y])
                {
                    cost[n.x, n.y] = cost[here.x, here.y] + n.z;
                    comeFrom[n.x, n.y] = here;
                }
                if (!touched[n.x, n.y]) {
                    touched[n.x, n.y] = true;
                    toVisit.Add(n);
                }
            }
        }
        List<Vector3Int> result = new List<Vector3Int>();
        Vector3Int traverseBack = end;
        while(true)
        {
            bork--;
            if (bork == 0)
                throw new System.Exception("BBB");
            if (traverseBack == o)
                break;
            if (game.map[traverseBack.y][traverseBack.x] == Game.S && !CanWalkOnFurniture)
            {
                // do not add to path
            } else
            {
                result.Add(traverseBack);
            }
            traverseBack = comeFrom[traverseBack.x, traverseBack.y];
            if (traverseBack == Vector3Int.one)
                throw new System.Exception("path error " + o + " > " + end);
        }
        result.Reverse();
        return result;
    }

    public List<Vector3Int> GetNeighbors(int x, int y, bool allowClimbing)
    {
        List<Vector3Int> result = new List<Vector3Int>();
        AddIfCanWalk(result, x - 1, y - 1, 14, allowClimbing);
        AddIfCanWalk(result, x + 0, y - 1, 10, allowClimbing);
        AddIfCanWalk(result, x + 1, y - 1, 14, allowClimbing);
        AddIfCanWalk(result, x - 1, y + 0, 10, allowClimbing);
        AddIfCanWalk(result, x + 1, y + 0, 10, allowClimbing);
        AddIfCanWalk(result, x - 1, y + 1, 14, allowClimbing);
        AddIfCanWalk(result, x + 0, y + 1, 10, allowClimbing);
        AddIfCanWalk(result, x + 1, y + 1, 14, allowClimbing);
        return result;
    }

    private void AddIfCanWalk(List<Vector3Int> tgt, int x, int y, int cost, bool allowClimbing)
    {
        if (x < 0 || y < 0 || x >= game.map[0].Length || y >= game.map.Length)
            return;
        int tile = game.map[y][x];
        if (CanWalkOnFurniture && tile == Game.S)
            tgt.Add(new Vector3Int(x, y, cost));
        if (allowClimbing && !CanWalkOnFurniture && tile == Game.S)
            tgt.Add(new Vector3Int(x, y, cost * 10));
        if (tile == Game.F)
            tgt.Add(new Vector3Int(x, y, cost));
    }

    // Update is called once per frame
    void Update()
    {
        if (IsMoving())
        {
            float moveDist = Time.deltaTime * speed * (turbo ? 2f : 1f);
            transform.localPosition += moveDir * moveDist;
            moveDistLeft -= moveDist;
            if (moveDistLeft < 0.01)
            {
                moveDistLeft = 0;
                transform.localPosition = game.GridToWorld(trgTile);
            }
        } else
        {
            GoToNext();
        }
    }

    public Vector3Int GetGridPos()
    {
        return game.WorldToGrid(transform.position);
    }

    public void Stop()
    {
        Target(GetGridPos());
    }

    public bool IsMoving()
    {
        return moveDistLeft != 0;
    }

    public void Target(Vector3Int pos)
    {
        path = FindPath(pos);
        GoToNext();
    }
    
    private void GoToNext()
    {
        if (path.Count == 0)
            return;
        Vector3Int trgTile = path[0];
        path.RemoveAt(0);
        InternalTarget(trgTile);
    }

    public void InternalTarget(Vector3Int pos) {
        trgTile = pos;
        trgPos = game.GridToWorld(trgTile);
        Vector3 route = trgPos - transform.localPosition;
        moveDistLeft = route.magnitude;
        moveDir = route.normalized;
    }
}
