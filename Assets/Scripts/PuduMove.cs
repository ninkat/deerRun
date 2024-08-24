using UnityEngine;

public class RandomAnimalMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // Speed of the animal's movement
    public float range = 5f; // Range within which the animal can move
    public float changeDirectionInterval = 3f; // Interval for changing direction
    public float pauseDuration = 1f; // Duration to pause after reaching the destination
    private Vector3 randomDestination; // Random destination point for the animal to move
    private float movementTimer; // Timer for movement
    private float pauseTimer; // Timer for pause
    private bool isPaused; // Flag to track whether the animal is paused

    void Start()
    {
        // Start moving immediately
        GenerateRandomDestination();
    }

    void Update()
    {
        if (!isPaused)
        {
            // Move the animal towards the random destination
            transform.position = Vector3.MoveTowards(transform.position, randomDestination, moveSpeed * Time.deltaTime);

            // Rotate the animal to face the direction it's moving
            if (transform.position != randomDestination)
            {
                Vector3 direction = randomDestination - transform.position;
                Quaternion rotation = Quaternion.LookRotation(direction);
                transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * 5f); // Adjust rotation speed here
            }

            // Check if the animal has reached its destination
            if (Vector3.Distance(transform.position, randomDestination) < 0.1f)
            {
                // Start the pause timer
                isPaused = true;
                pauseTimer = 0f;
            }
        }
        else
        {
            // Increment the pause timer
            pauseTimer += Time.deltaTime;

            // Check if the pause duration has passed
            if (pauseTimer >= pauseDuration)
            {
                // Resume movement and generate a new random destination
                isPaused = false;
                GenerateRandomDestination();
                movementTimer = 0f; // Reset the movement timer
            }
        }

        // Increment the movement timer only when not paused
        if (!isPaused)
        {
            movementTimer += Time.deltaTime;

            // Check if it's time to change direction
            if (movementTimer >= changeDirectionInterval)
            {
                // Generate a new random destination to move towards
                GenerateRandomDestination();
                // Reset the movement timer
                movementTimer = 0f;
            }
        }
    }

    void GenerateRandomDestination()
    {
        // Generate a random destination within the defined range around the current position
        randomDestination = transform.position + new Vector3(Random.Range(-range, range), 0f, Random.Range(-range, range));
    }
}








