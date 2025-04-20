using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Welick
{
    public partial class Form1 : Form
    {
        bool f = true;
        bool isTandem = false;
        int k, l;
        int[,] kv = new int[90, 3];
        int[,] matr_sdv = new int[3, 3];
        private double wheelRotationAngle = 0;
        private const int wheelRadius = 48;
        private const int spokeCount = 8;

        public Form1()
        {
            InitializeComponent();
        }

        private void ClearDrawing()
        {
            pictureBox1.Image = null;
            pictureBox1.Refresh();
        }

        private void Init_matr_preob(int k1, int l1)
        {
            matr_sdv[0, 0] = 1; matr_sdv[0, 1] = 0; matr_sdv[0, 2] = 0;
            matr_sdv[1, 0] = 0; matr_sdv[1, 1] = 1; matr_sdv[1, 2] = 0;
            matr_sdv[2, 0] = k1; matr_sdv[2, 1] = l1; matr_sdv[2, 2] = 1;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            timer1.Interval = 100;
            button3.Text = "Стоп";
            if (f == true)
            {
                timer1.Start();
            }
            else
            {
                timer1.Stop();
                button3.Text = "Старт";
            }
            f = !f;
        }

        private int[,] Multiply_matr(int[,] a, int[,] b)
        {
            int n = a.GetLength(0);
            int m = a.GetLength(1);
            int p = b.GetLength(1);

            int[,] r = new int[n, p];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < p; j++)
                {
                    r[i, j] = 0;
                    for (int k = 0; k < m; k++)
                    {
                        r[i, j] += a[i, k] * b[k, j];
                    }
                }
            }
            return r;
        }

        private void Init_Bicycle()
        {
            int shiftX = -165;

            // Рама велосипеда
            kv[0, 0] = -33 + shiftX; kv[0, 1] = -7; kv[0, 2] = 1;
            kv[1, 0] = 20 + shiftX; kv[1, 1] = -60; kv[1, 2] = 1;
            kv[2, 0] = 128 + shiftX; kv[2, 1] = -60; kv[2, 2] = 1;
            kv[3, 0] = 25 + shiftX; kv[3, 1] = 19; kv[3, 2] = 1;
            kv[4, 0] = 143 + shiftX; kv[4, 1] = -7; kv[4, 2] = 1;
            kv[5, 0] = 120 + shiftX; kv[5, 1] = -98; kv[5, 2] = 1;
            kv[6, 0] = 102 + shiftX; kv[6, 1] = -104; kv[6, 2] = 1;

            // Седло
            kv[7, 0] = 30 + shiftX; kv[7, 1] = 18; kv[7, 2] = 1;
            kv[8, 0] = 17 + shiftX; kv[8, 1] = -70; kv[8, 2] = 1;
            kv[9, 0] = 5 + shiftX; kv[9, 1] = -69; kv[9, 2] = 1;
            kv[10, 0] = 31 + shiftX; kv[10, 1] = -75; kv[10, 2] = 1;
            kv[11, 0] = 9 + shiftX; kv[11, 1] = -73; kv[11, 2] = 1;

            // Центр педального узла (удален, оставлены только педали)
            int pedalCenterX = 28 + shiftX;
            int pedalCenterY = 0;

            // Исходные координаты педалей (относительно центра)
            int pedalWidth = 40;
            int pedalHeight = 14;

            // Верхняя горизонтальная линия педали
            kv[30, 0] = pedalCenterX - pedalWidth/2; kv[30, 1] = pedalCenterY - pedalHeight/2; kv[30, 2] = 1;
            kv[31, 0] = pedalCenterX + pedalWidth/2; kv[31, 1] = pedalCenterY - pedalHeight/2; kv[31, 2] = 1;

            // Нижняя горизонтальная линия педали
            kv[32, 0] = pedalCenterX - pedalWidth/2; kv[32, 1] = pedalCenterY + pedalHeight/2; kv[32, 2] = 1;
            kv[33, 0] = pedalCenterX + pedalWidth/2; kv[33, 1] = pedalCenterY + pedalHeight/2; kv[33, 2] = 1;

            // Диагональные соединения
            kv[34, 0] = pedalCenterX - pedalWidth/2; kv[34, 1] = pedalCenterY - pedalHeight/2; kv[34, 2] = 1;
            kv[35, 0] = pedalCenterX + pedalWidth/2; kv[35, 1] = pedalCenterY + pedalHeight/2; kv[35, 2] = 1;

            kv[36, 0] = pedalCenterX + pedalWidth/2; kv[36, 1] = pedalCenterY - pedalHeight/2; kv[36, 2] = 1;
            kv[37, 0] = pedalCenterX - pedalWidth/2; kv[37, 1] = pedalCenterY + pedalHeight/2; kv[37, 2] = 1;

            // Матрица вращения
            double[,] rotationMatrix = CreateRotationMatrix(wheelRotationAngle);

            // Вращаем педали вокруг центра
            for (int i = 30; i <= 37; i++)
            {
                // Переносим в начало координат
                double x = kv[i, 0] - pedalCenterX;
                double y = kv[i, 1] - pedalCenterY;
                double w = 1;

                // Применяем вращение
                double newX = x * rotationMatrix[0, 0] + y * rotationMatrix[0, 1] + w * rotationMatrix[0, 2];
                double newY = x * rotationMatrix[1, 0] + y * rotationMatrix[1, 1] + w * rotationMatrix[1, 2];
                double newW = x * rotationMatrix[2, 0] + y * rotationMatrix[2, 1] + w * rotationMatrix[2, 2];

                // Возвращаем на место
                kv[i, 0] = pedalCenterX + (int)newX;
                kv[i, 1] = pedalCenterY + (int)newY;
                kv[i, 2] = 1;
            }

            // Если это тандем
            if (isTandem)
            {
                // Удлиняем раму
                kv[2, 0] = 228 + shiftX;

                // Добавляем вторую раму
                kv[50, 0] = 128 + shiftX; kv[50, 1] = -60; kv[50, 2] = 1;
                kv[51, 0] = 228 + shiftX; kv[51, 1] = -60; kv[51, 2] = 1;
                kv[52, 0] = 125 + shiftX; kv[52, 1] = 19; kv[52, 2] = 1;

                // Второе седло
                kv[53, 0] = 130 + shiftX; kv[53, 1] = 18; kv[53, 2] = 1;
                kv[54, 0] = 117 + shiftX; kv[54, 1] = -70; kv[54, 2] = 1;
                kv[55, 0] = 105 + shiftX; kv[55, 1] = -69; kv[55, 2] = 1;
                kv[56, 0] = 131 + shiftX; kv[56, 1] = -75; kv[56, 2] = 1;
                kv[57, 0] = 109 + shiftX; kv[57, 1] = -73; kv[57, 2] = 1;

                // Второй педальный узел (без центра)
                int tandemPedalCenterX = 130 + shiftX;
                int tandemPedalCenterY = 0;

                // Координаты педалей тандема (относительно центра)
                kv[60, 0] = tandemPedalCenterX - pedalWidth/2; kv[60, 1] = tandemPedalCenterY - pedalHeight/2; kv[60, 2] = 1;
                kv[61, 0] = tandemPedalCenterX + pedalWidth/2; kv[61, 1] = tandemPedalCenterY - pedalHeight/2; kv[61, 2] = 1;
                kv[62, 0] = tandemPedalCenterX - pedalWidth/2; kv[62, 1] = tandemPedalCenterY + pedalHeight/2; kv[62, 2] = 1;
                kv[63, 0] = tandemPedalCenterX + pedalWidth/2; kv[63, 1] = tandemPedalCenterY + pedalHeight/2; kv[63, 2] = 1;
                kv[64, 0] = tandemPedalCenterX - pedalWidth/2; kv[64, 1] = tandemPedalCenterY - pedalHeight/2; kv[64, 2] = 1;
                kv[65, 0] = tandemPedalCenterX + pedalWidth/2; kv[65, 1] = tandemPedalCenterY + pedalHeight/2; kv[65, 2] = 1;
                kv[66, 0] = tandemPedalCenterX + pedalWidth/2; kv[66, 1] = tandemPedalCenterY - pedalHeight/2; kv[66, 2] = 1;
                kv[67, 0] = tandemPedalCenterX - pedalWidth/2; kv[67, 1] = tandemPedalCenterY + pedalHeight/2; kv[67, 2] = 1;

                // Вращение педалей тандема (со смещением на 90 градусов)
                double[,] tandemRotationMatrix = CreateRotationMatrix(wheelRotationAngle + Math.PI/2);

                for (int i = 60; i <= 67; i++)
                {
                    double x = kv[i, 0] - tandemPedalCenterX;
                    double y = kv[i, 1] - tandemPedalCenterY;
                    double w = 1;

                    double newX = x * tandemRotationMatrix[0, 0] + y * tandemRotationMatrix[0, 1] + w * tandemRotationMatrix[0, 2];
                    double newY = x * tandemRotationMatrix[1, 0] + y * tandemRotationMatrix[1, 1] + w * tandemRotationMatrix[1, 2];
                    double newW = x * tandemRotationMatrix[2, 0] + y * tandemRotationMatrix[2, 1] + w * tandemRotationMatrix[2, 2];

                    kv[i, 0] = tandemPedalCenterX + (int)newX;
                    kv[i, 1] = tandemPedalCenterY + (int)newY;
                    kv[i, 2] = 1;
                }

                // Перемещаем переднее колесо и руль
                kv[4, 0] = 243 + shiftX;
                kv[5, 0] = 220 + shiftX;
                kv[6, 0] = 202 + shiftX;
            }

            // Обновляем точки колес
            UpdateWheelPoints();
        }

        private void UpdateWheelPoints()
        {
            // Центры колес
            int backCenterX = kv[0, 0];
            int backCenterY = kv[0, 1];
            int frontCenterX = kv[4, 0];
            int frontCenterY = kv[4, 1];

            // Матрица вращения
            double[,] rotationMatrix = CreateRotationMatrix(wheelRotationAngle);

            // Заднее колесо
            for (int i = 0; i < spokeCount; i++)
            {
                double angle = i * (Math.PI * 2) / spokeCount;
                double x = wheelRadius * Math.Cos(angle);
                double y = wheelRadius * Math.Sin(angle);
                double w = 1;

                double newX = x * rotationMatrix[0, 0] + y * rotationMatrix[0, 1] + w * rotationMatrix[0, 2];
                double newY = x * rotationMatrix[1, 0] + y * rotationMatrix[1, 1] + w * rotationMatrix[1, 2];
                double newW = x * rotationMatrix[2, 0] + y * rotationMatrix[2, 1] + w * rotationMatrix[2, 2];

                kv[12 + i, 0] = backCenterX + (int)newX;
                kv[12 + i, 1] = backCenterY + (int)newY;
                kv[12 + i, 2] = 1;
            }

            // Переднее колесо
            for (int i = 0; i < spokeCount; i++)
            {
                double angle = i * (Math.PI * 2) / spokeCount;
                double x = wheelRadius * Math.Cos(angle);
                double y = wheelRadius * Math.Sin(angle);
                double w = 1;

                double newX = x * rotationMatrix[0, 0] + y * rotationMatrix[0, 1] + w * rotationMatrix[0, 2];
                double newY = x * rotationMatrix[1, 0] + y * rotationMatrix[1, 1] + w * rotationMatrix[1, 2];
                double newW = x * rotationMatrix[2, 0] + y * rotationMatrix[2, 1] + w * rotationMatrix[2, 2];

                kv[20 + i, 0] = frontCenterX + (int)newX;
                kv[20 + i, 1] = frontCenterY + (int)newY;
                kv[20 + i, 2] = 1;
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            k = pictureBox1.Width / 2;
            l = pictureBox1.Height / 2;
            ClearDrawing();
            Draw_Bicycle();
        }

        private void Draw_Bicycle()
        {
            Init_Bicycle();
            Init_matr_preob(k, l);
            int[,] bicycle = Multiply_matr(kv, matr_sdv);

            Pen framePen = new Pen(Color.Blue, 3);
            Pen wheelPen = new Pen(Color.Black, 2);
            Pen spokePen = new Pen(Color.Gray, 1);
            Pen pedalPen = new Pen(Color.Blue, 2);

            Graphics g = Graphics.FromHwnd(pictureBox1.Handle);

            // Рисуем раму
            g.DrawLine(framePen, bicycle[0, 0], bicycle[0, 1], bicycle[1, 0], bicycle[1, 1]);
            g.DrawLine(framePen, bicycle[1, 0], bicycle[1, 1], bicycle[2, 0], bicycle[2, 1]);
            g.DrawLine(framePen, bicycle[2, 0], bicycle[2, 1], bicycle[3, 0], bicycle[3, 1]);
            g.DrawLine(framePen, bicycle[3, 0], bicycle[3, 1], bicycle[0, 0], bicycle[0, 1]);
            g.DrawLine(framePen, bicycle[4, 0], bicycle[4, 1], bicycle[5, 0], bicycle[5, 1]);
            g.DrawLine(framePen, bicycle[5, 0], bicycle[5, 1], bicycle[6, 0], bicycle[6, 1]);

            // Седло
            g.DrawLine(framePen, bicycle[7, 0], bicycle[7, 1], bicycle[8, 0], bicycle[8, 1]);
            g.DrawLine(framePen, bicycle[9, 0], bicycle[9, 1], bicycle[10, 0], bicycle[10, 1]);
            g.DrawLine(framePen, bicycle[10, 0], bicycle[10, 1], bicycle[11, 0], bicycle[11, 1]);
            g.DrawLine(framePen, bicycle[11, 0], bicycle[11, 1], bicycle[9, 0], bicycle[9, 1]);

            // Педали (без центра)
            g.DrawLine(pedalPen, bicycle[30, 0], bicycle[30, 1], bicycle[31, 0], bicycle[31, 1]);
            g.DrawLine(pedalPen, bicycle[32, 0], bicycle[32, 1], bicycle[33, 0], bicycle[33, 1]);
            g.DrawLine(pedalPen, bicycle[34, 0], bicycle[34, 1], bicycle[35, 0], bicycle[35, 1]);
            g.DrawLine(pedalPen, bicycle[36, 0], bicycle[36, 1], bicycle[37, 0], bicycle[37, 1]);

            // Тандем
            if (isTandem)
            {
                g.DrawLine(framePen, bicycle[1, 0], bicycle[1, 1], bicycle[50, 0], bicycle[50, 1]);
                g.DrawLine(framePen, bicycle[50, 0], bicycle[50, 1], bicycle[51, 0], bicycle[51, 1]);
                g.DrawLine(framePen, bicycle[51, 0], bicycle[51, 1], bicycle[52, 0], bicycle[52, 1]);
                g.DrawLine(framePen, bicycle[52, 0], bicycle[52, 1], bicycle[0, 0], bicycle[0, 1]);

                g.DrawLine(framePen, bicycle[53, 0], bicycle[53, 1], bicycle[54, 0], bicycle[54, 1]);
                g.DrawLine(framePen, bicycle[55, 0], bicycle[55, 1], bicycle[56, 0], bicycle[56, 1]);
                g.DrawLine(framePen, bicycle[56, 0], bicycle[56, 1], bicycle[57, 0], bicycle[57, 1]);
                g.DrawLine(framePen, bicycle[57, 0], bicycle[57, 1], bicycle[55, 0], bicycle[55, 1]);

                // Педали тандема (без центра)
                g.DrawLine(pedalPen, bicycle[60, 0], bicycle[60, 1], bicycle[61, 0], bicycle[61, 1]);
                g.DrawLine(pedalPen, bicycle[62, 0], bicycle[62, 1], bicycle[63, 0], bicycle[63, 1]);
                g.DrawLine(pedalPen, bicycle[64, 0], bicycle[64, 1], bicycle[65, 0], bicycle[65, 1]);
                g.DrawLine(pedalPen, bicycle[66, 0], bicycle[66, 1], bicycle[67, 0], bicycle[67, 1]);
            }

            // Колеса
            int backCenterX = bicycle[0, 0];
            int backCenterY = bicycle[0, 1];
            int frontCenterX = bicycle[4, 0];
            int frontCenterY = bicycle[4, 1];

            // Заднее колесо
            for (int i = 0; i < spokeCount; i++)
            {
                int nextIdx = (i == spokeCount - 1) ? 12 : 13 + i;
                g.DrawLine(wheelPen, bicycle[12 + i, 0], bicycle[12 + i, 1], bicycle[nextIdx, 0], bicycle[nextIdx, 1]);
                g.DrawLine(spokePen, backCenterX, backCenterY, bicycle[12 + i, 0], bicycle[12 + i, 1]);
            }

            // Переднее колесо
            for (int i = 0; i < spokeCount; i++)
            {
                int nextIdx = (i == spokeCount - 1) ? 20 : 21 + i;
                g.DrawLine(wheelPen, bicycle[20 + i, 0], bicycle[20 + i, 1], bicycle[nextIdx, 0], bicycle[nextIdx, 1]);
                g.DrawLine(spokePen, frontCenterX, frontCenterY, bicycle[20 + i, 0], bicycle[20 + i, 1]);
            }

            g.Dispose();
            framePen.Dispose();
            wheelPen.Dispose();
            spokePen.Dispose();
            pedalPen.Dispose();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            pictureBox1.Image = null;
        }

        private void timer1_Tick_1(object sender, EventArgs e)
        {
            k += 5;
            wheelRotationAngle += 0.1;
            ClearDrawing();
            Draw_Bicycle();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            isTandem = !isTandem;
            button4.Text = isTandem ? "Обычный велосипед" : "Тандем";
            ClearDrawing();
            Draw_Bicycle();
        }

        // Матрица вращения 3x3
        private double[,] CreateRotationMatrix(double angle)
        {
            double cosA = Math.Cos(angle);
            double sinA = Math.Sin(angle);

            return new double[3, 3]
            {
                { cosA, -sinA, 0 },
                { sinA, cosA, 0 },
                { 0, 0, 1 }
            };
        }
    }
}