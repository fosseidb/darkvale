using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class AncientTree : MonoBehaviour
{

    private int _woodLeft;
    private int _totalWood = 10;

    [SerializeField] private float shudderDuration, shudderMagnitude;
    public bool IsDepleted => _woodLeft <= 0;

    private void OnEnable()
    {
        _woodLeft = _totalWood;
    }

    public bool Take()
    {
        if (IsDepleted)
            return false;

        _woodLeft--;

        StartCoroutine(Shudder());

        if (IsDepleted)
            gameObject.SetActive(false);

        return true;
    }

    private IEnumerator Shudder()
    {
        //wait for animation
        yield return new WaitForSeconds(0.8f);

        Vector3 originalPos = transform.position;
        
        float elapsed = 0.0f;

        while (elapsed < shudderDuration)
        {
            float x = Random.Range(-1f, 1f) * shudderMagnitude + originalPos.x;
            float y = Random.Range(-1f, 1f) * shudderMagnitude + originalPos.y;
            float z = Random.Range(-1f, 1f) * shudderMagnitude + originalPos.z;

            transform.position = new Vector3(x, y, z);
            elapsed += Time.deltaTime;

            yield return null;
        }
    }

    [ContextMenu("Snap")]
    private void Snap()
    {
        if(NavMesh.SamplePosition(transform.position, out var hit, 5f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
    }

    public void SetAvailable(int amount) => _woodLeft = amount;
}
