using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class B03_Matrix3x3
{
    const int size = 3;
    float[,] matrix = new float[size, size];

    public B03_Matrix3x3()
    {
        // Sets all vars to 0
        for (int x = 0; x < size; ++x)
            for (int y = 0; y < size; ++y)
                matrix[x, y] = 0;
        // Sets [0][0], [1][1], [2][2] to 1
        for (int i = 0; i < size; ++i)
            matrix[i, i] = 1;
    }

    // This function sets the matrix equal to the identity matrix.
    public B03_Matrix3x3 Identity()
    {
        B03_Matrix3x3 result = new B03_Matrix3x3();
        // Sets all vars to 0
        for (int x = 0; x < size; ++x)
            for (int y = 0; y < size; ++y)
                result.matrix[x,y] = 0;
        // Sets [0][0], [1][1], [2][2] to 1
        for (int i = 0; i < size; ++i)
            result.matrix[i, i] = 1;

        return result;
    }

    // This function calculates the transpose matrix of Mtx and saves it in the result matrix.
    // (NOTE: Care must be taken when pResult = pMtx.)
    public B03_Matrix3x3 Transpose()
    {
        // Creates a new matrix;
        B03_Matrix3x3 result = new B03_Matrix3x3();

        // Sets [0][0], [1][1], [2][2] to the right thing
        for (int i = 0; i < size; ++i)
            result.matrix[i, i] = matrix[i, i];

        // Changes the first row with the first col
        result.matrix[0, 1] = matrix[1, 0];
        result.matrix[0, 2] = matrix[2, 0];
        // Changes the second row with the second col
        result.matrix[1, 0] = matrix[0, 1];
        result.matrix[1, 2] = matrix[2, 1];
        // Changes the thrid row with the thrid col
        result.matrix[2, 0] = matrix[0, 2];
        result.matrix[2, 1] = matrix[1, 2];
        // Sets the matrices equal

        return result;
    }

    public static B03_Matrix3x3 operator +(B03_Matrix3x3 lhs, B03_Matrix3x3 rhs)
    {
        B03_Matrix3x3 result = new B03_Matrix3x3();

        result.matrix[0, 0] = lhs.matrix[0, 0] + rhs.matrix[0, 0];
        result.matrix[0, 1] = lhs.matrix[0, 1] + rhs.matrix[0, 1];
        result.matrix[0, 2] = lhs.matrix[0, 2] + rhs.matrix[0, 2];

        result.matrix[1, 0] = lhs.matrix[1, 0] + rhs.matrix[1, 0];
        result.matrix[1, 1] = lhs.matrix[1, 1] + rhs.matrix[1, 1];
        result.matrix[1, 2] = lhs.matrix[1, 2] + rhs.matrix[1, 2];

        result.matrix[2, 0] = lhs.matrix[2, 0] + rhs.matrix[2, 0];
        result.matrix[2, 1] = lhs.matrix[2, 1] + rhs.matrix[2, 1];
        result.matrix[2, 2] = lhs.matrix[2, 2] + rhs.matrix[2, 2];

        return result;
    }

    public static B03_Matrix3x3 operator *(float lhs, B03_Matrix3x3 rhs)
    {
        B03_Matrix3x3 result = new B03_Matrix3x3();

        result.matrix[0, 0] = lhs * rhs.matrix[0, 0];
        result.matrix[0, 1] = lhs * rhs.matrix[0, 1];
        result.matrix[0, 2] = lhs * rhs.matrix[0, 2];

        result.matrix[1, 0] = lhs * rhs.matrix[1, 0];
        result.matrix[1, 1] = lhs * rhs.matrix[1, 1];
        result.matrix[1, 2] = lhs * rhs.matrix[1, 2];

        result.matrix[2, 0] = lhs * rhs.matrix[2, 0];
        result.matrix[2, 1] = lhs * rhs.matrix[2, 1];
        result.matrix[2, 2] = lhs * rhs.matrix[2, 2];

        return result;
    }

    public static B03_Matrix3x3 operator *(B03_Matrix3x3 lhs, B03_Matrix3x3 rhs)
    {
        // Creates a new matrix
        B03_Matrix3x3 result = new B03_Matrix3x3();

        result.matrix[0, 0]
            = lhs.matrix[0, 0] * rhs.matrix[0, 0]
            + lhs.matrix[0, 1] * rhs.matrix[1, 0]
            + lhs.matrix[0, 2] * rhs.matrix[2, 0];
        result.matrix[0, 1]
            = lhs.matrix[0, 0] * rhs.matrix[0, 1]
            + lhs.matrix[0, 1] * rhs.matrix[1, 1]
            + lhs.matrix[0, 2] * rhs.matrix[2, 1];
        result.matrix[0, 2]
            = lhs.matrix[0, 0] * rhs.matrix[0, 2]
            + lhs.matrix[0, 1] * rhs.matrix[1, 2]
            + lhs.matrix[0, 2] * rhs.matrix[2, 2];
        result.matrix[1, 0]
            = lhs.matrix[1, 0] * rhs.matrix[0, 0]
            + lhs.matrix[1, 1] * rhs.matrix[1, 0]
            + lhs.matrix[1, 2] * rhs.matrix[2, 0];
        result.matrix[1, 1]
            = lhs.matrix[1, 0] * rhs.matrix[0, 1]
            + lhs.matrix[1, 1] * rhs.matrix[1, 1]
            + lhs.matrix[1, 2] * rhs.matrix[2, 1];
        result.matrix[1, 2]
            = lhs.matrix[1, 0] * rhs.matrix[0, 2]
            + lhs.matrix[1, 1] * rhs.matrix[1, 2]
            + lhs.matrix[1, 2] * rhs.matrix[2, 2];
        result.matrix[2, 0]
            = lhs.matrix[2, 0] * rhs.matrix[0, 0]
            + lhs.matrix[2, 1] * rhs.matrix[1, 0]
            + lhs.matrix[2, 2] * rhs.matrix[2, 0];
        result.matrix[2, 1]
            = lhs.matrix[2, 0] * rhs.matrix[0, 1]
            + lhs.matrix[2, 1] * rhs.matrix[1, 1]
            + lhs.matrix[2, 2] * rhs.matrix[2, 1];
        result.matrix[2, 2]
            = lhs.matrix[2, 0] * rhs.matrix[0, 2]
            + lhs.matrix[2, 1] * rhs.matrix[1, 2]
            + lhs.matrix[2, 2] * rhs.matrix[2, 2];
        // Sets the result equal to the actual result
        return result;
    }

    public void ReducedRowEchelon()
    {
        // if the matrix is a zero matrix
        bool no_zero = false;
        for (int x = 0; x < size; ++x)
            for (int y = 0; y < size; ++y)
                if (matrix[x, y] != 0) no_zero = true;
        if (!no_zero) return;
    }

    void rowSwap(int row0, int row1)
    {
        B03_Matrix3x3 result = new B03_Matrix3x3();

        result.matrix[0, row0] = matrix[0, row1];
        result.matrix[1, row0] = matrix[1, row1];
        result.matrix[2, row0] = matrix[2, row1];

        result.matrix[0, row1] = matrix[0, row0];
        result.matrix[1, row1] = matrix[1, row0];
        result.matrix[2, row1] = matrix[2, row0];

        matrix[0, row0] = result.matrix[0, row0];
        matrix[1, row0] = result.matrix[1, row0];
        matrix[2, row0] = result.matrix[2, row0];

        matrix[0, row1] = result.matrix[0, row1];
        matrix[1, row1] = result.matrix[1, row1];
        matrix[2, row1] = result.matrix[2, row1];
    }

    void mRow(float scalar, int row)
    {
        matrix[0, row] = scalar * matrix[0, row];
        matrix[1, row] = scalar * matrix[1, row];
        matrix[2, row] = scalar * matrix[2, row];
    }

    void mRowAdd(float scalar, int row0, int row1)
    {
        matrix[0, row1] = scalar * matrix[0, row0] + matrix[0, row1];
        matrix[1, row1] = scalar * matrix[1, row0] + matrix[1, row1];
        matrix[2, row1] = scalar * matrix[2, row0] + matrix[2, row1];
    }
}
