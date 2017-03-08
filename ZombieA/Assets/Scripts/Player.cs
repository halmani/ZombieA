using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
	private NavMeshAgent agent;	
	private Transform trans;
	private Transform camTrans;

	public Camera playerCamera;
	public InputController leftInput;
	public InputController rightInput;
	public float transSpeed = 0f;
	public float jumpSpeed = 0f;
	public float rotSpeed = 0f;
	public float highPitchAngle = 80f;
	public float lowPitchAngle = -70f;


	public Text debugText;

	private void Start()
	{
		trans = transform;
		camTrans = playerCamera.transform;
		agent = GetComponent<NavMeshAgent>();
		Debug.Log("player is me");
	}
	
	private void Update()
	{
		// カメラ
		var rot = GetRotation();
		MoveAtLock(rot);

		// 平行移動
		var shift = GetShift();
		var jump = GetJump();
		MoveTranslation(shift, jump);
	}

	// -----------------------------------------------------------------
	Vector3 GetRotation()
	{
		Vector3 v = Vector3.zero;

		if (0 < v.magnitude)
			return v;
		return rightInput.DragDirection;
	}

	Vector3 GetShift()
	{
		var v = Vector3.zero;
		if (Input.GetKey(KeyCode.D))
			v.x += 1.0f;
		if (Input.GetKey(KeyCode.A))
			v.x -= 1.0f;
		if (Input.GetKey(KeyCode.W))
			v.z += 1.0f;
		if (Input.GetKey(KeyCode.S))
			v.z -= 1.0f;

		if (0 < v.magnitude)
			return v;
		return leftInput.DragDirection2Dto3D;
	}

	Vector3 GetJump()
	{
		var v = Vector3.zero;
		if (Input.GetKeyDown(KeyCode.Space))
		{
			Debug.Log("Input.GetKeyDown(KeyCode.Space)");
			v.y += 1;
		}
		if (0 < v.magnitude)
			return v;
		return Vector3.zero;
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

	// 実際の移動
	private void MoveTranslation(Vector3 shift, Vector3 jump)
	{
		MoveShift(shift, transSpeed);
		MoveShift(jump, jumpSpeed);
	}

	private void MoveShift(Vector3 shift, float speed)
	{
		if (0 < shift.magnitude)
		{
			var dir = Quaternion.AngleAxis(trans.eulerAngles.y, Vector3.up) * shift;
			agent.Move(dir.normalized * speed * Time.deltaTime);
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
}
