using Sapper.Infrastructure.Commands;
using Sapper.ViewModels.Base;
using System;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;

namespace Sapper.ViewModels
{
    internal class CellVM : ViewModel
    {
        #region Fields

        public bool IsFlag { get; set; }

        public int FontSize { get; private set; }

        #endregion

        #region Properties

        #region Value

        private object _value;

        public object Value
        {
            get => _value;
            set => Set(ref _value, value);
        }

        #endregion

        #region Visibility

        private Visibility _visibility;

        public Visibility Visibility
        {
            get => _visibility;
            set => Set(ref _visibility, value);
        }

        #endregion

        #region Uid

        private string _uid;

        public string Uid
        {
            get => _uid;
            set => Set(ref _uid, value);
        }

        #endregion

        #region Foreground

        private Brush _foreground;

        public Brush Foreground
        {
            get => _foreground;
            set => Set(ref _foreground, value);
        }

        #endregion

        #region Background

        private Brush _background;

        public Brush Background
        {
            get => _background;
            set => Set(ref _background, value);
        }

        #endregion

        #endregion

        #region Commands

        public ICommand SetFlagCommand { get; }

        public ICommand ClickBorderCommand { get; }

        public ICommand ClickLabelCommand { get; }

        #endregion

        public CellVM(int fontSize, object Value, Visibility Visibility, string Uid, Brush Foreground, Brush Background, Action<object> onSetFlag, Func<object, bool> canSetFlag, Action<object> onClickBorder, Func<object, bool> canClickBorder, Action<object> onClickLabel, Func<object, bool> canClickLabel)
        {
            this.Value = Value;
            this.Visibility = Visibility;
            this.Uid = Uid;
            this.Foreground = Foreground;
            this.Background = Background;
            this.SetFlagCommand = new LambdaCommand(onSetFlag, canSetFlag);
            this.ClickBorderCommand = new LambdaCommand(onClickBorder, canClickBorder);
            this.ClickLabelCommand = new LambdaCommand(onClickLabel, canClickLabel);
            this.IsFlag = false;
            this.FontSize = fontSize;
        }
    }
}
