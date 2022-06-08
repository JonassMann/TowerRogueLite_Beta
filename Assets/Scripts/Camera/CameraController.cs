using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private float sensitivity = 0.15f;
    private Vector2 targetPosition = Vector2.zero;
    private Rect screenRect = Rect.zero;
    [Space(8)]
    [SerializeField] private float movementOffset = 10f;
    [SerializeField] float offsetMoveSpeed = 25f;
    private Vector3 cameraOffset = Vector2.zero;

    private Character player;

    //private void Start()
    //{
    //    player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
    //}

    private void Update()
    {
        if (player == null)
        {
            var playerList = GameObject.FindGameObjectsWithTag("Player");
            if (playerList.Length > 0) player = GameObject.FindGameObjectWithTag("Player").GetComponent<Character>();
            else return;
        }

        screenRect = new Rect(0f, 0f, Screen.width, Screen.height);

        cameraOffset = Vector2.MoveTowards(cameraOffset, player.moveInput * movementOffset, offsetMoveSpeed * Time.fixedDeltaTime);

        if (player.shooting && screenRect.Contains(Input.mousePosition))
            targetPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition) + cameraOffset;
        else
            targetPosition = player.transform.position + cameraOffset;

        transform.position = Vector2.Lerp(player.transform.position, targetPosition, sensitivity);
    }

    
}
