using System;
using Combat.UI;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class CombatTrigger : MonoBehaviour
{
    [SerializeField] private EnemySetupData enemyData;
    [SerializeField] private Transform targetTransform;

    [SerializeField] private KeyCode testKey;

    private void Update()
    {
        if (Input.GetKeyDown(testKey)) {
            Trigger();
        }
    }

    [ContextMenu("Trigger")]
    public void Trigger()
    {
        if (!enemyData) {
            Debug.LogWarning("No enemy data provided", gameObject);
            return;
        }
        
        if(Application.isPlaying)
            CombatSceneLoader.Instance.LoadCombatWith(enemyData, targetTransform.position, targetTransform.rotation).Forget();
    }

    private void OnDrawGizmos()
    {
        if (!targetTransform)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawSphere(targetTransform.position, 0.1f);
        Gizmos.DrawLine(targetTransform.position, targetTransform.position + targetTransform.forward * 1.0f);
    }
}
