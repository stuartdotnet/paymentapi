using AutoMapper;
using PaymentAPI.Model;

namespace PaymentAPI.Models
{
    public class PaymentsMappingProfile : Profile
    {
        public PaymentsMappingProfile()
        {
            CreateMap<PaymentRequest, NewPaymentRequest>().ReverseMap();
            CreateMap<PaymentRequest, PaymentRequestItem>()
                .ForMember(m => m.PaymentStatus, opt => opt.MapFrom(x => (PaymentStatus)(int)x.Status));
        }
    }
}
