using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class MainSquare : MonoBehaviour
{
    private Vector2 _initialPoint;
    public Action<int, int> OnDropAction;

    private void Awake()
    {
        _initialPoint = transform.position;
        transform.position = new Vector3(PlayerPrefs.GetFloat(Constants.GreySquareXParamName, _initialPoint.x),
            PlayerPrefs.GetFloat(Constants.GreySquareYParamName, _initialPoint.y), 0.0f);
    }

    private void OnMouseDown()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            _initialPoint = transform.position;
            SavePosition();
        }
    }

    private void OnMouseDrag()
    {
        if (!EventSystem.current.IsPointerOverGameObject())
        {
            if (Camera.main is { })
            {
                var mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                transform.position = new Vector3(mousePosition.x, mousePosition.y, 0);
            }
        }
    }

    private void OnMouseUp()
    {
       RecalculatePosition();
    }

    private void RecalculatePosition()
    {
        var overlappingColliders = Physics2D.OverlapCircleAll(transform.position,
            Constants.SphereRadius);

        var place = Utilities.GetMaximumSquaredIntersectingObject(gameObject, overlappingColliders);
        if (place == null)
            transform.position = _initialPoint;
        else
        {
            transform.position = place.transform.position;
            SavePosition();
            if (place.GetComponent<Cell>() != null)
            {
                if (!place.GetComponent<Cell>().isFull)
                {
                    var cell = place.GetComponent<Cell>();
                    DeleteKeys();
                    OnDropAction?.Invoke(cell.row, cell.column);
                    Destroy(gameObject);
                }
                else
                {
                    transform.position = _initialPoint;
                    SavePosition();
                }
            }
            
        }
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat(Constants.GreySquareXParamName, transform.position.x);
        PlayerPrefs.SetFloat(Constants.GreySquareYParamName, transform.position.y);
        PlayerPrefs.Save();
    }

    private void DeleteKeys()
    {
        PlayerPrefs.DeleteKey(Constants.GreySquareXParamName);
        PlayerPrefs.DeleteKey(Constants.GreySquareYParamName);
        PlayerPrefs.Save();
    }
}
