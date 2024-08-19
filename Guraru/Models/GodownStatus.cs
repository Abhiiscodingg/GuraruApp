using GuraruRepository.Models;

namespace Guraru.Models
{
    public class GodownStatus
    {
        public RawQuality? Quality { get; set; }

        //Thread Weight remaining in Godown
        public int ThreadWtGodown { get; set; }
        //Thread Weight processed in Machines
        public int ThreadWtprocessed { get; set; }
        //Thread Weight currently on Machines
        public int CurrentWeightOnMachine { get; set; }
        //Cloth Weight Completed from Machines
        public int MachineCompletedWt { get; set; }
        //Machine Products in Godown
        public int MachineProductGodown { get; set; }
        //Cloth weight currently at Final Stage
        public int CurrentFinalStageWt { get; set; }
        //Cloth Weight completed from Final
        public int FinalStageCompleted { get; set; }
        //Final Cloth Product in Godown
        public int FinalProductGodown { get; set; }
        //Cloth weight exit from Godown
        public int GodownExit { get; set; }
    }
}
