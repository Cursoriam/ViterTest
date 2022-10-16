using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class Utilities
{
    private static float RectangularIntersectionArea(Rect r1, Rect r2)
    {
        var width = Math.Min(r1.xMax, r2.xMax) - Math.Max(r1.xMin, r2.xMin);
        var height = Math.Min(r1.yMax, r2.yMax) - Math.Max(r1.yMin, r2.yMin);
        
        if (width < 0 || height < 0)
            return 0.0f;

        return width * height;
    }

    private static Rect MakeRectFromGameObject(GameObject go)
    {
        var gameObjectSize = go.GetComponent<SpriteRenderer>().bounds.size;
        var gameObjectPosition = go.transform.position;
        var rect = new Rect(gameObjectPosition.x - gameObjectSize.x/2, gameObjectPosition.y - gameObjectSize.y/2,
            gameObjectSize.x, gameObjectSize.y);

        return rect;
    }

    public static GameObject GetMaximumSquaredIntersectingObject(GameObject go, Collider2D[] colliders)
    {
        var maxSquare = 0.0f;
        var mainSquareRect = MakeRectFromGameObject(go);
        GameObject place = null;

        foreach (var overlappingCollider in colliders)
        {
            if (overlappingCollider.gameObject != go)
            {
                var colliderRect = MakeRectFromGameObject(overlappingCollider.gameObject);
                var intersectionSquare = RectangularIntersectionArea(mainSquareRect, colliderRect);
                if (intersectionSquare > maxSquare)
                {
                    place = overlappingCollider.gameObject;
                    maxSquare = intersectionSquare;
                }
            }
        }
        return place;
    }
    
    public static bool IsPointerOverUIObject() {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current);
        eventDataCurrentPosition.position = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        var currentGameObject = EventSystem.current.currentSelectedGameObject;
        foreach (var result in results.ToList())
        {
            if (result.gameObject.transform.parent == currentGameObject.transform.parent)
                results.Remove(result);
        }
        return results.Count > 0;
    }
}
