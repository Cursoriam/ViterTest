using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject gameObjects;
    [SerializeField] private GameObject gridPrefab;
    [SerializeField] private GameObject bigPocketPrefab;
    [SerializeField] private GameObject greySquarePrefab;
    [SerializeField] private GameObject oneMoreTimeWindowImage;
    private Vector2 _bigPocketPrefabSize;
    private Vector3 _gridSpawnPoint;
    private Vector3 _bigPocketSpawnPoint;
    private GameObject _grid;
    private GameObject _mainSquare;
    private GameState _gameState;
    public static Action ActivateWinGameWindowAction;
    public static Action ActivateLoseGameAction;
    private Action _resetGameAction;
    [SerializeField] private UI ui;

    private void Awake()
    {
        BootsTrap();
    }

    private void WinGame()
    {
        _gameState = GameState.Won;
        SaveGameState();
        oneMoreTimeWindowImage.SetActive(true);
        ActivateWinGameWindowAction?.Invoke();
    }

    private void LoseGame()
    {
        _gameState = GameState.Lost;
        SaveGameState();
        oneMoreTimeWindowImage.SetActive(true);
        ActivateLoseGameAction?.Invoke();
    }

    private void BootsTrap()
    {
        InitializeVariables();
        
        //Grid
        SpawnGrid();
        
        //Pockets
        SpawnPockets();

        switch (_gameState)
        {
            case GameState.Played:
                //MainSquare
                SpawnMainSquare();
                InsertSquares();
                ui.ActivateAnimation();
                oneMoreTimeWindowImage.SetActive(false);
                break;
            case GameState.Won:
                ActivateWinGameWindowAction?.Invoke();
                break;
            case GameState.Lost:
                ActivateLoseGameAction?.Invoke();
                break;
        }
        
        EstablishConnections();
        ActivateActions();
    }
    
    private void InsertSquares()
    {
        var gridObject = _grid.GetComponent<Grid>();
        foreach (var column in Constants.ColumnsForSquares)
        {
            gridObject.InsertWithCheckingOnEmptiness(Constants.RowForSquare, column);
        }
    }
    
    private void InitializeVariables()
    {
        _gameState = (GameState) Enum.Parse(typeof(GameState),
            PlayerPrefs.GetString(Constants.GameStateParamName, GameState.Played.ToString()));
        SaveGameState();
        var bigPocketPrefabLocalScale = bigPocketPrefab.transform.localScale;
        _bigPocketPrefabSize = new Vector2(bigPocketPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.x * 
                                           bigPocketPrefabLocalScale.x, 
            bigPocketPrefab.GetComponent<SpriteRenderer>().sprite.bounds.size.y * bigPocketPrefabLocalScale.y);
    }

    private void ResetVariables()
    {
        _gameState = GameState.Played;
        SaveGameState();
        _resetGameAction?.Invoke();
        oneMoreTimeWindowImage.SetActive(false);
        ui.ActivateAnimation();
        InsertSquares();
        SpawnMainSquare();
        EstablishConnectionForMainSquare();
    }
    
    public void RestartGame()
    {
        ResetVariables();
    }

    private void SpawnGrid()
    {
        _gridSpawnPoint = Constants.ScreenCenter;
        var gridSize = new Vector3((Constants.Columns - 1) * _bigPocketPrefabSize.x,
            (Constants.Rows - 1) * _bigPocketPrefabSize.y);
        _gridSpawnPoint.x -= gridSize.x / 2;
        _gridSpawnPoint.y += gridSize.y / 2;
        _grid = Instantiate(gridPrefab, _gridSpawnPoint, Quaternion.identity);
        _grid.transform.SetParent(gameObjects.transform);
    }
    
    private void SpawnPockets()
    {
        var upperBorder = _gridSpawnPoint.y - (Constants.Rows * _bigPocketPrefabSize.y) - Constants.StandardIntend;
        var belowBorder = -Constants.ScreenDimensions.y + Constants.StandardIntend;
        var leftBorder = -Constants.ScreenDimensions.x + Constants.StandardIntend;
        var rightBorder = Constants.ScreenDimensions.x - Constants.StandardIntend;
        
        _bigPocketSpawnPoint = new Vector3(((rightBorder + leftBorder)/2+leftBorder)/2.0f,
            ((upperBorder + belowBorder)/2.0f+belowBorder)/2.0f, 1.0f);
        var bigPocket = Instantiate(bigPocketPrefab, _bigPocketSpawnPoint, Quaternion.identity);
        bigPocket.transform.SetParent(gameObjects.transform);

        var smallPocketSpawnPoint = new Vector3((rightBorder + (rightBorder + leftBorder)/2)/2,
            (upperBorder + (upperBorder + belowBorder)/2)/2, 1.0f);
        var smallPocket = Instantiate(bigPocketPrefab, smallPocketSpawnPoint,
            Quaternion.identity);
        
        smallPocket.transform.localScale *= 0.9f;
        smallPocket.transform.SetParent(gameObjects.transform);
    }

    private void SpawnMainSquare()
    {
        _mainSquare = Instantiate(greySquarePrefab, _bigPocketSpawnPoint, Quaternion.identity);
        _mainSquare.AddComponent<MainSquare>();
        _mainSquare.transform.SetParent(gameObjects.transform);
    }

    private void EstablishConnections()
    {
        var gridObject = _grid.GetComponent<Grid>();
        gridObject.WinGameAction += WinGame;
        gridObject.LoseGameAction += LoseGame;
        ActivateWinGameWindowAction += ui.ActivateWinGameWindow;
        ActivateLoseGameAction += ui.ActivateLoseGameWindow;

        if (_gameState == GameState.Played)
        {
            EstablishConnectionForMainSquare();
        }
        
        _resetGameAction += gridObject.RenewGrid;
    }

    private void EstablishConnectionForMainSquare()
    {
        var gridObject = _grid.GetComponent<Grid>();
        var mainSquareObject = _mainSquare.GetComponent<MainSquare>();
        mainSquareObject.OnDropAction += gridObject.AddSquareAndCheckForMatchThree;
    }

    private void ActivateActions()
    {
        switch (_gameState)
        {
            case GameState.Lost: 
                ActivateLoseGameAction?.Invoke();
                break;
            case GameState.Won:
                ActivateWinGameWindowAction?.Invoke();
                break;
        }
    }
    
    private void SaveGameState()
    {
        PlayerPrefs.SetString(Constants.GameStateParamName, _gameState.ToString());
        PlayerPrefs.Save();
    }
}
