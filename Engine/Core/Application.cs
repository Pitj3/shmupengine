﻿using Engine.Graphics;
using Engine.Entities;
using System.Diagnostics;

namespace Engine.Core
{
    public class Application : IDisposable
    {
        #region Private Data
        private bool _disposed;
        private bool _initialized;
        private bool _isRunning;
        private Stopwatch _gameTimer;
        private GameTime _gameTime;
        private TimeSpan _accumulatedElapsedTime;
        private long _previousTicks = 0;

        private Window _window;

        private FrameBuffer _frameBuffer;
        #endregion

        #region Public Data
        public static Application Instance { get; private set; }

        public IRenderer Renderer { get; private set; }

        public uint Width { get; set; }
        public uint Height { get; set; }
        public string Title { get; set; } = "Shmup Engine";

        public Scene Scene { get; private set; }
        #endregion

        #region Public API
        public Application(IRenderer renderer)
        {
            Instance = this;
            Renderer = renderer;

            Scene = new Scene();
        }

        ~Application()
        {
            Dispose(false);
        }

        public void Run()
        {
            AssertNotDisposed();

            if(Renderer == null)
            {
                throw new Exception("No Renderer set. Can't start application without a renderer");
            }

            if (!_initialized)
            {
                Initialize();
            }

            foreach (Entity e in Scene.Current.GetEntities())
            {
                foreach (IComponent c in e.GetComponents())
                {
                    c.Init();
                }
            }

            _isRunning = true;
            while (_isRunning)
            {
                Tick();
                Render();
            }

            Renderer.Shutdown();

            Renderer.Dispose();
        }

        public void Quit()
        {
            _isRunning = false;
        }
        #endregion

        #region IDisposable Implementation
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (_disposed) return;

            if (disposing)
            {
                Renderer.Dispose();
            }

            _disposed = true;
        }

        [DebuggerNonUserCode]
        private void AssertNotDisposed()
        {
            if (_disposed)
            {
                string name = GetType().Name;
                throw new ObjectDisposedException(
                    name, string.Format("The {0} object was used after being Disposed.", name));
            }
        }
        #endregion

        #region Public Events
        public virtual void OnInit()
        {
            
        }

        public virtual void OnUpdate(GameTime time)
        {
            foreach (Entity e in Scene.Current.GetEntities())
            {
                foreach(IComponent c in e.GetComponents())
                {
                    c.Update(time);
                }
            }
        }

        public virtual void OnRender(GameTime time)
        {
            foreach (Entity e in Scene.Current.GetEntities())
            {
                foreach (IComponent c in e.GetComponents())
                {
                    c.Render(time);
                }
            }
        }
        #endregion

        #region Internal API
        internal void Initialize()
        {
            // Init platform
            _window = Window.Create(Title, Width, Height);

            Renderer.Init();

            _frameBuffer = new FrameBuffer(Width, Height);

            OnInit();
            _initialized = true;
        }

        internal void Tick()
        {
            AssertNotDisposed();

            if(_window != null)
            {
                _window.PollEvents();

                if(_window.ShouldClose())
                {
                    _isRunning = false;
                }
            }

            Input.Poll();

            if (_gameTimer == null)
            {
                _gameTimer = new Stopwatch();
                _gameTimer.Start();
            }

            _gameTime ??= new GameTime();

            long currentTicks = _gameTimer.Elapsed.Ticks;
            _accumulatedElapsedTime += TimeSpan.FromTicks(currentTicks - _previousTicks);
            _previousTicks = currentTicks;

            _gameTime.ElapsedGametime = _accumulatedElapsedTime;
            _gameTime.TotalGametime += _accumulatedElapsedTime;

            _accumulatedElapsedTime = TimeSpan.Zero;

            OnUpdate(_gameTime);
        }

        internal void Render()
        {
            AssertNotDisposed();

            _frameBuffer.Bind();
            Renderer.BeginRender(_gameTime);
            OnRender(_gameTime);
            Renderer.EndRender();
            FrameBuffer.Unbind();

            Renderer.RenderFullscreenQuad(_frameBuffer.ID);

            _window.SwapBuffers();
        }
        #endregion
    }
}