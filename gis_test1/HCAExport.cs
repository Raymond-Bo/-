using System;
using System.Collections.Generic;

namespace gis_test1
{
    internal partial class  HCAExport
    {
        private List<BufferAnalysis.ExportInfo> exportInfoList;
        private Dictionary<int, List<BufferAnalysis.HCA>> hCADic;

        public HCAExport(ref Dictionary<int, List<BufferAnalysis.HCA>> hCADic, ref List<BufferAnalysis.ExportInfo> exportInfoList)
        {
            this.hCADic = hCADic;
            this.exportInfoList = exportInfoList;
        }

        internal void Show()
        {
            throw new NotImplementedException();
        }
    }
}