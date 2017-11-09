using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
	new private Rigidbody rigidbody;

	public Vector3 vec = Vector3.forward;
	public float speed = 100;


	private void Start()
	{
		rigidbody = GetComponent<Rigidbody>();
	}

	private void Update()
	{
		rigidbody.velocity = vec * speed;
	}

	private void OnCollisionEnter(Collision collider)
	{
		Destroy(gameObject);
	}
}
