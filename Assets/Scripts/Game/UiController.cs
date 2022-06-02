using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class UiController : Singleton<UiController>
{
    [Header("Score")]
    [SerializeField] TextMeshProUGUI _currentScoreInfo;
    Animator _scoreAnimator;
    [Header("Perfect star")]
    [SerializeField] GameObject _perfectStar;
    [SerializeField] float _starDurationOnScreen;
    [Header("")]
    [SerializeField] GameObject _exitWindow;
    [Header("game over")]
    [SerializeField] GameObject _congratulationObject;
    [SerializeField] GameObject _gameOverObejct;
    [SerializeField] GameObject _inputFieldObject;
    [SerializeField] TMP_InputField _inputField;
    [SerializeField] Button _saveButton;
    [Header("")]
    [SerializeField] TextMeshProUGUI _recordInfo;
    [Header("Force Bar")]
    [SerializeField] RectTransform _forceBarSlider;

    #region Mono Methods
    private void Start()
    {
        _scoreAnimator = _currentScoreInfo.transform.parent.GetComponent<Animator>();
        _recordInfo.text = GameManager.Instance.RecordScore.ToString();
    }
    private void OnEnable()
    {
        PlayerController.Instance.OnWhileJumpButtonPressed += OnWhileJumpButtonPressed;
        PlayerController.Instance.OnPointsScored += OnPointsScored;
        CollisionController.Instance.OnGameOver += OnGameOver;
        CollisionController.Instance.OnPerfectJump += OnPerfectJump;
        _inputField.onValueChanged.AddListener(delegate { OnInputValueChange(); });
    }
    private void OnDisable()
    {
        _inputField.onValueChanged.RemoveAllListeners();
    }
    #endregion

    #region Methods
    public void goToMainMenu()
    {
        GameManager.setDefaultCursor();
        playClickSound();
        GameManager.goToMainMenu();
    }
    /// <summary>
    /// Sves the score.
    /// </summary>
    public void saveScore()
    {
        ScoreData _scoreData = new ScoreData(_inputField.text,playerScore);
        GameManager.Instance.addScore(_scoreData);
        //When disable the button the event doesnt triggers.
        GameManager.setDefaultCursor();
        GameManager.Instance.startGame();
    }
    public void exitGame()
    {
        if (PlayerController.Instance.IsGameOver||_exitWindow.activeSelf)
            return;
        playClickSound();
        _exitWindow.SetActive(true);
    }
    public void playClickSound()
    {
        SoundManager.Instance.playBuuttonClickSound();
    }
    public void cancelExitAction()
    {
        GameManager.setDefaultCursor();
        _exitWindow.SetActive(false);
        playClickSound();

    }
    void hidePerfectStar()
    {
        _perfectStar.SetActive(false);
    }
    #endregion

    #region Events
    /// <summary>
    /// Shows the star indicating perfect jump.
    /// </summary>
    private void OnPerfectJump()
    {
        _perfectStar.SetActive(true);
    }
    /// <summary>
    /// Disable save button when input is empty or enable it otherwise.
    /// </summary>
    void OnInputValueChange()
    {
        _saveButton.interactable = _inputField.text.Length != 0;
    }
    /// <summary>
    /// Hides the perfect star past a time after animation ends.
    /// </summary>
    public void OnStarAnimationEnded()
    {
        Invoke("hidePerfectStar", _starDurationOnScreen);
    }
    /// <summary>
    /// Perform actions when game over animations ends.
    /// </summary>
    public void onGameOverAnimationEnded()
    {
        if (isItNewRecord)
        {
            //show congratulation text and play sounds and update record text
            SoundManager.Instance.playSound(SoundType.NewRecord);
            _congratulationObject.SetActive(true);
            _recordInfo.text = playerScore.ToString();
        }
        //shows save score windows if the score list is not full or the current score is higher than the las position in score list.
        if (doesPlayerScoreBelongToLeaderBoard&&playerScore!=0)
            _inputFieldObject.SetActive(true);
        else
            GameManager.Instance.startGame();
    }
    /// <summary>
    /// Unsubcribe events to avoid memory leaks.
    /// </summary>
    private void OnGameOver()
    {
        _gameOverObejct.SetActive(true);
        PlayerController.Instance.OnWhileJumpButtonPressed -= OnWhileJumpButtonPressed;
        PlayerController.Instance.OnPointsScored -= OnPointsScored;
        CollisionController.Instance.OnGameOver -= OnGameOver;
        CollisionController.Instance.OnPerfectJump -= OnPerfectJump;
    }
    /// <summary>
    /// Updates score text.
    /// </summary>
    /// <param name="score"></param>
    private void OnPointsScored(int score)
    {
        _currentScoreInfo.text = score.ToString();
        _scoreAnimator.Play(GameManager.ANIMATION_TAG);
    }
    /// <summary>
    /// Updates the force batr as long the jump buttons is pressed.
    /// </summary>
    /// <param name="_presseTime">The time being pressed.</param>
    /// <param name="maxValue">The max time the button can be pressed.</param>
    private void OnWhileJumpButtonPressed(float _presseTime, float maxValue)
    {
        float _scaleValue = _presseTime / maxValue;
        _forceBarSlider.localScale = new Vector3(_scaleValue, 1, 1);
    }
    #endregion

    #region Properties
    /// <summary>
    /// TRUE if score list is not full or the current score is higher than the las position in score list.
    /// </summary>
    bool doesPlayerScoreBelongToLeaderBoard => !GameManager.Instance.IsLeaderBoardFull|| GameManager.Instance.getLowestScore() < playerScore;
    /// <summary>
    /// Current player score points.
    /// </summary>
    int playerScore => PlayerController.Instance.CurrentScore;
    /// <summary>
    /// TRUE if the current score is higher than first position in score list.
    /// </summary>
    private bool isItNewRecord =>GameManager.Instance.RecordScore < playerScore;
    /// <summary>
    /// TRUE if exit windows is open.
    /// </summary>
    public bool isExitWindowOpen => _exitWindow.activeSelf;
    #endregion
}
