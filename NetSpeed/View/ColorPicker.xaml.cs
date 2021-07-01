using NetSpeed.Util;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace NetSpeed.View
{
    internal partial class ColorPicker : MenuItem, INotifyPropertyChanged
    {
        private Color color;
        private Brush selectedColor;
        private Brush selectedColorReverse;

        public Brush SelectedColor
        {
            get => selectedColor;
            set
            {
                Set(ref selectedColor, value);
                SelectedColorReverse = new SolidColorBrush(Color.FromRgb((byte)(255 - color.R), (byte)(255 - color.G), (byte)(255 - color.B)));
            }
        }

        public Brush SelectedColorReverse
        {
            get => selectedColorReverse;
            set => Set(ref selectedColorReverse, value);
        }






        public event PropertyChangedEventHandler PropertyChanged;
        private bool Set<T>(ref T field, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (EqualityComparer<T>.Default.Equals(field, newValue))
            {
                return false;
            }
            field = newValue;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
            return true;
        }

        public ColorPicker()
        {
            InitializeComponent();
            color = ColorUtil.HexToDecColor(AppSetting.TextColor);
            // 初始化圆的颜色
            SelectedColor = new SolidColorBrush(color);
            // 初始化十六进制
            // 初始化十进制

        }

        // 颜色更新通知
        // 重置颜色命令


    }
}
