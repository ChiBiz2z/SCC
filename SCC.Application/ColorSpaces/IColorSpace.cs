using MathNet.Numerics.LinearAlgebra;

namespace SCC.Application.ColorSpaces;

public interface IColorSpace
{
    Matrix<double> FromRgb(Matrix<double> rgb);
    Matrix<double> ToRgb(Matrix<double> space);
}