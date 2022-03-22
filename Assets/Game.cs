using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Game : MonoBehaviour
{
    public static int F = 0; // floor
    public static int W = 1; // wall
    public static int S = 3; // stuff and furniture
    private float childBounceTimeout = 0f;
    public Grid grid;
    public Clock clock;
    public Agent player;
    public Agent childA;
    public Agent childB;
    private float winning = 0f;
    public int[][] map = {
        new int[] { W, W, W, W, W, W, W, W, W, W, W, W },
        new int[] { W, S, S, S, S, F, F, W, F, F, F, W },
        new int[] { W, S, F, F, F, F, F, W, F, F, F, W },
        new int[] { W, S, F, F, F, S, F, W, F, F, F, W },
        new int[] { W, F, F, F, F, S, F, W, F, W, W, W },
        new int[] { W, F, F, F, F, F, F, F, F, F, F, F },
        new int[] { W, W, W, W, W, F, F, F, F, F, F, W },
        new int[] { W, S, F, F, W, F, F, S, F, F, F, W },
        new int[] { W, S, F, F, F, F, F, S, F, F, F, W },
        new int[] { W, F, F, S, W, F, F, F, F, F, F, W },
        new int[] { W, F, F, S, W, F, S, F, F, F, F, W },
        new int[] { W, W, W, W, W, W, W, W, W, W, W, W }
    };
    private List<Objective> goals;
    public Vector3Int gameChair;
    public Vector3Int coffeeMakingPosition;
    public Vector3Int brushTeethPosition;
    public Vector3Int catToiletPosition;
    public Vector3Int tvPosition;
    public Vector3Int tvPosition2;
    public Canvas objectivesCanvas;
    public Transform vomitPrototype;
    public Transform spriteLayer;
    public GameObject objectivePrototype;

    // Start is called before the first frame update
    void Start()
    {
        goals = new List<Objective>();
        SetCamera16x9();
        AddGoal(typeof(ChildABrushTeeth));
        AddGoal(typeof(ChildBBrushTeeth));
        AddGoal(typeof(ChildAGoToBed));
        AddGoal(typeof(ChildBGoToBed));
        AddGoal(typeof(CleanCatshit));
        AddGoal(typeof(MakeCoffee));
        AddGoal(typeof(GoGame));
    }

    public void Win()
    {
        winning = 1f;
        WinStuff.SetScore(clock.MinutesOfGame());
        clock.Stop();
        GetComponentInChildren<AudioSource>().volume *= 0.5f;
    }

    public void RemoveObjective(Objective o)
    {
        goals.Remove(o);
        Destroy(o.gameObject);
        for (int idx=0; idx<goals.Count; idx++)
        {
            Vector3 pos = new Vector3(objectivePrototype.transform.localPosition.x, objectivePrototype.transform.localPosition.y - idx, 0);
            goals[idx].GetComponent<RectTransform>().localPosition = pos;
        }
    }

    public void SpawnCatVomit(Vector3Int pos)
    {
        AddGoal(typeof(CleanCatVomit));
    }

    public void ObjectiveCompleted()
    {
        int unCompleted = 0;
        foreach (Objective goal in goals)
        {
            if (!goal.IsCompleted() && goal.GetComponent<GoGame>() == null)
                unCompleted += 1;
        }
        if (unCompleted == 0)
        {
            objectivesCanvas.GetComponentInChildren<GoGame>().GetComponent<Objective>().CanComplete();
            FindObjectOfType<Cat>().StopVomiting();
        }
    }


    private void AddGoal(System.Type o)
    {
        int idx = goals.Count;
        GameObject newObjectiveDisplay = Instantiate(objectivePrototype, objectivesCanvas.transform);
        newObjectiveDisplay.GetComponent<Objective>().game = this;
        Vector3 pos = new Vector3(objectivePrototype.transform.localPosition.x, objectivePrototype.transform.localPosition.y - idx, 0);
        newObjectiveDisplay.GetComponent<RectTransform>().localPosition = pos;
        newObjectiveDisplay.AddComponent(o);
        newObjectiveDisplay.SetActive(true);
        Objective objective = newObjectiveDisplay.GetComponent<Objective>();
        goals.Add(objective);
    }

    private void SetCamera16x9()
    {
        // set the desired aspect ratio (the values in this example are
        // hard-coded for 16:9, but you could make them into public
        // variables instead so you can set them at design time)
        float targetaspect = 16.0f / 9.0f;

        // determine the game window's current aspect ratio
        float windowaspect = (float)Screen.width / (float)Screen.height;

        // current viewport height should be scaled by this amount
        float scaleheight = windowaspect / targetaspect;

        // obtain camera component so we can modify its viewport
        Camera camera = GetComponent<Camera>();

        // if scaled height is less than current height, add letterbox
        if (scaleheight < 1.0f)
        {
            Rect rect = camera.rect;

            rect.width = 1.0f;
            rect.height = scaleheight;
            rect.x = 0;
            rect.y = (1.0f - scaleheight) / 2.0f;

            camera.rect = rect;
        }
        else // add pillarbox
        {
            float scalewidth = 1.0f / scaleheight;

            Rect rect = camera.rect;

            rect.width = scalewidth;
            rect.height = 1.0f;
            rect.x = (1.0f - scalewidth) / 2.0f;
            rect.y = 0;

            camera.rect = rect;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            return;
        }
        if (winning != 0)
        {
            winning -= Time.deltaTime;
            if (winning < 0 && winning != -10000f)
            {
                winning = -10000f;
                SceneManager.LoadSceneAsync("WinScene", LoadSceneMode.Single);
            }
            return;
        }
        if (childBounceTimeout <= 0)
        {
            childBounceTimeout = 1.0f;
            if (Mathf.Abs((childA.transform.position - childB.transform.position).magnitude) < 0.8)
            {
                Agent bounced = (Random.Range(0, 2) == 0) ? childA : childB;
                Vector3Int pos = bounced.GetGridPos();
                Child bouncedC = bounced.GetComponent<Child>();
                var tgts = bounced.GetNeighbors(pos.x, pos.y, false);
                int tgtI = Random.Range(0, tgts.Count);
                Vector3Int tgtFinal = tgts[tgtI];
                bounced.Stop();
                bounced.InternalTarget(tgtFinal);
                bouncedC.Unhappy();
            }
        }
        else {
            childBounceTimeout -= Time.deltaTime;
        }
        if (Input.GetMouseButtonDown(0))
        {
            OnPointerClick();
        }
    }

    public void FlipPlant(Plant p)
    {
        if (p.IsFlipped())
            return;
        p.Flip();
        AddGoal(typeof(RestorePlant));
    }

    public Vector3Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int gridPosition = grid.WorldToCell(worldPos);
        return new Vector3Int(gridPosition.x + 6, 5 - gridPosition.y, 0);
    }

    public Vector3 GridToWorld(Vector3Int gridPos)
    {
        return new Vector3(gridPos.x - 5.5f, 5.5f - gridPos.y, 0);
    }
    
    private void OnPointerClick()
    {
        Vector2 worldPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3Int mapPosition = WorldToGrid(worldPoint);
        if (mapPosition.x >= 0 && mapPosition.x < map[0].Length && mapPosition.y >= 0 && mapPosition.y < map.Length)
        {
            int tile = map[mapPosition.y][mapPosition.x];
            player.Target(mapPosition);
        }
    }
}
