using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cat : MonoBehaviour
{
    private Agent agent;
    private float sitTime;
    private float vomiting = 0;
    private bool canVomit = true;
    private bool checkPlant = false;
    private GameObject vomitText;
    public List<Plant> plants;
    private AudioSource plantBonk;

    private bool CanWalkOn(int x, int y)
    {
        int tile = agent.game.map[y][x];
        if (tile != Game.W)
            return true;
        return false;
    }

    public void StopVomiting()
    {
        canVomit = false;
    }

    // Start is called before the first frame update
    void Start()
    {
        plantBonk = GetComponent<AudioSource>();
        agent = GetComponent<Agent>();
        sitTime = 0;
        vomitText = transform.Find("vomitText").gameObject;
        GoRandom();
    }

    // Update is called once per frame
    void Update()
    {
        if (vomiting > 0)
        {
            vomiting -= Time.deltaTime;
            if (vomiting < 0) {
                vomitText.SetActive(false);
                GoRandom();
            }
            return;
        }
        if (!agent.IsMoving())
        {
            if (checkPlant && canVomit)
            {
                Vector3Int poz = agent.GetGridPos();
                foreach (Plant p in plants)
                {
                    if (poz == p.gridPos)
                    {
                        FindObjectOfType<Game>().FlipPlant(p);
                        plantBonk.Play();
                        GoRandom();
                        return;
                    }
                }
            }
            sitTime -= Time.deltaTime;
            if (sitTime < 0)
            {
                switch (Random.Range(0, 8))
                {
                    case 0:
                        Vomit();
                        break;
                    default:
                        GoRandom();
                        break;
                }
            }
        }
    }

    void Vomit()
    {
        if (!canVomit)
            return;
        agent.game.SpawnCatVomit(agent.GetGridPos());
        vomitText.SetActive(true);
        vomiting = 1.5f;
    }

    void GoRandom()
    {
        sitTime = Random.Range(1f, 5f);
        if (canVomit && Random.Range(0, 8) == 0)
        {
            int plantIdx = Random.Range(0, plants.Count);
            Plant plant = plants[plantIdx];
            agent.Target(plant.gridPos);
            checkPlant = true;
            return;
        }
        int x;
        int y;
        while (true)
        {
            x = Random.Range(0, agent.game.map[0].Length);
            y = Random.Range(0, agent.game.map.Length);
            if (CanWalkOn(x, y))
                break;
        }
        agent.Target(new Vector3Int(x, y, 0));
    }
}
