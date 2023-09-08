using System.IO;
using App.Scripts.Scenes.SceneWordSearch.Features.Level.Models.Level;
using UnityEngine;

namespace App.Scripts.Scenes.SceneWordSearch.Features.Level.BuilderLevelModel.ProviderWordLevel
{
    public class ProviderWordLevel : IProviderWordLevel
    {
        public LevelInfo LoadLevelData(int levelIndex)
        {
            string jsonFolderPath = "Assets/App/Resources/WordSearch/Levels/";
            string jsonFilePath = Path.Combine(jsonFolderPath, $"{levelIndex}.json");

            if (File.Exists(jsonFilePath))
            {
                string jsonContent = File.ReadAllText(jsonFilePath);
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