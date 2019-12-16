using OxyPlot;
using OxyPlot.Annotations;
using OxyPlot.Axes;
using OxyPlot.Series;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace OxyPlotExample1
{
    public partial class Form1 : Form
    {
        private DateTimeAxis _dateAxis; // X轴
        private LinearAxis _valueAxis; // Y轴

        private PlotModel _myPlotModel;
        private Random rand = new Random(); // 用来生成随机数

        public Form1()
        {
            InitializeComponent();
            //var myModel = new PlotModel { Title = "Example 1" };
            //myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Bottom, Minimum = -10, Maximum = 10 });
            //myModel.Axes.Add(new LinearAxis { Position = AxisPosition.Left, Minimum = -10, Maximum = 10 });
            //myModel.Series.Add(new FunctionSeries((double x) => { return 10 * Math.Cos(x); }, 0, 100, 0.1, "cos(x)"));
            //this.plotView1.Model = myModel;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // 定义Model
            _myPlotModel = new PlotModel()
            {
                Title = "Temp & Humi",
                LegendTitle = "Legend",
                LegendOrientation = LegendOrientation.Horizontal,
                LegendPlacement = LegendPlacement.Inside,
                LegendPosition = LegendPosition.TopRight,
                LegendBackground = OxyColors.Beige,
                LegendBorder = OxyColors.Black
            };

            //X轴
            _dateAxis = new DateTimeAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80,
                IsZoomEnabled = false,
                IsPanEnabled = false
            };
            _myPlotModel.Axes.Add(_dateAxis);

            //Y轴
            _valueAxis = new LinearAxis()
            {
                MajorGridlineStyle = LineStyle.Solid,
                MinorGridlineStyle = LineStyle.Dot,
                IntervalLength = 80,
                Angle = 60,
                IsZoomEnabled = false,
                IsPanEnabled = false,
                Maximum = 100,
                Minimum = -1
            };
            _myPlotModel.Axes.Add(_valueAxis);

            //添加标注线，温度上下限和湿度上下限
            var lineTempMaxAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                Y = 10,
                Text = "Temp MAX:10"
            };
            _myPlotModel.Annotations.Add(lineTempMaxAnnotation);

            var lineTempMinAnnotation = new LineAnnotation()
            {
                Type = LineAnnotationType.Horizontal,
                Y = 30,
                Text = "Temp Min:30",
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            _myPlotModel.Annotations.Add(lineTempMinAnnotation);

            var lineHumiMaxAnnotation = new OxyPlot.Annotations.LineAnnotation()
            {
                Type = LineAnnotationType.Horizontal,
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid,
                //lineMaxAnnotation.MaximumX = 0.8;
                Y = 75,
                Text = "Humi MAX:75"
            };
            _myPlotModel.Annotations.Add(lineHumiMaxAnnotation);

            var lineHumiMinAnnotation = new LineAnnotation()
            {
                Type = LineAnnotationType.Horizontal,
                Y = 35,
                Text = "Humi Min:35",
                Color = OxyColors.Red,
                LineStyle = LineStyle.Solid
            };
            _myPlotModel.Annotations.Add(lineHumiMinAnnotation);

            //添加两条曲线
            var series = new LineSeries()
            {
                Color = OxyColors.Green,
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.DarkGreen,
                MarkerType = MarkerType.Diamond,
                Title = "Temp",
                //Smooth = true
            };
            _myPlotModel.Series.Add(series);
            series = new LineSeries()
            {
                Color = OxyColors.Blue,
                StrokeThickness = 2,
                MarkerSize = 3,
                MarkerStroke = OxyColors.BlueViolet,
                MarkerType = MarkerType.Star,
                Title = "Humi",
                //Smooth = true
            };
            _myPlotModel.Series.Add(series);


            plotView1.Model = _myPlotModel;

            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    var delta = 1000 / 24;
                    var date = DateTime.Now;
                    _myPlotModel.Axes[0].Maximum = DateTimeAxis.ToDouble(date.AddSeconds(delta / 1000));

                    var lineSer = plotView1.Model.Series[0] as LineSeries;
                    lineSer.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), rand.Next(100, 300) / 10.0));
                    if (lineSer.Points.Count > 100)
                    {
                        lineSer.Points.RemoveAt(0);
                    }

                    lineSer = plotView1.Model.Series[1] as LineSeries;
                    lineSer.Points.Add(new DataPoint(DateTimeAxis.ToDouble(date), rand.Next(350, 750) / 10.0));
                    if (lineSer.Points.Count > 100)
                    {
                        lineSer.Points.RemoveAt(0);
                    }

                    _myPlotModel.InvalidatePlot(true);

                    Thread.Sleep(delta);
                }
            });
        }
    }
}
