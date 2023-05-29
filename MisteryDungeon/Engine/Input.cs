using Aiv.Fast2D;
using System;
using System.Collections.Generic;

namespace Aiv.Fast2D.Component {

    public enum MouseButton {
        LeftMouse,
        RightMouse
    }

    public enum JoystickButton {
        ButtonA,
        ButtonB,
        ButtonX,
        ButtonY,
        ButtonLeft,
        ButtonRight,
        ButtonUp,
        ButtonDown,
        ShoulderLeft,
        ShoulderRight,
        Start,
        None
    }

    public enum JoystickAxis {
        LeftStick_Horizontal,
        LeftStick_Vertical,
        RightStick_Horizontal,
        RightStick_Vertical,
        ShoulderTriggerLeft,
        ShoulderTriggerRight
    }

    public static class Input {

        private static Array keyArray;
        private static Array mouseArray;
        private static Array joystickArray;

        private static Dictionary<KeyCode, bool> lastKeyValues;
        private static Dictionary<MouseButton, bool> lastMouseButtonValues;
        private static Dictionary<JoystickButton, bool>[] lastJoystickButtonValues;

        private static Dictionary<string, UserButton> userButtons;
        private static Dictionary<string, UserAxis> userAxis;

        static Input () {
            keyArray = Enum.GetValues(typeof(KeyCode));
            mouseArray = Enum.GetValues(typeof(MouseButton));
            joystickArray = Enum.GetValues(typeof(JoystickButton));
            lastKeyValues = new Dictionary<KeyCode, bool>();
            foreach (KeyCode kc in keyArray) {
                lastKeyValues.Add(kc, false);
            }
            lastMouseButtonValues = new Dictionary<MouseButton, bool>();
            foreach(MouseButton mb in mouseArray) {
                lastMouseButtonValues.Add(mb, false);
            }
            lastJoystickButtonValues = new Dictionary<JoystickButton, bool>[Window.Joysticks.Length];
            for (int i = 0; i < lastJoystickButtonValues.Length; i++) {
                lastJoystickButtonValues[i] = new Dictionary<JoystickButton, bool>();
                foreach (JoystickButton jb in joystickArray) {
                    lastJoystickButtonValues[i].Add(jb, false);
                }
            }
            userButtons = new Dictionary<string, UserButton>();
            userAxis = new Dictionary<string, UserAxis>();
        }

        private static bool FromMouseButtonToBool (MouseButton mb) {
            switch (mb) {
                case MouseButton.LeftMouse:
                    return Game.Win.MouseLeft;
                case MouseButton.RightMouse:
                    return Game.Win.MouseRight;
                default:
                    return false;
            }
        }

        private static bool FromJoystickButtonToBool (int index, JoystickButton jb) {
            switch (jb) {
                case JoystickButton.ButtonA:
                    return Game.Win.JoystickA(index);
                case JoystickButton.ButtonB:
                    return Game.Win.JoystickB(index);
                case JoystickButton.ButtonDown:
                    return Game.Win.JoystickDown(index);
                case JoystickButton.ButtonLeft:
                    return Game.Win.JoystickLeft(index);
                case JoystickButton.ButtonRight:
                    return Game.Win.JoystickRight(index);
                case JoystickButton.ButtonUp:
                    return Game.Win.JoystickUp(index);
                case JoystickButton.ButtonX:
                    return Game.Win.JoystickX(index);
                case JoystickButton.ButtonY:
                    return Game.Win.JoystickY(index);
                case JoystickButton.Start:
                    return Game.Win.JoystickStart(index);
                default:
                    return false;

            }
        }

        public static void PerformLastKey () {
            foreach (KeyCode key in keyArray) {
                lastKeyValues[key] = Game.Win.GetKey(key);
            }
            foreach (MouseButton mb in mouseArray) {
                lastMouseButtonValues[mb] = FromMouseButtonToBool(mb);
            }
            for (int i = 0; i < lastJoystickButtonValues.Length; i++) {
                foreach(JoystickButton jb in joystickArray) {
                    lastJoystickButtonValues[i][jb] = FromJoystickButtonToBool(i,jb);
                }
            }
        }

        public static bool GetKeyDown (KeyCode keyCode) {
            return !lastKeyValues[keyCode] && Game.Win.GetKey(keyCode);
        }

        public static bool GetKey (KeyCode keyCode) {
            return lastKeyValues[keyCode] && Game.Win.GetKey(keyCode);
        }

        public static bool GetKeyUp (KeyCode keyCode) {
            return lastKeyValues[keyCode] && !Game.Win.GetKey(keyCode);
        }

        public static bool GetMouseButtonDown (MouseButton mb) {
            return !lastMouseButtonValues[mb] && FromMouseButtonToBool(mb);
        }

        public static bool GetMouseButton (MouseButton mb) {
            return lastMouseButtonValues[mb] &&  FromMouseButtonToBool(mb);
        }

        public static bool GetMouseButtonUp (MouseButton mb) {
            return lastMouseButtonValues[mb] && !FromMouseButtonToBool(mb);
        }

        public static bool GetJoystickButtonDown (int index, JoystickButton jb) {
            return !lastJoystickButtonValues[index][jb] && FromJoystickButtonToBool(index, jb);
        }

        public static bool GetJoystickButton (int index, JoystickButton jb) {
            return lastJoystickButtonValues[index][jb] && FromJoystickButtonToBool(index, jb);
        }

        public static bool GetJoystickButtonUp (int index, JoystickButton jb) {
            return lastJoystickButtonValues[index][jb] && !FromJoystickButtonToBool(index, jb);
        }

        public static float GetJoystickAxis (int index, JoystickAxis axis) {
            switch (axis) {
                case JoystickAxis.LeftStick_Horizontal:
                    return Game.Win.JoystickAxisLeft(index).X;
                case JoystickAxis.LeftStick_Vertical:
                    return Game.Win.JoystickAxisLeft(index).Y;
                case JoystickAxis.RightStick_Horizontal:
                    return Game.Win.JoystickAxisRight(index).X;
                case JoystickAxis.RightStick_Vertical:
                    return Game.Win.JoystickAxisRight(index).Y;
                case JoystickAxis.ShoulderTriggerLeft:
                    return Game.Win.JoystickTriggerLeft(index);
                case JoystickAxis.ShoulderTriggerRight:
                    return Game.Win.JoystickTriggerRight(index);
                default:
                    return 0;

            }
        }

        public static void AddUserButton (string actionName, ButtonMatch[] matches) {
            if (userButtons.ContainsKey(actionName)) return;
            userButtons.Add(actionName, new UserButton(matches));
        }

        public static bool GetUserButton (string actionName) {
            if (!userButtons.ContainsKey(actionName)) return false;
            return userButtons[actionName].GetButton();
        }

        public static bool GetUserButtonDown (string actionName) {
            if (!userButtons.ContainsKey(actionName)) return false;
            return userButtons[actionName].GetButtonDown();
        }

        public static bool GetUserButtonUp (string actionName) {
            if (!userButtons.ContainsKey(actionName)) return false;
            return userButtons[actionName].GetButtonUp();
        }

        public static void AddUserAxis (string actionName, AxisMatch[] matches) {
            userAxis.Add(actionName, new UserAxis(matches));
        }

        public static float GetAxis (string actionName) {
            if (!userAxis.ContainsKey(actionName)) return 0;
            return userAxis[actionName].GetAxis();
        }

    }

    public class UserButton {

        public ButtonMatch[] bindedMatches;

        public UserButton (ButtonMatch[] bindedMatches) {
            this.bindedMatches = bindedMatches;
        }

        public bool GetButton () {
            foreach (ButtonMatch b in bindedMatches) {
                if (!b.GetButton()) continue;
                return true;
            }
            return false;
        }

        public bool GetButtonDown () {
            bool value = false;
            foreach(ButtonMatch b in bindedMatches) {
                value = value || b.GetButtonDown();
            }
            return value && !GetButton();
        }

        public bool GetButtonUp () {
            bool value = false;
            foreach(ButtonMatch b in bindedMatches) {
                value = value || b.GetButtonUp();
            }
            return value && !GetButton();
        }

    }

    public abstract class ButtonMatch {
        public abstract bool GetButtonDown();
        public abstract bool GetButton();
        public abstract bool GetButtonUp();
    }

    public class KeyButtonMatch : ButtonMatch {

        private KeyCode match;

        public KeyButtonMatch (KeyCode match) {
            this.match = match;
        }

        public override bool GetButton() {
            return Input.GetKey(match);
        }

        public override bool GetButtonDown() {
            return Input.GetKeyDown(match);
        }

        public override bool GetButtonUp() {
            return Input.GetKeyUp(match);
        }

    }

    public class MouseButtonMatch : ButtonMatch {

        private MouseButton match;

        public MouseButtonMatch (MouseButton match) {
            this.match = match;
        }

        public override bool GetButton() {
            return Input.GetMouseButton(match);
        }

        public override bool GetButtonDown() {
            return Input.GetMouseButtonDown(match);
        }

        public override bool GetButtonUp() {
            return Input.GetMouseButtonUp(match);
        }

    }

    public class JoystickButtonMatch : ButtonMatch {

        private int index;
        private JoystickButton match;

        public JoystickButtonMatch (int index, JoystickButton match) {
            this.index = index;
            this.match = match;
        }

        public override bool GetButton() {
            return Input.GetJoystickButton(index, match);
        }

        public override bool GetButtonDown() {
            return Input.GetJoystickButtonDown(index, match);
        }

        public override bool GetButtonUp() {
            return Input.GetJoystickButtonUp(index, match);
        }
    }

    public class UserAxis {

        public AxisMatch[] axisMatch;
        
        public UserAxis (AxisMatch[] axisMatch) {
            this.axisMatch = axisMatch;
        }

        public float GetAxis () {
            float value = 0;
            foreach (AxisMatch match in axisMatch) {
                value += match.GetAxis();
            }
            value = value < -1 ? -1 : value > 1 ? 1 : value;
            return value;
        }

    }

    public abstract class AxisMatch {

        public abstract float GetAxis();

    }

    public class KeyAxisMatch : AxisMatch{

        private KeyCode negativeKeyCode;
        private KeyCode positiveKeyCode;

        public KeyAxisMatch (KeyCode negativeKeyCode, KeyCode positiveKeyCode) {
            this.negativeKeyCode = negativeKeyCode;
            this.positiveKeyCode = positiveKeyCode;
        }

        public override float GetAxis () {
            float value = 0;
            value -= Input.GetKey(negativeKeyCode) ? 1 : 0;
            value += Input.GetKey(positiveKeyCode) ? 1 : 0;
            return value;
        }

    }

    public class MouseAxisMatch : AxisMatch {

        private MouseButton negativeMouseButton;
        private MouseButton positiveMouseButton;

        public MouseAxisMatch(MouseButton negativeMouseButton, MouseButton positiveMouseButton) {
            this.negativeMouseButton = negativeMouseButton;
            this.positiveMouseButton = positiveMouseButton;
        }

        public override float GetAxis() {
            float value = 0;
            value -= Input.GetMouseButton(negativeMouseButton) ? 1 : 0;
            value += Input.GetMouseButton(positiveMouseButton) ? 1 : 0;
            return value;
        }

    }

    public class JoystickAxisMatch : AxisMatch {

        private JoystickAxis axis;
        private int index;

        public JoystickAxisMatch (JoystickAxis axis, int index) {
            this.axis = axis;
            this.index = index;
        }

        public override float GetAxis () {
            return Input.GetJoystickAxis(index, axis);
        }

    }
}
