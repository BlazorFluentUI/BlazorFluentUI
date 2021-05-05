using System;
using System.Collections.Generic;

namespace BlazorFluentUI
{
    public class MaskedTextFieldInternalState
    {
        //private (ChangeType changeType, int selectionStart, int selectionEnd) changeSelectionData;
        //private List<MaskValue> maskCharData = new();
        //private bool isFocused;
        //private bool moveCursorOnMouseUp;
        //private int maskCursorPosition;

        public List<MaskValue> MaskCharData { get; set; } = new();
        //{
        //    get => maskCharData;
        //    set
        //    {
        //        maskCharData = value;
        //        NotifyStateChanged();
        //    }
        //}

        public bool IsFocused { get; set; } = false;
        //{
        //    get => isFocused;
        //    set
        //    {
        //        isFocused = value;
        //        NotifyStateChanged();
        //    }
        //}

        public bool MoveCursorOnMouseUp { get; set; } = false;
        //{
        //    get => moveCursorOnMouseUp;
        //    set
        //    {
        //        moveCursorOnMouseUp = value;
        //        NotifyStateChanged();
        //    }
        //}

        public int MaskCursorPosition { get; set; }
        //{
        //    get => maskCursorPosition;
        //    set
        //    {
        //        maskCursorPosition = value;
        //        NotifyStateChanged();
        //    }
        //}

        public (ChangeType changeType, int? selectionStart, int? selectionEnd) ChangeSelectionData { get; set; }
        //{
        //    get => changeSelectionData;
        //    set
        //    {
        //        changeSelectionData = value;
        //        NotifyStateChanged();
        //    }
        //}

        public event Action? OnChange;

        private void NotifyStateChanged() => OnChange?.Invoke();
    }
}
