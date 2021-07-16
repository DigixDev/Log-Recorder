using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;

namespace Log_Recorder.Classes.Commands
{
    public static class CustomCommands
    {
        public static RoutedCommand ExitApplication = new RoutedCommand("ExitApplication", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.X, ModifierKeys.Control) });
        public static RoutedCommand Export = new RoutedCommand("Export", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.P, ModifierKeys.Control) });
        public static RoutedCommand AddSheet = new RoutedCommand("AddSheet", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.A, ModifierKeys.Control) });
        public static RoutedCommand DeleteActiveSheet = new RoutedCommand("DeleteActiveSheet", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.Delete, ModifierKeys.Control) });
        public static RoutedCommand RenameActiveSheet = new RoutedCommand("RenameActiveSheet", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.R, ModifierKeys.Control) });
        public static RoutedCommand ChangeType = new RoutedCommand("ChangeType", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control) });
        public static RoutedCommand ChangeTypeToWS = new RoutedCommand("ChangeTypeToWS", typeof(CustomCommands));
        public static RoutedCommand ChangeTypeToBH = new RoutedCommand("ChangeTypeToBH", typeof(CustomCommands));
        public static RoutedCommand ChangeTypeToRBH = new RoutedCommand("ChangeTypeToRBH", typeof(CustomCommands));
        public static RoutedCommand ChangeTypeToTP = new RoutedCommand("ChangeTypeToTP", typeof(CustomCommands));

        public static RoutedCommand NewProject = new RoutedCommand("NewProject", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.N, ModifierKeys.Control) });
        public static RoutedCommand OpenProject = new RoutedCommand("OpenProject", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.O, ModifierKeys.Control), new MouseGesture(MouseAction.MiddleClick) });
        public static RoutedCommand SaveProject = new RoutedCommand("SaveProject", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.S, ModifierKeys.Control) });
        public static RoutedCommand OpenProjectInfo = new RoutedCommand("OpenProjectInfo", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.I, ModifierKeys.Control) });
        public static RoutedCommand OpenUserList = new RoutedCommand("OpenUserList", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.U, ModifierKeys.Control) });
        public static RoutedCommand ShowStrataPatterns = new RoutedCommand("ShowStrataPatterns", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.E, ModifierKeys.Control) });
        public static RoutedCommand ChangePassword = new RoutedCommand("ChangePassword", typeof(CustomCommands), new InputGestureCollection() { new KeyGesture(Key.G, ModifierKeys.Control) });
    }
}
