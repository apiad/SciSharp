using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

using SciSharp.Collections;
using SciSharp.Geometry;
using SciSharp.Probabilities;
using SciSharp.Sorting;


namespace SciSharp.Examples.OfflineConvexHull2GDI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            points = new List<Point2>();
            borders = new List<Point2>();
            onHull = new List<Point2>();
            considered = new List<Point2>();
            syncRoot = new object();

            cbxPicker.SelectedIndex = 0;
        }

        private Thread drawingThread;
        private List<Point2> borders;
        private readonly List<Point2> points;
        private readonly List<Point2> onHull;
        private readonly List<Point2> considered;
        private readonly object syncRoot;
        private readonly RandomEx random = new RandomEx();

        private void PboxCanvasMouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                points.Add(new Point2(e.X, e.Y));
            }

            pboxCanvas.Invalidate();
        }

        private void PboxCanvasPaint(object sender, PaintEventArgs e)
        {
            e.Graphics.CompositingQuality = CompositingQuality.HighQuality;
            e.Graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            e.Graphics.SmoothingMode = SmoothingMode.HighQuality;

            foreach (var point2 in points)
            {
                e.Graphics.FillEllipse(Brushes.CornflowerBlue, (float)point2.X - 5f, (float)point2.Y - 5f, 10f, 10f);
            }

            lock (syncRoot)
            {
                for (int i = 0; i < onHull.Count - 1; i++)
                {
                    e.Graphics.DrawLine(Pens.Gray, (float)onHull[i].X, (float)onHull[i].Y,
                                        (float)onHull[i + 1].X, (float)onHull[i + 1].Y);
                }

                if (onHull.Count > 1)
                {
                    e.Graphics.DrawLine(Pens.Gray, (float)onHull[0].X, (float)onHull[0].Y,
                                           (float)onHull[onHull.Count - 1].X, (float)onHull[onHull.Count - 1].Y);
                }

                if (onHull.Count > 0 && considered.Count > 0)
                {
                    e.Graphics.DrawLine(Pens.DarkOrange, (float)onHull[0].X, (float)onHull[0].Y,
                                           (float)considered[0].X, (float)considered[0].Y);

                    e.Graphics.DrawLine(Pens.DarkOrange, (float)onHull[onHull.Count-1].X, (float)onHull[onHull.Count-1].Y,
                                           (float)considered[0].X, (float)considered[0].Y);
                }

                foreach (var point2 in onHull)
                {
                    e.Graphics.FillEllipse(Brushes.DarkGreen, (float)point2.X - 5f, (float)point2.Y - 5f, 10f, 10f);
                }
            }

            lock (syncRoot)
            {
                foreach (var point2 in considered)
                {
                    e.Graphics.FillEllipse(Brushes.DarkOrange, (float)point2.X - 5f, (float)point2.Y - 5f, 10f, 10f);
                }
            }

            foreach (var point2 in borders)
            {
                e.Graphics.FillEllipse(Brushes.Crimson, (float)point2.X - 5f, (float)point2.Y - 5f, 10f, 10f);
            }
        }

        private void BtnGoClick(object sender, EventArgs e)
        {
            OfflineConvexHull2 gs;

            if (cbxPicker.SelectedIndex == 0)
            {
                gs = new GrahamScan(points.ToArray(), new ArrayStack<Point2>(points.Count), new QuickSort<Point2>());
            }
            else
            {
                gs = new JarvisMarch(points.ToArray());
            }

            borders.Clear();
            onHull.Clear();
            considered.Clear();
            pboxCanvas.Invalidate();

            gs.PointConsidered += (o, args) =>
            {
                lock (syncRoot)
                {
                    considered.Add(args.Data);
                }

                pboxCanvas.Invalidate();
                Thread.Sleep((int)Math.Pow(2, tcbInterval.Value / 10d));

                lock (syncRoot)
                {
                    considered.Remove(args.Data);
                }

                pboxCanvas.Invalidate();
            };

            gs.PointEnteredHull += (o, args) =>
            {
                lock (syncRoot)
                {
                    onHull.Add(args.Data);
                }

                pboxCanvas.Invalidate();
                Thread.Sleep((int)Math.Pow(2, tcbInterval.Value / 10d));
            };

            gs.PointLeftHull += (o, args) =>
            {
                lock (syncRoot)
                {
                    onHull.Remove(args.Data);
                }

                pboxCanvas.Invalidate();
                Thread.Sleep((int)Math.Pow(2, tcbInterval.Value / 10d));
            };

            if (drawingThread != null)
                drawingThread.Abort();

            drawingThread = new Thread(() =>
                                       {
                                           borders = new List<Point2>(gs.Solve());
                                           pboxCanvas.Invalidate();
                                       });

            drawingThread.IsBackground = true;
            drawingThread.Start();
        }

        private void BtnClearClick(object sender, EventArgs e)
        {
            if (drawingThread != null)
                drawingThread.Abort();

            points.Clear();
            borders.Clear();
            onHull.Clear();
            considered.Clear();

            pboxCanvas.Invalidate();
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            if (drawingThread != null)
                drawingThread.Abort();

            borders.Clear();
            onHull.Clear();
            considered.Clear();

            pboxCanvas.Invalidate();
        }

        private void BtnUniformClick(object sender, EventArgs e)
        {
            double minX = 50;
            double maxX = pboxCanvas.ClientSize.Width - 50;
            double minY = 50;
            double maxY = pboxCanvas.ClientSize.Height - 50;

            for (int i = 0; i < 100; i++)
            {
                points.Add(new Point2(random.Uniform(minX, maxX), random.Uniform(minY, maxY)));
            }

            pboxCanvas.Invalidate();
        }

        private void BtnGaussianClick(object sender, EventArgs e)
        {
            double radius = Math.Min(pboxCanvas.ClientSize.Width / 18, pboxCanvas.ClientSize.Height / 18);
            double centerX = pboxCanvas.ClientSize.Width / 2d;
            double centerY = pboxCanvas.ClientSize.Height / 2d;

            for (int i = 0; i < 100; i++)
            {
                Point2 p;

                do
                {
                    p = new Point2(random.Normal(centerX, radius), random.Normal(centerY, radius));
                }
                while ((p - new Point2(centerX, centerY)).Length >
                    Math.Min((pboxCanvas.ClientSize.Width - 50) / 2d, (pboxCanvas.ClientSize.Height - 50) / 2d));

                points.Add(p);
            }

            pboxCanvas.Invalidate();
        }

        private void BtnCircularClick(object sender, EventArgs e)
        {
            double radius = Math.Min((pboxCanvas.ClientSize.Width - 50) / 2d, (pboxCanvas.ClientSize.Height - 50) / 2d);
            double centerX = pboxCanvas.ClientSize.Width / 2d;
            double centerY = pboxCanvas.ClientSize.Height / 2d;

            for (int i = 0; i < 100; i++)
            {
                var vonMisses = random.VonMisses();

                points.Add(new Point2(Math.Cos(vonMisses) * radius + centerX,
                                      Math.Sin(vonMisses) * radius + centerY));
            }

            pboxCanvas.Invalidate();
        }

        private void TcbIntervalScroll(object sender, EventArgs e)
        {
            lblSpeed.Text = "Update interval (log scale) = {0} ms".Formatted((int) Math.Pow(2, tcbInterval.Value/10d));
        }
    }
}
