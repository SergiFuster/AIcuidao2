using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FieldOfView : MonoBehaviour
{

	public float viewRadius;
	[Range(0, 360)]
	public float viewAngle;
	public Transform target;
	public bool targetDetected = false;

	public LayerMask targetMask;
	public LayerMask obstacleMask;

	void Start()
	{
		StartCoroutine("FindTargetsWithDelay", .2f);
	}


	IEnumerator FindTargetsWithDelay(float delay)
	{
		while (true)
		{
			yield return new WaitForSeconds(delay);
			FindVisibleTargets();
		}
	}

	void FindVisibleTargets()
	{
		targetDetected = false;
		float dstToTarget = (target.position - transform.position).magnitude;
		Debug.Log(dstToTarget + "-" + viewRadius);
		if(dstToTarget <= viewRadius)
        {
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
			{

				if (!Physics.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					targetDetected = true;
				}
			}
		}
	}


	public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
	{
		if (!angleIsGlobal)
		{
			angleInDegrees += -transform.eulerAngles.z;
		}
		return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), Mathf.Cos(angleInDegrees * Mathf.Deg2Rad), 0);
	}
}