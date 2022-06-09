using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartCharacter : MonoBehaviour
{
    public GameObject[] playerPrefabs;

    private void Awake()
    {
        Instantiate(playerPrefabs[PlayerPrefs.GetInt("SelectedCharacter", 0)], Vector3.zero, Quaternion.identity);
    }

    private void Start()
    {
        Destroy(gameObject);
    }
}
