using System.Collections.Generic;
using UnityEngine;

public class Respawn : MonoBehaviour
{
    public Transform respawnPoint;
    public string playerTag = "Player";
    public List<GameObject> players = new List<GameObject>();
    public AudioSource respawnSound;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(playerTag))
        {
            foreach (var player in players)
            {
                RespawnPlayer(player);

            }
            respawnSound.Play();
        }
    }

    private void RespawnPlayer(GameObject player)
    {
        if (player == null || respawnPoint == null) return;

        player.transform.position = respawnPoint.position;

        var rb = player.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.linearVelocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;
        }

        Debug.Log($"{player.name} has respawned at {player.transform.position}");
    }
}