using UnityEngine;
using TMPro;

public class TiltControl : MonoBehaviour {

    // CORE
    public GameObject maze;
    public float sensitivity = 9.8f;
    private Vector3 rotation;         // current Euler angle of the maze
    private GameObject[] collectibles;
    private Rigidbody playerRb; // Player rigid body
    private Vector3 movement;


    // UI
    public GameObject winMenu;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI display;

    // CAMS
    public GameObject staticCam;
    public GameObject mobileCam;

    // STATS
    private bool won;
    private int score;

    // Handle collisions
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("EndWall")) { // win the game
            won = true;
            winMenu.SetActive(true);
        } else if (other.gameObject.CompareTag("Collectible")) { // collect collectible
            other.gameObject.SetActive(false);
            score += 1;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Start is called before the first frame update
    private void Start()
    {
        // initialize core data
        playerRb = GetComponent<Rigidbody>();
        collectibles = GameObject.FindGameObjectsWithTag("Collectible");

        // handles camera setting
        SetMobileCam(SystemInfo.deviceType == DeviceType.Handheld);
        Reset();
    }

    private void SetMobileCam(bool isMobile)
    {
        staticCam.SetActive(!isMobile);
        mobileCam.SetActive(isMobile);
    }

    public void SwapCam()
    {
        SetMobileCam(staticCam.activeSelf);
    }

    // FixedUpdate is called at a fixed interval. This is useful for physics
    // simulation and also for the Rigidbody update.
    private void FixedUpdate()
    {
        if (!won)
        {
            if (playerRb.position.y <= -5)
            { // fell out of bounds
                Reset();
            }
            if (SystemInfo.deviceType == DeviceType.Handheld)
            {
                // Get gyro rotation
                Vector3 deviceRotation = Input.gyro.attitude.eulerAngles;
                display.text = "x: " + deviceRotation.x + ", y: " + deviceRotation.y + ", z: " + deviceRotation.z;
            }
            else
            {
                rotation += new Vector3(
                Input.GetAxis("Vertical"), 0f, -Input.GetAxis("Horizontal"));
                maze.transform.rotation = Quaternion.Euler(rotation);

            }
            //maze.transform.rotation = Quaternion.Euler(rotation);
        }
        else
        { // pause movement
            playerRb.linearVelocity = Vector3.zero;
        }
    }

    // Resets the state. This is called manually.
    public void Reset()
    {
        rotation = Vector3.zero;
        maze.transform.rotation = Quaternion.Euler(rotation);
        playerRb.transform.position = new Vector3(-0.5f, 0.5f, 2.5f);
        playerRb.linearVelocity = Vector3.zero;
        playerRb.angularVelocity = Vector3.zero;
        winMenu.SetActive(false);
        won = false;

        foreach (GameObject collectible in collectibles) {
            collectible.SetActive(true);
        }
        score = 0;
        scoreText.text = "Score: " + score.ToString();
    }
}