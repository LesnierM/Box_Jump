using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class mainMenuController : MonoBehaviour
{
    /// <summary>
    /// Time that need pass to hide main buttons and how the click label animation.
    /// </summary>
    [SerializeField] float _timeToHideButtons;
    [SerializeField]TextMeshProUGUI _interactableLabelObject;
    [Header("")]
    [SerializeField] GameObject _mainMenuButtonsObject;
    [Header("Leader board")]
    [SerializeField] GameObject _leaderBoardObject;
    [SerializeField] Transform _leaderBoardItemContainer;
    [SerializeField] scoreDataItem _scoreItem;
    [SerializeField] GameObject _backButton;
    private void Start()
    {
        definePlatformCorrectTextInfo();
    }

    /// <summary>
    /// SHows click on desktop machines and touch on tactil ones as mobiles.
    /// </summary>
    private void definePlatformCorrectTextInfo()
    {
        switch (Application.platform)
        {
            case RuntimePlatform.IPhonePlayer:
            case RuntimePlatform.Android:
                _interactableLabelObject.text = "Touch to play";
                break;
            default:
                _interactableLabelObject.text = "Click to play";
                break;
        }
    }
    /// <summary>
    /// Fills the leader board
    /// </summary>
    private void fillLeaderBoard()
    {
        int _position = 0;
        foreach (var item in GameManager.Instance.Scores)
            Instantiate(_scoreItem, _leaderBoardItemContainer).setData(item,++_position);
    }
    /// <summary>
    /// Starts the game 
    /// </summary>
    public void play()
    {
        setDefaultCursor();
        GameManager.Instance.startGame();
        playClickSound();
    }
    /// <summary>
    /// Shows main buttons.
    /// </summary>
    public void showMainMenu()
    {
        _leaderBoardObject.SetActive(false);
        _mainMenuButtonsObject.SetActive(true);
        _backButton.SetActive(false);
        activateCounterToHideMenus();
        playClickSound();
    }
    /// <summary>
    /// Hide all ui buttons and menu.
    /// </summary>
    void hideAllMenus()
    {
        _leaderBoardObject.SetActive(false);
        _mainMenuButtonsObject.SetActive(false);
        _backButton.SetActive(false);
        //when cursor hover a button and the timer is up all buttons hide and event doenst trigger so that is why i use this
        setDefaultCursor();
    }
    /// <summary>
    /// Shows main menu and hides interactable label.
    /// </summary>
    public void showMenus()
    {
        playClickSound();
        showMainMenu();
        _interactableLabelObject.gameObject.SetActive(false);
        //the same that before when click event doens trigger so i change cursor here
        setDefaultCursor();
    }
    /// <summary>
    /// Invoke the ide menus at especified time.
    /// </summary>
    void activateCounterToHideMenus()
    {
        Invoke("hideMenus", _timeToHideButtons);
    }
    /// <summary>
    /// Cancel the hide menu countdown in leader board menu.
    /// </summary>
    void deactivateCounterToHideMenus()
    {
        CancelInvoke();
    }
    /// <summary>
    /// Hides all menus and show interactable label.
    /// </summary>
    void hideMenus()
    {
        hideAllMenus();
        _interactableLabelObject.gameObject.SetActive(true);
    }
    /// <summary>
    /// Shows leader board and deactivate the hide menus countdown.
    /// </summary>
    public void showLeaderBoard()
    {
        if (isLeaderBoardEmpty)
            fillLeaderBoard();
        playClickSound();
        _leaderBoardObject.SetActive(true);
        _mainMenuButtonsObject.SetActive(false);
        _backButton.SetActive(true);
        deactivateCounterToHideMenus();
        //the same
        setDefaultCursor();
    }
    public void quitGame()
    {
        playClickSound();
        Application.Quit();
    }
    /// <summary>
    /// Hides leader board and shows main manu buttons.
    /// </summary>
    public void back()
    {
        playClickSound();
        showMainMenu();
        setDefaultCursor();
    }
    /// <summary>
    /// Sets the default cursor.
    /// </summary>
    private static void setDefaultCursor()
    {
        GameManager.setDefaultCursor();
    }
    void playClickSound()
    {
        SoundManager.Instance.playBuuttonClickSound();
    }

    /// <summary>
    /// TRUE if leader board is empty FALSE otherwise.Avoid filling in innecesarly.
    /// </summary>
    bool isLeaderBoardEmpty => _leaderBoardItemContainer.childCount == 0;
}
