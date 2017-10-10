using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskParticle : MonoBehaviour {


    private RectTransform maskRect;
    Vector3[] corners = new Vector3[4];

    private void Awake() {
        maskRect = GetComponent<RectTransform>();
        InitCorners();
    }

    private void OnEnable() {
        TraverseParticles(transform);
    }

    private void InitCorners() {
        maskRect.GetWorldCorners(corners);
    }

    private void TraverseParticles(Transform parent) {
        foreach (Transform child in parent) {
            SetCorners(child);
            TraverseParticles(child);
        }
    }

    private void SetCorners(Transform child) {
        if (corners.Length == 0) return;

        Renderer r = child.GetComponent<Renderer>();
        if (r == null) return;

        Material material = r.material;
        if (material == null) return;

        if (! material.shader.name.Equals("Custom/MaskAdditive")) return;

        material.SetFloat("_MinX", corners[0].x);
        material.SetFloat("_MaxX", corners[2].x);
        material.SetFloat("_MinY", corners[0].y);
        material.SetFloat("_MaxY", corners[2].y);
    }
}
 