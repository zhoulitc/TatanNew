using System.Runtime.Serialization;

namespace Tatan._12306Logic.Query
{
    [DataContract]
    public class TicketResult
    {
        [DataMember(Name = "queryLeftNewDTO")]
        public TicketBody Body { get; set; }

        /// <summary>
        /// 火车票信息的签名
        /// </summary>
        [DataMember(Name = "secretStr")]
        public string SecretStr { get; set; }

        [DataMember(Name = "buttonTextInfo")]
        public string ButtonTextInfo { get; set; }
    }

    [DataContract]
    public class TicketBody
    {
        [DataMember(Name = "train_no")]
        public string TrainNo { get; set; }

        /// <summary>
        /// 车次
        /// </summary>
        [DataMember(Name = "station_train_code")]
        public string StationTrainCode { get; set; }

        [DataMember(Name = "start_station_telecode")]
        public string BeginStationcode { get; set; }

        [DataMember(Name = "start_station_name")]
        public string BeginStationName { get; set; }

        [DataMember(Name = "end_station_telecode")]
        public string EndStationcode { get; set; }

        [DataMember(Name = "end_station_name")]
        public string EndStationName { get; set; }

        [DataMember(Name = "from_station_telecode")]
        public string FromStationcode { get; set; }

        [DataMember(Name = "from_station_name")]
        public string FromStationName { get; set; }

        [DataMember(Name = "to_station_telecode")]
        public string ToStationcode { get; set; }

        [DataMember(Name = "to_station_name")]
        public string ToStationName { get; set; }

        [DataMember(Name = "start_time")]
        public string StartTime { get; set; }

        [DataMember(Name = "arrive_time")]
        public string ArriveTime { get; set; }

        [DataMember(Name = "day_difference")]
        public int DayDifference { get; set; }

        [DataMember(Name = "train_class_name")]
        public string TrainClassName { get; set; }

        [DataMember(Name = "lishi")]
        public string Lishi { get; set; }

        [DataMember(Name = "canWebBuy")]
        public string CanWebBuy { get; set; }

        [DataMember(Name = "lishiValue")]
        public string LishiValue { get; set; }

        [DataMember(Name = "yp_info")]
        public string YpInfo { get; set; }

        [DataMember(Name = "control_train_day")]
        public string ControlTrainDay { get; set; }

        [DataMember(Name = "start_train_date")]
        public string StartTrainDate { get; set; }

        [DataMember(Name = "seat_feature")]
        public string SeatFeature { get; set; }

        [DataMember(Name = "yp_ex")]
        public string YpEx { get; set; }

        [DataMember(Name = "train_seat_feature")]
        public string TrainSeatFeature { get; set; }

        [DataMember(Name = "seat_types")]
        public string SeatTypes { get; set; }

        [DataMember(Name = "location_code")]
        public string LocationCode { get; set; }

        [DataMember(Name = "control_day")]
        public int ControlDay { get; set; }

        [DataMember(Name = "sale_time")]
        public string SaleTime { get; set; }

        [DataMember(Name = "is_support_card")]
        public string IsSupportCard { get; set; }

        [DataMember(Name = "gg_num")]
        public string GgNum { get; set; }

        /// <summary>
        /// 高级软卧
        /// </summary>
        [DataMember(Name = "gr_num")]
        public string GrNum { get; set; }

        /// <summary>
        /// 其它
        /// </summary>
        [DataMember(Name = "qt_num")]
        public string QtNum { get; set; }

        [DataMember(Name = "yb_num")]
        public string YbNum { get; set; }

        /// <summary>
        /// 软卧
        /// </summary>
        [DataMember(Name = "rw_num")]
        public string SoftLying { get; set; }

        /// <summary>
        /// 软座
        /// </summary>
        [DataMember(Name = "rz_num")]
        public string SoftSeat { get; set; }

        /// <summary>
        /// 无座
        /// </summary>
        [DataMember(Name = "wz_num")]
        public string NoneSeat { get; set; }

        /// <summary>
        /// 硬卧
        /// </summary>
        [DataMember(Name = "yw_num")]
        public string HardLying { get; set; }

        /// <summary>
        /// 硬座
        /// </summary>
        [DataMember(Name = "yz_num")]
        public string HardSeat { get; set; }

        /// <summary>
        /// 二等座
        /// </summary>
        [DataMember(Name = "ze_num")]
        public string SecondSeat { get; set; }

        /// <summary>
        /// 一等座
        /// </summary>
        [DataMember(Name = "zy_num")]
        public string FristSeat { get; set; }

        /// <summary>
        /// 特等座
        /// </summary>
        [DataMember(Name = "tz_num")]
        public string SuperSeat { get; set; }

        /// <summary>
        /// 商务座
        /// </summary>
        [DataMember(Name = "swz_num")]
        public string BusinessSeat { get; set; }
    }
}
