using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Calibration : MonoBehaviour {

    public GameObject map;

    public Transform referencePoint1;
    public Transform referencePoint2;

    Transform point1;
    Transform point2;

    public string point1ID;
    public string point2ID;

    float referenceDistance;
    float distance;

	// Use this for initialization
	void Start () {
        Calibrate();
	}
	
    void Calibrate(){
        point1 = GameObject.Find(point1ID).transform;
        point2 = GameObject.Find(point2ID).transform;

		Scale();
        Align();
        Rotate();
        Align();
    }

    void Scale(){
        referenceDistance = Vector3.Distance(referencePoint1.position, referencePoint2.position);
        distance = Vector3.Distance(point1.position, point2.position);

        float scaleFactor = distance / referenceDistance;
        map.transform.localScale *= scaleFactor;
    }

    void Align() {
        Vector3 direction = map.transform.position - referencePoint1.position;
        map.transform.position = point1.position + direction;    
    }

    void Rotate(){
        Vector3 mapReferenceVector = referencePoint1.position - referencePoint2.position;
        Vector3 mitsVector = point1.position - point2.position;

        float angle = Vector3.Angle(mapReferenceVector, mitsVector);
        Vector3 cross = Vector3.Cross(mapReferenceVector, mitsVector);
        
        if(cross.y < 0){
            angle = -angle;
        }
        transform.Rotate(new Vector3(0, angle, 0));
    }

}