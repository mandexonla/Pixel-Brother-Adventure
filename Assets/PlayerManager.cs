using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public CharacterDatabase _characterDatabase;

    public SpriteRenderer artworkSprite;
    public Animator animator;

    void Start()
    {
        int selectedCharacter = PlayerPrefs.GetInt("selectedCharacter");
        Character _character = _characterDatabase.GetCharacter(selectedCharacter);
        artworkSprite.sprite = _character.characterSprite;
        animator.runtimeAnimatorController = _character.characterAnimator;
    }
}
