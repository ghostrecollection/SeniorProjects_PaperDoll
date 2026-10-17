using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraChangesManager : MonoBehaviour
{
    // -------------------------------------
    // TITLE
    [Header("Camera Switch Set Up:")]
    [Space(5)]

    // CAMERA REFERENCES
    [Header("Cameras")]
    [Space(10)]
    [SerializeField] public CinemachineCamera zoneCam;
    [SerializeField] public CinemachineCamera mainCam;
    [SerializeField] public CinemachineCamera[] otherCameras;
    
    [Space(10)]

    // VALUES
    [Header("Priority & Bools")]
    [Space(10)]
    [SerializeField] int priorityNum;
    [SerializeField] bool withinZone;

    [Space(30)]

    // EVENT
    public UnityEvent trigger;
    // -------------------------------------


    // --- START ---
    public void Start()
    {
        // Finds and assigns all CinemachineCameras in the scene.
        otherCameras = FindObjectsByType<CinemachineCamera>();
    }


    // --- ON TRIGGER ENTER ---
    private void OnTriggerEnter(Collider other)
    {
        // When entering a certain collider, sets the withinZone bool to true.
        withinZone = true;
        // DEBUG --- Debug.Log("withinZone = " + withinZone);
    }


    // --- ON TRIGGER STAY ---
    private void OnTriggerStay(Collider other)
    {
        // Checks if the player is within the zone.
        if (withinZone == true && other.gameObject.CompareTag("Player"))
        {
            // Sets all other cameras to a priority of zero.
            foreach (var cam in otherCameras)
            {
                cam.Priority = 0;
            }
            // Sets the zoneCam to the highest priority --> Making it the active camera.
            zoneCam.Priority = priorityNum;
            trigger.Invoke();
        }
    }


    // --- ON TRIGGER EXIT ---
    private void OnTriggerExit(Collider other)
    {
        // When a collider is exited, sets the withinZone bool back to false.
        withinZone = false;
        // DEBUG --- Debug.Log("withinZone = " + withinZone);

        // Checks for player.
        if (other.gameObject.CompareTag("Player"))
        {
            // Sets all cameras to a priority of zero.
            foreach (var cam in otherCameras)
            {
                cam.Priority = 0;
            }
            // Sets the mainCam to the highest priority --> Making the default camera active again.
            mainCam.Priority = priorityNum;
            trigger.Invoke();
        }
    }

}
