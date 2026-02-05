using UnityEngine;


public class DoorController : MonoBehaviour
{
    [SerializeField] Fruits doorFruitType;
    [Range(0, 4)][SerializeField] int doorFruitNumber;
    private Animator aminatorDoor;

    public Fruits DoorFruitType { get => doorFruitType; }
    public int DoorFruitNumber { get => doorFruitNumber; set => doorFruitNumber = value; }

    private void Awake()
    {
        aminatorDoor = GetComponent<Animator>();

    }

    public void OpenDoor()
    {
        aminatorDoor.SetBool("IsOpen", true);
    }
    public void CloseDoor()
    {
        aminatorDoor.SetBool("IsOpen", false);
    }
}
