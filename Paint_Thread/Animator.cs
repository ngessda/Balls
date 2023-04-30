using System;
using System.Collections.Generic;
using System.Deployment.Application;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Paint_Thread
{
    public class Animator
    {
        private static Random _rand = new Random();

        private Graphics _canvasG;
        private Rectangle _canvasRectangle;
        private BufferedGraphics _bg;
        private Thread _t;
        private bool stop = false;
        private List<Ball> _balls = new List<Ball>();
        private Flag flag = new Flag();

        public Animator(Graphics graphics, Rectangle rect)
        {
            Update(graphics, rect);
            _bg = BufferedGraphicsManager.Current.Allocate(graphics, rect);
        }
        private void Update(Graphics graphics, Rectangle rect)
        {
            _canvasG = graphics;
            _canvasRectangle = rect;
        }
        private void Animate()
        {
            Thread.Sleep(5);
            var g = _bg.Graphics;
            while (!stop)
            {
                Thread.Sleep(5);
                g.Clear(Color.White);

                Monitor.Enter(_balls);
                flag.Check();
                foreach (var b in _balls)
                {
                    DrawBall(g, b);
                }
                Monitor.Exit(_balls);

                try
                {
                    _bg.Render();
                }
                catch { }
            }
            g.Clear(Color.White);
            try
            {
                _bg.Render();
            }
            catch { }
        }

        private void DrawBall(Graphics g, Ball ball)
        {
            g.FillEllipse(
                new SolidBrush(ball.Color),
                new Rectangle(
                    ball.X - ball.Radius, ball.Y - ball.Radius, 2 * ball.Radius, 2 * ball.Radius)
                );
        }
        public void AddBall()
        {
            var r = _rand.Next(0, 256);
            var g = _rand.Next(0, 256);
            var b = _rand.Next(0, 256);
            var color = Color.FromArgb(r, g, b);

            var ball = new Ball(
                _canvasRectangle.Width / 2, _canvasRectangle.Height / 2, 30, color, _canvasRectangle, _balls.Count, flag
                );

            Monitor.Enter(_balls);
            if (_balls.Count < 3)
            {
                _balls.Add(ball);
            }
            Monitor.Exit(_balls);

            ball.Start();
        }
        public void Start()
        {
            stop = false;
            if(_t == null || !_t.IsAlive)
            {
                ThreadStart ts = new ThreadStart(Animate);
                _t = new Thread(ts);
                _t.Start();
            }
        }
        public void Stop()
        {
            stop = true;
            Monitor.Enter(_balls);
            foreach(var ball in _balls)
            {
                ball.Stop();
            }
            Monitor.Exit(_balls);
            
        }
    }
}
