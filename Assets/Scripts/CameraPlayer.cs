using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraPlayer : MonoBehaviour
{

	public Transform target;

	public float smoothSpeed = 0.9f;
	public Vector3 offset;

	void Update()
	{
		Vector3 desiredPosition = new Vector3(target.position.x, target.position.y, transform.position.z);//target.position + offset;
		Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed);

		transform.position = Vector3.Lerp(transform.position, desiredPosition, Time.deltaTime * smoothSpeed);

		//transform.LookAt(target);
	}

}