using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
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
    [SerializeField] private List<GameObject> _text;
    
    
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
                    newPiece.GetComponent<CellHandler>().Setup(new Vector2Int(i,j),this); 
                    bombNumber -= 1; 
                    newCell.Value = -1;
                    newCell.Position = new Vector2Int(i, j);
                }
                else
                {
                    newPiece = Instantiate(_cellPrefab,_gridParent);
                    newPiece.GetComponent<CellHandler>().Setup(new Vector2Int(i,j),this);
                    newCell.Value = 0;
                    newCell.Position = new Vector2Int(i, j);
                }
                _text.Add(newPiece);
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
                Cell currentCell = _matrix[i, j];

                if (currentCell.Value != -1)
                {
                    int bombCount = 0;
                    
                    foreach (var neighbord in _bombNeighbord)
                    {
                        Vector2Int neighbordPos = new Vector2Int(i + neighbord.x, j + neighbord.y);

                        if (currentCell.IsCheckPos(neighbordPos, _lenght, _width))
                        {
                            if (_matrix[neighbordPos.x, neighbordPos.y].Value == -1)
                            {
                                bombCount++;
                            }
                        }
                    }
                    currentCell.Value = bombCount;
                }
            }
        }
    }

    public void CellReveal(Vector2Int pos)
    {
        Cell currentCell = _matrix[pos.x, pos.y];
        
        if (currentCell.IsRevealed) return;

        currentCell.IsRevealed = true;
        foreach (var text in _text)
        {
            CellHandler currentCellHandler = text.GetComponent<CellHandler>();
            if (currentCellHandler.Position == currentCell.Position)
            {
                currentCellHandler.Reveal(currentCell.Value); 
            }
        }

        if (currentCell.Value == -1 )
        {
            Defeat();
            return;
        }

        if (currentCell.Value > 0) return;

        foreach (var neighbord in _bombNeighbord)
        {
            Vector2Int neighbordPos = new Vector2Int(pos.x + neighbord.x, pos.y + neighbord.y);

            if (currentCell.IsCheckPos(neighbordPos, _lenght,_width))
            {
                CellReveal(neighbordPos);
            }
        }
    }

    public void Defeat()
    {
        Debug.Log("You Loose");
        Invoke("Restart", 1);
    }

    private void Restart()
    {
        foreach (Transform child in _gridParent.transform)
        {
            Destroy(child.gameObject);
        }
        
        _text.Clear();
        SetupMatrix();
        FindNeighbord();
    }
    
}
