using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionController : Singleton<CollisionController>
{
    [SerializeField] ParticleSystem _perfectJumpParticle;
    [SerializeField] float _maxDistanceForBonusPoint;
    /// <summary>
    /// Notifies when the game is over.
    /// </summary>
    public event GameManager.ParameterlessEventHandler OnGameOver;
    /// <summary>
    /// Notifies when perfect jump occurrs.
    /// </summary>
    public event GameManager.ParameterlessEventHandler OnPerfectJump;
    /// <summary>
    /// Avoid pointing with the same stand.
    /// </summary>
    GameObject _lastHittedStand;
    bool _isGameover;
    /// <summary>
    /// Point only when standing on stand not hiting by any side example when falls and hit a stand doesnt point.
    /// </summary>
    Vector2 _validContactNormal = new Vector2(0, 1);
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(GameManager.SEA_TAG) && OnGameOver != null)
        {
            _isGameover = true;
            SoundManager.Instance.playSound(SoundType.GameOver);
            OnGameOver();
        }
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(GameManager.STAND_TAG))
        {
            if (!isStandHittedAlredy(collision.gameObject) && !_isGameover)
            {
                List<ContactPoint2D> _contacts =new List<ContactPoint2D>();
                collision.GetContacts(_contacts);
                bool _validContact = false;
                foreach (var contact in _contacts)
                {
                    if (contact.normal ==_validContactNormal)
                    {
                        _validContact = true;
                        break;
                    }
                }
                if (!_validContact)
                    return;
                _lastHittedStand = collision.gameObject;
                //compare the difference of positions en x axy with max distance.
                float _standCenterPosition = collision.transform.position.x + collision.transform.localScale.x / 2;
                bool _perfectJump = Mathf.Abs((transform.position.x-_standCenterPosition))<=_maxDistanceForBonusPoint;
                SoundManager.Instance.playSound(SoundType.PointsGot);
                if (_perfectJump && OnPerfectJump != null)
                {
                    OnPerfectJump();
                    _perfectJumpParticle.Play();
                    SoundManager.Instance.playSound(SoundType.BonusPoints,true);
                }
                PlayerController.Instance.addPoints(_perfectJump?pointsPerPerfectJumpt:pointsPerStandHit);
            }
        }
    }
    /// <summary>
    /// Checks if an object is equal to the las object hitted.
    /// </summary>
    /// <param name="hittedStand">The object to check.</param>
    /// <returns>TRUE if it is the same FALSE otherwise.</returns>
    bool isStandHittedAlredy(GameObject hittedStand) =>_lastHittedStand!=null&&hittedStand.Equals(_lastHittedStand);
    int pointsPerStandHit => GameManager.Instance.PointsOnStandHit;
    int pointsPerPerfectJumpt => GameManager.Instance.BonusPoints;
}
