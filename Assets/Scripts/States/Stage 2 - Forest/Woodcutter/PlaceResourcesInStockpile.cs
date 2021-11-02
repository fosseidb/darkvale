using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceResourcesInStockpile : IState
{
    private readonly ZombieWoodcutter _woodcutter;
    private readonly Animator _animator;

    public PlaceResourcesInStockpile(ZombieWoodcutter woodcutter, Animator animator)
    {
        _woodcutter = woodcutter;
        _animator = animator;
    }

    public void OnEnter()
    {
    }

    public void OnExit()
    {
    }

    public void Tick()
    {
        if (_woodcutter.DropOffWood())
            _woodcutter.Stockpile.IncrementPile();
    }
}
