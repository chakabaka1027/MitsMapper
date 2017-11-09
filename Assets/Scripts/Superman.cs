using UnityEngine;
using System.Collections;
using UnityStandardAssets.Characters.FirstPerson;

public class Superman : MonoBehaviour {

	//initial speed
	public float speed;
	public Camera cam;
	public GameObject homeLoc;

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

		//press key to reset camera position to selected null
		if(Input.GetKey(KeyCode.R))
		{
			cam.transform.position = homeLoc.transform.position;
			cam.transform.rotation = homeLoc.transform.rotation;
		}

		//press shift to move faster
		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			speed = 300; 

		}
		else
		{
			//if shift is not pressed, reset to default speed
			speed = 100; 
		}
		//For the following 'if statements' don't include 'else if', so that the user can press multiple buttons at the same time
		//move camera to the left
		if(Input.GetKey(KeyCode.A))
		{
			cam.transform.position = cam.transform.position + cam.transform.right *-1 * speed * Time.deltaTime;
		}

		//move camera backwards
		if(Input.GetKey(KeyCode.S))
		{
			cam.transform.position = cam.transform.position + cam.transform.forward *-1 * speed * Time.deltaTime;

		}
		//move camera to the right
		if(Input.GetKey(KeyCode.D))
		{
			cam.transform.position = cam.transform.position + cam.transform.right * speed * Time.deltaTime;

		}
		//move camera forward
		if(Input.GetKey(KeyCode.W))
		{

			cam.transform.position = cam.transform.position + cam.transform.forward * speed * Time.deltaTime;
		}
		//move camera upwards
		if(Input.GetKey(KeyCode.Q))
		{
			cam.transform.position = cam.transform.position + cam.transform.up * speed * Time.deltaTime;
		}
		//move camera downwards
		if(Input.GetKey(KeyCode.Z))
		{
			cam.transform.position = cam.transform.position + cam.transform.up * -1 *speed * Time.deltaTime;
		}

	}
}