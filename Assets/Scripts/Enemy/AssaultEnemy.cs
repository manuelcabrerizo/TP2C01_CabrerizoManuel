using System.Collections;
using UnityEngine;

public class AssaultEnemy : Enemy
{

    [SerializeField] private AssaultEnemyData assaultEnemyData;
    private Vector3 targetPosition;

    private IEnumerator ai;

    private void Update()
    {
        Vector3 toTarget = targetPosition - body.position;
        float targetReachRadio = assaultEnemyData.TargetReachRadio;
        float viewRadio = assaultEnemyData.ViewRadio;
        if (toTarget.sqrMagnitude >= (targetReachRadio*targetReachRadio) &&
            toTarget.sqrMagnitude <= (viewRadio*viewRadio))
        {
            Vector3 forward = toTarget.normalized;
            Vector3 viewDir = (GameManager.Instance.GetPlayerPosition() - body.position).normalized;
            transform.rotation = Quaternion.LookRotation(viewDir);
            body.AddForce(forward * assaultEnemyData.Speed, ForceMode.Acceleration);
            Vector3 velocity = body.velocity;
            if (velocity.sqrMagnitude > (assaultEnemyData.MaxSpeed * assaultEnemyData.MaxSpeed))
            {
                body.velocity = velocity.normalized * assaultEnemyData.MaxSpeed;
            }
        }
    }

    private void StartAI()
    {
        if (ai != null)
        {
            StopCoroutine(ai);
        }
        ai = ProcessAI();
        StartCoroutine(ai);
    }

    private void StopAI()
    {
        if (ai != null)
        {
            StopCoroutine(ai);
            ai = null;
        }
    }

    private IEnumerator ProcessAI()
    {
        while (true)
        {
            targetPosition = GameManager.Instance.GetPlayerPosition();
            yield return new WaitForSeconds(assaultEnemyData.TimePerUpdate);
        }
    }

    public override void OnAquire()
    {
        base.OnAquire();
        StartAI();
    }

    public override void OnRelease()
    {
        base.OnRelease();
        StopAI();
    }
}
