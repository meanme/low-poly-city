using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class TrafficManager : MonoBehaviour {

    public Transform StartTransform;
    public Transform EndTransform;

    [System.Serializable]
    public class TrafficWaypoints
    {
        public Transform Start;
        public Transform End;
        public float Rotation;
    }

    public Transform CarsRoot;
    public List<GameObject> Cars;

    public List<TrafficWaypoints> Waypoints;

    public delegate void OnSpawnCarEvent(string resourcePath);
    public static event OnSpawnCarEvent OnSpawnCar; 

	// Use this for initialization
	void Start () {
        OnSpawnCar += SpawnCarCallback;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public static void SpawnCar(string resourcePath)
    {
        OnSpawnCar(resourcePath);
    }

    private void SpawnCarCallback(string resourcePath)
    {
        int waypointsIndex = Random.Range(0, Waypoints.Count);
        Transform start = Waypoints[waypointsIndex].Start;
        Transform end = Waypoints[waypointsIndex].End;


        
        var spawnedCar = Instantiate((GameObject)Resources.Load(resourcePath), start.localPosition, Quaternion.identity);
        spawnedCar.transform.Rotate(Vector3.up, Waypoints[waypointsIndex].Rotation);
        spawnedCar.transform.parent = CarsRoot.transform;

        Vector3[] path = { start.position, end.position };
        spawnedCar.transform.DOPath(path, 7)
            .SetEase(Ease.Linear)
            .OnComplete(() => OnTweenComplete(spawnedCar));
    }

    private void OnTweenComplete(GameObject spawnedCar)
    {
        Destroy(spawnedCar);
    }
}
