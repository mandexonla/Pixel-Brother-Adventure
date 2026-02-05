using UnityEngine;

public class LeverController : MonoBehaviour, IInteractable
{
    [SerializeField] DoorController _door;
    [SerializeField] GameObject checkMark;
    [SerializeField] GameObject leverFruit;

    private Animator LeverAnimator;
    bool IsLeverOn;
    bool CanLeverWork;

    private void Awake()
    {
        LeverAnimator = GetComponentInChildren<Animator>();
    }

    public void Interact()
    {
        if (!CanLeverWork)
        {
            TryActivateLever();
        }
        else
            TriggerLever();
    }

    private void TryActivateLever()
    {
        if (CheckCondition())
        {
            CanLeverWork = true;
            FruitManager.Instance.DecreaseFruitNumber(_door.DoorFruitType, _door.DoorFruitNumber);
            TriggerLever();
            checkMark.SetActive(true);
            leverFruit.SetActive(false);
        }
    }

    private void TriggerLever()
    {
        if (IsLeverOn)
        {
            LeverOff();
        }
        else
            LeverOn();
    }

    private void LeverOn()
    {
        IsLeverOn = true;
        LeverAnimator.SetBool("IsActive", true);
        _door.OpenDoor();
    }

    private void LeverOff()
    {
        IsLeverOn = false;
        LeverAnimator.SetBool("IsActive", false);
        _door.CloseDoor();
    }

    private bool CheckCondition()
    {
        return FruitManager.Instance.AreThereEnoughFruit(_door.DoorFruitType, _door.DoorFruitNumber);
    }
}
