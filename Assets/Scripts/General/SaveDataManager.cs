using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
public class SaveDataManager
{
    byte[] _encryptionExtraEntropy = { 7,8, 4, 3,8,4,7 };
    /// <summary>
    /// Capacity of score list.The ui can only hold five items.
    /// </summary>
    int _maxSavedScores = 5;
    /// <summary>
    /// the path of the file storing data.
    /// </summary>
    string _dataFilePath;
    List<ScoreData> _scores;
    /// <summary>
    /// Loads data or create a new list.
    /// </summary>
    public SaveDataManager()
    {
        _scores = new List<ScoreData>();
        _dataFilePath = Application.persistentDataPath + @"/data/data.dat";
        manageData(DataManageTypes.Load);
    }
    /// <summary>
    /// Manage data file.
    /// </summary>
    /// <param name="type">The action to perform with the data.</param>
    public void manageData(DataManageTypes type)
    {
        FileStream _dataFile = default;
        try
        {
            string _dataFileDirectoryPath = Path.GetDirectoryName(_dataFilePath);
            //create directory if it doesnt exists
            if (!Directory.Exists(_dataFileDirectoryPath))
                Directory.CreateDirectory(_dataFileDirectoryPath);
            //if file doesnt exist and it is loading return
            if (!File.Exists(_dataFilePath) && type == DataManageTypes.Load)
                return;
            _dataFile = new FileStream(_dataFilePath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
            //encrypt data file to make it a little harder to hack
            switch (type)
            {
                case DataManageTypes.Load:
                    _scores = new BinaryFormatter().Deserialize(_dataFile) as List<ScoreData>;
                    break;
                case DataManageTypes.Save:
                    new BinaryFormatter().Serialize(_dataFile, _scores);
                    break;
            }
            _dataFile.Close();
        }
        catch (Exception e)
        {
            if (_dataFile != null)
                _dataFile.Close();
            Debug.LogError(e.Message);
        }
    }
    /// <summary>
    /// Returns the las positions score in score list.
    /// </summary>
    /// <returns></returns>
    internal int getLowestScore()
    {
        return _scores!=null&&_scores.Count!=0?_scores[_scores.Count - 1].Score:0;
    }
    public void save()
    {
        sortList();
        manageData(DataManageTypes.Save);
    }
    /// <summary>
    /// Adds the score to list, if the list isn't full is added
    /// otherwise the last position is swapped with the new score data.
    /// </summary>
    /// <param name="scoreData"></param>
    public void addScore(ScoreData scoreData)
    {
        //indicats if ther is any change to save
        bool _save = true;
        if (_scores.Count < _maxSavedScores)
            _scores.Add(scoreData);
        else
        {
            if (_scores[_maxSavedScores-1].Score < scoreData.Score)
                _scores[_maxSavedScores-1] = scoreData;
            else
                _save = false;
        }
        if (_save)
            save();
    }
    /// <summary>
    /// Sorts the list from highest to lowest score.
    /// </summary>
    private void sortList()
    {
        for (int i = 0; i < _scores.Count-1; i++)
        {
            for (int j = 1+i; j < _scores.Count; j++)
            {
                if (_scores[i].Score < _scores[j].Score)
                {
                    var _tempData = _scores[i];
                    _scores[i] = _scores[j];
                    _scores[j] = _tempData;
                }
            }
        }
    }

    #region Properties 
    /// <summary>
    /// TRUE if score list is full.
    /// </summary>
    public bool isListFull => _scores.Count == _maxSavedScores;
    public int Record
    {
        get
        {
            if (_scores == null || _scores.Count == 0)
                return 0;
            else
                return _scores[0].Score;
        }
    }
    public List<ScoreData> Scores { get => _scores;}
    #endregion
}
public enum DataManageTypes
{
    None,
    Load,
    Save
}
