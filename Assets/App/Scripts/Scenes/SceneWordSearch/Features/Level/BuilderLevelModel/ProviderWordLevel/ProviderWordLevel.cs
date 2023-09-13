using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using System.IO;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            TextAsset jsonFile = Resources.Load<TextAsset>($"WordSearch/Levels/{levelIndex}");

            if (jsonFile != null)
            {
                string jsonContent = jsonFile.text;
                LevelInfo levelInfo = JsonUtility.FromJson<LevelInfo>(jsonContent);
                return levelInfo;
            }
            else
            {
                throw new FileNotFoundException($"Файл .json для уровня {levelIndex} не найден.");
            }
        }
    }
}