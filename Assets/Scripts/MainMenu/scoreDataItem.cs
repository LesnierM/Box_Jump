using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class scoreDataItem : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _position;
    [SerializeField] TextMeshProUGUI _name;
    [SerializeField] TextMeshProUGUI _score;
    public void setData(ScoreData scoreData,int position)
    {
        _position.text = position.ToString();
        _name.text = scoreData.name;
        _score.text = scoreData.Score.ToString();
    }
}
