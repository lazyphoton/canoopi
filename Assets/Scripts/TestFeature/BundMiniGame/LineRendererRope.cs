using UnityEngine;
using System.Collections;

public class LineRendererRope : MonoBehaviour
{
    public Transform startPoint;  // The anchor of the rope
    public Transform endPoint;    // The player (where the rope ends)
    private LineRenderer lineRenderer; // LineRenderer component
    public float distanceMaximaleCorde = 8;
    [SerializeField] private GameObject invisibleRopeTip; // where the player picks up the rope
    [SerializeField] private bool playerInRange;
    [SerializeField] private bool ropePickedUp;


    void Start()
    {
        lineRenderer = gameObject.GetComponent<LineRenderer>();
        //StartCoroutine(CheckDistanceCoroutine());
    }

    private void Update()
    {
        if (playerInRange == true && ropePickedUp == true)
        {
            lineRenderer.SetPosition(0, startPoint.position);
            lineRenderer.SetPosition(1, endPoint.position);
        }

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("Player entered");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(PlayerDropsRope());
            
        }
    }

    public void RopePickedUp(bool ropeIsPickedUp)
    {
        ropePickedUp = ropeIsPickedUp;
    }

    public bool IsRopePickedUp()
    {
        return ropePickedUp;
    }

    private IEnumerator PlayerDropsRope()
    {
        if (ropePickedUp == true)
        {
            invisibleRopeTip.transform.position = endPoint.position;
        }
        playerInRange = false;
        yield return new WaitForSeconds(0.1f);
        RopePickedUp(false);
        Debug.Log("Player left");
    }
}



