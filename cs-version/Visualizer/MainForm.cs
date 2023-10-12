using System;
using OfficeOpenXml;
using System.Windows.Forms;
using System.IO;
using System.Collections.Generic;
using System.Drawing;

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
                                points[points.Count - 1].Add(new Point((int) float.Parse(worksheet.Cells[row, 1].Value?.ToString()), (int) float.Parse(worksheet.Cells[row, 2].Value?.ToString())));
                            }
                        }
                    }
                    catch (Exception ex1)
                    {
                        Console.WriteLine(ex1);
                    }
                    } else
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

                    int divisionCounter = DivisionCounter.Value;
                    List<List<Point>> newPoints = points;
                    for (int i = 0; i < divisionCounter; i++)
                    {
                        for (int j = 0; j < newPoints.Count; j++)
                        {
                            newPoints[j] = ChaikinSmooth(newPoints[j]);
                        }
                    }

                    Console.WriteLine(newPoints[0].Count);

                    graphicResult = new Bitmap(ResultGraphic.Width, ResultGraphic.Height);

                    graphic = Graphics.FromImage(graphicResult);
                    graphic.Clear(Color.Black);

                    for (int i = 0; i < newPoints.Count; i++)
                    {
                        for (int j = 0; j < newPoints[i].Count; j++)
                        {
                            Console.WriteLine(newPoints[i][j].X);
                            Console.WriteLine(newPoints[i][j].Y);

                            if (j < newPoints[i].Count - 1) {
                                graphic.DrawLine(new Pen(Color.White, (float)2.5), newPoints[i][j].X, newPoints[i][j].Y, newPoints[i][j+1].X, newPoints[i][j+1].Y);
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

        private List<Point> ChaikinSmooth(List<Point> points)
        {
            List<Point> result = new List<Point>();

            result.Add(points[0]);

            for (int i = 0; i < points.Count-1; i++)
            {
                Point previous = points[i];
                Point next = points[i+1];

                Point a = new Point(previous.X * 3 / 4 + next.X * 1 / 4, previous.Y * 3 / 4 + next.Y * 1 / 4);
                Point b = new Point(previous.X * 1 / 4 + next.X * 3 / 4, previous.Y * 1 / 4 + next.Y * 3 / 4);

                result.Add(a);
                result.Add(b);
            }

            result.Add(points[points.Count - 1]);

            return result;
        }

        public static List<Point> ConvertToCoordinateSystem(List<Point> points)
        {
            List<Point> convertedPoints = new List<Point>();
            int maxX = int.MinValue;
            int maxY = int.MinValue;

            foreach (var point in points)
            {
                if (point.X > maxX)
                    maxX = point.X;
                if (point.Y > maxY)
                    maxY = point.Y;
            }

            int kx = 800 / maxX;
            int ky = 800 / maxY;

            foreach (Point point in points)
            {
                int scaledX = point.X * kx, scaledY = point.Y * ky;

                convertedPoints.Add(new Point(scaledX, scaledY));
            }

            return convertedPoints;
        }
    }
}
