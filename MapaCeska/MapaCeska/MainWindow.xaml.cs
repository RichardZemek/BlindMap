using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace MapaCeska
{
    public partial class MainWindow : Window
    {
        List<MapPoint> points = new List<MapPoint>()
        {
            new MapPoint(){ Name="Brno", XPercent=0.69, YPercent=0.80 },
            new MapPoint(){ Name="Praha", XPercent=0.32, YPercent=0.45 },
            new MapPoint(){ Name="Ostrava", XPercent=0.75, YPercent=0.40 }
        };

        MapPoint activeCity;
        Random rnd = new Random();

        int score = 0;
        int round = 0;
        List<MapPoint> remainingPoints;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void MainWindow_Loaded(object sender, RoutedEventArgs e)
        {
            remainingPoints = new List<MapPoint>(points);
            DrawPoints();
            SetRandomCity();
            UpdateScore();
        }

        void SetRandomCity()
        {
            if (remainingPoints.Count == 0)
            {
                MessageBox.Show($"Konec hry! Skóre: {score}/{points.Count}");
                return;
            }

            int index = rnd.Next(remainingPoints.Count);
            activeCity = remainingPoints[index];
            remainingPoints.RemoveAt(index);

            round++;
            ActiveCityText.Text = $"Kolo {round}/{points.Count} - Najdi město: {activeCity.Name}";
        }

        void DrawPoints()
        {
            OverlayCanvas.Children.Clear();

            foreach (var point in points)
            {
                Button btn = new Button();
                btn.Content = point.Name;
                btn.Tag = point;

                btn.Click += Btn_Click;

                double x = point.XPercent * MapImage.ActualWidth;
                double y = point.YPercent * MapImage.ActualHeight;

                Canvas.SetLeft(btn, x);
                Canvas.SetTop(btn, y);

                OverlayCanvas.Children.Add(btn);
            }
        }

        private void Btn_Click(object sender, RoutedEventArgs e)
        {
            Button btn = sender as Button;
            MapPoint point = btn.Tag as MapPoint;

            if (point == activeCity)
            {
                score++;
                MessageBox.Show("Správně!");
            }
            else
            {
                MessageBox.Show("Špatně!");
            }

            UpdateScore();
            SetRandomCity();
        }

        void UpdateScore()
        {
            ScoreText.Text = $"Skóre: {score}";
        }

        private void MapImage_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            DrawPoints();
        }

        private void MapImage_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var pos = e.GetPosition(MapImage);

            double x = pos.X / MapImage.ActualWidth;
            double y = pos.Y / MapImage.ActualHeight;

            MessageBox.Show($"XPercent = {x:F2}  YPercent = {y:F2}");
        }
    }
}