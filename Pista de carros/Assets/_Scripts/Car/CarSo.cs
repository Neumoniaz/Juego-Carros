
using UnityEngine;
using UnityEngine.Video;

[CreateAssetMenu(fileName = "NewCar", menuName = "Car/NewCar")]

public class CarSo : ScriptableObject
{
    public float speed;
    public float brakeForce;
    public float angle;
    public VideoClip videoClip;
    public string carName;
    public GameObject carPrefab;
}
