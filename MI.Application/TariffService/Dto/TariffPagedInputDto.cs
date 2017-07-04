using System;

namespace MI.Application.Dto
{
    public class TariffPagedInputDto: PagedInputDto
    {
        public string Community { get; set; }
        public string Building { get; set; }
        public string RoomNo { get; set; }

        public DateTime? TariffStartDate{ get; set; }

        public DateTime? TariffEndDate { get; set; }

        public int IsPayment { get; set; } = 2;
    }
}
