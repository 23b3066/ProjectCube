using UnityEngine;
using System.Collections.Generic;

public class Teleporter : MonoBehaviour
{
    public Transform teleportDestination; // テレポート先
    public string playerTag = "Player";
    public List<GameObject> players = new List<GameObject>();
    public GimmickController_test floorCountScript;
    public GimmickController floorCountScriptNormal;
    public SceneFader sceneFader; 

    public bool isNormal = false;

    public Select_BGM select_BGM;


    private void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(playerTag))
        {
            select_BGM.SE();
            if (!players.Contains(other.gameObject))
            {  
                players.Add(other.gameObject);
            }

            if(isNormal)
                floorCountScriptNormal.FloorCount++;
            else
                floorCountScript.FloorCount++;

            foreach (var player in players)
            {
                RespawnPlayer(player);
            }

            if (sceneFader != null)
            {
                sceneFader.FadeAndLoadScene("Start");
            }
            else
            {
                Debug.LogWarning("SceneFader が設定されていません");
            }
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        if (player == null || teleportDestination == null) return;

        player.transform.position = teleportDestination.position;

        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log($"{player.name} has respawned at {player.transform.position}");
    }
}

    // private void Teleport(Collider player)
    // {
    //     Vector3 targetPos = teleportDestination.position + Vector3.up * yOffset;
    //     player.transform.position = targetPos;

    //     Rigidbody rb = player.GetComponent<Rigidbody>();
    //     if (rb != null)
    //     {
    //         rb.linearVelocity = Vector3.zero;          // 修正済み
    //         rb.angularVelocity = Vector3.zero;
    //     }

    //     Debug.Log("Teleported to: " + targetPos);
    // }
// }
