using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace AGVManagement.MapPaint
{
   public class TagManag
    {
        /// <summary>
        /// 根据TagID查询字典中的Label
        /// </summary>
        /// <returns></returns>
        public Label TagSelct(int TagID)
        {
            foreach (int item in MapInstrument.valuePairs.Keys)
            {
                if (item.Equals(TagID))
                {
                    return MapInstrument.valuePairs[item];
                }
            }
            return null;
        }

        /// <summary>
        /// Tag删除
        /// </summary>
        /// <param name="TagID"></param>
        public void TagDelete(int TagID, Canvas canvas)
        {
            canvas.Children.Remove(MapInstrument.valuePairs[TagID]);
            MapInstrument.valuePairs.Remove(TagID);
            CircuitDelete(canvas,TagID);
        }



        /// <summary>
        /// 删除信标关联线路
        /// </summary>
        /// <param name="TagID"></param>
        public void CircuitDelete(Canvas canvas,int TagID)
        {
            foreach (WirePointArray item in MapInstrument.wirePointArrays.ToArray())
            {
                if (item.GetPoint.TagID.Equals(TagID))
                {
                    canvas.Children.Remove(item.GetPath);
                    if (item.Paths != null)
                    {
                        foreach (var ite in item.Paths)
                        {
                            canvas.Children.Remove(ite);
                        }
                    }
                    MapInstrument.wirePointArrays.Remove(item);
                }
                else if (item.GetWirePoint.TagID.Equals(TagID))
                {
                    canvas.Children.Remove(item.GetPath);
                    if (item.Paths != null)
                    {
                        foreach (var ite in item.Paths)
                        {
                            canvas.Children.Remove(ite);
                        }
                    }
                    MapInstrument.wirePointArrays.Remove(item);
                }
            }
        }

    }
}
