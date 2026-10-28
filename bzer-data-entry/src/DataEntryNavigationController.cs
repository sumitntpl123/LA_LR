using PhoneticBengaliInput;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using Utils;

namespace bzer_data_entry.src
{
    internal class DataEntryNavigationController : NavigationController
    {
        private readonly Dictionary<PBIControl, PBIControl.OnReady> ReadyHandlers;

        private readonly Dictionary<Control, KeyEventHandler> KeyHandlers;

        protected DataEntryNavigationController(Control parent) : base(parent)
        {
            ReadyHandlers = new Dictionary<PBIControl, PBIControl.OnReady>();
            KeyHandlers = new Dictionary<Control, KeyEventHandler>();
        }

        private void MoveToFirstControl()
        {
            var firstControl = Controls.FirstOrDefault();
            if (firstControl.Control != null)
            {
                firstControl.Control.Select();
                firstControl.Control.Focus();
            }
        }

        private void MoveToNextControl(int index)
        {
            var nextControl = NextControl(index);
            if (nextControl != null)
            {
                nextControl.Select();
                nextControl.Focus();
            }
        }

        protected override void OnInit(bool enable)
        {
            if (Parent is Form parentForm)
            {
                var keyHandler = new KeyEventHandler((_, keyArgs) =>
                {
                    if (keyArgs.KeyCode == Keys.Escape)
                    {
                        parentForm.Close();
                    }
                });
                KeyHandlers.Add(parentForm, keyHandler);
            }
            foreach (var controlInfo in Controls)
            {
                var keyHandler = new KeyEventHandler((_, keyArgs) =>
                {
                    if (keyArgs.KeyCode == Keys.Return)
                    {
                        ControlEntryCompleted(controlInfo.Index);
                    }
                });
                KeyHandlers.Add(controlInfo.Control, keyHandler);
            }
            var firstControl = Controls.FirstOrDefault();
            if (firstControl.Control != null)
            {
                if (firstControl.Control is PBIControl pbiFirstControl)
                {
                    var readyHandler = new PBIControl.OnReady(() =>
                    {
                        pbiFirstControl.Select();
                        pbiFirstControl.Focus();
                    });
                    ReadyHandlers.Add(pbiFirstControl, readyHandler);
                }
            }
            if (enable)
            {
                Enable();
            }
        }

        protected override void OnEnable(bool enabled)
        {
            if (!enabled)
            {
                foreach (var controlEvtPair in ReadyHandlers)
                {
                    controlEvtPair.Key.Ready -= controlEvtPair.Value;
                }
                foreach (var controlEvtPair in KeyHandlers)
                {
                    if (controlEvtPair.Key is PBITextBox pbiTextBox)
                    {
                        pbiTextBox.InnerKeyDown -= controlEvtPair.Value;
                    }
                    else if (controlEvtPair.Key is TextBox textBox)
                    {
                        textBox.KeyDown -= controlEvtPair.Value;
                    }
                    else if (controlEvtPair.Key is Form form)
                    {
                        form.KeyPreview = false;
                        form.KeyDown -= controlEvtPair.Value;
                    }
                }
            }
            else
            {
                foreach (var controlEvtPair in ReadyHandlers)
                {
                    controlEvtPair.Key.Ready += controlEvtPair.Value;
                }
                foreach (var controlEvtPair in KeyHandlers)
                {
                    if (controlEvtPair.Key is PBITextBox pbiTextBox)
                    {
                        pbiTextBox.InnerKeyDown += controlEvtPair.Value;
                    }
                    else if (controlEvtPair.Key is TextBox textBox)
                    {
                        textBox.KeyDown += controlEvtPair.Value;
                    }
                    else if (controlEvtPair.Key is Form form)
                    {
                        form.KeyPreview = true;
                        form.KeyDown += controlEvtPair.Value;
                    }
                }
            }
        }

        protected override void OnControlEntryCompleted(int index, string name, Control control)
        {
            MoveToNextControl(index);
        }

        protected override void OnControlError(int index, string name, Control control)
        {
            control.Select();
            control.Focus();
        }

        protected override void OnReset()
        {
            foreach (var controlInfo in Controls)
            {
                if (controlInfo.Control is PBITextBox pbiTextBox)
                {
                    pbiTextBox.EnglishText = string.Empty;
                    pbiTextBox.BengaliPhoneticText = string.Empty;
                }
                else if (controlInfo.Control is PBIComboBox pbiCombo)
                {
                    pbiCombo.EnglishText = string.Empty;
                    pbiCombo.BengaliPhoneticText = string.Empty;
                    pbiCombo.SelectedIndex = -1;
                }
                else if (!(controlInfo.Control is IButtonControl))
                {
                    controlInfo.Control.Text = string.Empty;
                }
            }
            MoveToFirstControl();
        }

        public class Builder : Builder<DataEntryNavigationController>
        {
            public Builder(Control parent) : base(() => new DataEntryNavigationController(parent)) { }
        }
    }
}
