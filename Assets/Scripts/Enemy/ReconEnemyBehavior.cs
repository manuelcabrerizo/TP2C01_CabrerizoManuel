using UnityEngine;

public class ReconEnemyBehavior : EnemyBehavior
{
    [SerializeField] private ReconEnemyData reconEnemyData;
    private Transform[] patrolPath = null;
    private int targetIndex = 0;
    private int direction = 1;

    private void FixedUpdate()
    {
        if (patrolPath == null)
        {
            return;
        }

        Vector3 toTarget = patrolPath[targetIndex].position - transform.position;
        float targetReachRadio = reconEnemyData.TargetReachRadio;
        if (toTarget.sqrMagnitude > (targetReachRadio*targetReachRadio))
        {
            Vector3 forward = toTarget.normalized;
            transform.rotation = Quaternion.LookRotation(forward);
            body.AddForce(forward * reconEnemyData.Speed, ForceMode.Acceleration);

            Vector3 velocity = body.velocity;
            if (velocity.sqrMagnitude > (reconEnemyData.MaxSpeed * reconEnemyData.MaxSpeed))
            {
                body.velocity = velocity.normalized * reconEnemyData.MaxSpeed;
            }

        }
        else
        {
            if (targetIndex == (patrolPath.Length - 1))
            {
                direction = -1;
            }
            if (targetIndex == 0)
            {
                direction = 1;
            }
            targetIndex += direction;
        }
    }

    public void SetPatrolPoints(Transform[] path)
    {
        patrolPath = path;
    }

    public override void OnAquire()
    {
        base.OnAquire();
    }

    public override void OnRelease()
    {
        base.OnRelease();
    }

    private Transform FindClosestPatrolTarget()
    {
        Transform closest = null;
        int closestIndex = -1;
        float minDistSq = float.MaxValue;
        for (int i = 0; i < patrolPath.Length; ++i)
        {
            Transform targte = patrolPath[i];
            float distSq = (targte.position - transform.position).sqrMagnitude;
            if (distSq < minDistSq)
            {
                minDistSq = distSq;
                closestIndex = i;
            }
        }
        if (closestIndex != -1)
        {
            closest = patrolPath[closestIndex];
        }
        return closest;
    }
}
