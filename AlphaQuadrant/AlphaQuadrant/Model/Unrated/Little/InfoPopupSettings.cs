using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AlphaQuadrant
{
    public class InfoPopupSettings
    {
        #region Fields
        #endregion

        #region Properties
        public bool IsColonize { get; set; }
        public bool IsWorkerShipCreate { get; set; }
        public bool IsTerraform { get; set; }
        public bool IsCross { get; set; }
        public bool IsName { get; set; }
        public bool IsStationBuilder { get; set; }

        public static readonly InfoPopupSettings Clear = new InfoPopupSettings(); 
        #endregion

        #region Construct
        public InfoPopupSettings(bool isCross, bool isName, bool isColonize, bool isWorker, bool isTerraform, bool isStationBuilder)
        {
            IsColonize = isColonize;
            IsWorkerShipCreate = isWorker;
            IsTerraform = isTerraform;
            IsCross = isCross;
            IsName = isName;
            IsStationBuilder = isStationBuilder;
        }
        public InfoPopupSettings(bool isCross, bool isName, bool isColonize, bool isWorker, bool isTerraform) 
            : this(isCross, isName, isColonize, isWorker, isTerraform, false) { }
        public InfoPopupSettings(bool isCross, bool isName, bool isColonize, bool isWorker) : this(isCross, isName, isColonize, isWorker, false) { }
        public InfoPopupSettings(bool isCross, bool isName, bool isColonize) : this(isCross, isName, isColonize, false) { }
        public InfoPopupSettings(bool isCross, bool isName) : this(isCross, isName, false) { }
        public InfoPopupSettings(bool isCross) : this(isCross, false) { }
        public InfoPopupSettings() : this(false) { }
        #endregion
    }
}
