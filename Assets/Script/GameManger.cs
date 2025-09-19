using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManger : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] private GameObject _cellPrefab;
    
    [Header("Matrix Settings")]
    [SerializeField] private Transform _gridParent;
    [SerializeField] private int _lenght;
    [SerializeField] private int _width;
    
    [Header("Settings")]
    [SerializeField] private int _minBomb;
    [SerializeField] private int _maxBomb;
    [SerializeField] private List<Vector2Int> _bombNeighbord = new List<Vector2Int>();
    
    
    private Cell [,] _matrix;
    

    private void Awake()
    {
        _matrix =  new Cell[_lenght, _width];
    }

    private void Start()
    {
        SetupMatrix();
        FindNeighbord();
    }

    private void SetupMatrix()
    {
        int bombNumber = Random.Range(_minBomb, _maxBomb);
        
        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                GameObject newPiece = null;
                Cell newCell = new Cell();
                float random =  Random.Range(0f, 1f);

                if (random < 0.2 && bombNumber > 0)
                { 
                    newPiece = Instantiate(_cellPrefab,_gridParent); 
                    newPiece.GetComponent<CellHandler>().Setup(new Vector2Int(i,j),"-1"); 
                    bombNumber -= 1; 
                    newCell.Value = -1;
                    newCell.IsBomb = true;
                    newCell.Position = new Vector2Int(i, j);
                }
                else
                {
                    newPiece = Instantiate(_cellPrefab,_gridParent);
                    newPiece.GetComponent<CellHandler>().Setup(new Vector2Int(i,j), "0");
                    newCell.Value = 0;
                    newCell.Position = new Vector2Int(i, j);
                }
                _matrix[i, j] = newCell;
            }
        }
    }

    private void FindNeighbord()
    {
        for (int i = 0; i < _matrix.GetLength(0); i++)
        {
            for (int j = 0; j < _matrix.GetLength(1); j++)
            {
                Cell neighbordPiece; 
                neighbordPiece = _matrix[i, j];
                if (neighbordPiece.Value != -1)
                {
                    foreach (var neighbord in _bombNeighbord)
                    {
                        if (neighbordPiece.IsCheckPos(new Vector2Int(i,j),_lenght, _width ))
                        {
                            if (_matrix[neighbord.x, neighbord.y].Value == -1 )
                            {
                                neighbordPiece.Value += 1;
                            }
                        }
                    }
                }
            }
        }
    }
    
}
