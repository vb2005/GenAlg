using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApp8
{
    public partial class Form1 : Form
    {
        // Генерация случайных чисел
        Random rnd = new Random(DateTime.Now.Millisecond);

        // Точки из файла
        List<Point> points = new List<Point>();

        // Картинка с точками
        Bitmap bmp = new Bitmap(@"C:\1\1.png");

        // Текущая популяция
        List<Genome> genomes = new List<Genome>();

        public Form1()
        {
            InitializeComponent();
            pictureBox1.Image = bmp;
            for (int x = 0; x < bmp.Width; x++)
                for (int y = 0; y < bmp.Height; y++)
                    if (bmp.GetPixel(x, y).G < 128)
                        points.Add(new Point(x, y));
        }

        struct Genome
        {
            public double X0 { get; set; }
            public double Y0 { get; set; }
            public double R { get; set; }

            public static Genome Cross(Genome p1, Genome p2)
            {
                Genome p3 = new Genome();

                p3.R = (p1.R + p2.R) / 2.0;
                p3.X0 = (p1.X0 + p2.X0) / 2.0;
                p3.Y0 = (p1.Y0 + p2.Y0) / 2.0;

                return p3;
            }
        }

        double Loss(Genome g)
        {
            return points.Sum(p =>
            Math.Pow(
            Math.Pow(g.X0 - p.X, 2) +
            Math.Pow(g.Y0 - p.Y, 2) -
            g.R * g.R, 2));
        }

        private void button1_Click(object sender, EventArgs e)
        {
            // Создание начальной популяции
            for (int i = 0; i < 1000; i++)
                genomes.Add(new Genome()
                {
                    R = rnd.Next(-10000,10000),
                    X0 = rnd.Next(-10000, 10000),
                    Y0 = rnd.Next(-10000, 10000)
                });

            Application.Idle += Application_Idle;
        }

        private void Application_Idle(object sender, EventArgs e)
        {
            // Сортировка выборка гибель
            genomes = genomes
                .OrderBy(p => Loss(p))
                .Take(500)
                .ToList();

            // Скрещивание
            for (int i = 0; i < 500; i++)
                genomes.Add(Genome.Cross(
                    genomes[rnd.Next(500)],
                    genomes[rnd.Next(500)]
                    ));

            pictureBox2.Image = bmp;
            foreach (var p in genomes)
            {
                pictureBox2.CreateGraphics().DrawEllipse(
                    Pens.Red,
                    (int)(p.X0 - p.R),
                    (int)(p.Y0 - p.R),
                    (int)(p.R * 2), 
                    (int)(p.R * 2));
            }

            System.Threading.Thread.Sleep(200);
        }
    }



}
