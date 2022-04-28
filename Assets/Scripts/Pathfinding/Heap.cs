using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class Heap<T> where T : IHeapItem<T>
{
    List<T> items;

    public Heap()
    {
        items = new List<T>();
    }

    public List<T> GetAll()
    {
        return items;
    }

    public void Add(T item)
    {
        item.HeapIndex = items.Count;
        items.Add(item);
        SortUp(item);
    }

    public T RemoveFirst()
    {
        T firstItem = items[0];
        items[0] = items[items.Count-1];
        items[0].HeapIndex = 0;
        items.RemoveAt(items.Count-1);
        if(items.Count > 0)
            SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(T item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return items.Count;
        }
    }

    public bool Contains(T item)
    {
        return items.Contains(item);
    }

    private void SortUp(T item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            T parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
                Swap(item, parentItem);
            else
                break;
        }
    }

    private void SortDown(T item)
    {
        while (true)
        {
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int childIndexRight = item.HeapIndex * 2 + 2;
            int swapIndex = 0;

            if (childIndexLeft < items.Count)
            {
                swapIndex = childIndexLeft;

                if (childIndexRight < items.Count && items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    swapIndex = childIndexRight;

                if (item.CompareTo(items[swapIndex]) < 0)
                    Swap(item, items[swapIndex]);
                else
                    return;
            }
            else
                return;
        }
    }

    private void Swap(T itemA, T itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;

        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

public interface IHeapItem<T> : IComparable<T>
{
    int HeapIndex
    {
        get;
        set;
    }
}