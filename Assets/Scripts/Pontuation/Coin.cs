using StatePattern.Main;
using StatePattern.Player;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    [SerializeField] private float timerToSpawn = 1f;
    [SerializeField] private float spawnForceAmount = 1f;
    [SerializeField] private float movementAcceleration = 0.2f;
    [SerializeField] private float speedCap = 5f;

    private float timer;

    private Rigidbody rb;

    private bool isActive = false;

    private PlayerView playerView;

    private bool isTouchingPlayer = false;

    private bool hasGivenPoint = false;
    private float momentSpeed = 0f;


    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        timer = timerToSpawn;
        Vector3 initialForce = new Vector3();
        initialForce.x = Random.Range(-1f, 1f);
        initialForce.y = Random.Range(0f, 1f);
        initialForce.z = Random.Range(-1f, 1f);
        rb.AddForce(initialForce * spawnForceAmount);
    }

    private void Update()
    {
        timer -= Time.deltaTime;

        if (timer > 0)
        {
            return;
        }

        if(playerView != null)
        {
            Vector3 dir = playerView.transform.position - transform.position;
            dir.Normalize();
            momentSpeed = Mathf.Min(momentSpeed + movementAcceleration, speedCap);
            rb.velocity = dir * momentSpeed;

            if(isTouchingPlayer && !hasGivenPoint)
            {
                GameService.Instance.PontuationService.AddPoints(1);
                hasGivenPoint = true;
                Destroy(gameObject);
            }
        }
    }



    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
            playerView = player;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
            isTouchingPlayer = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerView>(out PlayerView player))
        {
            isTouchingPlayer = false;
        }
    }
}
