﻿using NetSpeed.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSpeed.View
{
    internal partial class ColorPicker : MenuItem, INotifyPropertyChanged
    {
        public event Action UpdateTextColor;
        private Brush selectedColor;
        private Brush selectedColorReverse;
        private string selectedColorText;
        private byte selectedR;
        private byte selectedG;
        private byte selectedB;

        public Brush SelectedColor
        {
            get => selectedColor;
            set => Set(ref selectedColor, value);
        }

        public Brush SelectedColorReverse
        {
            get => selectedColorReverse;
            set => Set(ref selectedColorReverse, value);
        }

        public string SelectedColorText
        {
            get => selectedColorText;
            set => Set(ref selectedColorText, value);
        }

        public byte SelectedR
        {
            get => selectedR;
            set
            {
                _ = Set(ref selectedR, value);
                ApplyNewColor();
            }
        }

        public byte SelectedG
        {
            get => selectedG;
            set
            {
                _ = Set(ref selectedG, value);
                ApplyNewColor();
            }
        }

        public byte SelectedB
        {
            get => selectedB;
            set
            {
                _ = Set(ref selectedB, value);
                ApplyNewColor();
            }
        }

        private RelayCommand resetColorCommand;
        public RelayCommand ResetColorCommand => resetColorCommand ?? (resetColorCommand = new RelayCommand(() => { Init(AppSetting.DefaultTextColor, true); }));

        #region 属性更改通知
        public event PropertyChangedEventHandler PropertyChanged;
        private bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
        private void RaisePropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
        #endregion

        public ColorPicker()
        {
            InitializeComponent();
            Init(AppSetting.TextColor);
        }

        private void Init(string hexColor, bool isUpdate = false)
        {
            Color color = ColorUtil.HexToDecColor(hexColor);
            selectedR = color.R;
            selectedG = color.G;
            selectedB = color.B;
            RaisePropertyChanged("SelectedR");
            RaisePropertyChanged("SelectedG");
            RaisePropertyChanged("SelectedB");
            ApplyNewColor(isUpdate);
        }

        private void ApplyNewColor(bool isUpdate = true)
        {
            Color color = new Color { A = 255, R = SelectedR, G = SelectedG, B = SelectedB };
            SelectedColor = new SolidColorBrush(color);
            SelectedColorReverse = new SolidColorBrush(Color.FromRgb((byte)~color.R, (byte)~color.G, (byte)~color.B));
            SelectedColorText = ColorUtil.DecToHexColor(color);
            if (isUpdate)
            {
                AppSetting.TextColor = SelectedColorText;
                UpdateTextColor?.Invoke();
            }
        }
    }
}
