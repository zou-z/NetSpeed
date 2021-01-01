using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace NetSpeed.Module
{
    public partial class ColorSelector : MenuItem, INotifyPropertyChanged
    {
        private readonly char[] array = new char[16] { '0', '1', '2', '3', '4', '5', '6', '7', '8', '9', 'A', 'B', 'C', 'D', 'E', 'F' };
        public event PropertyChangedEventHandler PropertyChanged;

        public delegate void SetTextColorDelegate(Brush color);
        public event SetTextColorDelegate SetTextColor;

        public Brush Color { get; set; } = Brushes.White;
        public string HexColor { get; set; } = "#FFFFFF";
        public byte R { get; set; } = 255;
        public byte G { get; set; } = 255;
        public byte B { get; set; } = 255;

        public ColorSelector()
        {
            InitializeComponent();
            this.DataContext = this;
        }

        private void ColorSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Color = new SolidColorBrush(System.Windows.Media.Color.FromRgb(R, G, B));
            HexColor = $"#{DecToHex(R)}{DecToHex(G)}{DecToHex(B)}";
            ApplyProperty();
            SetTextColor(Color);
        }

        private void ApplyProperty()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Color"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("HexColor"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("R"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("G"));
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("B"));
        }

        private string DecToHex(byte hex)
        {
            return $"{array[(hex & 0xf0) >> 4]}{array[hex & 0x0f]}";
        }

        private void ResetColor(object sender, RoutedEventArgs e)
        {
            Color = Brushes.White;
            HexColor = "#FFFFFF";
            R = G = B = 255;
            ApplyProperty();
            SetTextColor(Color);
        }
    }
}
