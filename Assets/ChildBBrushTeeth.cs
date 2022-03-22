using UnityEngine;

class ChildBBrushTeeth : MonoBehaviour
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
        objective.SetTitle("Captain brush teeth");
        objective.CanComplete();
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsInPosition())
        {
            timeLeft -= Time.deltaTime;
            game.childB.GetComponent<Child>().brushText.SetActive(true);
        }
        else
        {
            timeLeft += Time.deltaTime * 0.05f;
            game.childB.GetComponent<Child>().brushText.SetActive(false);
        }
        if (timeLeft > timeTotal)
            timeLeft = timeTotal;
        objective.SetCompletion((timeTotal - timeLeft) / timeTotal);
        if (timeLeft <= 0)
        {
            objective.Complete();
            game.childB.GetComponent<Child>().brushText.SetActive(false);
            game.childB.GetComponent<Child>().DidBrush();
            game.ObjectiveCompleted();
        }
    }


    private bool IsInPosition()
    {
        if (game.childB.IsMoving())
            return false;
        if (game.childB.GetComponent<Child>().IsUnhappy())
            return false;
        if (!game.childB.GetGridPos().Equals(game.brushTeethPosition))
            return false;
        return true;
    }
}
