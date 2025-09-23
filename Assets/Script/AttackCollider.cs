using UnityEngine;

public class AttackCollider : MonoBehaviour
{

    public EnemyChase enemyChase;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("AttackColliderIN");
            enemyChase.knockBack();
        }

    }
    private void OnParticleCollision(GameObject obj)
    {
        Debug.Log("ParticleCollision");
        if (obj.CompareTag("Player"))
        {
            Debug.Log("ParticleCollisionIN");
            enemyChase.knockBack();
        }
    }
}
