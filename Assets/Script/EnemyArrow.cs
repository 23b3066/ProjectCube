using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyArrow : MonoBehaviour
{
    [Header("References")]
    [SerializeField, Tooltip("プレイヤーオブジェクト")]
    private Transform player = null;

    [SerializeField, Tooltip("プレイヤーを映すカメラ")]
    private Transform cameraTransform = null;

    [Header("Settings")]
    [SerializeField, Tooltip("敵を探すタグ")]
    private string enemyTag = "Enemy";

    private Transform nearestEnemy = null;

    void Update()
    {
        FindNearestEnemy();
        if (nearestEnemy != null)
        {
            TurnAroundDirectionTarget();
        }
    }

    /// <summary>
    /// 一番近いEnemyを探す
    /// </summary>
    private void FindNearestEnemy()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float minDistance = Mathf.Infinity;
        Transform closest = null;

        foreach (GameObject enemy in enemies)
        {
            if (enemy == null) continue;

            float distance = Vector3.Distance(player.position, enemy.transform.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                closest = enemy.transform;
            }
        }

        nearestEnemy = closest;
    }

    /// <summary>
    /// 矢印を最も近い敵の方向に回転させる
    /// </summary>
    private void TurnAroundDirectionTarget()
    {
        // プレイヤーからターゲットへの方向
        Vector3 Direction = (nearestEnemy.position - player.position).normalized;

        // ターゲット方向を向く回転
        Quaternion RotationalVolume = Quaternion.LookRotation(Direction, Vector3.up);

        // モデルのデフォルト前方向に合わせた補正
        Quaternion modelOffset = Quaternion.Euler(0, 180f, 0);
        // 適用
        transform.rotation = RotationalVolume * modelOffset;
    }

}
