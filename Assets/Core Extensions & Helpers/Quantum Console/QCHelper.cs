using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using QFSW.QC;
using System;

namespace Core.Extensions
{
    public static class QCHelper
    {
        public static bool IsOpen => QuantumConsole.Instance == null ? false : QuantumConsole.Instance.IsActive;
        public static void BindOpenAction(Action a)
        {
            QuantumConsole.Instance.OnActivate += a;
        }
        public static void BindCloseAction(Action a)
        {
            QuantumConsole.Instance.OnDeactivate += a;
        }
        public static void ReleaseOpenAction(Action a)
        {
            QuantumConsole.Instance.OnActivate -= a;
        }
        public static void ReleaseCloseAction(Action a)
        {
            QuantumConsole.Instance.OnDeactivate -= a;
        }
    }
}
