using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using MathNet.Numerics.LinearAlgebra.Double;
using Matrix = System.Drawing.Drawing2D.Matrix;

namespace ColorSpaceConverter;

public static class ColorConvertor
{
    public static void BitMapRunner(Bitmap image)
    {
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var rgb = RgbFromZeroToOne(new[] { pixel.R, pixel.G, pixel.B });

                var lms = RgbToLms(rgb);

                var lab = LmsToLab(lms);
                var lms2 = LabToLms(lab);


                var finalRgb = RgbNormalizer(LmsToRgb(lms2));

                image.SetPixel(x, y, Color.FromArgb(finalRgb.R, finalRgb.G, finalRgb.B));
            }
        }
    }

    public static Matrix<double> RgbToLms(Matrix<double> rgb) => ColorSpaceConstants.RgbToLms.Multiply(rgb);

    public static Matrix<double> LmsToLab(Matrix<double> lms)
    {
        for (int i = 0; i < 3; i++)
            if (lms[i, 0] == 0)
                lms[i, 0] = lms[i, 0] < 0.01176 ? 0.01176 : lms[i, 0];

        return ColorSpaceConstants.LmsToLab.Multiply(Matrix<double>.Log10(lms));
    }

    public static Matrix<double> LabToLms(Matrix<double> lab)
    {
        var lmsComma = ColorSpaceConstants.LabToLms.Multiply(lab);

        var lms = Matrix<double>.Build.DenseOfArray(new[,]
        {
            { Math.Pow(10, lmsComma[0, 0]) },
            { Math.Pow(10, lmsComma[1, 0]) },
            { Math.Pow(10, lmsComma[2, 0]) },
        });

        return lms;
    }

    public static Matrix<double> LmsToRgb(Matrix<double> lms) => ColorSpaceConstants.LmsToRgb.Multiply(lms);

    public static Matrix<double> RgbFromZeroToOne(byte[] rgb) => Matrix<double>.Build.DenseOfArray(new[,]
    {
        { rgb[0] / 255.0 * 0.92157 },
        { rgb[1] / 255.0 * 0.92157 },
        { rgb[2] / 255.0 * 0.92157 },
    });

    public static (byte R, byte G, byte B) RgbNormalizer(Matrix<double> rgb)
    {
        var arrRgb = rgb.ToArray();
        for (int i = 0; i < arrRgb.GetLength(0); i++)
        {
            // arrRgb[i, 0] = arrRgb[i, 0] < 0.01176 ? 0.01176 : arrRgb[i, 0];
            // arrRgb[i, 0] = arrRgb[i, 0] > 0.92157 ? 0.92157 : arrRgb[i, 0];
            arrRgb[i, 0] *= 255;
        }

        return (
            (byte)arrRgb[0, 0],
            (byte)arrRgb[1, 0],
            (byte)arrRgb[2, 0]
        );
    }
}