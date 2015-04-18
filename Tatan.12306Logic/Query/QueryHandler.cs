using System;
using System.Collections.Generic;
using System.Net;
using Tatan.Common.Extension.Net;
using Tatan._12306Logic.Common;

namespace Tatan._12306Logic.Query
{
    public static class QueryHandler
    {
        public static object Condition;

        public static event Action<IDictionary<string, string>> InitBefore;

        public static event Action<IDictionary<string, string>, HttpWebResponse> InitAfter;

        public static event Action<IDictionary<string, string>> RequestBefore;

        public static event Action<IDictionary<string, string>, HttpWebResponse> RequestAfter;

        /// <summary>
        /// 初始化请求Cookie，产生会话Id
        /// </summary>
        /// <param name="input"></param>
        public static void Init(IDictionary<string, string> input)
        {
            if (InitBefore != null)
                InitBefore(input);

            var response = CommonHandler.Request(@"Query\Init", input);
            CommonHandler.SetCookie(input, response);

            if (InitAfter != null)
                InitAfter(input, response);
        }

        public static bool Request(IDictionary<string, string> input)
        {
            if (RequestBefore != null)
                RequestBefore(input);

            Log(input);

            var response = CommonHandler.Request(@"Query\Query", input);
            var tickets = response.GetJsonObject<CommonResponse<List<TicketResult>>>();
            var result = false;
            if (tickets == null)
            {
                return false;
            }
            if (!tickets.Status)
            {
                return false;
            }

            foreach (var ticket in tickets.Data)
            {
                if (HasTicket(ticket.Body))
                {
                    SetTicket(input, ticket);
                    GotoOrder(input);
                    result = true;
                    break;
                }
            }

            if (RequestAfter != null)
                RequestAfter(input, response);

            return result;
        }

        private static void SetTicket(IDictionary<string, string> input, TicketResult ticket)
        {
            input["secret"] = ticket.SecretStr;
            input["trainNo"] = ticket.Body.TrainNo;
            input["trainCode"] = ticket.Body.StationTrainCode;
            input["seatType"] = ticket.Body.TrainSeatFeature;
            input["yp_info"] = ticket.Body.YpInfo;
            input["trainDate"] = ticket.Body.StartTrainDate;
            input["trainLocation"] = ticket.Body.LocationCode;
        }

        private static void Log(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Query\Log", input);
            var commonResponse = response.GetJsonObject<CommonResponse<CommonResult>>();
            if (!commonResponse.Status)
            {
                throw new Exception("query log error");
            }
        }

        public static void GotoOrder(IDictionary<string, string> input)
        {
            if (!CheckUser(input))
            {
                throw new Exception("check error.");
            }

            var response = CommonHandler.Request(@"Query\GotoOrder", input);
            var commonResponse = response.GetJsonObject<CommonResponse<object>>();
            if (!commonResponse.Status)
            {
                throw new Exception("goto order error.");
            }
        }

        private static bool CheckUser(IDictionary<string, string> input)
        {
            var response = CommonHandler.Request(@"Query\CheckUser", input);
            var commonResponse = response.GetJsonObject<CommonResponse<CheckResult>>();
            return commonResponse.Data.Flag;
        }

        private static bool HasTicket(string number)
        {
            return number != "无" && number != "--";
        }

        private static bool HasTicket(TicketBody ticket, TicketType types = TicketType.All)
        {
            if ((types & TicketType.NoneSeat) == TicketType.NoneSeat)
            {
                if (HasTicket(ticket.NoneSeat))
                    return true;
            }
            if ((types & TicketType.HardSeat) == TicketType.HardSeat)
            {
                if (HasTicket(ticket.HardSeat))
                    return true;
            }
            if ((types & TicketType.SoftSeat) == TicketType.SoftSeat)
            {
                if (HasTicket(ticket.SoftSeat))
                    return true;
            }
            if ((types & TicketType.HardLying) == TicketType.HardLying)
            {
                if (HasTicket(ticket.HardLying))
                    return true;
            }
            if ((types & TicketType.SoftLying) == TicketType.SoftLying)
            {
                if (HasTicket(ticket.SoftLying))
                    return true;
            }
            if ((types & TicketType.SecondSeat) == TicketType.SecondSeat)
            {
                if (HasTicket(ticket.SecondSeat))
                    return true;
            }
            if ((types & TicketType.FristSeat) == TicketType.FristSeat)
            {
                if (HasTicket(ticket.FristSeat))
                    return true;
            }
            if ((types & TicketType.SuperSeat) == TicketType.SuperSeat)
            {
                if (HasTicket(ticket.SuperSeat))
                    return true;
            }
            if ((types & TicketType.BusinessSeat) == TicketType.BusinessSeat)
            {
                if (HasTicket(ticket.BusinessSeat))
                    return true;
            }
            return false;
        }
    }

    /// <summary>
    /// 火车票类型
    /// </summary>
    [Flags]
    public enum TicketType
    {
        /// <summary>
        /// 无座
        /// </summary>
        NoneSeat = 1,

        /// <summary>
        /// 硬座
        /// </summary>
        HardSeat = 2,

        /// <summary>
        /// 软座
        /// </summary>
        SoftSeat = 4,

        /// <summary>
        /// 硬卧
        /// </summary>
        HardLying = 8,

        /// <summary>
        /// 软卧
        /// </summary>
        SoftLying = 16,

        /// <summary>
        /// 二等座
        /// </summary>
        SecondSeat = 32,

        /// <summary>
        /// 一等座
        /// </summary>
        FristSeat = 64,

        /// <summary>
        /// 特等座
        /// </summary>
        SuperSeat = 128,

        /// <summary>
        /// 商务座
        /// </summary>
        BusinessSeat = 256,

        /// <summary>
        /// 全部
        /// </summary>
        All = NoneSeat | HardSeat | SoftSeat | HardLying | SoftLying | SecondSeat | FristSeat | SuperSeat | BusinessSeat
    }
}