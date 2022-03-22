using UnityEngine;

class ChildABrushTeeth : MonoBehaviour
{
    private Objective objective;
    private Game game;
    private float timeTotal = 20.0f;
    private float timeLeft;

    void Start()
    {
        timeLeft = timeTotal;
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.SetTitle("Stripes brush teeth");
        objective.CanComplete();
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition())
        {
            timeLeft -= Time.deltaTime;
            game.childA.GetComponent<Child>().brushText.SetActive(true);
        }
        else
        {
            timeLeft += Time.deltaTime * 0.05f;
            game.childA.GetComponent<Child>().brushText.SetActive(false);
        }
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            game.childA.GetComponent<Child>().brushText.SetActive(false);
            game.childA.GetComponent<Child>().DidBrush();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.childA.IsMoving())
            return false;
        if (game.childA.GetComponent<Child>().IsUnhappy())
            return false;
        if (!game.childA.GetGridPos().Equals(game.brushTeethPosition))
            return false;
        return true;
    }
}
