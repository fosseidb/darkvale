using UnityEngine;

public class ForestStage : IState
{
    private GameManager _gameManager;

    public float stage2DurationInMinutes = 15f;

    public ForestStage(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    public void OnEnter()
    {
        
        //Set start time
        _gameManager.StageTimer = stage2DurationInMinutes * 60f;

        // Notify UI
        _gameManager.InvokeNewStage(2);

        // setting map scenery to stage 2
        _gameManager.SetLiveStage(2);
    }

    public void OnExit()
    {
        _gameManager.SetPostStage(2);
    }

    public void Tick()
    {
        _gameManager.StageTimer -= Time.deltaTime;
        _gameManager.MasterTimer += Time.deltaTime;
        _gameManager.UpdateUITimers(2);
    }
}
