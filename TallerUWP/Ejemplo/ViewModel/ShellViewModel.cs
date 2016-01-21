using Ejemplo.Common;
using Ejemplo.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ejemplo.ViewModel
{
    class ShellViewModel : ViewModelBase
    {
        public ShellViewModel()
        {
            // Build the menu
            Menu.Add(new MenuItem() { Glyph = "", Text = "Ejercicio 1. Hello World", NavigationDestination = typeof(Ejercicio1) });
            Menu.Add(new MenuItem() { Glyph = "", Text = "Ejercicio 2. Controles", NavigationDestination = typeof(Ejercicio2) });
            Menu.Add(new MenuItem() { Glyph = "", Text = "Ejercicio 3. Visual States", NavigationDestination = typeof(Ejercicio3) });
            Menu.Add(new MenuItem() { Glyph = "", Text = "Ejercicio 4. Pivot", NavigationDestination = typeof(Ejercicio4) });

            Menu.Add(new MenuItem() { Glyph = "", Text = "Ejercicio 5. Selectores", NavigationDestination = typeof(Ejercicio5) });
        }
    }
}
