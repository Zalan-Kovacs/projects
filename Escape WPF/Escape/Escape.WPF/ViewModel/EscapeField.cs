using Escape.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Escape.WPF.ViewModel
{
    public class EscapeField :ViewModelBase
    {
        private Boolean _isEnabled;
        private String _text = String.Empty;
        private String _color = String.Empty;
        
        public Boolean IsEnabled
        {
            get { return _isEnabled; }
            set
            {
                if (_isEnabled != value)
                {
                    _isEnabled = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public String Text
        {
            get { return _text; }
            set
            {
                if (_text != value)
                {
                    _text = value;
                    OnPropertyChanged();
                }
            }
        }
        public String Color
        {
            get { return _color; }
            set
            {
                if (_color != value)
                {
                    _color = value;
                    OnPropertyChanged(nameof(Color));
                }
            }
        }
        public int X { get; set; }
        public int Y { get; set; }

        public DelegateCommand? StepCommand { get; set; }

    }
}
