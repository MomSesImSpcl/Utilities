#if REWIRED
using MomSesImSpcl.Extensions;
using Rewired.Integration.UnityUI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace MomSesImSpcl.Utilities
{
    /// <summary>
    /// <see cref="RewiredStandaloneInputModule"/> with non interactable UI element skip during navigation.
    /// </summary>
    public sealed class CustomRewiredStandaloneInputModule : RewiredStandaloneInputModule
    {
        #region Methods
        protected override bool SendMoveEventToSelectedObject()
        {
            // Never allow movement while recompiling.
            if (base.recompiling)
            {
                return false;
            }
            
            var _time = Rewired.ReInput.time.unscaledTime; // Get the current time.
            var _movement = base.GetRawMoveVector(); // Check for zero movement and clear.
            
            if (Mathf.Approximately(_movement.x, 0f) && Mathf.Approximately(_movement.y, 0f)) 
            {
                base.m_ConsecutiveMoveCount = 0;
                return false;
            }

            // Check if movement is in the same direction as previously
            var _similarDir = Vector2.Dot(_movement, base.m_LastMoveVector) > 0;

            // Check if a button/key/axis was just pressed this frame.
            base.CheckButtonOrKeyMovement(out var _buttonDownHorizontal, out var _buttonDownVertical);

            AxisEventData _axisEventData = null;

            // If user just pressed button/key/axis, always allow movement.
            var _allow = _buttonDownHorizontal || _buttonDownVertical;
            
            
            if (_allow) // We had a button down event. 
            {

                // Get the axis move event now so we can tell the direction it will be moving.
                _axisEventData = base.GetAxisEventData(_movement.x, _movement.y, 0f);

                // Make sure the button down event was in the direction we would be moving, otherwise don't allow it.
                // This filters out double movements when pressing somewhat diagonally and getting down events for both -
                // horizontal and vertical at the same time but both ending up being resolved in the same direction.
                var _moveDir = _axisEventData.moveDir;
                _allow = (_moveDir is MoveDirection.Up or MoveDirection.Down && _buttonDownVertical) || (_moveDir is MoveDirection.Left or MoveDirection.Right && _buttonDownHorizontal);
            }

            if (!_allow) 
            {
                // Apply repeat delay and input actions per second limits
                if (base.m_RepeatDelay > 0.0f) 
                { 
                    // Otherwise, user held down key or axis.
                    // If direction didn't change at least 90 degrees, wait for delay before allowing consecutive event.
                    if (_similarDir && base.m_ConsecutiveMoveCount == 1) // This is the 2nd tick after the initial that activated the movement in this direction. 
                    { 
                        // If direction changed at least 90 degree, or we already had the delay, repeat at repeat rate.
                        _allow = _time > base.m_PrevActionTime + base.m_RepeatDelay;
                    } 
                    else 
                    {
                        // Apply input actions per second limit.
                        _allow = _time > base.m_PrevActionTime + 1f / base.m_InputActionsPerSecond; 
                    }
                } 
                else // Not using a repeat delay. 
                { 
                    // Apply input actions per second limit.
                    _allow = _time > base.m_PrevActionTime + 1f / base.m_InputActionsPerSecond; 
                }
            }

            if (!_allow)
            {
                return false; // Movement not allowed, done.
            }

            // Get the axis move event.
            _axisEventData ??= base.GetAxisEventData(_movement.x, _movement.y, 0f);
            
            base.eventSystem.currentSelectedGameObject.GetComponent<Selectable>().GetNextActive(_axisEventData.moveDir, out var _previous);
            
            if (_axisEventData.moveDir != MoveDirection.None) 
            {
                ExecuteEvents.Execute(_previous.gameObject, _axisEventData, ExecuteEvents.moveHandler);

                if (!_similarDir)
                {
                    base.m_ConsecutiveMoveCount = 0;
                }
                if (base.m_ConsecutiveMoveCount == 0 || !(_buttonDownHorizontal || _buttonDownVertical))
                {
                    base.m_ConsecutiveMoveCount++;
                }
                
                base.m_PrevActionTime = _time;
                base.m_LastMoveVector = _movement;
            } 
            else 
            {
                base.m_ConsecutiveMoveCount = 0;
            }

            return _axisEventData.used;
        }
        #endregion
    }
}
#endif
