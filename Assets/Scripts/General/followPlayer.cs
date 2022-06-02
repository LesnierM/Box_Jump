using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class followPlayer : MonoBehaviour
{
    /// <summary>
    /// Follows player position at least is game over.
    /// </summary>
    private void LateUpdate()
    {
        if (PlayerController.Instance.IsGameOver)
            return;
        Transform _playerPosition = PlayerController.Instance.transform;
        transform.position = new Vector3(_playerPosition.position.x, transform.position.y, transform.position.z);
    }
}
