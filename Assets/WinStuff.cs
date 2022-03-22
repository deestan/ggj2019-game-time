using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class WinStuff : MonoBehaviour
{
    static int score;
    public Text WinText;
    public Text ScoreText;
    private float reactTime = 1f;

    public static void SetScore(int v) {
        score = v;
    }

    // Start is called before the first frame update
    void Start()
    {
        SetCamera16x9();
        SetWinText();
    }

    private void SetWinText()
    {
        ScoreText.text = "Game Time:\n" + score + " minutes";
        if (score < 5)
        {
            WinText.text = "You stare at your game collection for a few seconds before falling asleep.";
        }
        else if (score < 60)
        {
            WinText.text = "You manage to squeeze in a few minutes of light gaming before you go to bed.";
        }
        else if (score < 180)
        {
            WinText.text = "You manage to game for an hour or so before going to bed. Not bad.";
        } else
        {
            WinText.text = "Well done! You got several hours of gaming in before going to bed.";
        }
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
        reactTime -= Time.deltaTime;
        if (reactTime < 0)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Application.Quit();
                return;
            }
            if (Input.anyKeyDown)
            {
                SceneManager.LoadSceneAsync("MainMenu", LoadSceneMode.Single);
            }
        }
    }
}
