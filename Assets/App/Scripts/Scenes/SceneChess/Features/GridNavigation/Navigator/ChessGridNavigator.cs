using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            foreach (var item in grid.Pieces)
            {
                Debug.Log($"PieceModel: {item.PieceModel} CellPosition: {item.CellPosition} View:{item.View}");
            }

            Debug.Log($"SIZE: {grid.Size}");
            Debug.Log($"unit: {unit}");
            Debug.Log($"from: {from}");
            Debug.Log($"to: {to}");

            if (unit == ChessUnitType.Pon)
                return new List<Vector2Int>() { new Vector2Int(7, 7) };
            else
                return null;
        }
    }
}