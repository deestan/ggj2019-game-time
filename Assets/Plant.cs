using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plant : MonoBehaviour
{
    private bool leveraged;
    public Transform sprite;
    private Game game;
    public Vector3Int gridPos;
    public bool IsTrashcan;

    private void SetLook()
    {
        if (leveraged)
        {
            sprite.rotation = Quaternion.Euler(0, 0, -90f);
            sprite.localPosition = new Vector3(0.2f, -0.1f, 0f);
        } else
        {
            sprite.rotation = Quaternion.Euler(0, 0, 0);
            sprite.localPosition = new Vector3(0, 0, 0);
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        game = FindObjectOfType<Game>();
    }

    public bool IsFlipped()
    {
        return leveraged;
    }

    public void Flip()
    {
        leveraged = true;
        SetLook();
    }

    public void Restore()
    {
        leveraged = false;
        SetLook();
    }

    // Update is called once per frame
    void Update()
    {
    }
}
