using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestructionWon : IState
{
    GameManager _gameManager;
    public DestructionWon(GameManager gamemanager)
    {
        _gameManager = gamemanager;
    }
    public void OnEnter()
    {
        //stop gameplay

        // change camera to game over camera

        //activate and populate game recap UI
    }

    public void OnExit()
    {
        //Quit map
    }

    public void Tick()
    {
        throw new System.NotImplementedException();
    }

}
