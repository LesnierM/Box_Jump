using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class cloudsAnimation : MonoBehaviour
{
    [SerializeField] MoveDirections _direction;
    [SerializeField] float _speed = .001f;
    /// <summary>
    /// The material to scroll texture.
    /// </summary>
    Material _material;
    private void Awake()
    {
        var _image = GetComponent<Image>();
        //Create an instance to avoid modifiying other obejcts.
        _material = Instantiate(_image.material);
        _image.material = _material;
    }
    private void FixedUpdate()
    {
        _material.mainTextureOffset = _material.mainTextureOffset + (_speed*currentDirection* Time.deltaTime );
    }
    /// <summary>
    /// Converts direction enum to direction vector2.
    /// </summary>
    Vector2 currentDirection => GameManager.getDirectionVector2FromSpeedDirection(_direction);
}
