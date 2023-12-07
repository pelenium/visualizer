using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using OfficeOpenXml;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Visualizer
{
    public partial class MainForm : Form
    {
        private static Bitmap graphicResult;
        private static Graphics graphic;
        private static FileInfo fileInfo;

        public MainForm()
        {
            InitializeComponent();
        }

        private void LoadBtn_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = "Text files(*.xlsx)|*.xlsx|All files(*.*)|*.*";

            if (openFileDialog.ShowDialog() == DialogResult.Cancel)
                return;

            string filename = openFileDialog.FileName;

            fileInfo = new FileInfo(filename);
        }

        private void DrawButton_Click(object sender, EventArgs e)
        {
            try
            {
                ExcelPackage.LicenseContext = LicenseContext.NonCommercial;
                using (var excelPackage = new ExcelPackage(fileInfo))
                {
                    string list = ListName.Text;
                    ExcelWorksheet worksheet = excelPackage.Workbook.Worksheets[list];

                    List<List<Point>> points1 = new List<List<Point>>();
                    List<List<Point>> points2 = new List<List<Point>>();
                    List<List<Point>> points3 = new List<List<Point>>();

                    int current = 0;
                    if (worksheet != null)
                    {
                        try
                        {
                            Console.WriteLine(worksheet.Dimension.Rows);
                            Console.WriteLine();
                            for (int row = 2; row <= worksheet.Dimension.Rows; row++)
                            {
                                Console.WriteLine(worksheet.Cells[row, 1].Value?.ToString());

                                if (worksheet.Cells[row, 1].Value?.ToString().Trim(' ') == "/")
                                {
                                    Console.WriteLine("found");
                                    Console.WriteLine("found");
                                    Console.WriteLine("found");
                                    current++;
                                    continue;
                                }

                                switch (current)
                                {
                                    case 0:
                                        if (points1.Count == 0 || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value?.ToString()))
                                        {
                                            points1.Add(new List<Point>());
                                            continue;
                                        }
                                        break;
                                    case 1:
                                        if (points2.Count == 0 || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value?.ToString()))
                                        {
                                            points2.Add(new List<Point>());
                                            continue;
                                        }
                                        break;
                                    case 2:
                                        if (points3.Count == 0 || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value?.ToString()))
                                        {
                                            points3.Add(new List<Point>());
                                            continue;
                                        }
                                        break;
                                }
                                switch (current)
                                {
                                    case 0:
                                        points1[points1.Count - 1].Add(new Point((int)float.Parse(worksheet.Cells[row, 1].Value?.ToString()), (int)float.Parse(worksheet.Cells[row, 2].Value?.ToString())));
                                        break;
                                    case 1:
                                        points2[points2.Count - 1].Add(new Point((int)float.Parse(worksheet.Cells[row, 1].Value?.ToString()), (int)float.Parse(worksheet.Cells[row, 2].Value?.ToString())));
                                        break;
                                    case 2:
                                        points3[points3.Count - 1].Add(new Point((int)float.Parse(worksheet.Cells[row, 1].Value?.ToString()), (int)float.Parse(worksheet.Cells[row, 2].Value?.ToString())));
                                        break;
                                }
                            }
                        }
                        catch (Exception ex1)
                        {
                            Console.WriteLine(ex1);
                        }
                    }
                    else
                    {
                        MessageBox.Show(
                            "Incorrect Excel list",
                            "Error",
                            MessageBoxButtons.OK,
                            MessageBoxIcon.Error,
                            MessageBoxDefaultButton.Button1,
                            MessageBoxOptions.DefaultDesktopOnly);
                        return;
                    }

                    points1.RemoveAll(i => i.Count == 0);
                    points2.RemoveAll(i => i.Count == 0);
                    points3.RemoveAll(i => i.Count == 0);

                    Console.WriteLine();
                    Console.WriteLine("AAAAAAAAAAAAAAAAAAAAAAAAA");
                    Console.WriteLine();

                    foreach (var i in points2)
                    {
                        Console.WriteLine(i.Count);
                        foreach (var j in i)
                        {
                            Console.WriteLine($"{j.X} {j.Y}");
                        }
                        Console.WriteLine();
                    }

                    var graphicResult1 = drawPoints(points1, false, 2);
                    var graphicResult2 = drawPoints(points2, false, 2);
                    var graphicResult3 = drawPoints(points3, false, 2);

                    graphicResult1.graphic.Save("result1.png");
                    graphicResult2.graphic.Save("result2.png");
                    graphicResult3.graphic.Save("result3.png");

                    var img1 = Image.FromFile("result1.png");
                    var img2 = Image.FromFile("result2.png");
                    var img3 = Image.FromFile("result3.png");

                    graphic.DrawImage(img1, new Point(0, 0));
                    graphic.DrawImage(img2, new Point(0, 0));
                    graphic.DrawImage(img3, new Point(0, 0));

                    ResultGraphic.Image = graphicResult;
                }
            }
            catch (Exception ex2)
            {
                Console.WriteLine(ex2);
            }
        }

        private static Graphic drawPoints(List<List<Point>> points, bool shouldFill, int n)
        {
            graphicResult = new Bitmap(1000, 800);

            graphic = Graphics.FromImage(graphicResult);

            int maxX = 0, maxY = 0, minX = 0, minY = 0;
            points = ConvertToCoordinateSystem(points, ref maxX, ref maxY, ref minX, ref minY);

            if (shouldFill)
            {
                Console.WriteLine(points.Count);
                for (int i = 0; i < points.Count; i++)
                {
                    if (i == 0 || i == points.Count - 1)
                    {
                        for (int j = 0; j < points[i].Count; j++)
                        {
                            if (j < points[i].Count - 1)
                            {
                                graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][j].X, points[i][j].Y, points[i][j + 1].X, points[i][j + 1].Y);
                            }
                        }
                        if (i == 0)
                        {
                            graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][0].X, points[i][0].Y, points[i + 1][0].X, points[i + 1][0].Y);
                            graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][points[i].Count - 1].X, points[i][points[i].Count - 1].Y, points[i + 1][points[i + 1].Count - 1].X, points[i + 1][points[i + 1].Count - 1].Y);
                        }
                    }
                    else
                    {
                        Console.WriteLine(i);
                        Console.WriteLine(points[i + 1][0]);
                        graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][0].X, points[i][0].Y, points[i + 1][0].X, points[i + 1][0].Y);
                        graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][points[i].Count - 1].X, points[i][points[i].Count - 1].Y, points[i + 1][points[i + 1].Count - 1].X, points[i + 1][points[i + 1].Count - 1].Y);
                    }
                }

                FloodFill(graphicResult, maxX - (maxX - minX) / 2, maxY - (maxY - minY) / 2, Color.FromArgb(128, Color.Black));

                for (int i = 0; i < points.Count; i++)
                {
                    for (int j = 0; j < points[i].Count; j++)
                    {
                        if (j < points[i].Count - 1)
                        {
                            graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black), (float)2.5), points[i][j].X, points[i][j].Y, points[i][j + 1].X, points[i][j + 1].Y);
                        }
                    }
                }
            }
            else
            {
                /* 
                TODO:
                Исправить баг с соединением точек (узнать точный вариант соединения)                
                */

                switch (n)
                {
                    case 1:
                        for (int i = 0; i < points.Count - 1; i++)
                        {
                            Console.WriteLine(points[i].Count);
                            Console.WriteLine(points[i + 1].Count);
                            for (int j = 0; j < points[i].Count; j++)
                            {
                                Console.WriteLine($"i - {i}, j - {j}");
                                graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black)), points[i][j].X, points[i][j].Y, points[i + 1][j].X, points[i + 1][j].Y);
                            }
                        }
                        break;
                    case 2:
                        Console.WriteLine(points.Count);
                        for (int i = 0; i < points.Count; i++)
                        {
                            for (int j = 0; j < points[i].Count - 1; j++)
                            {
                                graphic.DrawLine(new Pen(Color.FromArgb(128, Color.Black)), points[i][j].X, points[i][j].Y, points[i][j+1].X, points[i][j+1].Y);
                            }
                        }
                        break;

                }

            }

            return new Graphic(maxX, maxY, minX, minY, graphicResult);
        }

        public static List<List<Point>> ConvertToCoordinateSystem(List<List<Point>> points, ref int MaxX, ref int MaxY, ref int MinX, ref int MinY)
        {
            List<List<Point>> convertedPoints = new List<List<Point>>();
            float maxX = int.MinValue;
            float maxY = int.MinValue;

            float minX = int.MaxValue;
            float minY = int.MaxValue;

            foreach (var line in points)
            {
                foreach (var point in line)
                {
                    if (point.X > maxX)
                    {
                        maxX = point.X;
                    }

                    if (point.Y > maxY)
                    {
                        maxY = point.Y;
                    }

                    if (point.X < minX)
                    {
                        minX = point.X;
                    }

                    if (point.Y < minY)
                    {
                        minY = point.Y;
                    }
                }
            }

            float kx = 1000 / maxX;
            float ky = 800 / maxY;

            for (int i = 0; i < 5; i++)
            {
                graphic.DrawLine(new Pen(Color.FromArgb(255, Color.Black), (float)2.5), 1000 / 5 * i, 800, 1000 / 5 * i, 795);
                graphic.DrawLine(new Pen(Color.FromArgb(255, Color.Black), (float)2.5), 0, 800 - (800 / 5 * i), 5, 800 - (800 / 5 * i));
                if (i != 0)
                {
                    graphic.DrawString((maxX / 5 * i).ToString(), new Font("Times New Roman", 12, FontStyle.Regular), new SolidBrush(Color.Black), 1000 / 5 * i - 12, 770);
                    graphic.DrawString((maxX / 5 * (5 - i)).ToString(), new Font("Times New Roman", 12, FontStyle.Regular), new SolidBrush(Color.Black), 15, 800 / 5 * i - 9);
                }
            }

            maxX *= kx;
            maxY *= ky;
            minX *= kx;
            minY *= ky;

            MaxX = (int)maxX;
            MaxY = (int)maxY;
            MinX = (int)minX;
            MinY = (int)minY;

            foreach (var line in points)
            {
                List<Point> converted = new List<Point>();
                foreach (var point in line)
                {
                    int scaledX = (int)(point.X * kx - minX / 2);
                    int scaledY = 800 - (int)(point.Y * ky - minY / 2);

                    converted.Add(new Point(scaledX, scaledY));
                }

                convertedPoints.Add(converted);
            }

            return convertedPoints;
        }
        private static void FloodFill(Bitmap bitmap, int x, int y, Color color)
        {
            BitmapData data = bitmap.LockBits(new Rectangle(0, 0, bitmap.Width, bitmap.Height), ImageLockMode.ReadWrite, PixelFormat.Format32bppArgb);
            int[] bits = new int[data.Stride / 4 * data.Height];
            Marshal.Copy(data.Scan0, bits, 0, bits.Length);

            LinkedList<Point> check = new LinkedList<Point>();
            int floodTo = color.ToArgb();
            int floodFrom = bits[x + y * data.Stride / 4];
            bits[x + y * data.Stride / 4] = floodTo;

            if (floodFrom != floodTo)
            {
                check.AddLast(new Point(x, y));
                while (check.Count > 0)
                {
                    Point cur = check.First.Value;
                    check.RemoveFirst();

                    foreach (Point off in new Point[] { new Point(0, -1), new Point(0, 1), new Point(-1, 0), new Point(1, 0) })
                    {
                        Point next = new Point(cur.X + off.X, cur.Y + off.Y);
                        if (next.X >= 0 && next.Y >= 0 &&
                            next.X < data.Width &&
                            next.Y < data.Height)
                        {
                            if (bits[next.X + next.Y * data.Stride / 4] == floodFrom)
                            {
                                check.AddLast(next);
                                bits[next.X + next.Y * data.Stride / 4] = floodTo;
                            }
                        }
                    }
                }
            }

            Marshal.Copy(bits, 0, data.Scan0, bits.Length);
            bitmap.UnlockBits(data);
        }

        private void ResultGraphic_Click(object sender, EventArgs e)
        {

        }
    }
}
