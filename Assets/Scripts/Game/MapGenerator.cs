using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MapGenerator : Singleton<MapGenerator>
{
    [Header("Stand sizes")]
    [Header("Height")]
    [SerializeField] float _minStandHeight;
    [SerializeField] float _maxStandHeight;
    [Header("Width")]
    [SerializeField] float _minStandWidth;
    [SerializeField] float _maxStandWidth;
    [Header("Separation")]
    [SerializeField] float _minStandSeparation;
    [SerializeField] float _maxStandSeparation;
    [Header("References")]
    [SerializeField] Transform _stand;
    /// <summary>
    /// Holds the information of the screen width in units to know how many stands instantiate to fill a map block with screen size.
    /// </summary>
    float _blockFreeSpace;
    /// <summary>
    /// Indicates if it the first stand to not point it and make it the same width always.
    /// </summary>
    bool _isFirstStand;
    /// <summary>
    /// Holds the last stand position to instantiate the rest when the last stand is shown.
    /// </summary>
    float _lastBlockRightBorderPosition = default;

    private void Start()
    {
        _isFirstStand = true;
        generateBlock();
    }
    /// <summary>
    /// Genrates the map blocks.
    /// </summary>
    public void generateBlock()
    {
        _blockFreeSpace = screenRightBorderPosition;
        Transform _block = new GameObject().transform;
        _block.position = new Vector3(0, -2.335f, 0);
        _block.name = "MapBlock";
        _block.parent = transform;
        //have track of the last stand placed to add the component needed to spawn the next block and destroy the block when get invisible to screen.
        Transform _instantiatedStand = null;
        //While ther is free space place stands
        while (_blockFreeSpace >= 0)
        {
            _instantiatedStand = Instantiate(_stand, _block);
            //change stand tag to not score the first stand
            if (_isFirstStand)
                _instantiatedStand.tag = "Untagged";
            //sizes
            float _standHeight = Random.Range(_minStandHeight, _maxStandHeight);
            float _standWidth = _isFirstStand ? _maxStandWidth: Random.Range(_minStandWidth, _maxStandWidth);

            float _separation = Random.Range(_minStandSeparation, _maxStandSeparation);
            //position
            Vector2 _standLocalPosition = new Vector2((_isFirstStand ? 0 : _lastBlockRightBorderPosition + _separation), 0);
            _lastBlockRightBorderPosition = _standLocalPosition.x + _standWidth;
            
            _instantiatedStand.localScale = new Vector2(_standWidth, _standHeight);

            _instantiatedStand.localPosition = _standLocalPosition;
            //update remaining space
            _blockFreeSpace -=_standWidth + _separation;
            //disable first stand
            _isFirstStand = false;
        }
        //add the component needed for destruction and spawning of map blocks
        _instantiatedStand.gameObject.AddComponent<mapBlockSpawnNotifier>();
    }
    /// <summary>
    /// The size of the screen in units.
    /// </summary>
    private static float screenRightBorderPosition=> GameManager.Instance.ScreenRightBorderPosition;
}
