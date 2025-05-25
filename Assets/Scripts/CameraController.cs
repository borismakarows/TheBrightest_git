using System;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set;}
    [SerializeField] Transform battleTransform;
    [HideInInspector] public Transform RT_Transform;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void MoveToBattle(Transform LocalTransform)
    {
        RT_Transform = LocalTransform;
        transform.SetPositionAndRotation(battleTransform.position, battleTransform.rotation);
    }

    public void MoveToOldPosition()
    {
        transform.SetPositionAndRotation(RT_Transform.position, RT_Transform.rotation);
    }

    public static implicit operator CameraController(UI_Manager v)
    {
        throw new NotImplementedException();
    }
}
