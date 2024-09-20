using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private Rigidbody playerRb;
    public float speed;
    private GameObject focalPoint;
    public bool hasPowerup;
    private float powerupStrength = 15f;
    public GameObject powerupIndicator;
    private Vector3 respawnPosition; // New variable to store the respawn position
    public float fallThreshold = -10f; // Threshold for when the player is considered to have "fallen"


    // Start is called before the first frame update
    void Start()
    {
        playerRb = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        respawnPosition = transform.position; // Set the respawn position to the player's starting position
    }

    // Update is called once per frame
    void Update()
    {
        // Handle movement
        float forwardInput = Input.GetAxis("Vertical");
        playerRb.AddForce(focalPoint.transform.forward * speed * forwardInput);

        // Position the powerup indicator just below the player
        powerupIndicator.transform.position = transform.position + new Vector3(0, -0.5f, 0);

        // Check if the player falls below the threshold
        if (transform.position.y < fallThreshold)
        {
            RespawnPlayer();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Powerup"))
        {
            hasPowerup = true;
            Destroy(other.gameObject);
            powerupIndicator.gameObject.SetActive(true);
            StartCoroutine(PowerupCountdownRoutine());
        }
    }

    IEnumerator PowerupCountdownRoutine()
    {
        yield return new WaitForSeconds(7);
        powerupIndicator.gameObject.SetActive(false);
        hasPowerup = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && hasPowerup)
        {
            Rigidbody enemyRigidbody = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayFromplayer = (collision.gameObject.transform.position - transform.position);

            Debug.Log("Collided With: " + collision.gameObject.name + " with powerup set to " + hasPowerup);
            enemyRigidbody.AddForce(awayFromplayer * powerupStrength, ForceMode.Impulse);
        }
    }

    // New method to respawn the player
    void RespawnPlayer()
    {
        Debug.Log("Player fell! Respawning...");
        transform.position = respawnPosition; // Reset the player's position to the respawn point
        playerRb.velocity = Vector3.zero;    // Reset the player's velocity
    }
}
  