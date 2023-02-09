using System.Drawing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.LinearAlgebra.Complex.Solvers;
using MathNet.Numerics.LinearAlgebra.Double;

namespace ColorSpaceConverter;

public static class ColorConvertor
{
    public static void RgbToLab(Bitmap image)
    {
        for (int x = 0; x < image.Width; x++)
        {
            for (int y = 0; y < image.Height; y++)
            {
                var pixel = image.GetPixel(x, y);

                var lms = Matrix<double>.Build.DenseOfArray(new[,]
                          {
                              { 0.3811, 0.5783, 0.0402 },
                              { 0.1967, 0.7244, 0.0782 },
                              { 0.0241, 0.1288, 0.8444 }
                          })
                          *
                          Matrix<double>.Build.DenseOfArray(new[,]
                          {
                              { pixel.R / 255.0 },
                              { pixel.G / 255.0 },
                              { pixel.B / 255.0 }
                          });

                var log10Lms = new double[3];
                for (int i = 0; i < 3; i++)
                {
                    if (lms[i, 0] == 0)
                    {
                        lms[i, 0] = lms[i, 0] < 0.01176 ? 0.01176 : lms[i, 0];
                    }
                    log10Lms[i] = Math.Log10(lms[i, 0]);
                }

                var lab = Matrix<double>.Build.DenseOfArray(new[,]
                          {
                              { 0.5774, 0, 0 },
                              { 0, 0.4082, 0 },
                              { 0, 0, 0.7071 }
                          })
                          *
                          Matrix<double>.Build.DenseOfArray(new double[,]
                          {
                              { 1, 1, 1 },
                              { 1, 1, -2 },
                              { 1, -1, 0 }
                          })
                          *
                          Matrix<double>.Build.DenseOfArray(new[,]
                          {
                              { log10Lms[0] },
                              { log10Lms[1] },
                              { log10Lms[2] }
                          });
            }
        }
    }
}