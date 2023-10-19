using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Drawing;
using OfficeOpenXml;
using System.Drawing.Text;

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

                    List<List<Point>> points = new List<List<Point>>();
                    if (worksheet != null)
                    {
                        try
                        {
                            for (int row = 1; row <= worksheet.Dimension.Rows; row++)
                            {
                                if (points.Count == 0 || string.IsNullOrWhiteSpace(worksheet.Cells[row, 1].Value?.ToString()))
                                {
                                    points.Add(new List<Point>());
                                }
                                else
                                {
                                    points[points.Count - 1].Add(new Point((int)float.Parse(worksheet.Cells[row, 1].Value?.ToString()), (int)float.Parse(worksheet.Cells[row, 2].Value?.ToString())));
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

                    graphicResult = new Bitmap(ResultGraphic.Width, ResultGraphic.Height);

                    graphic = Graphics.FromImage(graphicResult);
                    graphic.Clear(Color.White);

                    Console.WriteLine(points.Count);

                    points = ConvertToCoordinateSystem(points);
                    List<Point> lineEnding = new List<Point>();
                    for (int i = 0; i < points.Count; i++)
                    {
                        for (int j = 0; j < points[i].Count; j++)
                        {
                            if (j < points[i].Count - 1)
                            {
                                graphic.DrawLine(new Pen(Color.Black, (float)2.5), points[i][j].X, points[i][j].Y, points[i][j + 1].X, points[i][j + 1].Y);
                                graphic.DrawLine(new Pen(Color.Black, (float)2.5), points[i][j].X, points[i][j].Y, points[i][j + 1].X, points[i][j + 1].Y);

                            }
                        }
                    }

                    graphicResult.Save("result.png");
                    ResultGraphic.Image = graphicResult;
                }
            } catch (Exception ex2)
            {
                Console.WriteLine(ex2);
            }
        }

        public static List<List<Point>> ConvertToCoordinateSystem(List<List<Point>> points)
        {
            List<List<Point>> convertedPoints = new List<List<Point>>();
            float maxX = int.MinValue;
            float maxY = int.MinValue;

            float minX = int.MaxValue;
            float minY = int.MaxValue;

            foreach (var line in points)
            {
                foreach (var point in line )
                {
                    Console.WriteLine($"{point.X} {point.Y}");

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

            Console.WriteLine();
            Console.WriteLine($"maxX = {maxX}, maxY = {maxY}");
            Console.WriteLine($"minX = {minX}, minY = {minY}");
            Console.WriteLine();
            
            float kx = 1000 / maxX;
            float ky = 800 / maxY;

            Console.WriteLine($"kx = {kx}, ky = {ky}");
            Console.WriteLine();

            maxX *= kx;
            maxY *= ky;
            minX *= kx;
            minY *= ky;

            Console.WriteLine($"maxX = {maxX}, maxY = {maxY}");
            Console.WriteLine($"minX = {minX}, minY = {minY}");
            Console.WriteLine();

            int lengthX = ((int)maxX).ToString().Length;
            int lengthY = ((int)maxY).ToString().Length;

            var foo = new PrivateFontCollection();

            for (int i = 0; i < 5; i++)
            {
                graphic.DrawLine(new Pen(Color.Black, (float)2.5), 1000 / 5 * i, 800, 1000 / 5 * i, 795);
                graphic.DrawLine(new Pen(Color.Black, (float)2.5), 0, 800 / 5 * i, 5, 800 / 5 * i);
                if (i != 0)
                    graphic.DrawString((maxX / 5 * i).ToString(), new Font("Times New Roman", 24, FontStyle.Regular), new SolidBrush(Color.Black), 1000 / 5 * i - 25, 750);
            }



            foreach (var line in points)
            {
                List<Point> converted = new List<Point>();
                foreach (var point in line)
                {
                    int scaledX = (int) (point.X * kx - minX / 2);
                    int scaledY = 800 - (int) (point.Y * ky - minY / 2);

                    Console.WriteLine($"{scaledX} {scaledY}");

                    converted.Add(new Point(scaledX, scaledY));
                }
                Console.WriteLine();

                convertedPoints.Add(converted);
            }

            return convertedPoints;
        }
    }
}
