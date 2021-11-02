using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderWon : IState
{
    GameManager _gameManager;
    public OrderWon(GameManager gameManager)
    {
        _gameManager = gameManager;
    }
    public void OnEnter()
    {
        throw new System.NotImplementedException();
    }

    public void OnExit()
    {
        throw new System.NotImplementedException();
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }

}
