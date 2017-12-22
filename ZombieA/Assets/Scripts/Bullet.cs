using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	new private Rigidbody rigidbody;

	public Vector3 vec = Vector3.forward;


	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
		rigidbody.AddForce(vec);
	}

	private void OnCollisionEnter(Collision collider)
	{
		Destroy(gameObject);
	}
}
