using System;
using Code.Scripts.Classes;
using UnityEngine;

namespace Code.Scripts.Managers
{
    public class ESDataManager : BaseManager<ESDataManager>
    {
        private const string DataKey = "gameData";

        [SerializeField] public GameData gameData;

        private void Awake()
        {
            Load();
        }

        public void Load()
        {
            if (ES3.FileExists())
            {
                if (ES3.KeyExists(DataKey)) gameData = ES3.Load(DataKey, gameData);
            }
        }

        public void Save()
        {
            ES3.Save(DataKey, gameData);
        }

        public CheckPoint GetCheckPoint()
        {
            return gameData.checkPoint;
        }

        public void SetCheckPoint(CheckPoint checkPoint)
        {
            gameData.checkPoint = checkPoint;
            Save();
        }
    }
}