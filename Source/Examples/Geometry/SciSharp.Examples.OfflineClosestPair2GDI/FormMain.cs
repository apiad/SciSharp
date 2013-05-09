using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Threading;
using System.Windows.Forms;

using SciSharp.Collections;
using SciSharp.Geometry;
using SciSharp.Probabilities;
using SciSharp.Sorting;


namespace SciSharp.Examples.OfflineClosestPair2GDI
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();

            points = new List<Point2>();
            syncRoot = new object();

            cbxPicker.SelectedIndex = 0;
        }

        private Thread drawingThread;
        private readonly List<Point2> points;
        private LineSweepEventArgs args;
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

            if (args != null)
            {
                lock (syncRoot)
                {
                    if (args != null)
                    {
                        foreach (var point in args.Points)
                        {
                            e.Graphics.FillEllipse(Brushes.DarkGreen, (float)point.X - 5f, (float)point.Y - 5f, 10f, 10f);
                        }

                        if (args.LeftMin != null)
                        {
                            e.Graphics.DrawLine(Pens.DarkGreen, (float)args.LeftMin.Value.A.X, (float)args.LeftMin.Value.A.Y, (float)args.LeftMin.Value.B.X, (float)args.LeftMin.Value.B.Y);
                        }

                        if (args.RightMin != null)
                        {
                            e.Graphics.DrawLine(Pens.DarkGreen, (float)args.RightMin.Value.A.X, (float)args.RightMin.Value.A.Y, (float)args.RightMin.Value.B.X, (float)args.RightMin.Value.B.Y);
                        }

                        e.Graphics.DrawLine(Pens.Black, (float)args.BestSoFar.A.X, (float)args.BestSoFar.A.Y, (float)args.BestSoFar.B.X, (float)args.BestSoFar.B.Y);
                        e.Graphics.FillEllipse(Brushes.Crimson, (float)args.BestSoFar.A.X - 5f, (float)args.BestSoFar.A.Y - 5f, 10f, 10f);
                        e.Graphics.FillEllipse(Brushes.Crimson, (float)args.BestSoFar.B.X - 5f, (float)args.BestSoFar.B.Y - 5f, 10f, 10f);

                        e.Graphics.DrawLine(Pens.DarkOrange, (float)args.CurrentPair.A.X, (float)args.CurrentPair.A.Y, (float)args.CurrentPair.B.X, (float)args.CurrentPair.B.Y);
                        e.Graphics.FillEllipse(Brushes.DarkOrange, (float)args.CurrentPair.A.X - 5f, (float)args.CurrentPair.A.Y - 5f, 10f, 10f);
                        e.Graphics.FillEllipse(Brushes.DarkOrange, (float)args.CurrentPair.B.X - 5f, (float)args.CurrentPair.B.Y - 5f, 10f, 10f);
                    }
                }
            }
        }

        private void BtnGoClick(object sender, EventArgs e)
        {
            OfflineClosestPair2 cp;

            //if (cbxPicker.SelectedIndex == 0)
            {
                cp = new LineSweep(points.ToArray(), new QuickSort<Point2>(), new QuickSort<int>()) { DoEvents = true };
                (cp as LineSweep).Step += (o, eventArgs) =>
                                          {
                                              args = eventArgs;

                                              pboxCanvas.Invalidate();
                                              Thread.Sleep((int)Math.Pow(2, tcbInterval.Value / 10d));
                                          };
            }

            args = null;
            pboxCanvas.Invalidate();

            if (drawingThread != null)
                drawingThread.Abort();

            drawingThread = new Thread(() =>
                                       {
                                           cp.Solve();
                                           pboxCanvas.Invalidate();
                                       }) { IsBackground = true };

            drawingThread.Start();
        }

        private void BtnClearClick(object sender, EventArgs e)
        {
            if (drawingThread != null)
                drawingThread.Abort();

            points.Clear();
            args = null;

            pboxCanvas.Invalidate();
        }

        private void BtnResetClick(object sender, EventArgs e)
        {
            if (drawingThread != null)
                drawingThread.Abort();

            args = null;

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
            lblSpeed.Text = "Update interval (log scale) = {0} ms".Formatted((int)Math.Pow(2, tcbInterval.Value / 10d));
        }
    }
}
