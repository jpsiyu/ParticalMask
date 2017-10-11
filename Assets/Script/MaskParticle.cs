using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaskParticle : MonoBehaviour {


    private RectTransform maskRect;
    Vector3[] corners = new Vector3[4];

    private void Awake() {
        InitCorners();
    }

    private void OnEnable() {
        TraverseParticles(transform);
    }

    private void FindMask() {
        Transform start = transform;
        Transform parent;
        Mask mask;
        while (maskRect == null) {
            parent = start.parent;
            if (parent == null) break;
            mask = parent.GetComponent<Mask>();
            if (mask == null) {
                start = parent;
            }
            else {
                maskRect = parent.GetComponent<RectTransform>();
            }
        }
    }

    private void InitCorners() {
        FindMask();
        if (maskRect == null) return;
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
        if (maskRect == null) return;

        Renderer r = child.GetComponent<Renderer>();
        if (r == null) return;

        Material material = r.material;
        if (material == null) return;

        if (!material.shader.name.Equals("Custom/MaskAdditive")) {
            material.shader = Shader.Find("Custom/MaskAdditive");
        }

        material.SetFloat("_MinX", corners[0].x);
        material.SetFloat("_MaxX", corners[2].x);
        material.SetFloat("_MinY", corners[0].y);
        material.SetFloat("_MaxY", corners[2].y);
    }
}
 