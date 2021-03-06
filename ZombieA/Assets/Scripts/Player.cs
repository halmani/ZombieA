﻿using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class Player : Character
{
	new private Rigidbody rigidbody;
	private Transform trans;
	private Transform camTrans;
	private bool pushGunButton = false;

	public Camera playerCamera;
	public InputController leftInput;
	public InputController rightInput;
	public float transSpeed = 0f;
	public float dashSpeed = 1f;
	public float jumpSpeed = 0f;
	public float rotSpeed = 0f;
	public float highPitchAngle = 80f;
	public float lowPitchAngle = -70f;

	public Weapon weapon;

	public Text debugText;

	// ------------------------------------------------------------------------------------------
	private void Start()
	{
		trans = transform;
		camTrans = playerCamera.transform;
		rigidbody = GetComponent<Rigidbody>();
//		Debug.Log("player is me");
	}

	private void Update()
	{
		// カメラ
		var rot = GetRotation();
		MoveAtLock(rot);

		// 平行移動
		var shift = GetShift();
		MoveTranslation(shift);

		// 発射
		if (Input.GetKeyDown(KeyCode.Space) || pushGunButton)
		{
			weapon.Shot();
			pushGunButton = false;
		}
	}

	// -----------------------------------------------------------------
	public void PushGunButton()
	{
		pushGunButton = true;
	}

	private Vector3 GetRotation()
	{
		var v = Vector3.zero;

		v.x =  Input.GetAxis("Horizontal_R") ;
		v.y = -Input.GetAxis("Vertical_R");

		if (0 < v.magnitude)
			return v;
		return rightInput.DragDirection;
	}

	private Vector3 GetShift()
	{
		var v = Vector3.zero;
		v.x = Input.GetAxis("Horizontal");
		if (v.x != 0)
		{
//			Debug.Log("x");
			if (Input.GetKey(KeyCode.D))
				v.x += 1.0f;
			if (Input.GetKey(KeyCode.A))
				v.x -= 1.0f;
		}
		v.z = Input.GetAxis("Vertical");
		if (v.z != 0)
		{
//			Debug.Log("z");
			if (Input.GetKey(KeyCode.W))
				v.z += 1.0f;
			if (Input.GetKey(KeyCode.S))
				v.z -= 1.0f;
		}
		if (0 < v.magnitude)
			return v;
		return leftInput.DragDirection2Dto3D;
	}

	// 実際のカメラ回転
	private void MoveAtLock(Vector3 rot)
	{
		// カメラ
		if (0 < rot.magnitude)
		{
			// y軸回転(キャラごと)
			var yaw = (new Vector3(0, rot.x, 0) * rotSpeed * Time.deltaTime);
			trans.Rotate( yaw );

			// x軸回転
			var pitch = CalcPitch(new Vector3(-rot.y, 0, 0));
			CameraRotate(pitch);

			if (debugText != null)
			{
				debugText.text = "Yaw " + yaw + "\n";
				debugText.text += "Pitch " + pitch + "\n";
			}
		}
	}

	private float GetShiftSpeed()
	{
		if (Input.GetKey(KeyCode.LeftShift))
			return dashSpeed;
		return transSpeed;
	}

	// 実際の移動
	private void MoveTranslation(Vector3 shift)
	{
		MoveShift(shift, GetShiftSpeed());
	}

	private void MoveShift(Vector3 shift, float speed)
	{
		if (0 < shift.magnitude)
		{
			var dir = Quaternion.AngleAxis(trans.eulerAngles.y, Vector3.up) * shift;
			var vec = dir.normalized * speed;
			rigidbody.velocity = vec;
		}
		else
		{
			rigidbody.velocity = Vector3.zero;
		}
	}

	private Vector3 CalcPitch(Vector3 pitch)
	{
		var nextPitch = pitch * rotSpeed * 0.5f * Time.deltaTime;
		return nextPitch;
	}

	private void CameraRotate(Vector3 pitch)
	{
		camTrans.Rotate( pitch );
		var euler = camTrans.localEulerAngles;
		if(euler.x <= 180f)
		{
			euler.x = Mathf.Clamp(euler.x, 0f, -lowPitchAngle);
		}
		if(180f < euler.x)
		{
			euler.x = Mathf.Clamp(euler.x, 360f-highPitchAngle, 360f);
		}
		euler.y = 0;
		euler.z = 0;
		camTrans.localEulerAngles = euler;
	}

	private void OnTriggerEnter(Collider collider)
	{
		var item = collider.gameObject.GetComponent<Item>();
		if (item != null)
		{
			switch (item.type)
			{
				case Item.Type.Bullet:
					weapon.Add(item.quantity);
					break;
				default:
					break;
			}
			Destroy(item.gameObject);
		}
	}
}
