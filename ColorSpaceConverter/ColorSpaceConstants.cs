using MathNet.Numerics.LinearAlgebra;

namespace ColorSpaceConverter;

public static class ColorSpaceConstants
{
    /// <summary>
    /// Need to be multiplied with log10(LMS)
    /// </summary>
    public static Matrix<double> LmsToLab => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { 0.5774, 0.5774, 0.5774 },
        { 0.4082, 0.4082, -0.8164 },
        { 0.7071, -0.7071, 0 }
    });

    /// <summary>
    /// Need to be multiplied with Lab and then
    /// l = 10 ^ l'
    /// m = 10 ^ m'
    /// s = 10 ^ s'
    /// </summary>
    public static Matrix<double> LabToLms => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { 0.5774, 0.4082, 0.7071 },
        { 0.5774, 0.4082, -1.4142 },
        { 0.5774, -0.4082, 0 }
    });

    /// <summary>
    /// Need to be multiplied with Rgb
    /// </summary>
    public static Matrix<double> RgbToLms => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { 0.3811, 0.5783, 0.0402 },
        { 0.1967, 0.7244, 0.0782 },
        { 0.0241, 0.1288, 0.8444 }
    });

    /// <summary>
    /// Need to be multiplied with LMS
    /// </summary>
    public static Matrix<double> LmsToRgb => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { 4.4679, -3.5873, 0.1193 },
        { -1.2186, 2.3809, -0.1624 },
        { 0.0497, -0.2439, 1.2045 }
    });
}