using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : Singleton<PlayerController>
{
    public delegate void OnJumpButtonPressedEventHandler(float _presseTime, float maxValue);
    public delegate void OnSCoreGainEventHandler(int score);
    /// <summary>
    /// Notifies when the player gains points.
    /// </summary>
    public event OnSCoreGainEventHandler OnPointsScored;
    /// <summary>
    /// Notifies the time the jump buttons is been pressed so far.
    /// </summary>
    public event OnJumpButtonPressedEventHandler OnWhileJumpButtonPressed;
    [Header("Ground check")]
    [SerializeField] float _groundCheckDistance;
    [SerializeField] LayerMask _groundLayerMask;
    [Header("")]
    [SerializeField] float _playerSpawnHeigth = 1.65f;
    [Header("")]
    [SerializeField] float _maxPressedTime;
    [Header("Force")]
    [SerializeField] float _forceScaler;
    [SerializeField] Vector2 _defaultJumpForceApplied;
    Rigidbody2D _rigidbody;
    float _jumpPressStartTime;
    bool _isJumpPressed;
    int _currentScore;
    bool _isGameOver;

    #region Mono Methods
    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody2D>();
        transform.position = new Vector3(transform.lossyScale.x/2, _playerSpawnHeigth);
    }
    private void OnEnable()
    {
        CollisionController.Instance.OnGameOver += OnGameOver;
    }
    private void OnDisable()
    {
        CollisionController.Instance.OnGameOver -= OnGameOver;
    }
    private void Update()
    {
        updateJumpButtonPressedTime();
    }
    private void OnBecameInvisible()
    {
        //avoiod the player falling into infinity when user inputing name
        _rigidbody.gravityScale=0;
        _rigidbody.velocity = Vector2.zero;
    }
    #endregion

    #region Methods and events
    /// <summary>
    /// Notifies in evryframe the time the jump button is been pressed.
    /// </summary>
    private void updateJumpButtonPressedTime()
    {
        if (_isJumpPressed && OnWhileJumpButtonPressed != null)
            OnWhileJumpButtonPressed(timePressed, _maxPressedTime);
    }
    private void OnGameOver()
    {
        _isGameOver = true;
    }
    /// <summary>
    /// Capture the time when pressed.
    /// </summary>
    public void pressedDownJumpButton()
    {
        if (!isGrounded||_isGameOver||Math.Abs(_rigidbody.angularVelocity)>.001f||UiController.Instance.isExitWindowOpen)
            return;
        _isJumpPressed = true;
        _jumpPressStartTime = Time.time;
    }
    /// <summary>
    /// Perform the jump.
    /// </summary>
    public void releasedJumpButton()
    {
        if (!_isJumpPressed||_isGameOver)
            return;
        SoundManager.Instance.playSound(SoundType.Jump);
        _isJumpPressed = false;
        _rigidbody.AddForce(_defaultJumpForceApplied * _forceScaler * timePressed);
    }
    /// <summary>
    /// Add points to current score and norifies its current state.
    /// </summary>
    /// <param name="addedScore"></param>
    public void addPoints(int addedScore)
    {
        _currentScore += addedScore;
        if (OnPointsScored != null)
            OnPointsScored(_currentScore);
    }
    #endregion

    #region Properties
    /// <summary>
    /// Check if it is grounded or not.
    /// </summary>
    bool isGrounded => Physics2D.BoxCast(transform.position, transform.localScale,0, Vector2.down,_groundCheckDistance,_groundLayerMask);
    /// <summary>
    /// The diference between the time jump button was pressed to now.
    /// </summary>
    private float timePressed
    {
        get
        {
            float _timePressed = Time.time - _jumpPressStartTime;
            return Math.Clamp(_timePressed, 0, _maxPressedTime);
        }
    }
    public int CurrentScore { get => _currentScore;}
    public bool IsGameOver { get => _isGameOver; }
    #endregion
}
