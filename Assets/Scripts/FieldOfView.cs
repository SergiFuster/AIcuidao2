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

	public float meshResolution;
	public MeshFilter viewMeshFilter;
	Mesh viewMesh;

	void Start()
	{
		viewMesh = new Mesh();
		viewMesh.name = "View Mesh";
		viewMeshFilter.mesh = viewMesh;
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

    private void LateUpdate()
    {
		DrawFieldOfView();
    }
    void FindVisibleTargets()
	{
		targetDetected = false;
		float dstToTarget = (target.position - transform.position).magnitude;
		if(dstToTarget <= viewRadius)
        {
			Vector3 dirToTarget = (target.position - transform.position).normalized;
			if (Vector3.Angle(transform.up, dirToTarget) < viewAngle / 2)
			{

				if (!Physics2D.Raycast(transform.position, dirToTarget, dstToTarget, obstacleMask))
				{
					targetDetected = true;
				}
			}
		}
	}

	void DrawFieldOfView()
    {
		int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
		float stepAngleSize = viewAngle / stepCount;
		List<Vector3> viewPoints = new List<Vector3>();
		for(int i = 0; i <= stepCount; i++)
        {
			float angle = -transform.eulerAngles.z - viewAngle / 2 + stepAngleSize * i;
			ViewCastInfo newViewCast = ViewCast(angle);
			viewPoints.Add(newViewCast.point);
        }

		int vertexCount = viewPoints.Count + 1;
		Vector3[] vertices = new Vector3[vertexCount];
		int[] triangles = new int[(vertexCount - 2) * 3];

		vertices[0] = Vector3.zero;

		for(int i = 0; i < vertexCount - 1; i++)
        {
			vertices[i+1] = transform.InverseTransformPoint(viewPoints[i]);

			if(i < vertexCount - 2)
            {
				triangles[i * 3] = 0;
				triangles[i * 3 + 1] = i + 1;
				triangles[i * 3 + 2] = i + 2;
            }

        }

		viewMesh.Clear();
		viewMesh.vertices = vertices;
		viewMesh.triangles = triangles;
		viewMesh.RecalculateNormals();
    }

	ViewCastInfo ViewCast(float globalAngle)
    {
		Vector3 dir = DirFromAngle(globalAngle, true);
		RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, viewRadius, obstacleMask);

		if (hit.collider != null)
        {
			return new ViewCastInfo(true, hit.point, hit.distance, globalAngle);
        }
        else
        {
			return new ViewCastInfo(false, transform.position + dir * viewRadius, viewRadius, globalAngle);
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

	public struct ViewCastInfo
    {
		public bool hit;
		public Vector2 point;
		public float dst;
		public float angle;
		public ViewCastInfo(bool _hit, Vector2 _point, float _dst, float _angle)
        {
			hit = _hit;
			point = _point;
			dst = _dst;
			angle = _angle;
        }
    }
}