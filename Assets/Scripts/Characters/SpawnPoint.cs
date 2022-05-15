using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public GameObject charPrefab;

    private GameObject character;

    private void OnEnable()
    {
        character = Instantiate(charPrefab, transform.position, transform.rotation, transform);
    }

    private void OnDisable()
    {
        Destroy(character);
    }
}
