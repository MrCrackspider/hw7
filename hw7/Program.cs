//#undef DEBUG

using System;
using System.IO;


namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            // Reading input.txt

            // p
            StreamReader reader = new StreamReader("input3.txt");
            string temp = reader.ReadLine();
            temp = temp.Replace('.', ',');
#if DEBUG
            Console.WriteLine(temp);
#endif

            // N
            double p = Convert.ToDouble(temp);
            temp = reader.ReadLine();
            int N = int.Parse(temp);
#if DEBUG
            Console.WriteLine(N);
#endif

            // reding points
            double[] x = new double[N];
            double[] y = new double[N];
            double[] z = new double[N];
            string[] temp1 = new string[3];
            for (int i = 0; i < N; i++)
            {
                temp = reader.ReadLine();
                temp = temp.Replace('.', ',');
                temp1 = temp.Split('\t');
                x[i] = Convert.ToDouble(temp1[0]);
                y[i] = Convert.ToDouble(temp1[1]);
                z[i] = Convert.ToDouble(temp1[2]);
#if DEBUG
                Console.WriteLine(x[i] + "\t" + y[i] + "\t" + z[i]);
#endif
            }

            Random rnd = new Random();
            double[,] list = new double[5, 10]; // в этом массиве будет список параметров ABCD, из них будет выбираться наиболее удачные
            double A = 0, B = 0, C = 0, D = 0;
            double Ratio = 0; // кол-во удовлетворительных результатов при подстановке точек в уравнение плоскости

            // 3 рандомные точки
            double X1 = 0;
            double Y1 = 0;
            double Z1 = 0;
            double X2 = 0;
            double Y2 = 0;
            double Z2 = 0;
            double X3 = 0;
            double Y3 = 0;
            double Z3 = 0;

            // индексы рандомных точек
            int Ind1 = 0;
            int Ind2 = 0;
            int Ind3 = 0;

            // кол-во рассматриваемых плоскостей уравнения которых удовлетворяют [-p, p]
            int Accuracy = 10;

            for (int i = 0; i < Accuracy;)
            {
                // get random point
                Ind1 = rnd.Next(0, N);
                Ind2 = rnd.Next(0, N);
                Ind3 = rnd.Next(0, N);

                // если
                // совпадают координаты Х у всех 3х точек И совпадают координаты Y у всех 3х точек ИЛИ
                // совпадают координаты Х у всех 3х точек И совпадают координаты Z у всех 3х точек ИЛИ
                // совпадают координаты Y у всех 3х точек И совпадают координаты Z у всех 3х точек ИЛИ
                // совпадают индексы
                while (((x[Ind1] == x[Ind2] && x[Ind1] == x[Ind3]) && (y[Ind1] == y[Ind2] && y[Ind1] == y[Ind3])) ||
                       ((x[Ind1] == x[Ind2] && x[Ind1] == x[Ind3]) && (z[Ind1] == z[Ind2] && z[Ind1] == z[Ind3])) ||
                       ((y[Ind1] == y[Ind2] && y[Ind1] == y[Ind3]) && (z[Ind1] == z[Ind2] && z[Ind1] == z[Ind3])) ||
                       (Ind1 == Ind2) || (Ind1 == Ind3) || (Ind2 == Ind3) || (A == B && A == C && A == D))
                {
                    Ind1 = rnd.Next(0, N);
                    Ind2 = rnd.Next(0, N);
                    Ind3 = rnd.Next(0, N);
                    X1 = x[Ind1];
                    Y1 = y[Ind1];
                    Z1 = z[Ind1];
                    X2 = x[Ind2];
                    Y2 = y[Ind2];
                    Z2 = z[Ind2];
                    X3 = x[Ind3];
                    Y3 = y[Ind3];
                    Z3 = z[Ind3];
                    A = Math.Round(Y1 * (Z2 - Z3) + Y2 * (Z3 - Z1) + Y3 * (Z1 - Z2), 6);
                    B = Math.Round(Z1 * (X2 - X3) + Z2 * (X3 - X1) + Z3 * (X1 - X2), 6);
                    C = Math.Round(X1 * (Y2 - Y3) + X2 * (Y3 - Y1) + X3 * (Y1 - Y2), 6);
                    D = Math.Round(-(X1 * (Y2 * Z3 - Y3 * Z2) + X2 * (Y3 * Z1 - Y1 * Z3) + X3 * (Y1 * Z2 - Y2 * Z1)), 6);
#if DEBUG
                    Console.WriteLine("first point: " + X1 + " " + Y1 + " " + Z1);
                    Console.WriteLine("second point: " + X2 + " " + Y2 + " " + Z2);
                    Console.WriteLine("third point: " + X3 + " " + Y3 + " " + Z3);
#endif
                }

#if DEBUG
                Console.WriteLine("A: " + A + ", B: " + B + ", C: " + C + ", D: " + D);
#endif
                Ratio = 0;
                for (int j = 0; j < N; j++)
                {
#if DEBUG
                    Console.WriteLine(Math.Round(A * x[j] + B * y[j] + C * z[j] + D, 6));
#endif
                    if (A * x[j] + B * y[j] + C * z[j] + D >= -p && A * x[j] + B * y[j] + C * z[j] + D <= p)
                    {
                        Ratio++;
                    }
                }
#if DEBUG
                Console.WriteLine("amount: " + Ratio + "\n");
#endif
                if (Ratio / N > 0.5)
                { 
                    list[0, i] = A;
                    list[1, i] = B;
                    list[2, i] = C;
                    list[3, i] = D;
                    list[4, i] = Ratio;
                    i++;
                }
            }
#if DEBUG
            for (int k = 0; k < 5; k++)
            {
                for (int m = 0; m < 10; m++)
                {
                    Console.Write(list[k, m] + "\t");
                }
                Console.WriteLine();
            }
#endif
            double Optimal = list[4, 0];
            int OptimalIndex = 0;
            for (int i = 0; i < Accuracy; i++)
            {
                if (list[4, i] > Optimal)
                {
                    Optimal = list[4, i];
                    OptimalIndex = i;
                }
            }
#if DEBUG
            Console.WriteLine("Optimal: " + Optimal + "\n");
#endif
            Console.WriteLine(list[0, OptimalIndex] + "\t" + list[1, OptimalIndex] + "\t" + 
                              list[2, OptimalIndex] + "\t" + list[3, OptimalIndex]);
            Console.ReadKey();
        }
    }
}