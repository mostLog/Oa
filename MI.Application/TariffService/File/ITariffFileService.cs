using MI.Application.Dto;

namespace MI.Application.File
{
    public interface ITariffFileService
    {
        ExcelDto ExcelExport(TariffPagedInputDto input);
    }
}
