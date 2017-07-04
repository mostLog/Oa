using MI.Application.Dto;

namespace MI.Application.File
{
    public interface IFlightFeeFileService
    {
        ExcelDto ExcelExport(FlightFeePagedInputDto input);
    }
}
