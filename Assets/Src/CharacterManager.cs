using UnityEngine;
using UnityEngine.UI;
public class CharacterManager : MonoBehaviour
{
    public CharacterDatabase _characterDatabase;

    public Text nameText;
    public SpriteRenderer artworkSprite;

    private Animator animator;

    private int selectedCharacter = 0;

    // Start is called before the first frame update
    void Start()
    {
        Debug.Log(_characterDatabase);
        animator = GetComponent<Animator>();
        UpdateCharacter(PlayerPrefs.GetInt("SelectedCharacter"));
    }

    public void NextCharacter()
    {
        selectedCharacter++;

        if (selectedCharacter >= _characterDatabase.CharacterCount)
        {
            selectedCharacter = 0;
        }
        Debug.Log("Next clicked");
        UpdateCharacter(selectedCharacter);
    }

    public void BackCharacter()
    {
        selectedCharacter--;

        if (selectedCharacter < 0)
        {
            selectedCharacter = _characterDatabase.CharacterCount - 1;
        }
        UpdateCharacter(selectedCharacter);
    }

    private void UpdateCharacter(int selectedCharacter)
    {
        Character _character = _characterDatabase.GetCharacter(selectedCharacter);
        artworkSprite.sprite = _character.characterSprite;
        nameText.text = _character.characterName;
        animator.runtimeAnimatorController = _character.characterAnimator;
        PlayerPrefs.SetInt("SelectedCharacter", selectedCharacter);
        PlayerPrefs.Save();
        Debug.Log("Character updated to: " + PlayerPrefs.GetInt("SelectedCharacter"));
    }
}
