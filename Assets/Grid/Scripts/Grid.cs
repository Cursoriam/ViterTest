using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Grid : MonoBehaviour
{
    [SerializeField] private GameObject cellPrefab;
    [SerializeField] private GameObject greySquarePrefab;
    private GameObject[,] _cells;
    public Action WinGameAction;
    public Action LoseGameAction;

    private void Awake()
    {
        _cells = new GameObject[Constants.Rows, Constants.Columns];
        SpawnCells();
    }
    
    private void SpawnCells()
    {
        var position = transform.position;
        for (var j = 0; j < Constants.Rows; j++)
        {
            for (var i = 0; i < Constants.Columns; i++)
            {
                var cell = Instantiate(cellPrefab, new Vector3(position.x + i,
                        position.y - j, 1), Quaternion.identity, transform);
                _cells[j, i] = cell;
                _cells[j, i].GetComponent<Cell>().row = j;
                _cells[j, i].GetComponent<Cell>().column = i;
                _cells[j, i].GetComponent<Cell>().isFull = (PlayerPrefs.GetInt("(" + j + " " + i + ")", 0)
                                                            == 1);
                
                if (_cells[j, i].GetComponent<Cell>().isFull)
                {
                    InsertSquare(j, i);
                }
            }
        }
    }

    

    public void AddSquareAndCheckForMatchThree(int row, int column)
    {
        InsertSquare(row, column);
        List<GameObject> totalMatches = new List<GameObject>();
        
        totalMatches.AddRange(FindMatchesInRow(row, column));
        
        totalMatches.AddRange(FindMatchesInColumn(row, column));
        

        if (totalMatches.Count != 0)
        {
            RemoveSquares(totalMatches);
            WinGameAction?.Invoke();
        }
        else
        {
            LoseGameAction?.Invoke();
        }
    }

    private List<GameObject> FindMatchesInRow(int row, int column)
    {
        var matches = new List<GameObject>();
        matches.Add(_cells[row, column]);

        if (row > 0)
        {
            for (var i = row - 1; i >= 0; i--)
            {
                if (_cells[i, column].GetComponent<Cell>().isFull)
                {
                    matches.Add(_cells[i, column]);
                }
                else
                {
                    break;
                }
            }
        }

        if (row < Constants.Rows - 1)
        {
            for (var i = row + 1; i < Constants.Rows; i++)
            {
                if (_cells[i, column].GetComponent<Cell>().isFull)
                {
                    matches.Add(_cells[i, column]);
                }
                else
                {
                    break;
                }
            }
        }
        
        if(matches.Count < Constants.Rows)
            matches.Clear();

        return matches;
    }

    private List<GameObject> FindMatchesInColumn(int row, int column)
    {
        var matches = new List<GameObject>();
        matches.Add(_cells[row, column]);
        if (column > 0)
        {
            for (var j = column - 1; j >= 0; j--)
            {
                if (_cells[row, j].GetComponent<Cell>().isFull)
                {
                    matches.Add(_cells[row, j]);
                }
                else
                {
                    break;
                }
            }
        }

        if (column < Constants.Columns - 1)
        {
            for (var j = column + 1; j < Constants.Columns; j++)
            {
                if (_cells[row, j].GetComponent<Cell>().isFull)
                {
                    matches.Add(_cells[row, j]);
                }
                else
                {
                    break;
                }
            }
        }
        
        if(matches.Count < Constants.Columns)
            matches.Clear();

        return matches;
    }

    public void InsertWithCheckingOnEmptiness(int row, int column)
    {
        if (!_cells[row, column].GetComponent<Cell>().isFull)
        {
            InsertSquare(row, column);
        }
    }

    public void InsertSquare(int row, int column)
    {
        var cell = _cells[row, column];

        var square = Instantiate(greySquarePrefab, cell.transform.position, cell.transform.rotation);
        square.transform.parent = cell.transform;
        cell.GetComponent<Cell>().isFull = true;
        SaveGrid();
    }

    private void RemoveSquares(List<GameObject> matches)
    {
        foreach (var cell in matches)
        {
            cell.GetComponent<Cell>().isFull = false;
            StartCoroutine(RemoveSquare(cell.transform.GetChild(0).gameObject));
        }
        SaveGrid();
    }

    private IEnumerator RemoveSquare(GameObject square)
    {
        var currentPosition = square.transform.localScale;
        var coordinate = Vector3.zero;
        var elapsedTime = 0.0f;
        var movementTime = Constants.SquareDestructionTime;
        while (elapsedTime < movementTime)
        {
            square.transform.localScale = Vector3.Lerp(currentPosition, coordinate, elapsedTime);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        square.transform.localScale = coordinate;
        Destroy(square);
        yield return null;
    }

    private void ClearCell()
    {
        for (var row = 0; row < Constants.Rows; row++)
        {
            for (var column = 0; column < Constants.Columns; column++)
            {
                if (_cells[row, column].GetComponent<Cell>().isFull)
                {
                    var cellObject = _cells[row, column].GetComponent<Cell>();
                    Destroy(_cells[row, column].transform.GetChild(0).gameObject);
                    cellObject.isFull = false;
                }
            }
        }
        
        SaveGrid();
    }

    public void RenewGrid()
    {
        ClearCell();
    }

    private void SaveGrid()
    {
        for (var i = 0; i < Constants.Rows; i++)
        {
            for (var j = 0; j < Constants.Columns; j++)
            {
                if (_cells[i, j] != null)
                {
                    var boolInInt = _cells[i, j].GetComponent<Cell>().isFull ? 1 : 0;
                    PlayerPrefs.SetInt("(" + i + " " + j + ")", boolInInt);
                }
            }
        }
        PlayerPrefs.Save();
    }
}
