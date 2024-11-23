using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public static class Util
{
    public static T GetOrAddComponent<T>(GameObject go) where T : Component
    {
        T component = go.GetComponent<T>();
        if (component == null)
            component = go.AddComponent<T>();

        return component;
    }

    public static GameObject FindChild(GameObject go, string name = null, bool recursive = false)
    {
        Transform transform = FindChild<Transform>(go, name, recursive);
        if (transform == null)
            return null;

        return transform.gameObject;
    }

    public static T FindChild<T>(GameObject go, string name = null, bool recursive = false) where T : UnityEngine.Object
    {
        if (go == null)
            return null;

        if (recursive == false)
        {
            for (int i = 0; i < go.transform.childCount; i++)
            {
                Transform transform = go.transform.GetChild(i);
                if (string.IsNullOrEmpty(name) || transform.name == name)
                {
                    T component = transform.GetComponent<T>();
                    if (component != null)
                        return component;
                }
            }
        }
        else
        {
            foreach (T component in go.GetComponentsInChildren<T>())
            {
                if (string.IsNullOrEmpty(name) || component.name == name)
                    return component;
            }
        }

        return null;
    }

    public static T ParseEnum<T>(string value)
    {
        return (T)Enum.Parse(typeof(T), value, true);
    }

    public static IEnumerator CoWaitMethod(Action method, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        method();
    }

    public static float GetColliderHeight(Collider collider)
    {
        if(collider is BoxCollider box)
        {
            return box.size.y;
        }

        Debug.Log($"{collider.name}의 콜라이더 타입 설정이 되지않음");
        return 0f;
    }

    /// <summary>
    /// #헥사코드
    /// </summary>
    public static Color HexColor(string hexCode)
    {
        if (UnityEngine.ColorUtility.TryParseHtmlString(hexCode, out Color color))
            return color;

        Debug.LogError("[UnityExtension::HexColor]invalid hex code - " + hexCode);
        return Color.white;
    }

#if UNITY_EDITOR
    public static GameObject Editor_InstantiateObject(Transform transform)
    {
        GameObject tempObject = new GameObject();
        GameObject go = GameObject.Instantiate(tempObject, transform);
        GameObject.DestroyImmediate(tempObject);
        return go;
    }

    public static GameObject Editor_InstantiateObject(GameObject original, Transform transform = null)
    {
        GameObject go = GameObject.Instantiate(original, transform);
        go.name = original.name;
        GameObject.DestroyImmediate(original);
        return go;
    }

    public static void Editor_FileDelete(string path, string fileExtension = ".json")
    {
        if (fileExtension[0] != '.')
            fileExtension = $".{fileExtension}";

        if (File.Exists($"{path}{fileExtension}"))
        {
            File.Delete($"{path}{fileExtension}");
            File.Delete($"{path}.meta");
        }
    }
#endif
}