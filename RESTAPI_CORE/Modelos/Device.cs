namespace RESTAPI_CORE.Modelos
{
    public class Device
    {
        public int IdDevice { get; set; }
        public int DeviceTypeId { get; set; }
        public string DisplayName { get; set; }
        public int ControllerId { get; set; }
        public bool Enabled { get; set; }
    }
}
