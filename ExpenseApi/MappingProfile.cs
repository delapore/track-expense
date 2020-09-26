using AutoMapper;
using ExpenseApi.DataTransferObjects;
using ExpenseApi.Models;

namespace ExpenseApi
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Expense, ExpenseDto>();
            CreateMap<ExpenseForCreationDto, Expense>();
            CreateMap<ExpenseForUpdateDto, Expense>();
        }
    }
}
