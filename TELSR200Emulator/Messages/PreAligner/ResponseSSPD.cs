
using System.Text;

namespace TELSR200Emulator.Messages.PreAligner
{
    public class ResponseSSPD : BaseResponse
    {
        public ResponseSSPD(BaseMessage req) : base(req) { }

        public override string Generate(Device device)
        {
            _responseBuilder = new StringBuilder();

            CommandSSPD req = (CommandSSPD)_request;

            _responseBuilder.Append(req.Level);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.SpeedType);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Axis);
            _responseBuilder.Append(',');
            _responseBuilder.Append(req.Speed);

            return base.Generate(device);
        }
    }
}
