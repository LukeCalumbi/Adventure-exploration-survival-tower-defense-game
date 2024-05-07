using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class SquareMatrix<T>
{
    List<T> values;
    int n;
    T defaultValue;

    public SquareMatrix(int n, T defaultValue)
    {
        values = new List<T>(n*n);
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
                values.Add(defaultValue);

        this.defaultValue = defaultValue;
        this.n = n;
    }

    public T GetValue(int i, int j)
    {
        if (!IsPositionValid(i, j))
            throw new System.Exception("Position Out of Matrix Bounds");

        return values[j * n + i];
    }

    public void SetValue(int i, int j, T value)
    {
        if (!IsPositionValid(i, j))
            throw new System.Exception("Position Out of Matrix Bounds");

        values[j * n + i] = value;
    }

    public List<MatrixPosition> GetNeighbourPositions(int i, int j)
    {
        if (!IsPositionValid(i, j))
            throw new System.Exception("Position Out of Matrix Bounds");

        List<MatrixPosition> positions = new List<MatrixPosition> {
            new MatrixPosition(i + 1, j),
            new MatrixPosition(i, j - 1),
            new MatrixPosition(i - 1, j),
            new MatrixPosition(i, j + 1)
        }; 

        return new List<MatrixPosition>(positions.Where((MatrixPosition position) => position.IsValid(n)));
    }

    public List<MatrixPosition> GetAllPositions()
    {
        List<MatrixPosition> matrixPositions = new List<MatrixPosition>(n*n);
        for (int j = 0; j < n; j++)
            for (int i = 0; i < n; i++)
                    matrixPositions.Add(new MatrixPosition(i, j));

        return matrixPositions;
    }

    public Dictionary<int, T> GetLineValues(int j)
    {
        Dictionary<int, T> pairs = new Dictionary<int, T>();
        for (int i = 0; i < n; i++)
            pairs.Add(i, GetValue(i, j));

        return pairs;
    }

    public void SetLine(int j, T value)
    {
        for (int i = 0; i < n; i++)
            SetValue(i, j, value);
    }

    public Dictionary<int, T> GetColumnValues(int i)
    {
        Dictionary<int, T> pairs = new Dictionary<int, T>();
        for (int j = 0; j < n; j++)
            pairs.Add(j, GetValue(i, j));

        return pairs;
    }

    public void SetColumn(int i, T value)
    {
        for (int j = 0; j < n; j++)
            SetValue(i, j, value);
    }

    public bool IsPositionValid(int i, int j)
    {
        return i >= 0 && i < n && j >= 0 && j < n;
    }

    public SquareMatrix<T> DeepCopy()
    {
        SquareMatrix<T> copy = new SquareMatrix<T>(n, defaultValue);
        for (int j = 0; j < n; j++) 
        {
            for (int i = 0; i < n; i++)
            {
                T value = values[j * n + i];
                copy.SetValue(i, j, value);
            }
        }

        return copy;
    }

    public override string ToString()
    {
        String line = "";

        for (int j = 0; j < n; j++) 
        {
            for (int i = 0; i < n; i++)
            {
                line = String.Format("{0} {1}", line, GetValue(i, j));
            }

            line = String.Format("{0}\n", line);
        }

        return line;
    }
}

public class MatrixPosition
{
    public int i;
    public int j;

    public MatrixPosition(int i, int j)
    {
        this.i = i;
        this.j = j;
    }

    public bool IsValid(int n)
    {
        return i >= 0 && i < n && j >= 0 && j < n;
    }
}
