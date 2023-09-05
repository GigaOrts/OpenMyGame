using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System.Collections.Generic;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        public GridFillWords LoadModel(int index)
        {
            List<string> dictionary = new List<string>();
            List<string> levels = new List<string>();

            TextAsset dictionaryAsset = Resources.Load<TextAsset>("Fillwords/words_list");
            if (dictionaryAsset != null)
            {
                string[] dictionaryLines = dictionaryAsset.text.Split('\n');
                dictionary.AddRange(dictionaryLines);
            }
            else
            {
                Debug.LogError("Не удалось загрузить словарь из ресурсов.");
            }

            TextAsset levelsAsset = Resources.Load<TextAsset>("Fillwords/pack_0");
            if (levelsAsset != null)
            {
                string[] levelLines = levelsAsset.text.Split('\n');
                levels.AddRange(levelLines);
            }
            else
            {
                Debug.LogError("Не удалось загрузить уровни из ресурсов.");
            }

            if (index < 0 || index >= levels.Count)
            {
                return null;
            }

            string levelData = levels[index];
            string[] levelRows = levelData.Split(new char[] { '\n', '\r' }, System.StringSplitOptions.RemoveEmptyEntries);

            int numRows = levelRows.Length;
            int numCols = levelRows[0].Length;

            GridFillWords grid = new GridFillWords(new Vector2Int(numCols, numRows));

            for (int i = 0; i < numRows; i++)
            {
                string row = levelRows[i];
                for (int j = 0; j < numCols; j++)
                {
                    char letter = row[j];
                    CharGridModel charGridModel = new CharGridModel(letter);
                    grid.Set(i, j, charGridModel);
                }
            }

            return grid;
        }
    }
}
