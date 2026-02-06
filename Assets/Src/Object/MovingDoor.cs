using UnityEngine;

public class MovingDoor : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;
    public Transform body;
    public Transform body1;
    public float moveSpeed = 2f;

    [SerializeField] Fruits doorFruitType;
    [Range(0, 4)][SerializeField] int doorFruitNumber;

    public Fruits DoorFruitType { get => doorFruitType; }
    public int DoorFruitNumber { get => doorFruitNumber; set => doorFruitNumber = value; }

    private bool isMoving;

    // Movement state for body
    private Vector3 bodyStartPos;
    private Vector3 bodyTargetPos;

    // Movement state for body1
    private Vector3 body1StartPos;
    private Vector3 body1TargetPos;

    // Movement timing
    private float startTime;
    private float journeyLengthBody;
    private float journeyLengthBody1;

    // Original positions to return to on Close
    private Vector3 bodyOriginalPos;
    private Vector3 body1OriginalPos;

    private void Awake()
    {
        if (body != null) bodyOriginalPos = body.position;
        if (body1 != null) body1OriginalPos = body1.position;
    }

    private void Update()
    {
        if (isMoving)
        {
            MoveDoor();
        }
    }

    public void OpenDoor()
    {
        if (body != null)
        {
            bodyStartPos = body.position;
            bodyTargetPos = pointB.position;
            journeyLengthBody = Vector3.Distance(bodyStartPos, bodyTargetPos);
        }

        if (body1 != null)
        {
            body1StartPos = body1.position;
            body1TargetPos = pointA.position;
            journeyLengthBody1 = Vector3.Distance(body1StartPos, body1TargetPos);
        }

        startTime = Time.time;
        isMoving = true;
    }

    public void CloseDoor()
    {
        if (body != null)
        {
            bodyStartPos = body.position;
            bodyTargetPos = bodyOriginalPos;
            journeyLengthBody = Vector3.Distance(bodyStartPos, bodyTargetPos);
        }

        if (body1 != null)
        {
            body1StartPos = body1.position;
            body1TargetPos = body1OriginalPos;
            journeyLengthBody1 = Vector3.Distance(body1StartPos, body1TargetPos);
        }

        startTime = Time.time;
        isMoving = true;
    }

    void MoveDoor()
    {
        float distCovered = (Time.time - startTime) * moveSpeed;
        bool bodyFinished = true;
        bool body1Finished = true;

        if (body != null && journeyLengthBody > 0.01f)
        {
            float fracBody = distCovered / journeyLengthBody;
            body.position = Vector3.Lerp(bodyStartPos, bodyTargetPos, fracBody);
            if (fracBody < 1f) bodyFinished = false;
        }

        if (body1 != null && journeyLengthBody1 > 0.01f)
        {
            float fracBody1 = distCovered / journeyLengthBody1;
            body1.position = Vector3.Lerp(body1StartPos, body1TargetPos, fracBody1);
            if (fracBody1 < 1f) body1Finished = false;
        }

        if (bodyFinished && body1Finished)
        {
            isMoving = false;
        }
    }
}
