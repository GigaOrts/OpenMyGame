using System;
using System.Collections.Generic;
using App.Scripts.Scenes.SceneChess.Features.ChessField.GridMatrix;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Piece;
using App.Scripts.Scenes.SceneChess.Features.ChessField.Types;
using UnityEngine;

namespace App.Scripts.Scenes.SceneChess.Features.GridNavigation.Navigator
{
    public class ChessGridNavigator : IChessGridNavigator
    {
        private ChessGrid _grid;

        public List<Vector2Int> FindPath(ChessUnitType unit, Vector2Int from, Vector2Int to, ChessGrid grid)
        {
            _grid = grid;

            List<Vector2Int> openSet = new List<Vector2Int>();
            HashSet<Vector2Int> closedSet = new HashSet<Vector2Int>();

            Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            Dictionary<Vector2Int, float> gScore = new Dictionary<Vector2Int, float>();
            Dictionary<Vector2Int, float> fScore = new Dictionary<Vector2Int, float>();

            openSet.Add(from);
            gScore[from] = 0;
            fScore[from] = 0;

            foreach (ChessUnit piece in _grid.Pieces)
            {
                closedSet.Add(piece.CellPosition);
            }

            while (openSet.Count > 0)
            {
                Vector2Int current = FindLowestFScore(openSet, fScore);

                if (current == to)
                {
                    return ReconstructPath(cameFrom, current);
                }

                openSet.Remove(current);
                closedSet.Add(current);

                foreach (Vector2Int neighbor in GetValidMovesForCurrentUnit(unit, current))
                {
                    if (closedSet.Contains(neighbor))
                    {
                        continue;
                    }

                    float tentativeGScore = gScore[current] + 1;

                    if (!openSet.Contains(neighbor) || tentativeGScore < gScore[neighbor])
                    {
                        cameFrom[neighbor] = current;
                        gScore[neighbor] = tentativeGScore;
                        fScore[neighbor] = gScore[neighbor];

                        if (!openSet.Contains(neighbor))
                        {
                            openSet.Add(neighbor);
                        }
                    }
                }
            }

            return null;
        }

        private Vector2Int FindLowestFScore(List<Vector2Int> openSet, Dictionary<Vector2Int, float> fScore)
        {
            Vector2Int lowest = openSet[0];
            foreach (Vector2Int node in openSet)
            {
                if (fScore.ContainsKey(node) && fScore[node] < fScore[lowest])
                {
                    lowest = node;
                }
            }

            return lowest;
        }

        private List<Vector2Int> ReconstructPath(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            List<Vector2Int> path = new List<Vector2Int> { current };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                path.Insert(0, current);
            }

            path.RemoveAt(0);
            return path;
        }

        private List<Vector2Int> GetValidMovesForCurrentUnit(ChessUnitType unit, Vector2Int position)
        {
            switch (unit)
            {
                case ChessUnitType.Pon:
                    return GetValidMovesForPon(position);
                case ChessUnitType.King:
                    return GetValidMovesForKing(position);
                case ChessUnitType.Queen:
                    return GetValidMovesForQueen(position);
                case ChessUnitType.Rook:
                    return GetValidMovesForRook(position);
                case ChessUnitType.Knight:
                    return GetValidMovesForKnight(position);
                case ChessUnitType.Bishop:
                    return GetValidMovesForBishop(position);
                default:
                    throw new NotImplementedException("Не определены правила для данной фигуры");
            }
        }

        private List<Vector2Int> GetValidMovesForPon(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>
            {
                new Vector2Int(position.x, position.y + 1),
                new Vector2Int(position.x, position.y - 1)
            };

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesForKing(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>
            {
                new Vector2Int(position.x - 1, position.y - 1),
                new Vector2Int(position.x, position.y - 1),
                new Vector2Int(position.x + 1, position.y - 1),
                new Vector2Int(position.x - 1, position.y),
                new Vector2Int(position.x + 1, position.y),
                new Vector2Int(position.x - 1, position.y + 1),
                new Vector2Int(position.x, position.y + 1),
                new Vector2Int(position.x + 1, position.y + 1)
            };

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesForQueen(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();

            validMoves.AddRange(GetValidMovesInDirection(position, -1, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, 0, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, -1, 0));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, 0));
            validMoves.AddRange(GetValidMovesInDirection(position, -1, 1));
            validMoves.AddRange(GetValidMovesInDirection(position, 0, 1));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, 1));

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesForRook(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();

            validMoves.AddRange(GetValidMovesInDirection(position, 0, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, -1, 0));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, 0));
            validMoves.AddRange(GetValidMovesInDirection(position, 0, 1));

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesForKnight(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>
            {
                new Vector2Int(position.x - 1, position.y - 2),
                new Vector2Int(position.x + 1, position.y - 2),
                new Vector2Int(position.x - 2, position.y - 1),
                new Vector2Int(position.x + 2, position.y - 1),
                new Vector2Int(position.x - 2, position.y + 1),
                new Vector2Int(position.x + 2, position.y + 1),
                new Vector2Int(position.x - 1, position.y + 2),
                new Vector2Int(position.x + 1, position.y + 2)
            };

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesForBishop(Vector2Int position)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();

            validMoves.AddRange(GetValidMovesInDirection(position, -1, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, -1));
            validMoves.AddRange(GetValidMovesInDirection(position, -1, 1));
            validMoves.AddRange(GetValidMovesInDirection(position, 1, 1));

            return validMoves;
        }

        private List<Vector2Int> GetValidMovesInDirection(Vector2Int position, int dx, int dy)
        {
            List<Vector2Int> validMoves = new List<Vector2Int>();
            Vector2Int current = position;
            bool isOutOfBounds;

            while (true)
            {
                current += new Vector2Int(dx, dy);

                isOutOfBounds = current.x < 0 || current.x >= _grid.Size.x || current.y < 0 || current.y >= _grid.Size.y;

                if (isOutOfBounds || _grid.Get(current) != null)
                    break;

                validMoves.Add(current);
            }

            return validMoves;
        }
    }
}