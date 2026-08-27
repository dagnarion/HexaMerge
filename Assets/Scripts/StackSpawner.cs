using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using NaughtyAttributes;
using UnityEngine;
using Random = UnityEngine.Random;

public class StackSpawner : MonoBehaviour
{
    [SerializeField] private Transform[] spawnPosition;
    [SerializeField] private Color[] color;
    [SerializeField] private CellStack cellStackPrefab;
    [SerializeField] private Cell cellPrefab;
    [SerializeField] private DragHandler dragHandler;
    [MinMaxSlider(2, 8), SerializeField] private Vector2Int range;
    private int count = 0;

    private void Start()
    {
        Spawn();
    }

    private void OnEnable()
    {
        dragHandler.OnPlaced += SpawnCell;
    }

    private void OnDisable()
    {
        dragHandler.OnPlaced += SpawnCell;
    }

    public void SpawnCell(Vector3Int pos,CellStack currentStack)
    {
        count++;
        if (count >= 3)
        {
            count = 0;
            Spawn();
        }
    }
    
    
    private void Spawn()
    {
        List<CellStack> cellStacks = new List<CellStack>();
        for (int i = 0; i < spawnPosition.Length; i++)
        {
            spawnPosition[i].Clear();
            CellStack stacks = Instantiate<CellStack>(cellStackPrefab, spawnPosition[i].position, Quaternion.identity);
            stacks.SetParent(spawnPosition[i]);
            cellStacks.Add(stacks);
        }

        Color[] colorHolder = getRandColour();
        for (int i = 0; i < cellStacks.Count; i++)
        {
            int amount = Random.Range(range.x, range.y);
            int ratio = Random.Range(0, amount);
            for (int j = 0; j < amount; j++)
            {
                
                Cell cell = Instantiate<Cell>(cellPrefab, spawnPosition[i].position, Quaternion.identity);
                cellStacks[i].Add(cell);
                cell.SetParent(cellStacks[i].transform);
                cell.transform.position = spawnPosition[i].position.With(y: spawnPosition[i].position.y + .2f * j);
                cell.color = (ratio < j) ? colorHolder[0] : colorHolder[1];
            }
        }
    }

    private Color[] getRandColour()
    {
        List<Color> colors = new List<Color>();
        colors.AddRange(color);
        if (colors.Count <= 0)
        {
            Debug.LogError("There wasn't have color in holder");
            return null;
        }

        int index = Random.Range(0, colors.Count);
        Color firstcolor = colors[index];
        colors.RemoveAt(index);

        if (colors.Count <= 0)
        {
            Debug.LogError("There weren't have enough color");
            return null;
        }
        
        index = Random.Range(0, colors.Count);
        Color secondcolor = colors[index];
        colors.RemoveAt(index);
        
        return new Color[]{firstcolor,secondcolor};
    }
}