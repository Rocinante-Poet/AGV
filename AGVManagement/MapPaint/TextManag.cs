using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AGVManagement.MapPaint
{
    public class TextManag
    {
        /// <summary>
        /// 根据TagID查询字典中的Label
        /// </summary>
        /// <returns></returns>
        public Label TextSelct(int TagID)
        {
            foreach (int item in MapInstrument.GetKeyValues.Keys)
            {
                if (item.Equals(TagID))
                {
                    return MapInstrument.GetKeyValues[item];
                }
            }
            return null;
        }

        /// <summary>
        /// 文字删除
        /// </summary>
        /// <param name="TagID"></param>
        public void TextDelete(int TagID, Canvas canvas)
        {
            canvas.Children.Remove(MapInstrument.GetKeyValues[TagID]);
            MapInstrument.GetKeyValues.Remove(TagID);
        }
    }
}
