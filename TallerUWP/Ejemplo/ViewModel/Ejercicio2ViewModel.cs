using Ejemplo.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Metadata;

namespace Ejemplo.ViewModel
{
    public class Ejercicio2ViewModel : ViewModelBase
    {
        public Ejercicio2ViewModel()
        {
            var api = "Windows.Phone.UI.Input.HardwareButtons";
            TieneGPS = ApiInformation.IsTypePresent(api);
        }

        private bool _tieneGPS;
        public bool TieneGPS
        {
            get { return _tieneGPS; }
            set { SetProperty(ref _tieneGPS, value); }
        }


    }
}
