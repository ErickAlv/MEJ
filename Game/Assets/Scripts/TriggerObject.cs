using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerObject : MonoBehaviour {
    public string gizmo;
    public Color color;

	private void OnDrawGizmos()
    {
        Gizmos.DrawIcon(transform.position, gizmo);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position, transform.localScale);
    }
}
