using AutoMapper;
using AdvancedBusinessAPI.Application.DTOs;
using AdvancedBusinessAPI.Domain;

namespace AdvancedBusinessAPI.Application.Mapping;

public class MappingProfile : Profile
{
  public MappingProfile()
  {
    CreateMap<MotoCreateDto, Moto>();
    CreateMap<MotoUpdateDto, Moto>();

    CreateMap<ManutencaoCreateDto, Manutencao>();
    CreateMap<ManutencaoUpdateDto, Manutencao>();
  }
}
