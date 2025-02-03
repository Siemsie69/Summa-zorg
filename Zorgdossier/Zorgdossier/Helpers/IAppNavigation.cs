using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Zorgdossier.Helpers
{
    public interface IAppNavigation
    {
        public object ActiveViewModel
        {
            get; set;
        }
    }
}