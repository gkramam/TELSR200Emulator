using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Xml;

namespace TELSR200Emulator.Messages
{
    abstract public class BaseResponse
    {
        protected BaseMessage _request;

        protected string _commandName;

        protected StringBuilder _responseBuilder;
        public BaseResponse(BaseMessage request)
        {
            _request = request;
            _responseBuilder = new StringBuilder();
        }

        protected Dictionary<string, string> GetXMLDictionary()
        {
            if (_request != null)
            {
                if (_request.UnitNumber == 1)
                {
                    if (this is BaseEndOfExec)
                        return AppConfiguration.ManipulatorEoEs[_request.CommandName];
                    else
                        return AppConfiguration.ManipulatorResponses[_request.CommandName];
                }
                else
                {
                    if (this is BaseEndOfExec)
                        return AppConfiguration.PreAlignerEoEs[_request.CommandName];
                    else
                        return AppConfiguration.PreAlignerResponses[_request.CommandName];
                }
            }

            return null;
        }

        public virtual Dictionary<string, string> ReadXML(XmlDocument xmlDoc)
        {
            var doc = xmlDoc;
            var status1Nodes = doc.GetElementsByTagName("Status1").Item(0).ChildNodes;
            ResponseStatus1 status1 = ResponseStatus1.None;
            foreach (XmlNode sn1 in status1Nodes)
            {
                switch (sn1.Name)
                {
                    case "UnitReady":
                        status1 = int.Parse(sn1.InnerText) == 1 ? status1 | ResponseStatus1.UnitReady : status1;
                        break;
                    case "ErrorOccured":
                        status1 = int.Parse(sn1.InnerText) == 1 ? status1 | ResponseStatus1.ErrorOccured : status1;
                        break;
                    case "ServoOff":
                        status1 = int.Parse(sn1.InnerText) == 1 ? status1 | ResponseStatus1.ServoOff : status1;
                        break;
                }
            }
            var status2Nodes = doc.GetElementsByTagName("Status2").Item(0).ChildNodes;
            ResponseStatus2 status2 = ResponseStatus2.None;
            foreach (XmlNode sn2 in status2Nodes)
            {
                switch (sn2.Name)
                {
                    case "BatteryVoltageDropped":
                        status2 = int.Parse(sn2.InnerText) == 1 ? status2 | ResponseStatus2.BattVoltDropped : status2;
                        break;
                    case "Blade1_Vac_Grip_HasWafer":
                        status2 = int.Parse(sn2.InnerText) == 1 ? status2 | ResponseStatus2.Blade1_Vac_Grip_HasWafer : status2;
                        break;
                    case "Blade2_LineSensor_Haswafer":
                        status2 = int.Parse(sn2.InnerText) == 1 ? status2 | ResponseStatus2.Blade2_LineSensor_Haswafer : status2;
                        break;
                }
            }

            string ackCd = doc.GetElementsByTagName("AckCD").Item(0).InnerText;
            string Delay = doc.GetElementsByTagName("Delay").Item(0).InnerText;

            Dictionary<string, string> ret = new Dictionary<string, string>();
            ret.Add("Delay", Delay);
            ret.Add("Status1", ResponseStatusCalculator.Calculate((byte)status1));
            ret.Add("Status2", ResponseStatusCalculator.Calculate((byte)status2));
            ret.Add("AckCD", ackCd);

            return ret;
        }

        public virtual string Generate(Device device)
        {
            string status = string.Empty;
            string ackcd = string.Empty;

            if (AppConfiguration.useXmlFilesForReplies)
            {
                var xmlData = GetXMLDictionary();
                status = xmlData["Status1"] + xmlData["Status2"];
                ackcd = xmlData["AckCD"];
                int delay = Convert.ToInt32(xmlData["Delay"]);
                if(delay >0)
                {
                    Thread.Sleep(delay);
                }
                status = ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus1()) + ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus2());
                ackcd = "0000";
            }

            StringBuilder temp = new StringBuilder();

            temp.Append(',');
            temp.Append(_request.UnitNumber);
            temp.Append(',');

            if (AppConfiguration.useSequenceNumber)
            {
                temp.Append(_request.SeqNum.Value.ToString("D2"));
                temp.Append(',');
            }

            //temp.Append(ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus1()));
            //temp.Append(ResponseStatusCalculator.Calculate((byte)device.GetResponseStatus2()));
            temp.Append(status);

            temp.Append(',');
            //temp.Append("0000");//ACkCD & Errcd are same
            temp.Append(ackcd);
            temp.Append(',');
            temp.Append(_request.CommandName);
            temp.Append(',');

            _responseBuilder.Insert(0, temp.ToString());

            if (AppConfiguration.checkSumCheck)
            {
                var chksum = CheckSum.Compute(_responseBuilder.ToString());
                _responseBuilder.Append(',');
                _responseBuilder.Append(chksum);
            }

            _responseBuilder.Append('\r');

            if (this is BaseEndOfExec)
                _responseBuilder.Insert(0, '!');
            else
                _responseBuilder.Insert(0, '$');

            return _responseBuilder.ToString();
        }
    }
}
