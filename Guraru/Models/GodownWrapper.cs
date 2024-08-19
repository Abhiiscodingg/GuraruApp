using GuraruRepository.Models;

namespace Guraru.Models
{
    public class GodownWrapper
    {
        public List<GodownStatus>? GodownStatuses { get; set; }
        public List<RawQuality>? RawQualities    { get; set; }
    }
}
