using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    void Start()
    {
        
    }

    void Update()
    {
        
    }

    public void SelectCharacter()
    {
        SceneManager.LoadScene("Persistent");
        SceneManager.LoadScene("Playtest", LoadSceneMode.Additive);
    }
}
