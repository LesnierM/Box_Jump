using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class mapBlockSpawnNotifier : MonoBehaviour
{
    /// <summary>
    /// Indicates if the next map block was instantiated or not.
    /// </summary>
    bool _nextBlockInstatiated;
    /// <summary>
    /// Create the next map block when appears in screen.
    /// </summary>
    private void OnBecameVisible()
    {
        if (!_nextBlockInstatiated)
        {
            MapGenerator.Instance.generateBlock();
            _nextBlockInstatiated = true;
        }
    }
    /// <summary>
    /// Destroy its parent map block when get out of the screen.
    /// </summary>
    private void OnBecameInvisible()
    {
        if (_nextBlockInstatiated)
            Destroy(transform.parent.gameObject);
    }
}
