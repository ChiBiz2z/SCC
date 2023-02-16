using ColorSpaceConverter;
using MathNet.Numerics.LinearAlgebra;

namespace SCC.Application.ColorSpaces;

public class HslColorSpace : IColorSpace
{
    public Matrix<double> FromRgb(Matrix<double> rgb) => ColorConvertor.RgbToHsl(rgb);

    public Matrix<double> ToRgb(Matrix<double> space) => ColorConvertor.HslToRgb(space);
}