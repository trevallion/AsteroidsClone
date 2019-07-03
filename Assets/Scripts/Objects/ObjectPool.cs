using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    void Activate();
    void Deactivate();
    void MoveTo(Vector3 position);
}

[System.Serializable]
public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    [SerializeField]
    protected GameObject _objectPrefab;

    public List<T> AllObjects { get; set; }

    private Transform ObjectPoolParent { get; set; }

    private Stack<T> AvailableObjects { get; set; }

    public ObjectPool() { }

    public ObjectPool(GameObject prefab, Vector3 inactiveObjectPosition, int defaultObjectsInPool)
    {
        _objectPrefab = prefab;
        Initialize(inactiveObjectPosition, defaultObjectsInPool);
    }

    public void Initialize(Vector3 inactiveObjectPosition, int defaultObjectsInPool)
    {
        AvailableObjects = new Stack<T>();
        AllObjects = new List<T>();
        ObjectPoolParent = CreatePoolParent(inactiveObjectPosition);
        T newObject;
        for (int i = 0; i < defaultObjectsInPool; ++i)
        {
            newObject = InstantiateNewObject();
            AvailableObjects.Push(newObject);
            AllObjects.Add(newObject);
        }
    }

    public void Return(T returningObject)
    {
        returningObject.Deactivate();
        returningObject.MoveTo(ObjectPoolParent.position);
        AvailableObjects.Push(returningObject);
    }

    public T Retrieve()
    {
        T retrievedObject;
        if (AvailableObjects.Count > 0)
        {
            retrievedObject = AvailableObjects.Pop();
        }
        else
        {
            retrievedObject = InstantiateNewObject();
        }

        return retrievedObject;
    }

    private T InstantiateNewObject()
    {
        GameObject newGameObject = GameObject.Instantiate(_objectPrefab, ObjectPoolParent.position, Quaternion.identity, ObjectPoolParent);

        T objectInstance = newGameObject.GetComponent<T>();

        if (objectInstance == null)
        {
            throw new System.InvalidOperationException($"Prefab for {nameof(T)} object pool does not contain an instance of {nameof(T)}.");
        }
        objectInstance.Deactivate();
        return objectInstance;
    }

    private Transform CreatePoolParent(Vector3 inactiveObjectPosition)
    {
        string parentObjectName = $"{typeof(T)}PoolParent";
        GameObject poolParent = new GameObject(parentObjectName);
        poolParent.transform.position = inactiveObjectPosition;
        return poolParent.transform;
    }
}
