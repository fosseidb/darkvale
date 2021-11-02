using UnityEngine;
using System.Linq;

public class SearchForResource : IState
{

    private readonly ZombieWoodcutter _woodcutter;

    public SearchForResource(ZombieWoodcutter woodcutter)
    {
        _woodcutter = woodcutter;
    }

    public void OnEnter(){    }

    public void OnExit(){    }

    public void Tick()
    {
        _woodcutter.Target = ChooseOneOfTheNearestResources(5);
    }

    private AncientTree ChooseOneOfTheNearestResources(int nearestNumber)
    {
        AncientTree temp = Object.FindObjectsOfType<AncientTree>()
            .OrderBy(t => Vector3.Distance(_woodcutter.transform.position, t.transform.position))
            .Where(t => t.IsDepleted == false)
            .Take(nearestNumber)
            .OrderBy(t => Random.Range(0, int.MaxValue))
            .FirstOrDefault();
        return temp;

    }
}
