using ColorSpaceConverter;
using MathNet.Numerics.LinearAlgebra;

namespace SCC.Application.ColorSpaces;

public class LabColorSpace : IColorSpace
{
    public Matrix<double> FromRgb(Matrix<double> rgb) => ColorConvertor.LmsToLab(ColorConvertor.RgbToLms(rgb));

    public Matrix<double> ToRgb(Matrix<double> space) => ColorConvertor.LmsToRgb(ColorConvertor.LabToLms(space));
}