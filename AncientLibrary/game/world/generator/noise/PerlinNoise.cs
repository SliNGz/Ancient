using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ancient.game.world.generator.noise
{
    public class PerlinNoise
    {
        private int seed;
        private int repeat;

        private int[] permutation = new int[256];
        private int[] p;

        public PerlinNoise(int seed, int repeat = -1)
        {
            this.seed = seed;
            this.repeat = repeat;

            Random rand = new Random(seed);

            for (int i = 0; i < permutation.Length; i++)
            {
                permutation[i] = -1;
            }

            for (int i = 0; i < permutation.Length; i++)
            {
                while (true)
                {
                    int index = rand.Next() % 256;
                    if (permutation[index] == -1)
                    {
                        permutation[index] = i;
                        break;
                    }
                }
            }

            p = new int[512];

            for (int x = 0; x < 512; x++)
            {
                p[x] = permutation[x % 256];
            }
        }

        public double OctavePerlin(double x, double y, double z, int octaves, double persistence)
        {
            double total = 0;
            double frequency = 1;
            double amplitude = 1;
            double maxValue = 0;

            for (int i = 0; i < octaves; i++)
            {
                total += perlin(x * frequency, y * frequency, z * frequency) * amplitude;

                maxValue += amplitude;

                amplitude *= persistence;
                frequency *= 2;
            }

            return total / maxValue;
        }

        public double perlin(double x, double y, double z)
        {
            if (repeat > 0)
            {
                x = x % repeat;
                y = y % repeat;
                z = z % repeat;
            }

            int xi = (int)x & 255;
            int yi = (int)y & 255;
            int zi = (int)z & 255;
            double xf = x - (int)x;
            double yf = y - (int)y;

            double zf = z - (int)z;
            double u = fade(xf);
            double v = fade(yf);
            double w = fade(zf);

            int aaa, aba, aab, abb, baa, bba, bab, bbb;
            aaa = p[p[p[xi] + yi] + zi];
            aba = p[p[p[xi] + inc(yi)] + zi];
            aab = p[p[p[xi] + yi] + inc(zi)];
            abb = p[p[p[xi] + inc(yi)] + inc(zi)];
            baa = p[p[p[inc(xi)] + yi] + zi];
            bba = p[p[p[inc(xi)] + inc(yi)] + zi];
            bab = p[p[p[inc(xi)] + yi] + inc(zi)];
            bbb = p[p[p[inc(xi)] + inc(yi)] + inc(zi)];

            double x1, x2, y1, y2;
            x1 = lerp(grad(aaa, xf, yf, zf),
                        grad(baa, xf - 1, yf, zf),
                        u);
            x2 = lerp(grad(aba, xf, yf - 1, zf),
                        grad(bba, xf - 1, yf - 1, zf),
                          u);
            y1 = lerp(x1, x2, v);

            x1 = lerp(grad(aab, xf, yf, zf - 1),
                        grad(bab, xf - 1, yf, zf - 1),
                        u);
            x2 = lerp(grad(abb, xf, yf - 1, zf - 1),
                          grad(bbb, xf - 1, yf - 1, zf - 1),
                          u);
            y2 = lerp(x1, x2, v);

            return (lerp(y1, y2, w) + 1) / 2;
        }

        public int inc(int num)
        {
            num++;
            if (repeat > 0) num %= repeat;

            return num;
        }

        public static double grad(int hash, double x, double y, double z)
        {
            int h = hash & 15;
            double u = h < 8 ? x : y;

            double v;

            if (h < 4)
                v = y;
            else if (h == 12 || h == 14)
                v = x;
            else
                v = z;

            return ((h & 1) == 0 ? u : -u) + ((h & 2) == 0 ? v : -v);
        }

        public static double fade(double t)
        {
            return t * t * t * (t * (t * 6 - 15) + 10);
        }

        public static double lerp(double a, double b, double x)
        {
            return a + x * (b - a);
        }
    }
}