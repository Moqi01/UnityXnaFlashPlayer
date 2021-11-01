using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using XnaFlash.Actions;
using XnaFlash.Actions.Functions;
using XnaFlash.Actions.Objects;
using XnaFlash.Content;
using XnaFlash.Swf.Structures;
using XnaFlash.Swf.Tags;
using XnaVG;

namespace XnaFlash.Movie
{
    public class Button : StageObject
    {
        protected ButtonInfo _button;

        public override bool Visible
        {
            get { return base.Visible; }
            set
            {
                if (value && !base.Visible) SetButtonState(State.Up);
                base.Visible = value;
            }
        }
        public State CurrentState { get; private set; }

        internal Button(RootMovieClip root, ButtonInfo info, DisplayObject container)
            : base(root, container)
        {
            _button = info;
            
            CurrentState = State.Over;
            SetButtonState(State.Up);
            LoadActions();
        }

        public void OnMouseDown()
        {
            SetButtonState(State.Down);
        }
        public void OnMouseUp()
        {
            SetButtonState(State.Over);
        }
        public override bool OnMouseMove()
        {
            if (!Visible)
                return false;

            Root.ButtonStack.PushCombineLeft(_container.Matrix);
            bool val = _displayList.OnMouseMove();
            if (val)
                SetButtonState(State.Up);
            else
                val = HandleMouseMove();
            Root.ButtonStack.Pop();
            return val;
        }
        public override void OnNextFrame()
        {
            if (Root.ActiveButton != this)
                SetButtonState(State.Up);
            base.OnNextFrame();
        }
        
        private void SetButtonState(State state)
        {
            if (state == CurrentState)
                return;

            if (state != State.Up)
                Root.ActiveButton = this;

            switch (state)
            {
                case State.Over:
                    _displayList.ProcessButtonParts(_button.Parts.Where(p => p.Over), this);
                    break;
                case State.Down:
                    _displayList.ProcessButtonParts(_button.Parts.Where(p => p.Down), this);                    
                    break;
                default:
                    _displayList.ProcessButtonParts(_button.Parts.Where(p => p.Up), this);
                    break;
            }

            RunEvent(CurrentState, state, true);
            CurrentState = state;
        }
        private bool HandleMouseMove()
        {
            if (!_button.CheckHit(Root.MousePosition, Root.ButtonStack.Matrix))
            {
                SetButtonState(State.Up);
                return false;
            }

            SetButtonState(State.Over);
            return true;
        }
        private void RunEvent(State fromState, State toState, bool mouseInside)
        {
            if (fromState == State.Up)
            {
                if (toState == State.Over)
                    RunEvent(Event.onRollOver);
                //else if (toState
            }
            else if (fromState == State.Over)
            {
                if (toState == State.Up)
                    RunEvent(Event.onRollOut);
                else if (toState == State.Down)
                    RunEvent(Event.onPress);                
            }
            else if (fromState == State.Down)
            {
                if (toState == State.Over)
                    RunEvent(Event.onRelease);
            }

        }
        private void LoadActions()
        {
            foreach (var action in _button.Actions)
            {
                if (action.IdleToOverUp) _eventActions[(int)Event.onRollOver] = action.Actions;
                if (action.IdleToOverDown) _eventActions[(int)Event.onDragOver] = action.Actions;
                if (action.OutDownToIdle) _eventActions[(int)Event.onReleaseOutside] = action.Actions;
                if (action.OutDownToOverDown) _eventActions[(int)Event.onDragOver] = action.Actions;
                if (action.OverDownToOutDown) _eventActions[(int)Event.onDragOut] = action.Actions;
                if (action.OverDownToOverUp) _eventActions[(int)Event.onRelease] = action.Actions;
                if (action.OverUpToOverDown) _eventActions[(int)Event.onPress] = action.Actions;
                if (action.OverUpToIdle) _eventActions[(int)Event.onRollOut] = action.Actions;
                if (action.OverDownToIdle) _eventActions[(int)Event.onDragOut] = action.Actions;                
            }
        }

        public enum State
        {
            Up,
            Over,
            Down
        }
    }
}
    