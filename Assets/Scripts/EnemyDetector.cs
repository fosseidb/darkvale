using System.Collections;
using UnityEngine;

public class EnemyDetector : MonoBehaviour
{

    public bool EnemyInRange => _detectedEnemy != null;

    private PlayerProfile _detectedEnemy;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerProfile>())
        {     
            _detectedEnemy = other.gameObject.GetComponent<PlayerProfile>();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if(other.GetComponent<PlayerProfile>() == _detectedEnemy)
        {
            StartCoroutine(ClearDetectedEnemyAfterDelay());
        }
    }

    private IEnumerator ClearDetectedEnemyAfterDelay()
    {
        yield return new WaitForSeconds(3f);
        _detectedEnemy = null;
    }

    public Vector3 GetNearestEnemyPosition()
    {
        return _detectedEnemy?.transform.position ?? Vector3.zero;
    }
}
