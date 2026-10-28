using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

namespace Utils
{
    internal class NamedControl
    {
        public string Name;

        public Control Control;
    }

    internal class IndexedControl
    {
        public int Index;

        public Control Control;
    }

    public class NavigationController
    {
        private readonly Control _Parent;

        private readonly SortedList<int, NamedControl> IndexedControls;

        private readonly Dictionary<string, IndexedControl> NamedControls;

        protected Control Parent { get => _Parent; }

        protected IEnumerable<(Control Control, int Index, string Name)> Controls
        {
            get => IndexedControls
                .Select(indexNamedControlPair => (indexNamedControlPair.Value.Control, indexNamedControlPair.Key, indexNamedControlPair.Value.Name));
        }

        protected Control NextControl(int index)
        {
            var nextKeyIndex = IndexedControls.IndexOfKey(index) + 1;
            if (nextKeyIndex < IndexedControls.Count)
            {
                return IndexedControls[IndexedControls.Keys[nextKeyIndex]].Control;
            }
            return null;
        }

        protected Control PrevControl(int index)
        {
            var prevKeyIndex = IndexedControls.IndexOfKey(index) - 1;
            if (prevKeyIndex > -1)
            {
                return IndexedControls[IndexedControls.Keys[prevKeyIndex]].Control;
            }
            return null;
        }

        protected Control GetControl(int index)
        {
            if (IndexedControls.ContainsKey(index))
            {
                return IndexedControls[index].Control;
            }
            return null;
        }

        protected Control GetControl(string controlName)
        {
            if (NamedControls.ContainsKey(controlName))
            {
                return NamedControls[controlName].Control;
            }
            return null;
        }

        protected virtual void OnInit(bool enable)
        {
            Debug.WriteLine($"[{Parent.Name}]: Navigationcontroller started");
        }

        protected virtual void OnReset()
        {
            Debug.WriteLine($"[{Parent.Name}]: Navigationcontroller reset");
        }

        protected virtual void OnEnable(bool enabled)
        {
            Debug.WriteLine($"[{Parent.Name}]: Navigationcontroller {(enabled ? "en" : "dis")}abled");
        }

        protected virtual void OnControlEntryCompleted(int index, string name, Control control)
        {
            Debug.WriteLine($"[{Parent.Name}]: Entry completed for {name}:{control.Name} at index {index}");
        }

        protected virtual void OnControlError(int index, string name, Control control)
        {
            Debug.WriteLine($"[{Parent.Name}]: Error for {name}:{control.Name} at index {index}");
        }

        protected NavigationController(Control parent)
        {
            _Parent = parent;
            IndexedControls = new SortedList<int, NamedControl>();
            NamedControls = new Dictionary<string, IndexedControl>();
        }

        private void AddControl(int index, string controlName, Control control)
        {
            IndexedControls.Add(index, new NamedControl { Name = controlName, Control = control });
            NamedControls.Add(controlName, new IndexedControl { Index = index, Control = control });
        }

        private bool RemoveControl(int index)
        {
            var found = false;
            if (IndexedControls.ContainsKey(index))
            {
                var namedControl = IndexedControls[index];
                Debug.Assert(NamedControls.ContainsKey(namedControl.Name));
                found = NamedControls.Remove(namedControl.Name);
                found = found && IndexedControls.Remove(index);
            }
            return found;
        }

        private bool RemoveControl(string controlName)
        {
            var found = false;
            if (NamedControls.ContainsKey(controlName))
            {
                var indexedControl = NamedControls[controlName];
                Debug.Assert(IndexedControls.ContainsKey(indexedControl.Index));
                found = IndexedControls.Remove(indexedControl.Index);
                found = found && NamedControls.Remove(controlName);
            }
            return found;
        }

        private void Ready(bool enable = true)
        {
            OnInit(enable);
        }

        public void Reset()
        {
            OnReset();
        }

        public void Enable(bool enable = true)
        {
            OnEnable(enable);
        }

        public void ControlEntryCompleted(int index)
        {
            if (IndexedControls.ContainsKey(index))
            {
                var namedControl = IndexedControls[index];
                OnControlEntryCompleted(index, namedControl.Name, namedControl.Control);
            }
        }

        public void ControlEntryCompleted(string controlName)
        {
            if (NamedControls.ContainsKey(controlName))
            {
                var indexedControl = NamedControls[controlName];
                OnControlEntryCompleted(indexedControl.Index, controlName, indexedControl.Control);
            }
        }

        public void ControlError(int index)
        {
            if (IndexedControls.ContainsKey(index))
            {
                var namedControl = IndexedControls[index];
                OnControlError(index, namedControl.Name, namedControl.Control);
            }
        }

        public void ControlError(string controlName)
        {
            if (NamedControls.ContainsKey(controlName))
            {
                var indexedControl = NamedControls[controlName];
                OnControlError(indexedControl.Index, controlName, indexedControl.Control);
            }
        }

        public class Builder<T> where T : NavigationController
        {
            private readonly T PartialController;

            public Builder(Func<T> controllerInit)
            {
                PartialController = controllerInit();
            }

            public void AddControl(int index, string controlName, Control control)
            {
                PartialController.AddControl(index, controlName, control);
            }

            public bool RemoveControl(int index)
            {
                return PartialController.RemoveControl(index);
            }

            public bool RemoveControl(string controlName)
            {
                return PartialController.RemoveControl(controlName);
            }

            public T Ready(bool enable = true)
            {
                PartialController.Ready(enable);
                return PartialController;
            }
        }
    }

    public class NavigationControllerSet
    {
        private readonly List<NavigationController> Controllers;

        public NavigationControllerSet()
        {
            Controllers = new List<NavigationController>();
        }

        public NavigationControllerSet(params NavigationController[] controllers) : this()
        {
            Controllers.AddRange(controllers);
        }

        public void Enable(NavigationController controller)
        {
            foreach (var ctlr in Controllers)
            {
                ctlr.Enable(ctlr == controller);
            }
        }

        public void DisableAll()
        {
            foreach (var ctlr in Controllers)
            {
                ctlr.Enable(false);
            }
        }
    }
}
