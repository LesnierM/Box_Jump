using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    public delegate void ParameterlessEventHandler();
    [Header("Tags")]
    public const string STAND_TAG = "Stand";
    public const string SEA_TAG = "Sea";
    public const string ANIMATION_TAG = "animation";
    public const string IDLE_ANIMATION_TAG = "idle";
    [Header("Points")]
    [SerializeField] int _pointsOnStandHit=10;
    [SerializeField] int _bonusPoints = 20;
    [Header("")]
    [SerializeField] Texture2D _hadnCursor;
    float _screenRightBorderPosition;
    int _recordScore;
    /// <summary>
    /// Holds the score list and manage save and load.
    /// </summary>
    SaveDataManager _saveDataManager;

    private void Start()
    {
        _saveDataManager = new SaveDataManager();
        _recordScore = _saveDataManager.Record;
        //the information of the screen
        _screenRightBorderPosition = (Camera.main.orthographicSize * Screen.width * 2) / Screen.height;
        SoundManager.Instance.playMusic(SceneManager.GetActiveScene().buildIndex==0?SoundType.MainMenuMusic:SoundType.GameMusic);
    }

    #region Methods
    /// <summary>
    /// Loads game scene and plays its bgm.
    /// </summary>
    public void startGame()
    {
        SceneManager.LoadScene(1);
        SoundManager.Instance.playMusic(SoundType.GameMusic);
    }
    /// <summary>
    /// Adds the new score data to the list and updates the record data.
    /// </summary>
    /// <param name="scoreData">The score data to add.</param>
    internal void addScore(ScoreData scoreData)
    {
        _saveDataManager.addScore(scoreData);
        _recordScore = _saveDataManager.Record;
    }
    /// <summary>
    /// Returns the last position score in list.
    /// </summary>
    /// <returns></returns>
    public int getLowestScore()
    {
        return _saveDataManager.getLowestScore();
    }
    /// <summary>
    /// Sets hand cursor for button hover.
    /// </summary>
   public  void setHandCursor()
    {
        Cursor.SetCursor(_hadnCursor, Vector2.zero, CursorMode.Auto);
    }
    #endregion

    #region Static Methods
    /// <summary>
    /// Set the default cursor.
    /// </summary>
    internal static void setDefaultCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
    /// <summary>
    /// Converts the direction enum to vetor2. 
    /// </summary>
    /// <param name="direction">The direction enum to convert.</param>
    public static Vector2  getDirectionVector2FromSpeedDirection(MoveDirections direction)
    {
        switch (direction)
        {
            case MoveDirections.Right:
                return Vector2.right;
            case MoveDirections.Left:
                return Vector2.left;
            default:
                return Vector2.zero;
        }
    }
    /// <summary>
    /// Loads main menu scene.
    /// </summary>
    public static void goToMainMenu()
    {
        SceneManager.LoadScene(0);
        SoundManager.Instance.playMusic(SoundType.MainMenuMusic);
    }

    #endregion

    #region properties
    /// <summary>
    /// The position in units of the right border of screen.
    /// </summary>
    public float ScreenRightBorderPosition { get => _screenRightBorderPosition; }
    public int RecordScore { get => _recordScore; }
    public int PointsOnStandHit { get => _pointsOnStandHit;  }
    /// <summary>
    /// True if score list is full.
    /// </summary>
    public bool IsLeaderBoardFull => _saveDataManager.isListFull;
    /// <summary>
    /// The score list.
    /// </summary>
    public List<ScoreData> Scores => _saveDataManager.Scores;
    public int BonusPoints { get => _bonusPoints;  }
    #endregion
}
