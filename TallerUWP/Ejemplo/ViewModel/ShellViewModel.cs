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
            Menu.Add(new MenuItem() { Glyph = "1", Text = "Ejercicio 1. Hello World", NavigationDestination = typeof(Ejercicio1) });
            Menu.Add(new MenuItem() { Glyph = "2", Text = "Ejercicio 2. Controles", NavigationDestination = typeof(Ejercicio2) });
            Menu.Add(new MenuItem() { Glyph = "3", Text = "Ejercicio 3. Visual States", NavigationDestination = typeof(Ejercicio3) });
            Menu.Add(new MenuItem() { Glyph = "4", Text = "Ejercicio 4. Pivot", NavigationDestination = typeof(Ejercicio4) });

            Menu.Add(new MenuItem() { Glyph = "5", Text = "Ejercicio 5. Selectores", NavigationDestination = typeof(Ejercicio5) });
            Menu.Add(new MenuItem() { Glyph = "6a", Text = "Ejercicio 6a. Live Tiles - Badges", NavigationDestination = typeof(Ejercicio6a) });
            Menu.Add(new MenuItem() { Glyph = "6b", Text = "Ejercicio 6b. Live Tiles - Primary And Secondary", NavigationDestination = typeof(Ejercicio6b) });
            Menu.Add(new MenuItem() { Glyph = "7", Text = "Ejercicio 7. Notificaciones Toast", NavigationDestination = typeof(Ejercicio7) });
        }
    }
}
