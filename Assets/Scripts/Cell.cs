using UnityEngine;

public class Cell : MonoBehaviour
{
    [SerializeField] private new Renderer render;
    [SerializeField] private Collider collider;
    public Color color
    {
        get { return render.material.color; }
        set { render.material.color = value; }
    }

    public void SetParent(Transform parent) => transform.SetParent(parent);

    public void SetPosition(Vector3 position)
    {
        transform.position = position; // sau dottwen vao
    }
    public void UnSelect() => collider.enabled = false;
    public void CanSelect() => collider.enabled = true;
    
    
}