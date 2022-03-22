using UnityEngine;

class GoGame : MonoBehaviour
{
    private Objective objective;
    private Game game;

    void Start()
    {
        game = FindObjectOfType<Game>();
        objective = GetComponent<Objective>();
        objective.SetTitle("Sit down to Game");
    }

    void Update()
    {
        if (objective.IsCompleted())
            return;
        if (IsSatisfied())
        {
            objective.Complete();
            game.Win();
        }
    }

    private bool IsSatisfied()
    {
        if (!objective.IsCanComplete())
            return false;
        if (game.player.IsMoving())
            return false;
        if (!game.player.GetGridPos().Equals(game.gameChair))
            return false;
        return true;
    }
}
