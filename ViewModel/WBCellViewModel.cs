using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Final.ViewModel
{
    public class WBCellViewModel
    {

        public static Object CreatWBStatusCell(WBStatusViewModel viewModel)
        {
            if (viewModel.status.retweeted_status == null)
            {
                return new WBCell(viewModel);
            }
            else
            {
                return new RetweetWBCell(viewModel);
            }
        }
    }
}
