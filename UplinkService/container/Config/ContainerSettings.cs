namespace UplinkService.Config
{
    public class ContainerSettings
    {
        public string SendToAllServers { get; set; }

        public string PriorityServerIP { get; set; }

        public string PriorityServerPort { get; set; }

        public string OnlySendToTtn { get; set; }

        public string TtnPort { get; set; }
        
        public string TtnIP { get; set; }

        public string TtnRouter { get; set; }
    }
}
