using System.Windows.Controls;
using Zth.VM;

namespace Zth.Pages
{
    public class CommonPage : Page
    {
        public virtual CommonVM Vm { get; set; } = new CommonVM();
    }
}