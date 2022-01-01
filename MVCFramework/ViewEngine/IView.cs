using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MVCFramework.ViewEngine
{
    public interface IView
    {
        public string ExecuteTemplate(object viewModel);
    }
}
