using System;
using UnityEngine;

public class Slot : MonoBehaviour
{
    [SerializeField] private new Renderer render;
    [SerializeField] private Color hightLightColor;
    public CellStack CellStack { get; private set; }
    private Color baseColor;
    public bool CanPlaced => CellStack == null;
    private void Start()
    {
        baseColor = render.material.color;
    }

    public void SetCellStack(CellStack cellStack) => this.CellStack = cellStack;
    public void Selected() => render.material.color = hightLightColor;
    public void Deselected() => render.material.color = baseColor;
    

}