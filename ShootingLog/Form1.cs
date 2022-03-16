using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using IronOcr;
using AForge;
using Accord.Imaging;
using System.IO;
using ShootingLog.Model;
using System.Diagnostics;
using System.Windows.Forms.DataVisualization.Charting;
using System.Windows;
using Point = System.Drawing.Point;
using Microsoft.ML.Data;
using Microsoft.ML;
using Extreme.Mathematics;
using Extreme.Statistics;
using CenterSpace.NMath.Core;
using CenterSpace.NMath.Stats;

namespace ShootingLog
{
    public partial class Form1 : Form
    {
        public class Prediction
        {
            [ColumnName("Score")]
            public Single y { get;  set; }
        }

        Bitmap pic = new Bitmap(@"E:\Work\INTERNSHIP\internship2 new\rois\r3.jpg");
        Controller controller;
        Bitmap ready;
        MLContext mlContext = new MLContext();
        public Form1()
        {
            InitializeComponent();
            pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox1.Image = pic;
            controller = new Controller(pic);
            
            chart1.ChartAreas[0].AxisX.Crossing = 0;
            chart1.ChartAreas[0].AxisY.Crossing = 0;
            chart1.Series["Series1"].MarkerStyle = MarkerStyle.Circle;

            
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            ready = controller.roi;
            pictureBox1.Image = ready;
            ReadTheImage(controller);
        }

        private void ReadTheImage(Controller controller)
        {
            List<Coardinate> coardinates = controller.isCoardinates?  
                controller.ReadCoardinates(): controller.ReadResults();
            float[] arr = SlopeAndIntercept(coardinates);

            //result.Text = controller.Read(ready);

            
            if (controller.isCoardinates)
            {
                //chart1.Series["Series2"].Points.DataBindXY(new[] { -1000, arr[1] + (-1000) * arr[0] }, new[] {900, arr[1] +900* arr[0] } );
                chart1.ChartAreas[0].AxisX.Minimum = -1000;
                chart1.ChartAreas[0].AxisX.Maximum = 1000;
                chart1.ChartAreas[0].AxisY.Minimum = -1000;
                chart1.ChartAreas[0].AxisY.Maximum = 1000;
                chart1.Series["Series2"].Points.AddXY(1000, arr[1] + 1000 * arr[0]);
                chart1.Series["Series2"].Points.AddXY(-1000, arr[1] + -1000 * arr[0]);
                chart1.DataBind();
                
            }
            else
            {
                chart1.ChartAreas[0].AxisX.Minimum = -1;
                chart1.ChartAreas[0].AxisX.Maximum = 11;
                chart1.ChartAreas[0].AxisY.Minimum = -1;
                chart1.ChartAreas[0].AxisY.Maximum = 12;
                chart1.Series["Series2"].Points.AddXY(0, arr[1] + 0 * arr[0]);
                chart1.Series["Series2"].Points.AddXY(11, arr[1] + 11 * arr[0]);
                chart1.DataBind();

            }
            foreach (var p in coardinates)
            {
                chart1.Series["Series1"].Points.AddXY(p.x, p.y);
            }
            //Hide Secondary axis
            chart1.ChartAreas[0].AxisX.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MajorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisY.MinorGrid.LineWidth = 0;
            chart1.ChartAreas[0].AxisX.LabelStyle.Enabled = false;


            MessageBox.Show(arr[0].ToString() + "  " + arr[1].ToString());
            string s = Regression(coardinates);
            MessageBox.Show(s);
        }

        private float[] SlopeAndIntercept(List<Coardinate> coardinates)
        {/*
            coardinates.Clear();
            coardinates.Add(new Coardinate(2, 1));
            coardinates.Add(new Coardinate(1, 2));
            coardinates.Add(new Coardinate(4, 3));*/
            float xmean = 0;
            float ymean = 0;
            double x2mean = 0;
            float xymean = 0;


            for (int i = 0; i < coardinates.Count; i++)
            {
                xmean+= coardinates.ElementAt(i).x;
                ymean += coardinates.ElementAt(i).y;
                x2mean +=Math.Pow(coardinates.ElementAt(i).x, 2);
                xymean += coardinates.ElementAt(i).x * coardinates.ElementAt(i).y;

            }
            xmean = xmean / coardinates.Count;
            ymean = ymean / coardinates.Count;
            x2mean = x2mean / coardinates.Count;
            xymean = xymean / coardinates.Count;

            float a = (float)(  ((xmean * ymean)- xymean) / ((Math.Pow(xmean, 2)- x2mean )));
            float b = ymean - a * xmean;
            float[] arr = {a,b };
            return arr;
        }

        [STAThread]
        private string Regression(List<Coardinate> coardinates)
        {
             Coardinate[] points = coardinates.ToArray();
            IDataView trainingData = mlContext.Data.LoadFromEnumerable(coardinates);
            var pipeline = mlContext.Transforms.Concatenate("Features",  "x")
                .Append(mlContext.Regression.Trainers.Sdca("y", maximumNumberOfIterations: 100));
            var model = pipeline.Fit(trainingData);
            var predictions = model.Transform(trainingData);
            var matrix = mlContext.Regression.Evaluate(predictions, labelColumnName:"x");

            var x = new Coardinate(10, 0);
            var y = mlContext.Model.CreatePredictionEngine<Coardinate, Prediction>(model).Predict(x);
            
            return $"R^2={matrix.RSquared}  Prediction x= { x.x}  then Y= {y.y }";


            /*
             
             * List<double> x = new List<double>();
            List<double> y = new List<double>();
            foreach (var c in coardinates)
            {
                x.Add(c.x);
                y.Add(c.y);
            }
            Double[] xs = x.ToArray();
            Double[] ys = y.ToArray();
            string s = "";
            SimpleRegressionModel model1 = new SimpleRegressionModel(xs, ys);
            model1.Compute();
            foreach (var parameter in model1.Parameters)
                s += parameter.ToString() + "\n";
            s+=string.Format("Residual standard error: {0:F2}\n", model1.StandardError );
            s += string.Format("R-Squared: {0:F3}\n", model1.RSquared);
            s += string.Format("Adjusted R-Squared: {0:F3}\n", model1.AdjustedRSquared);
            s += string.Format("F-statistic: {0:F3}\n", model1.FStatistic);

            s += model1.AnovaTable.ToString();

            return s;
            */



        }

        private void btnPrepare_Click(object sender, EventArgs e)
        {
            
            string path="";
            string initPath = @"E:\Work\INTERNSHIP\internship2 new\rois";
            openFileDialog1.InitialDirectory = initPath;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                path = openFileDialog1.FileName;
                Bitmap b1 = new Bitmap(path);
                Controller c1 = new Controller(b1);
                MessageBox.Show("You need to select the coorddinates and the result");
                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    path = openFileDialog1.FileName;
                    Bitmap b2 = new Bitmap(path);
                    Controller c2 = new Controller(b2);
                    if (c1.isCoardinates != c2.isCoardinates)
                    {
                        UseControllers(c1, c2);
                        MessageBox.Show("Done");
                    }
                }
            }
        }

        private void UseControllers(Controller c1, Controller c2)
        {
            List<Coardinate> l1, l2;
            l1 = new List<Coardinate>();
            l2 = new List<Coardinate>();
            if (c1.isCoardinates) {
                l1 = c1.ReadCoardinates();
                l2 = c2.ReadResults();
                CreateDistances(l1, l2);
            } else { 
                l1 = c1.ReadResults();
                l2 = c2.ReadCoardinates();
                CreateDistances(l2,l1);
            }
        }

        private void CreateDistances(List<Coardinate> lCoaardinates, List<Coardinate> lResults)
        {
            DistanceResult tmp;
            List<DistanceResult> newList = new List<DistanceResult>();
            for (int i = 0; i < lCoaardinates.Count; i++)
            {
                tmp = new DistanceResult(lCoaardinates[i].x, lCoaardinates[i].y, lResults[i].y);
                newList.Add(tmp);
            }
            FileController.WriteToFile(newList);
        }

        private void btnVisualize_Click(object sender, EventArgs e)
        {
            List<DistanceResult> list = FileController.ReadFile();
            list.Sort();
            string msg = "";
            foreach (var item in list)
            {
                msg += item.ToString() + "\n";
            }
            FileController.WriteLog(msg);
        }
    }
    
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                          