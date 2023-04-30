using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management.Instrumentation;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Paint_Thread
{
    public class Ball
    {
        private int _x;
        private int _y;
        private int _radius;
        private Color _color;
        private Thread t;
        private bool stop = true;
        private float _dx = 1f;
        private float _dy = 1f;
        private Rectangle _frame;
        private int _index = 0;
        private Flag _locker;

        public int Index => _index;
        public int X => _x;
        public int Y => _y;
        public int Radius => _radius;
        public Color Color => _color;
        private bool OnCenterY
        {
            get
            {
                if (Y >= _frame.Height / 2 && Y <= _frame.Height / 2) 
                {
                    return true;
                }
                return false;
            }
        }
        private bool OnCenterX
        {
            get
            {
                if (X >= _frame.Width / 2 && X <= _frame.Width / 2) 
                {
                    return true;
                }
                return false;
            }
        }
        public Ball(int x, int y, int radius, Color color, Rectangle rect)
        {
            _x = x;
            _y = y;
            _radius = radius;
            _color = color;
            _frame = rect;
        }
        public Ball(int x, int y, int radius, Color color, Rectangle rect, int index, Flag locker)
        {
            _x = x;
            _y = y;
            _radius = radius;
            _color = color;
            _frame = rect;
            _index = index;
            _locker = locker;
        }
        public Ball(int x, int y, int radius, Rectangle rect)
        {
            _x = x;
            _y = y;
            _radius = radius;
            _frame = rect;
        }

        private void Process()
        {
            while (!stop)
            {
                Thread.Sleep(5);
                if (_index == 0)
                {
                    MoveDown();
                }
                else if (_index == 1)
                {
                    MoveLeft();
                }
                else if(_index == 2)
                {
                    MoveRight();
                }
            }
        }

        private void MoveDown()
        {
            if (_y + _radius >= _frame.Height)
            {
                _dy *= -1;
            }
            _y += (int)_dy;
            if (OnCenterY)
            {
                lock (_locker)
                {
                    _locker.Increment();
                    Monitor.Wait(_locker);
                }
                _dy *= -1;
            }
        }
        private void MoveLeft()
        {
            if (_x - _radius <= 0)
            {
                _dx *= -1;
            }
            _x -= (int)_dx;
            if (OnCenterX)
            {
                lock (_locker)
                {
                    _locker.Increment();
                    Monitor.Wait(_locker);
                }
                _dx *= -1;
            }
        }
        private void MoveRight()
        {
            if (_x + _radius >= _frame.Width)
            {
                _dx *= -1;
            }
            _x += (int)_dx;
            if (OnCenterX)
            {
                lock (_locker)
                {
                    _locker.Increment();
                    Monitor.Wait(_locker);
                }
                _dx *= -1;
            }
        }
        public void Start()
        {
            stop = false;
            if (t == null || !t.IsAlive)
            {
                var ts = new ThreadStart(Process);
                t = new Thread(ts);
                t.Start();
            }
        }
        public void Stop()
        {
            stop = true;
        }
        public void SetColor(Color color)
        {
            _color = color;
        }
    }
}
