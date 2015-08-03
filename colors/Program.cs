using System;
using System.Drawing;
using System.Windows.Forms;
using System.ComponentModel;

namespace colors
{
    class MainClass : Form
    {
        private Timer timer;
        private DateTime start;
        private double speed;

        MainClass()
        {
            Size = new Size(600, 600);
            BackColor = Color.Black;

            timer = new Timer (new Container ());
            timer.Interval = 100;
            timer.Enabled = true;
            timer.Tick += new EventHandler (this.OnTick);

            start = DateTime.Now;
            speed = 2.0;

            CenterToScreen ();
        }

        public static void Main (string[] args)
        {
            Application.Run (new MainClass ());
        }

        private int d2i(double x) {
            return (int)(255.5 * Math.Abs (x));
        }

        private void OnTick(Object sender, EventArgs args)
        {
            DateTime now = DateTime.Now;
            double delta = (now - start).TotalMinutes;
            double r = Math.Sin (delta * speed);
            double g = Math.Sin (delta * speed * 3.0);
            double b = Math.Sin (delta * speed * 11.0);
            Color color = Color.FromArgb (d2i(r), d2i(g), d2i(b));
            BackColor = color;
        }
    }
}
