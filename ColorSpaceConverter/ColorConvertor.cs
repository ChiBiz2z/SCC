using MathNet.Numerics.LinearAlgebra;

namespace ColorSpaceConverter;

public static class ColorConvertor
{
    public static Matrix<double> RgbToLms(Matrix<double> rgb) => ColorSpaceConstants.RgbToLms.Multiply(rgb);

    public static Matrix<double> LmsToLab(Matrix<double> lms)
    {
        for (int i = 0; i < 3; i++)
            if (lms[i, 0] == 0)
                lms[i, 0] = lms[i, 0] < 0.01176 ? 0.01176 : lms[i, 0];

        return ColorSpaceConstants.LmsToLab.Multiply(Matrix<double>.Log10(lms));
    }

    public static Matrix<double> LabToLms(Matrix<double> lab) =>
        ColorSpaceConstants.LabToLms.Multiply(lab).Map(x => Math.Pow(10, x));


    public static Matrix<double> LmsToRgb(Matrix<double> lms) => ColorSpaceConstants.LmsToRgb.Multiply(lms);

    public static Matrix<double> RgbFromZeroToOne(byte[] rgb) => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { rgb[0] / 255.0 },
        { rgb[1] / 255.0 },
        { rgb[2] / 255.0 },
    });

    public static (byte R, byte G, byte B) RgbNormalizer(Matrix<double> rgb)
    {
        var arrRgb = rgb.ToArray();
        for (int i = 0; i < arrRgb.GetLength(0); i++)
        {
            if (arrRgb[i, 0] < 0.01176)
                arrRgb[i, 0] = 0.01176;

            if (arrRgb[i, 0] > 0.92157)
                arrRgb[i, 0] = 0.92157;

            arrRgb[i, 0] *= 255;
        }

        return (
            (byte)arrRgb[0, 0],
            (byte)arrRgb[1, 0],
            (byte)arrRgb[2, 0]
        );
    }
}