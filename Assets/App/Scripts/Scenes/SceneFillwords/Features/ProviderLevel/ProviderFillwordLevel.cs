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

            if (TryParseLevel(index) == false)
                return null;

            int squaredSize = 0;
            for (int i = 0; i < _words.Count; i++)
                for (int j = 0; j < _words[i].Count; j++)
                    squaredSize++;

            int gridSize = (int)Math.Sqrt(squaredSize);

            GridFillWords gridFillWords = new GridFillWords(new Vector2Int(gridSize, gridSize));

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

            return gridFillWords;
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

        private bool TryParseLevel(int index)
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

            for (int i = 0; i <= elementCounter; i++)
            {
                if (_words[i].Count != _positions[i].Count)
                {
                    return false;
                }
            }

            return true;
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