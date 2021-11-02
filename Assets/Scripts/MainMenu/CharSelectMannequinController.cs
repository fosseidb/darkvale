using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharSelectMannequinController : MonoBehaviour
{

    [SerializeField] private GameObject[] characterModels;
    [SerializeField] private CharacterData[] characterDatas;
    [SerializeField] private GameObject mannequin;

    [SerializeField] private GUIManager guiManager;
    [SerializeField] private SceneManager sceneManager;

    private int activeCharacter = 0;

    // Start is called before the first frame update
    void Start()
    {
        UpdateActiveChar();
        guiManager.UpdateActiveCharacter(characterDatas[activeCharacter]);
    }

    public void ConfirmCharacter()
    {
        PlayerPrefs.SetInt("characterIndex", activeCharacter);
        sceneManager.LoadScene(1);
    }

    public void ShuffleRight()
    {
        activeCharacter++;
        if (activeCharacter > characterModels.Length - 1)
            activeCharacter = 0;
        UpdateActiveChar();
    }

    public void ShuffleLeft()
    {
        activeCharacter--;
        if (activeCharacter < 0)
            activeCharacter = characterModels.Length - 1;
        UpdateActiveChar();
    }

    public void SelectChar(int index)
    {
        activeCharacter = index;
        UpdateActiveChar();
        guiManager.UpdateActiveCharacter(characterDatas[index]);
    }

    private void UpdateActiveChar() {
        for (int i = 0; i < characterModels.Length; i++)
        {
            characterModels[i].SetActive(activeCharacter == i);
        }
    }
}
