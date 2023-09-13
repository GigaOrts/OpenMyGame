using App.Scripts.Scenes.SceneFillwords.Features.FillwordModels;
using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace App.Scripts.Scenes.SceneFillwords.Features.ProviderLevel
{
    public class ProviderFillwordLevel : IProviderFillwordLevel
    {
        private readonly List<string> _dictionary;
        private readonly List<string> _levels;

        private List<List<char>> _words;
        private List<List<int>> _positions;

        public int LevelsToParse { get; private set; }

        public ProviderFillwordLevel()
        {
            _dictionary = new List<string>();
            _words = new List<List<char>>();
            _levels = new List<string>();
            _positions = new List<List<int>>();
        }

        public GridFillWords LoadModel(int index)
        {
            index--;

            LoadResources(_dictionary, "words_list");
            LoadResources(_levels, "pack_0");
            ParseLevel(index);

            for (int i = 0; i < _words.Count; i++)
            {
                if (_words[i].Count != _positions[i].Count)
                {
                    return null;
                }
            } 

            HashSet<int> uniqueIndexes = new();
            List<int> totalIndexes = new();

            foreach (var levelIndexes in _positions)
            {
                foreach (var letterIndex in levelIndexes)
                {
                    uniqueIndexes.Add(letterIndex);
                    totalIndexes.Add(letterIndex);
                }
            }

            if (totalIndexes.Count != uniqueIndexes.Count)
            {
                return null;
            }

            int squaredSize = 0;
            for (int i = 0; i < _words.Count; i++)
            {
                for (int j = 0; j < _words[i].Count; j++)
                    squaredSize++;
            }

            double gridSizeDouble = Math.Sqrt(squaredSize);
            int gridSize = (int)gridSizeDouble;

            if (gridSizeDouble != gridSize)
            {
                return null;
            }

            int maxIndex = squaredSize - 1;
            foreach (var levelIndexes in _positions)
            {
                foreach (var letterIndex in levelIndexes)
                {
                    if (letterIndex > maxIndex)
                        return null;
                }
            }

            GridFillWords gridFillWords = new GridFillWords(new Vector2Int(gridSize, gridSize));
            FillGrid(gridSize, gridFillWords);

            return gridFillWords;
        }

        private void FillGrid(int gridSize, GridFillWords gridFillWords)
        {
            for (int i = 0; i < _words.Count; i++)
            {
                for (int j = 0; j < _words[i].Count; j++)
                {
                    char letter = _words[i][j];
                    int position = _positions[i][j];
                    string basedPosition = DecimalToBased(position, gridSize);

                    if (basedPosition.Length < 2)
                        basedPosition = basedPosition.Insert(0, "0");

                    int row = int.Parse(basedPosition[0].ToString());
                    int column = int.Parse(basedPosition[1].ToString());

                    gridFillWords.Set(row, column, new CharGridModel(letter));
                }
            }
        }

        private void LoadResources(List<string> storage, string fileName)
        {
            TextAsset asset = Resources.Load<TextAsset>($"Fillwords/{fileName}");
            if (asset != null)
            {
                string[] lines = asset.text.Split("\r\n");
                storage.AddRange(lines);
            }
            else
            {
                throw new FileNotFoundException("Не удалось загрузить словарь из ресурсов.");
            }
        }

        private void ParseLevel(int index)
        {
            _words = new List<List<char>>();
            _positions = new List<List<int>>();

            int spaceCounter = 0;
            int elementCounter = 0;
            string temp = string.Empty;

            foreach (char letter in _levels[index])
            {
                if (int.TryParse(letter.ToString(), out int result))
                {
                    temp += result.ToString();
                }
                else if (letter == ' ')
                {
                    if (spaceCounter % 2 == 0)
                    {
                        string word = _dictionary[int.Parse(temp)];

                        _words.Add(new List<char>());
                        _words[elementCounter] = new List<char>(word);

                        _positions.Add(new List<int>());
                    }
                    else
                    {
                        _positions[elementCounter].Add(int.Parse(temp));
                        elementCounter++;
                    }

                    temp = string.Empty;
                    spaceCounter++;
                }
                else if (letter == ';')
                {
                    if (spaceCounter % 2 != 0)
                    {
                        _positions[elementCounter].Add(int.Parse(temp));
                        temp = string.Empty;
                    }
                }
            }

            _positions[elementCounter].Add(int.Parse(temp));
        }

        private string DecimalToBased(int decimalNumber, int toBase)
        {
            if (decimalNumber == 0)
                return "0";

            string based = "";
            while (decimalNumber > 0)
            {
                int remainder = decimalNumber % toBase;
                based = remainder + based;
                decimalNumber /= toBase;
            }

            return based;
        }
    }
}