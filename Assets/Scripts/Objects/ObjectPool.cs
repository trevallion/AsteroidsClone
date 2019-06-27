using System.Collections.Generic;
using UnityEngine;

public interface IPoolableObject
{
    void Activate();
    void Deactivate();
    void MoveTo(Vector3 position);
}

public class ObjectPool<T> where T : MonoBehaviour, IPoolableObject
{
    private GameObject ObjectPrefab { get; set; }

    private Transform ObjectPoolParent { get; set; }

    private Stack<T> AvailableObjects { get; set; }

    public ObjectPool(GameObject prefab, Vector3 inactiveObjectPosition, int defaultObjectsInPool)
    {
        AvailableObjects = new Stack<T>();
        ObjectPrefab = prefab;
        ObjectPoolParent = CreatePoolParent(inactiveObjectPosition);
        for (int i = 0; i < defaultObjectsInPool; ++i)
        {
            AvailableObjects.Push(InstantiateNewObject());
        }
    }

    public void Return(T returningObject)
    {
        returningObject.Deactivate();
        returningObject.MoveTo(ObjectPoolParent.position);
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

        retrievedObject.Activate();
        return retrievedObject;
    }

    private T InstantiateNewObject()
    {
        GameObject newGameObject = GameObject.Instantiate(ObjectPrefab, ObjectPoolParent.position, Quaternion.identity, ObjectPoolParent);

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
        string parentObjectName = $"{nameof(T)}PoolParent";
        GameObject poolParent = new GameObject(parentObjectName);
        poolParent.transform.position = inactiveObjectPosition;
        return poolParent.transform;
    }
}
