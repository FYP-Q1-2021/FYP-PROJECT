using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterSelect : MonoBehaviour
{
    [SerializeField] private GameObject player;

    public void SelectCharacter()
    {
        GameObject p = Instantiate(player);
        DontDestroyOnLoad(p);
        SceneManager.LoadScene("Persistent");
        SceneManager.LoadScene("Playtest", LoadSceneMode.Additive);
    }
}
