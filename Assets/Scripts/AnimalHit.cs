using UnityEngine;

public class Animal : MonoBehaviour
{
    public int maxHitPoints = 100;
    private int currentHitPoints;
    public float damageCooldown = 1f; // Cooldown time in seconds
    private bool canTakeDamage = true;

    [SerializeField] public Healthbar _healthbar;

    public AudioClip hitSound;
    public AudioClip deathSound;

    public GameObject hitParticles; // Assign the particle effect in the inspector

    private AudioSource audioSource;
    private Rigidbody rb;

    private void Start()
    {
        currentHitPoints = maxHitPoints;
        audioSource = GetComponent<AudioSource>();
        rb = GetComponent<Rigidbody>();
        _healthbar.UpdateHealthBar(maxHitPoints,currentHitPoints);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (canTakeDamage && collision.gameObject.CompareTag("Sword"))
        {
            TakeDamage(10); // Adjust damage as needed
            StartCoroutine(DamageCooldown());

            if (hitParticles != null)
            {
                ContactPoint contact = collision.contacts[0];
                Quaternion rot = Quaternion.FromToRotation(Vector3.up, contact.normal);
                Vector3 pos = contact.point;

                GameObject explosionParticle = Instantiate(hitParticles, pos, rot);
            }
        }
    }

    private void TakeDamage(int damage)
    {
        currentHitPoints -= damage;
        Debug.Log(gameObject.name + " took " + damage + " damage.");
        _healthbar.UpdateHealthBar(maxHitPoints,currentHitPoints);
        if (hitSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(hitSound);
        }

        if (currentHitPoints <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log(gameObject.name + " has died.");

        if (deathSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(deathSound);
        }

        // Apply explosion force to fly off
        Vector3 explosionPosition = transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPosition, 5f); // Adjust radius as needed
        foreach (Collider hit in colliders)
        {
            Rigidbody hitRb = hit.GetComponent<Rigidbody>();
            if (hitRb != null)
            {
                hitRb.AddExplosionForce(500f, explosionPosition, 5f, 3.0f); // Adjust parameters as needed
            }
        }

        // Schedule deletion two seconds after fatal damage
        Invoke("DestroyAnimal", 2f);
    }

    private void DestroyAnimal()
    {
        // You can add more actions here before destroying the animal
        Destroy(gameObject);
    }

    private System.Collections.IEnumerator DamageCooldown()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }
}

