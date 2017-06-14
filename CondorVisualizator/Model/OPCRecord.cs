

namespace OPCRecord
{
    public class OpcRecord
    {
        /// <summary>
        /// Значение
        /// </summary>
        public object Value
        {
            get;
            set;
        }
        /// <summary>
        /// Качество
        /// </summary>
        public short Quality
        {
            get;
            set;
        }
        /// <summary>
        /// Метка времени
        /// </summary>
        public string Timestamp
        {
            get;
            set;
        }
    }
    public class SOpcRecord
    {
        public OpcRecord OpcRec
        {
            get;
            set;
        }
        public double Sec
        {
            get;
            set;
        }
    }

}