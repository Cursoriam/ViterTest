using UnityEngine;

public class Constants
{
    public const float SphereRadius = 1.0f;
    public const int RowForSquare = 1;
    public static readonly int[] ColumnsForSquares = {0, 2};
    public const string WinGameText = "Победа";
    public const string LoseGameText = "Ошибка";
    public const string GreySquareXParamName = "GreySquareX";
    public const string GreySquareYParamName = "GreySquareY";
    public const string GameStateParamName = "GameState";
    public const string GameSceneName = "GameScene";
    public static readonly Vector2 ScreenDimensions = Camera.main.ScreenToWorldPoint(
        new Vector2(Screen.width, Screen.height));
    public const float StandardIntend = 0.5f;
    public const int Rows = 3;
    public const int Columns = 3;
    public static readonly Vector2 ScreenCenter = new Vector3((ScreenDimensions.x - ScreenDimensions.x)/2, 
        (ScreenDimensions.y - ScreenDimensions.y)/2);
    public const string AnimatorParamName = "gameObjectActivated";
    public const float SquareDestructionTime = 1.0f;
    public const string MusicVolumeParamName = "MusicVolume";
    public const string MusicPanelIsActiveParamName = "MusicPanelIsActive";
}
