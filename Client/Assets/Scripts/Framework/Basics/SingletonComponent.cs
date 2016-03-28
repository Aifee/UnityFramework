using UnityEngine;
using System.Collections;

public abstract class SingletonComponent<T> : MonoBehaviour where T : SingletonComponent<T> {

    private static T instance = null;

    protected virtual void Awake() {
        if (instance != null) {
            DestroyImmediate(gameObject);
            return;
        }
        instance = this as T;
    }

    protected virtual void OnDestroy() {
        instance = null;
    }

    public static T Instance { get { return instance; } }

}