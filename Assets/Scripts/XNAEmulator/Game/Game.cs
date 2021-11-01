using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Microsoft.Xna.Framework
{
    public class Game : IDisposable
	{
		private GameComponentCollection _components;
        private bool _initialized;
        Content.ContentManager content;
		GraphicsDevice graphicsDevice;
		DrawQueue drawQueue;
        bool _isFixedTimeStep = true;
        long totalTicks = 0;
        public static Game Instance;
        private bool suppressDraw;
        public void SuppressDraw()
        {
            this.suppressDraw = true;
        }

        public DrawQueue DrawQueue {
			get {
                
                return this.drawQueue;
			}
			set {
				drawQueue = value;
			}
		}
        public ContentManager Content
        {
            get { return this.content; }
            set { this.content = value; }
        }

        public GraphicsDevice GraphicsDevice
        {
            get
            {
				if(graphicsDevice == null)
                {
					graphicsDevice = new GraphicsDevice(DrawQueue);
                    
                }
				
				return graphicsDevice;
            }
        }

        public bool IsFixedTimeStep
        {
            get
            {
                return this._isFixedTimeStep;
            }
            set
            {
                this._isFixedTimeStep = value;
            }
        }

        public Game()
        {
            content = new ContentManager(null, "");
            if (Instance == null)
                Instance = this;
            _components = new GameComponentCollection();


            services = new GameServiceContainer();
            GraphicsDeviceManager graphicsDeviceManagers = new GraphicsDeviceManager(this);
            services.AddService(typeof(IGraphicsDeviceService), graphicsDeviceManagers);
        }

        protected virtual void Update(GameTime gameTime)
        {  
        }
		
		public GameComponentCollection Components
		{
			get
			{
				return this._components;
			}
		}

        protected virtual void Draw(GameTime gameTime)
        {
        }
        protected virtual void LoadContent()
        {
        }
        protected virtual void UnloadContent()
        {
        }
        public virtual void Exit()
        {
        }
        protected virtual void BeginRun()
        {
        }
        protected virtual void EndRun()
        {
        }

        public GameWindow Window
        {
            get
            {
                // TODO
                return new UnityGameWindow();
            }
        }

        private GameServiceContainer services;

        public GameServiceContainer Services
        {
            get
            {
                return services ;
            }
        }

        private void Components_ComponentAdded(object sender, GameComponentCollectionEventArgs e)
        {
            e.GameComponent.Initialize();
           // this.CategorizeComponent(e.GameComponent);
        }

        // Token: 0x06000255 RID: 597 RVA: 0x00007E2A File Offset: 0x0000602A
        private void Components_ComponentRemoved(object sender, GameComponentCollectionEventArgs e)
        {
          //  this.DecategorizeComponent(e.GameComponent);
        }

        public void DoInitialize()
        {
            //this.AssertNotDisposed();
            //if (this.GraphicsDevice == null && this.graphicsDeviceManager != null)
            //{
            //    this._graphicsDeviceManager.CreateDevice();
            //}
            //this.Platform.BeforeInitialize();
            this.Initialize();
            //this.CategorizeComponents();
            this._components.ComponentAdded += this.Components_ComponentAdded;
            this._components.ComponentRemoved += this.Components_ComponentRemoved;
        }

        protected virtual void Initialize()
        {
            //this.applyChanges(this.graphicsDeviceManager);
            //this.InitializeExistingComponents();
            //this._graphicsDeviceService = (IGraphicsDeviceService)this.Services.GetService(typeof(IGraphicsDeviceService));
            //if (this._graphicsDeviceService != null && this._graphicsDeviceService.GraphicsDevice != null)
            //{
                this.LoadContent();
           // }
        }

        public void Run()
        {
            //throw new NotImplementedException();
            if (!this._initialized)
            {
                Begin();
                this._initialized = true;
            }
        }
        protected virtual void Dispose(bool disposing)
        {
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private TimeSpan targetElapsedTime;
        public TimeSpan TargetElapsedTime
        {
            get
            {
                return this.targetElapsedTime;
            }
            set
            {
                if (value <= TimeSpan.Zero)
                {
                    UnityEngine.Debug.LogError("!"); 
                    //throw new ArgumentOutOfRangeException("value", Resources.TargetElaspedCannotBeZero);
                }
                this.targetElapsedTime = value;
            }
        }

        public void Begin()
        {
            this.DoInitialize();
            
            BeginRun();
            // XNA's first update call has a zero elapsed time, so do one now.
            GameTime gameTime = new GameTime(new TimeSpan(0), new TimeSpan(0), new TimeSpan(0, 0, 0, 0, 0), new TimeSpan(0, 0, 0, 0, 0));
			Update(gameTime);
        }

        protected virtual void OnExiting(object sender, EventArgs args)
        {
            //if (this.Exiting != null)
            //{
            //    this.Exiting(null, args);
            //}
            Exit();
        }

        public void Tick(float deltaTime)
        {
            long microseconds = (int)(deltaTime * 1000000);
			long ticks = microseconds * 10;
            totalTicks += ticks;
            GameTime gameTime = new GameTime(new TimeSpan(0), new TimeSpan(0), new TimeSpan(totalTicks), new TimeSpan(ticks));
            Update(gameTime);
            Draw(gameTime);
        }

        private bool isMouseVisible;
        public bool IsMouseVisible
        {
            get
            {
                return this.isMouseVisible;
            }
            set
            {
                this.isMouseVisible = value;
                if (this.Window != null)
                {
                    this.Window.IsMouseVisible = value;
                }
            }
        }

        private IGraphicsDeviceManager graphicsDeviceManager;
        protected virtual void EndDraw()
        {
            if (this.graphicsDeviceManager != null)
            {
                this.graphicsDeviceManager.EndDraw();
            }
            //Logger.EndLogEvent(LoggingEvent.Draw, "");
        }

        private bool isActive;
        public bool IsActive
        {
            get
            {
                bool flag = false;
                //if (GamerServicesDispatcher.IsInitialized)
                //{
                //    flag = Guide.IsVisible;
                //}
                UnityEngine.Debug.Log("?");
                return true;
                return this.isActive && !flag;
            }
        }

    }

    public interface IGraphicsDeviceManager
    {
        // Token: 0x060000E9 RID: 233
        void CreateDevice();

        // Token: 0x060000EA RID: 234
        bool BeginDraw();

        // Token: 0x060000EB RID: 235
        void EndDraw();
    }

    public enum ErrorCodes : uint
    {
        // Token: 0x04000562 RID: 1378
        Success,
        // Token: 0x04000563 RID: 1379
        Pending = 997u,
        // Token: 0x04000564 RID: 1380
        NotConnected = 1167u,
        // Token: 0x04000565 RID: 1381
        Empty = 4306u,
        // Token: 0x04000566 RID: 1382
        Busy = 170u,
        // Token: 0x04000567 RID: 1383
        AccessDenied = 5u,
        // Token: 0x04000568 RID: 1384
        AlreadyExists = 183u,
        // Token: 0x04000569 RID: 1385
        D3DERR_WRONGTEXTUREFORMAT = 2289436696u,
        // Token: 0x0400056A RID: 1386
        D3DERR_TOOMANYOPERATIONS = 2289436701u,
        // Token: 0x0400056B RID: 1387
        D3DERR_DRIVERINTERNALERROR = 2289436711u,
        // Token: 0x0400056C RID: 1388
        D3DERR_NOTFOUND = 2289436774u,
        // Token: 0x0400056D RID: 1389
        D3DERR_MOREDATA,
        // Token: 0x0400056E RID: 1390
        D3DERR_DEVICELOST,
        // Token: 0x0400056F RID: 1391
        D3DERR_DEVICENOTRESET,
        // Token: 0x04000570 RID: 1392
        D3DERR_NOTAVAILABLE = 2289436784u,
        // Token: 0x04000571 RID: 1393
        D3DERR_OUTOFVIDEOMEMORY = 2289435004u,
        // Token: 0x04000572 RID: 1394
        D3DERR_INVALIDCALL = 2289436786u,
        // Token: 0x04000573 RID: 1395
        XACTENGINE_E_ALREADYINITIALIZED = 2328297473u,
        // Token: 0x04000574 RID: 1396
        XACTENGINE_E_NOTINITIALIZED,
        // Token: 0x04000575 RID: 1397
        XACTENGINE_E_EXPIRED,
        // Token: 0x04000576 RID: 1398
        XACTENGINE_E_NONOTIFICATIONCALLBACK,
        // Token: 0x04000577 RID: 1399
        XACTENGINE_E_NOTIFICATIONREGISTERED,
        // Token: 0x04000578 RID: 1400
        XACTENGINE_E_INVALIDUSAGE,
        // Token: 0x04000579 RID: 1401
        XACTENGINE_E_INVALIDDATA,
        // Token: 0x0400057A RID: 1402
        XACTENGINE_E_INSTANCELIMITFAILTOPLAY,
        // Token: 0x0400057B RID: 1403
        XACTENGINE_E_NOGLOBALSETTINGS,
        // Token: 0x0400057C RID: 1404
        XACTENGINE_E_INVALIDVARIABLEINDEX,
        // Token: 0x0400057D RID: 1405
        XACTENGINE_E_INVALIDCATEGORY,
        // Token: 0x0400057E RID: 1406
        XACTENGINE_E_INVALIDCUEINDEX,
        // Token: 0x0400057F RID: 1407
        XACTENGINE_E_INVALIDWAVEINDEX,
        // Token: 0x04000580 RID: 1408
        XACTENGINE_E_INVALIDTRACKINDEX,
        // Token: 0x04000581 RID: 1409
        XACTENGINE_E_INVALIDSOUNDOFFSETORINDEX,
        // Token: 0x04000582 RID: 1410
        XACTENGINE_E_READFILE,
        // Token: 0x04000583 RID: 1411
        XACTENGINE_E_UNKNOWNEVENT,
        // Token: 0x04000584 RID: 1412
        XACTENGINE_E_INCALLBACK,
        // Token: 0x04000585 RID: 1413
        XACTENGINE_E_NOWAVEBANK,
        // Token: 0x04000586 RID: 1414
        XACTENGINE_E_SELECTVARIATION,
        // Token: 0x04000587 RID: 1415
        XACTENGINE_E_MULTIPLEAUDITIONENGINES,
        // Token: 0x04000588 RID: 1416
        XACTENGINE_E_WAVEBANKNOTPREPARED,
        // Token: 0x04000589 RID: 1417
        XACTENGINE_E_NORENDERER,
        // Token: 0x0400058A RID: 1418
        XACTENGINE_E_INVALIDENTRYCOUNT,
        // Token: 0x0400058B RID: 1419
        XACTENGINE_E_SEEKTIMEBEYONDCUEEND,
        // Token: 0x0400058C RID: 1420
        XACTENGINE_E_AUDITION_WRITEFILE = 2328297729u,
        // Token: 0x0400058D RID: 1421
        XACTENGINE_E_AUDITION_NOSOUNDBANK,
        // Token: 0x0400058E RID: 1422
        XACTENGINE_E_AUDITION_INVALIDRPCINDEX,
        // Token: 0x0400058F RID: 1423
        XACTENGINE_E_AUDITION_MISSINGDATA,
        // Token: 0x04000590 RID: 1424
        XACTENGINE_E_AUDITION_UNKNOWNCOMMAND,
        // Token: 0x04000591 RID: 1425
        XACTENGINE_E_AUDITION_INVALIDDSPINDEX,
        // Token: 0x04000592 RID: 1426
        XACTENGINE_E_AUDITION_MISSINGWAVE,
        // Token: 0x04000593 RID: 1427
        XACTENGINE_E_AUDITION_CREATEDIRECTORYFAILED,
        // Token: 0x04000594 RID: 1428
        XACTENGINE_E_AUDITION_INVALIDSESSION,
        // Token: 0x04000595 RID: 1429
        ZDKSYSTEM_E_AUDIO_INSTANCELIMIT = 2343370753u,
        // Token: 0x04000596 RID: 1430
        ZDKSYSTEM_E_AUDIO_INVALIDSTATE,
        // Token: 0x04000597 RID: 1431
        ZDKSYSTEM_E_AUDIO_INVALIDDATA,
        // Token: 0x04000598 RID: 1432
        CAPTURE_ENGINE_E_DEVICEGONE = 2364407809u,
        // Token: 0x04000599 RID: 1433
        DIRECTRENDERING_E_INVALID_MODE = 2150814720u,
        // Token: 0x0400059A RID: 1434
        DIRECTRENDERING_E_ELEMENT_NOT_IN_VISUALTREE = 2281703676u,
        // Token: 0x0400059B RID: 1435
        VFW_E_NO_AUDIO_HARDWARE = 2147746390u,
        // Token: 0x0400059C RID: 1436
        E_INVALIDARG = 2147942487u,
        // Token: 0x0400059D RID: 1437
        E_FAIL = 2147500037u,
        // Token: 0x0400059E RID: 1438
        E_ABORT = 2147500036u,
        // Token: 0x0400059F RID: 1439
        E_ACCESSDENIED = 2147942405u,
        // Token: 0x040005A0 RID: 1440
        E_NOTIMPL = 2147500033u,
        // Token: 0x040005A1 RID: 1441
        E_OUTOFMEMORY = 2147942414u,
        // Token: 0x040005A2 RID: 1442
        STRSAFE_E_INSUFFICIENT_BUFFER = 2147942522u,
        // Token: 0x040005A3 RID: 1443
        REGDB_E_CLASSNOTREG = 2147746132u,
        // Token: 0x040005A4 RID: 1444
        ERROR_SHARING_VIOLATION = 2147942432u
    }
}
